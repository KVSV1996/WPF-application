using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TreeSize.Models;

namespace TreeSize.ViewModels
{
    public class TreeListViewFolderModel
    {      
        public TreeListViewFolderItems treeListViewFolderItem;
        private ConcurrentStack<TreeListViewFolderItems> concurrentStackFolder = new ConcurrentStack<TreeListViewFolderItems>();
        private Counting сounting = new Counting();
        private FileHelper fileHelper = new FileHelper();
        private List<Task> tasks = new List<Task>();       

        public TreeListViewFolderModel(TreeListViewFolderItems treeListViewFolderItem)
        {
            this.treeListViewFolderItem = treeListViewFolderItem;
        }

        public Task ViewAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            if (directoryInfo == null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            Task.Run(() => RootView(directoryInfo));
            Task.Run(() => DataView(directoryInfo));

            return Task.CompletedTask;
        }

        private void RootView(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            TopDirInfo(directoryInfo);
            PreparingFoldersView(directoryInfo);                       
        }

        private void DataView(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            SubfoldersInfo(directoryInfo);
            GetFiles(directoryInfo);            
        }

        private void TopDirInfo(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            TreeListFolderItem item;
            item = new TreeListFolderItem
            {
                Name = directoryInfo.Name,
                FullName = directoryInfo.FullName,
                Image = fileHelper.SetFolderImage(directoryInfo),                
                HierarchyLevel = -1,                
                Size = сounting.StringSize(0),
            };
            Task.Run(() => сounting.GetSize(item));
            treeListViewFolderItem.AddToFolder(item);
        }
        private void PreparingFoldersView(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            foreach (DirectoryInfo dirInfo in directoryInfo.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                TreeListFolderItem item;
                item = new TreeListFolderItem
                {
                    Name = dirInfo.Name,
                    Image = fileHelper.SetFolderImage(dirInfo),                                                           
                };
                treeListViewFolderItem.AddToFolder(item);
            }
        }

        private void SubfoldersInfo(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            try
            {                
                DirectoryInfo[] dirs = directoryInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
                Thread.Sleep(250);
                foreach (DirectoryInfo d in dirs)
                {
                    tasks.Add(Task.Run(() => FoldersData(d)));
                }
            }
            catch { }
        }

        private void FoldersData(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            TreeListFolderItem item;
            item = new TreeListFolderItem
            {
                Name = directoryInfo.Name,
                Size = сounting.StringSize(0),
                FullName = directoryInfo.FullName,                              
                Image = fileHelper.SetFolderImage(directoryInfo),                
            };

            if (directoryInfo.EnumerateFileSystemInfos("*",
                    new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = false }).Any())
            {
                item.Items = new ObservableCollection<TreeListFolderItem>()
                {
                    new TreeListFolderItem()
                };
            }

            Task.Run(() => сounting.GetSize(item));
            concurrentStackFolder.Push(treeListViewFolderItem);
            treeListViewFolderItem.ChangeItem(concurrentStackFolder, item);

        }

        private void GetFiles(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            TreeListFolderItem item;
            FileInfo[] fileInfo = directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo _fileInfo in fileInfo)
            {
                item = new TreeListFolderItem
                {
                    Name = _fileInfo.Name,
                    Size = сounting.StringSize(_fileInfo.Length),
                    FullName = _fileInfo.FullName,                                     
                    Image = fileHelper.SetFileImage(),                    
                };
                treeListViewFolderItem.AddToFolder(item);
            }
        }
    }
}
