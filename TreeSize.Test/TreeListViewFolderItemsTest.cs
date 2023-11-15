using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TreeSize.ViewModels;

namespace TreeSize.Test
{
    [TestClass]
    public class TreeListViewFolderItemsTest
    {
        TreeListViewFolderItems treeListViewFolder = new TreeListViewFolderItems();        

        [TestMethod]
        public void GetSubfoldersItemsTest()
        {                  
            string path = @".\Images";

            var colection = treeListViewFolder.GetSubfoldersItems(path);
            
            Assert.AreEqual(4, colection.Count);

        }
    }
}
