using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeBookmarker.Classes
{
    public enum TreeItemType
    {
        Folder, Bookmark
    }

    public class TreeItem
    {
        public TreeItem()
        {

        }
        /// <summary>
        /// Type of item
        /// </summary>
        public TreeItemType Type { get; set; }
        /// <summary>
        /// Item name/display name
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// URL of item
        /// </summary>
        public String URL { get; set; }
        public String ScreenshotFileName { get; set;}
    }
}
