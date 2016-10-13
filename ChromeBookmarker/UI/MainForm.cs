using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MouseGlobalHook;
using WindowHelper;
using ChromeBookmarker.Classes;
using ChromeBookmarker.UI;
using System.IO;
using System.Xml.Serialization;


namespace ChromeBookmarker
{
    public partial class MainForm : Form
    {
        private const String BookmarksSaveFile = "Bookmarks.xml";


        /// <summary>
        /// Form to be used to edit bookmarks, it is not local cause we are using show not showdialog (not to block event chain)
        /// </summary>
        private NewBookmarkForm newBookmarkFrm;

        /// <summary>
        /// is windows in drag mode (between MouseDown and MouseUp)
        /// </summary>
        private Boolean _IsDragging = false;
        /// <summary>
        /// PSTree with all bookmarks
        /// </summary>
        private PSTreeNode<TreeItem> Bookmarks;
        public MainForm()
        {
            InitializeComponent();

            //create base "Bookmarks" node and add it to Bookmarks collection
            TreeItem baseItem = new TreeItem();
            baseItem.Type = TreeItemType.Folder;
            baseItem.Name = "Bookmarks";
            Bookmarks = new PSTreeNode<TreeItem>(baseItem);

            //load all preserved items from XML
            LoadTreeItemsFromXML(BookmarksSaveFile);
            //create tree from Bookmarks collection
            RecreateBookmarksTree();
            //Get global mouse hook and redirect callback to -> OnGlobalMouseEvent procedure
            CreateGlobalMouseHook();
            ResetLinkDisplayPanel();
        }

        /// <summary>
        /// Clear/create bookmark tree from Bookmarks property
        /// </summary>
        private void RecreateBookmarksTree()
        {
            //clear all existing nodes
            cbTreeView.Nodes.Clear();

            //create base Bookmark node
            TreeNode baseNode = cbTreeView.Nodes.Add("Bookmarks");
            baseNode.Tag = Bookmarks;
            baseNode.ImageIndex = 0;
            baseNode.SelectedImageIndex = 0;

            //recreate all nodes for children
            foreach(PSTreeNode<TreeItem> currChild in Bookmarks.Children)
            {
                CreateNodeInTree(currChild, baseNode);
            }

            //expand root node
            baseNode.Expand();            
        }

        /// <summary>
        /// Recursive function to recreate all nodes in tree view
        /// </summary>
        /// <param name="NodeToCreate">Node to be added to tree view</param>
        /// <param name="parentNode">Parent node in tree view to which the node should be added</param>
        private void CreateNodeInTree(PSTreeNode<TreeItem> NodeToCreate, TreeNode parentNode)
        {
            //create base node for sent node
            TreeNode newNode = new TreeNode();
            newNode.Name = NodeToCreate.Value.Name;
            newNode.Text = NodeToCreate.Value.Name;
            newNode.Tag = NodeToCreate;
            //set icon
            if (NodeToCreate.Value.Type == TreeItemType.Bookmark)
            {
                newNode.ImageIndex = 1;
                newNode.SelectedImageIndex = 1;
            }else
            {
                newNode.ImageIndex = 0;
                newNode.SelectedImageIndex = 0;
            }

            parentNode.Nodes.Add(newNode);


            //call the function on all children so their children can be created too
            foreach(PSTreeNode<TreeItem> currChild in NodeToCreate.Children)
            {
                CreateNodeInTree(currChild, newNode);
            }
        }

