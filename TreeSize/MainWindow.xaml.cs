using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TreeSize.Models;
using TreeSize.ViewModels;

namespace TreeSize
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<TreeListFolderItem> TreeListViewItems { get; set; }
        private TreeListViewFolderItems treeItems = new TreeListViewFolderItems();
        private TreeListViewFolderModel treeModel;
        

        public MainWindow()
        {
            InitializeComponent();

            treeModel = new TreeListViewFolderModel(treeItems);
            TreeListViewItems = new ObservableCollection<TreeListFolderItem>();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using CommonOpenFileDialog dlg = new CommonOpenFileDialog("Select a folder");
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dlg.Multiselect = false;
            dlg.AllowNonFileSystemItems = false;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {                
                TreeListViewItems.Clear();                
                treeModel.treeListViewFolderItem.folders.Clear();                
                string folderPath = dlg.FileName;
                ChoiceFolder.Text = folderPath;
                DataContext = treeItems;
                Task.Run(() => treeModel.ViewAsync(folderPath));
            }
        }        

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
        }

        private void TreeView_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }
            var context = (TreeListViewFolderItems)DataContext;
            var item = e.OriginalSource as TreeViewItem;
            var folder = item.Header as TreeListFolderItem;           
            if (folder.Items != null)
            {
                folder.Items.Clear();
                var subFolder = context.GetSubfoldersItems(folder.FullName);
                foreach (var subItem in subFolder)
                {
                    folder.Items.Add(subItem);
                }
            }            
            
        }
    }
}
