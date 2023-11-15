using System;
using System.IO;
using System.Threading.Tasks;
using TreeSize.Models;

namespace TreeSize
{
    public class Counting
    {
        private EnumerationOptions option = new EnumerationOptions
        {
            AttributesToSkip = FileAttributes.System,
            RecurseSubdirectories = true
        };

        public Task GetSize(TreeListFolderItem folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }
            return Task.Run(() =>
            {
                if (Directory.Exists(folder.FullName))
                    try
                    {
                        long size = 0;
                        DirectoryInfo di = new DirectoryInfo(folder.FullName);
                        var fileInfoCollection = di.EnumerateFiles("*", option);
                        foreach (var fileInfo in fileInfoCollection)
                        {
                            size += fileInfo.Length;
                            folder.Size = StringSize(size);
                        }
                        if (!(folder.Size != null))
                        {
                            folder.Size = null;
                        }
                    }
                    catch (Exception)
                    {

                    }
            });
        }

        public string StringSize(long size)
        {
            int buteSize = 1024;

            if (size < buteSize)
            {
                return size + " byte";
            }
            if (size / buteSize < buteSize)
            {
                return Math.Round((decimal)size / buteSize, 1) + " Kb";
            }
            if (size / buteSize / buteSize < buteSize)
            {
                return Math.Round((decimal)size / buteSize / buteSize, 1) + " Mb";
            }
            else
            {
                return Math.Round((decimal)size / buteSize / buteSize / buteSize, 1) + " Gb";
            }
        }
    }
}