        /// <summary>
        /// Method to load bookmarks tree from XML file using serialization
        /// </summary>
        /// <param name="AFilename">XML filename to load</param>
        private void LoadTreeItemsFromXML(String AFilename)
        {
            if (!File.Exists(AFilename))
                return;

            //Create a stream reader and load list from it using xml serializer
            using (StreamReader reader = new StreamReader(AFilename))
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(PSTreeNode<TreeItem>));
                    Bookmarks = (PSTreeNode<TreeItem>)xmlSerializer.Deserialize(reader);
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        /// <summary>
        /// Method to save bookmarks tree to XML file using serialization
        /// </summary>
        /// <param name="AFilename">XML filename to use for saving</param>
        private void SaveTreeItemsToXML(String AFilename)
        {
            //create a stream writer and save list to it using xml serializer
            using (StreamWriter writer = new StreamWriter(AFilename))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(PSTreeNode<TreeItem>));
                xmlSerializer.Serialize(writer, Bookmarks);
            }

        }
        /// <summary>
        /// Creates global mouse hook and adds events to it
        /// </summary>
        private void CreateGlobalMouseHook()
        {
            MouseHook.OnLeftMouseDown += new LLMouseEvent(OnGlobalLeftMouseDown);
            MouseHook.OnLeftMouseUp += new LLMouseEvent(OnGlobalLeftMouseUp);
            MouseHook.OnMouseMove += new LLMouseEvent(OnGlobalMouseMove);
            MouseHook.SetHook();
        }

        /// <summary>
        /// removes events and global mouse hook
        /// </summary>
        private void RemoveGlobalMouseHook()
        {
            MouseHook.OnLeftMouseDown -= new LLMouseEvent(OnGlobalLeftMouseDown);
            MouseHook.OnLeftMouseUp -= new LLMouseEvent(OnGlobalLeftMouseUp);
            MouseHook.OnMouseMove -= new LLMouseEvent(OnGlobalMouseMove);
            MouseHook.RemoveHook();
        }

        private void OnGlobalLeftMouseDown(int nCode, IntPtr wParam, IntPtr lParam)
        {
            _IsDragging = true;
        }

        private void OnGlobalLeftMouseUp(int nCode, IntPtr wParam, IntPtr lParam)
        {
            _IsDragging = false;

            //check if mouse is on chrome bookmarker window if its not exit procedure
            Point mousePos = GetMouseCoordinatesFromlParam(lParam);

            if (!this.RectangleToScreen(this.DisplayRectangle).Contains(mousePos))
            {
                ResetLinkDisplayPanel();
                return;
            }

            //get window under mouse, its caption and class
            IntPtr hwnd = WindowHandlingHelper.GetWindowUnderPoint(mousePos);
            String windowClass = WindowHandlingHelper.GetWindowClassName(hwnd);
            String windowCaption = WindowHandlingHelper.GetWindowCaption(hwnd);

            //if window class doesnt contain CHROME reset the panel so it shows that its not chrome window
            if (!windowClass.ToUpper().Contains("CHROME"))
                return;

            //if we reached this point it means we have a winner ladies and gentleman! 
            if (newBookmarkFrm == null)
                newBookmarkFrm = new NewBookmarkForm();

            //setup new bookmark window
            newBookmarkFrm.TreeViewToAddTo = cbTreeView;
            newBookmarkFrm.BookmarkTree = Bookmarks;
            newBookmarkFrm.teBookmarkTitle.Text = windowCaption;
            newBookmarkFrm.teLink.Text = "";

            //reset its position
            newBookmarkFrm.Top = Top + 100;
            newBookmarkFrm.Left = Left + 25;

            //show it
            newBookmarkFrm.Show(hwnd);
        }

        /// <summary>
        /// Decode lParam struct of hook callback procedure and get Point coordinates from it
        /// </summary>
        /// <param name="lParam">lParam from global hook event</param>
        /// <returns></returns>
        private Point GetMouseCoordinatesFromlParam(IntPtr lParam)
        {
            //get mouse data
            MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));            
            return new Point(hookStruct.pt.x, hookStruct.pt.y);
        }


        private void OnGlobalMouseMove(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //we only want to check borders if we are currently dragging
            if (!_IsDragging)
                return;

            //check if mouse is on chrome bookmarker window if its not exit procedure
            Point mousePos = GetMouseCoordinatesFromlParam(lParam);

            if (!this.RectangleToScreen(this.DisplayRectangle).Contains(mousePos))
            {
                ResetLinkDisplayPanel();
                return;
            }
        
            //get window under mouse, its caption and class
            IntPtr hwnd = WindowHandlingHelper.GetWindowUnderPoint(mousePos);           
            String windowClass = WindowHandlingHelper.GetWindowClassName(hwnd);
            String windowCaption = WindowHandlingHelper.GetWindowCaption(hwnd); 
            
            //if window class doesnt contain CHROME reset the panel so it shows that its not chrome window
            if (!windowClass.ToUpper().Contains("CHROME"))
            {
                ResetLinkDisplayPanel();
                return;
            }

            //display chrome window caption in link display panel
            SetLinkInDisplayPanel(windowCaption);
        }

        /// <summary>
        /// Set link display panel to default look and feel
        /// </summary>
        private void ResetLinkDisplayPanel()
        {
            lblDragWindowCaption.Text = "";
            pnlDropWindowPanel.BackColor = Color.WhiteSmoke;
        }
        /// <summary>
        /// set link display panel look so it shows that window is in drag n drop range
        /// </summary>
        /// <param name="windowCaption">caption of panel; title of chrome window</param>
        private void SetLinkInDisplayPanel(String windowCaption)
        {
            lblDragWindowCaption.Text = windowCaption;
            pnlDropWindowPanel.BackColor = Color.PaleGreen;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveTreeItemsToXML(BookmarksSaveFile);
            RemoveGlobalMouseHook();
            //save user window position and size
            Mainwindow.Default.Left = Left;
            Mainwindow.Default.Top = Top;
            Mainwindow.Default.Width = Width;
            Mainwindow.Default.Height = Height;
            Mainwindow.Default.Save();
        }


        /// <summary>
        /// Event handler for node clicks; opens context menus on right click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null)
                return;

            if (e.Button == MouseButtons.Right)
            {
                //get clicked node PS node
                PSTreeNode<TreeItem> clickedNode = (PSTreeNode<TreeItem>)e.Node.Tag;
                //depending on clicked node open folder or bookmark context menu
                if (clickedNode.Value.Type == TreeItemType.Bookmark)
                {
                    cmsBookmark.Show(cbTreeView.PointToScreen(e.Location));
                }else
                {
                    cmsFolder.Show(cbTreeView.PointToScreen(e.Location));
                }
            }
        }

        /// <summary>
        /// Folder context menu -> new folder method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //get selected node in tree view
            TreeNode selectedNode = cbTreeView.SelectedNode;

            //this should not happen but never safe enough
            if (selectedNode == null)
                return;

            //get current bookmark tree node
            PSTreeNode<TreeItem> currPSNode = (PSTreeNode<TreeItem>)selectedNode.Tag;

            //create edit folder name form
            FolderEditForm newFolderFrm = new FolderEditForm();
            newFolderFrm.FolderName = "";

            //if user said ok, save the folder
            if (newFolderFrm.ShowDialog() == DialogResult.OK)
            {
                //create item to be saved in node
                TreeItem newFolderItem = new TreeItem();
                newFolderItem.Name = newFolderFrm.FolderName;
                newFolderItem.Type = TreeItemType.Folder;                

                //add new bookmark tree node and save item in it
                PSTreeNode<TreeItem> newPSNode = currPSNode.AddChild(newFolderItem);

                //create new tree view node and link bookmark tree node to it
                TreeNode newTreeNode = selectedNode.Nodes.Add(newFolderItem.Name);
                newTreeNode.Tag = newPSNode;
                newTreeNode.ImageIndex = 0;
                newTreeNode.SelectedImageIndex = 0;

                //expand root node of treeview so we can view our new node
                selectedNode.Expand();
            }
            
        }

        /// <summary>
        /// Folder context menu -> edit folder method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //get selected node in tree view
            TreeNode selectedNode = cbTreeView.SelectedNode;

            //this should not happen but you never know :O
            if (selectedNode == null)
                return;

            //get current bookmark tree node
            PSTreeNode<TreeItem> currPSNode = (PSTreeNode<TreeItem>)selectedNode.Tag;

            //dont allow editing base root node (Bookmark one)
            if (currPSNode == Bookmarks)
                return;

            //create edit folder name form
            FolderEditForm newFolderFrm = new FolderEditForm();
            newFolderFrm.FolderName = currPSNode.Value.Name;

            //if user said ok, save the folder changed name
            if (newFolderFrm.ShowDialog() == DialogResult.OK)
            {
                currPSNode.Value.Name = newFolderFrm.FolderName;
                selectedNode.Name = newFolderFrm.FolderName;
                selectedNode.Text = newFolderFrm.FolderName;
            }

        }

        /// <summary>
        /// Folder context menu -> delete folder method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        { 
            //get selected node in tree view
            TreeNode selectedNode = cbTreeView.SelectedNode;

            //this should not happen but you never know :O
            if (selectedNode == null)
                return;

            //get current bookmark tree node 
            PSTreeNode<TreeItem> currPSNode = (PSTreeNode<TreeItem>)selectedNode.Tag;

            //dont allow deleting base root node (Bookmark one)
            if (currPSNode == Bookmarks)
                return;

            //minimum safeguard; if there are bookmarks in folder ask permision to delete it. If we asked it every time on bookmark i think it would be too annoying
            if (selectedNode.Nodes.Count > 0
                && MessageBox.Show("Folder contains bookmarks are you sure you want to delete folder?", "Delete query", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            //get bookmark tree node parent node
            PSTreeNode<TreeItem> currPSNodeParent = (PSTreeNode<TreeItem>)selectedNode.Parent.Tag;

            //remove nodes
            currPSNodeParent.Children.Remove(currPSNode);
            selectedNode.Remove();
        }

        /// <summary>
        /// method from context menu to delete bookmark
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //get selected node in tree view
            TreeNode selectedNode = cbTreeView.SelectedNode;

            //this should not happen but you never know :O
            if (selectedNode == null)
                return;

            //get current bookmark tree node
            PSTreeNode<TreeItem> currPSNode = (PSTreeNode<TreeItem>)selectedNode.Tag;

            //dont allow deleting base root node (Bookmark one) , again should not happen cause context menu is for bookmarks only
            if (currPSNode == Bookmarks)
                return;

            //get bookmark tree node parent node
            PSTreeNode<TreeItem> currPSNodeParent = (PSTreeNode<TreeItem>)selectedNode.Parent.Tag;

            //deselect image
            pbPreview.Image = null;

            //remove nodes
            currPSNodeParent.Children.Remove(currPSNode);
            selectedNode.Remove();

            //delete the screenshot file
            if (File.Exists(currPSNode.Value.ScreenshotFileName))
            {
                File.Delete(currPSNode.Value.ScreenshotFileName);
            }
        }

        /// <summary>
        /// Event when tree view node selection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //there is no node, exit
            if (e.Node == null)
                return;

            //get PS bookmark node and belonging tree item
            PSTreeNode<TreeItem> selectedItem = (PSTreeNode<TreeItem>)e.Node.Tag;
            TreeItem selectedNode = selectedItem.Value;

            //clear image/url
            pbPreview.Image = null;
            lActiveBookmarkLink.Text = "";
            lActiveBookmarkLink.Links.Clear();

            //if node is bookmark then show screenshot and setup url
            if (selectedNode.Type == TreeItemType.Bookmark)
            {
                //setup url
                lActiveBookmarkLink.Text = selectedNode.URL;
                lActiveBookmarkLink.Links.Add(0, selectedNode.URL.Length, selectedNode.URL);
                //if screenshot file exists display it
                if (selectedNode.ScreenshotFileName != String.Empty && File.Exists(selectedNode.ScreenshotFileName))
                {
                    //we do it like a copy of loaded bitmap so the loaded bitmap can be released. if its not done this way file lock remains open to file
                    using (Bitmap loadedBitmap = new Bitmap(selectedNode.ScreenshotFileName))
                    {
                        pbPreview.Image = new Bitmap(loadedBitmap);
                    }
                }
            }
        }

        /// <summary>
        /// URL link clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lActiveBookmarkLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //start chrome incognito and display url
            System.Diagnostics.Process.Start("chrome.exe", "--incognito " + e.Link.LinkData.ToString());
        }

        private void cbTreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void cbTreeView_DragDrop(object sender, DragEventArgs e)
        {
            //get the drop point and destination node
            Point dropPoint = cbTreeView.PointToClient(new Point(e.X, e.Y));
            TreeNode destinationNode = cbTreeView.GetNodeAt(dropPoint);

            //get dragged node
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            //if node is not droppable exit method
            if (!IsNodeDroppable(draggedNode, destinationNode))
                return;

            //get PSNodes so we can rearange them
            PSTreeNode<TreeItem> draggedPSNode = (PSTreeNode<TreeItem>)draggedNode.Tag;
            PSTreeNode<TreeItem> destinationPSNode = (PSTreeNode<TreeItem>)destinationNode.Tag;

            //remove dragged node
            draggedNode.Remove();
            Bookmarks.RemoveChild(draggedPSNode);
            
            //add dragged node to destination node children nodes
            destinationNode.Nodes.Add(draggedNode);
            destinationPSNode.Children.Add(draggedPSNode);

            //show new bookmark in tree
            destinationNode.Expand();

        }

        /// <summary>
        /// Method to check if one treeview node can be dragdropped on another
        /// </summary>
        /// <param name="draggedNode">Treeview node to be drag n dropped</param>
        /// <param name="dropTarget">Destination treeview node</param>
        /// <returns></returns>
        private Boolean IsNodeDroppable(TreeNode draggedNode, TreeNode dropTarget)
        {
            //we can't drop node on itself
            if (draggedNode == dropTarget)
                return false;

            //get PSNode for drop target
            PSTreeNode<TreeItem> dropPSNode = (PSTreeNode<TreeItem>)dropTarget.Tag;

            //if the drop target is not folder then we cant drop it
            if (dropPSNode.Value.Type != TreeItemType.Folder)
                return false;

            //NOTE: also for simplicity of app we dont allow moving folders, only bookmarks
            PSTreeNode<TreeItem> draggedPSNode = (PSTreeNode<TreeItem>)draggedNode.Tag;
            if (draggedPSNode.Value.Type != TreeItemType.Bookmark)
                return false;

            //last check we need to do is see if drop target is child of dragged node (that could be messy)
            List<TreeNode> childrenNodes = new List<TreeNode>();
            EnumerateChildNodes(draggedNode, childrenNodes);
            if (childrenNodes.Contains(dropTarget))
                return false;

            return true;
        }

        /// <summary>
        /// Function to recursively enumerate all child nodes of a single treeview node
        /// </summary>
        /// <param name="ParentNode">Parent node to enumerate; will not be included in list</param>
        /// <param name="ListOfNodes">List of nodes to contain children nodes</param>
        private void EnumerateChildNodes(TreeNode ParentNode, List<TreeNode> ListOfNodes)
        {
            foreach(TreeNode ChildNode in ParentNode.Nodes)
            {
                ListOfNodes.Add(ChildNode);
                EnumerateChildNodes(ChildNode, ListOfNodes);
            }
        }

        private void cbTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //load window position and size
            Left = Mainwindow.Default.Left;
            Top = Mainwindow.Default.Top;
            Width = Mainwindow.Default.Width;
            Height = Mainwindow.Default.Height;

            //start the save timer
            saveTimer.Start();
        }

        /// <summary>
        /// Save timer event; save Bookmarks every minute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveTimer_Tick(object sender, EventArgs e)
        {
            SaveTreeItemsToXML(BookmarksSaveFile);
        }
    }
}
