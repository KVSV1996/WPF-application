using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TreeSize.Models;

namespace TreeSize.ViewModels
{
    public class TreeListViewFolderItems : INotifyPropertyChanged
    {
        private ObservableCollection<TreeListFolderItem> _folders;
        private Counting сounting = new Counting();
        private FileHelper fileHelper = new FileHelper();        

        public TreeListViewFolderItems()
        {
            _folders = new ObservableCollection<TreeListFolderItem>();
        }

        public ObservableCollection<TreeListFolderItem> folders
        {
            get { return _folders; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        delegate void AddItem(TreeListFolderItem treeListFolderItem);
        public void AddToFolder(TreeListFolderItem treeListFolderItem)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                folders.Add(treeListFolderItem);
            }
            else
            {
                AddItem add = AddToFolder;
                Application.Current.Dispatcher.BeginInvoke(add, treeListFolderItem);
            }
        }

        delegate void changeItem(ConcurrentStack<TreeListViewFolderItems> stack, TreeListFolderItem item);
        public void ChangeItem(ConcurrentStack<TreeListViewFolderItems> stack, TreeListFolderItem item)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                TreeListViewFolderItems number;
                stack.TryPop(out number);
                for (int index = 0; index < number.folders.Count(); index++)
                {
                    if (number.folders[index].Name == item.Name)
                    {
                        folders[index] = item;
                    }
                }
            }
            else
            {
                changeItem Change = ChangeItem;
                Application.Current.Dispatcher.BeginInvoke(Change, stack, item);
            }
        }

        public ObservableCollection<TreeListFolderItem> GetSubfoldersItems(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            ObservableCollection<TreeListFolderItem> childrenItems = new ObservableCollection<TreeListFolderItem>();
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                DirectoryInfo[] dirs = dir.GetDirectories("*", SearchOption.TopDirectoryOnly);
                FileInfo[] files = dir.GetFiles("*", SearchOption.TopDirectoryOnly);
                foreach (DirectoryInfo _dir in dirs)
                {
                    TreeListFolderItem item;

                    item = new TreeListFolderItem
                    {
                        Name = _dir.Name,
                        Size = сounting.StringSize(0),
                        FullName = _dir.FullName,                        
                        Image = fileHelper.SetFolderImage(_dir),                       
                    };

                    if (_dir.EnumerateFileSystemInfos("*",
                            new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = false }).Any())
                    {
                        item.Items = new ObservableCollection<TreeListFolderItem>()
                        {
                            new TreeListFolderItem()
                        };
                    }

                    Task.Run(() => сounting.GetSize(item));
                    childrenItems.Add(item);
                }

                foreach (FileInfo _file in files)
                {
                    childrenItems.Add(new TreeListFolderItem
                    {
                        Name = _file.Name,
                        FullName = _file.FullName,
                        Size = сounting.StringSize(_file.Length),                        
                        Image = fileHelper.SetFileImage(),                       
                    });
                }
            }
            catch { }

            return childrenItems;
        }
    }


}
