using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChromeBookmarker.UI
{
    public partial class FolderEditForm : Form
    {
        public String FolderName
        {
            get { return teFolderName.Text; }
            set { teFolderName.Text = value; }
        }

        public FolderEditForm()
        {
            InitializeComponent();
        }

        private void PerformSave()
        {
            //if folder name is empty report error and exit method
            if (FolderName == String.Empty)
            {
                MessageBox.Show("Folder name is empty", "Folder name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //return everything is ok
            DialogResult = DialogResult.OK;
        }

        private void PerformCancel()
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            PerformCancel();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PerformSave();
        }

        private void FolderEditForm_KeyDown(object sender, KeyEventArgs e)
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
    }
}
