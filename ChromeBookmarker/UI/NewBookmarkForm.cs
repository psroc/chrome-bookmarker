using System;
using System.Windows.Forms;
using ChromeBookmarker.Classes;
using System.Windows.Automation;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using WindowHelper;

namespace ChromeBookmarker.UI
{
    public partial class NewBookmarkForm : Form
    {
        IntPtr ChromeHWND;
        public TreeView TreeViewToAddTo { get; set; }
        public PSTreeNode<TreeItem> BookmarkTree { get; set; }
        public NewBookmarkForm()
        {
            InitializeComponent();
        }

        private void PerformSave()
        {
            if (teBookmarkTitle.Text == String.Empty)
            {
                MessageBox.Show("Bookmark title is empty!");
                return;
            }

            if (teLink.Text == String.Empty)
            {
                MessageBox.Show("Link is empty!");
                return;
            }

            if (TreeViewToAddTo.SelectedNode == null)
            {
                MessageBox.Show("Bookmark category is not selected");
                return;
            }

            //get the tree node that will be used as parent
            TreeNode newNodeParent = TreeViewToAddTo.SelectedNode;
            //if its a bookmark and not a folder get one lever higher (folder)
            if (((PSTreeNode<TreeItem>)newNodeParent.Tag).Value.Type != TreeItemType.Folder)
                newNodeParent = newNodeParent.Parent;

            PSTreeNode<TreeItem> currentBookmarksNode = (PSTreeNode<TreeItem>)newNodeParent.Tag;

            //save screenshot file
            String ssFilename = Guid.NewGuid().ToString() + ".jpg";
            pbScreenshot.Image.Save(ssFilename, ImageFormat.Jpeg);


            //fill in new tree item
            TreeItem newTI = new TreeItem();
            newTI.Name = teBookmarkTitle.Text;
            newTI.URL = teLink.Text;
            newTI.ScreenshotFileName = ssFilename;
            newTI.Type = TreeItemType.Bookmark;

            PSTreeNode<TreeItem> newBookmarkNode = currentBookmarksNode.AddChild(newTI);

            //fill in new tree item node
            TreeNode newNode = new TreeNode();
            newNode.Name = newTI.Name;
            newNode.Text = newTI.Name;
            newNode.Tag = newBookmarkNode;
            newNode.ImageIndex = 1;
            newNode.SelectedImageIndex = 1;
            newNodeParent.Nodes.Add(newNode);

            
            this.Hide();
        }

        private void SaveHWNDScreenshot(IntPtr HWND)
        {
            //TODO: bitblt is making problems with aero giving black screen for some window handles, we are using a not nice, dirty fix, get whole desktop and cut out out window
            //thats a problem if window is not topmost, partly covered etc...

            //get hdc of target
            IntPtr desktopHWND = WindowHandlingHelper.GetDesktopWindow();

            IntPtr hdcSource = WindowHandlingHelper.GetWindowDC(desktopHWND);

            //get rectangle 
            RECT wRectangle = new RECT();
            WindowHandlingHelper.GetWindowRect(HWND,ref wRectangle);

            RECT dRect = new RECT();
            WindowHandlingHelper.GetWindowRect(desktopHWND, ref dRect);

            Debug.Write(wRectangle.top.ToString() + ";" + wRectangle.left.ToString() + ";" + wRectangle.bottom.ToString() + ";" + wRectangle.right.ToString());

            //create destination device context and destination bitmap
            IntPtr hdcDest = WindowHandlingHelper.CreateCompatibleDC(hdcSource);
            IntPtr destBitmap = WindowHandlingHelper.CreateCompatibleBitmap(hdcSource, wRectangle.right - wRectangle.left, wRectangle.bottom - wRectangle.top);

            //copy bitmap
            IntPtr hPrevious = WindowHandlingHelper.SelectObject(hdcDest, destBitmap);
            WindowHandlingHelper.BitBlt(hdcDest, 0, 0, wRectangle.right - wRectangle.left, wRectangle.bottom - wRectangle.top, hdcSource, wRectangle.left, wRectangle.top, GDIConstants.SRCCOPY);
            WindowHandlingHelper.SelectObject(hdcDest, hPrevious);
            WindowHandlingHelper.DeleteDC(hdcDest);
            WindowHandlingHelper.ReleaseDC(desktopHWND, hdcSource);
            Image finalImg = Image.FromHbitmap(destBitmap);
            WindowHandlingHelper.DeleteObject(destBitmap);

            //set picture box image to newly captured one
            pbScreenshot.Image = finalImg;
        }

        public void Show(IntPtr HWND)
        {
            ChromeHWND = HWND;            
            Show();
            eventTimer.Start();
        }

        private void PerformCancel()
        {
            teBookmarkTitle.Text = "";
            teLink.Text = "";
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PerformSave();
        }

        /// <summary>
        /// Using automation returns a URL from chrome url bar
        /// </summary>
        /// <param name="HWND">hwnd of chrome window</param>
        /// <returns></returns>
        private String GetURLFromChromeHWND(IntPtr HWND)
        {
            //TODO: fix method so it doesn't need a timer delay to function properly 
            //this function is the main reason we have a timer (which makes no sense) but windows automation is not playing nice
            //it shows like hwnd doesnt have a "address and search bar" and when you wait 100 ms then it has that window. not sure what is going on here

            //get chrome window from its hwnd
            AutomationElement chromeWind = AutomationElement.FromHandle(HWND);
            //get address bar

            AutomationElement chromeUrlBar = chromeWind.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar", PropertyConditionFlags.IgnoreCase));

            //if url bar is found return url
            if (chromeUrlBar != null)
            {
                AutomationPattern[] patterns = chromeUrlBar.GetSupportedPatterns();
                if (patterns.Length > 0)
                {
                    ValuePattern val = (ValuePattern)chromeUrlBar.GetCurrentPattern(patterns[0]);
                    return val.Current.Value;
                }
            }

            return "";
        }

        private void BookmarkEditForm_KeyDown(object sender, KeyEventArgs e)
        {
            //perform save on enter key
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                PerformSave();
            }
            //perform cancel on escape key
            else if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                PerformCancel();
            }
        }

        private void BookmarkEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if user is closing just hide
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            PerformCancel();
        }


        private void eventTimer_tick(object sender, EventArgs e)
        {
            eventTimer.Stop();
            //save screenshot 
            SaveHWNDScreenshot(ChromeHWND);
            //get url from chrome window 
            teLink.Text = GetURLFromChromeHWND(ChromeHWND);
        }
    }
}
