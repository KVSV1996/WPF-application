using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace TreeSize
{
    public class FileHelper
    {
        public BitmapSource SetFolderImage(DirectoryInfo dirInfo)
        {
            string image;

            if (dirInfo.FullName.Split('\\')[1] == "")
            {
                image = "Images/drive.png";
            }
            else if (dirInfo.Attributes.HasFlag(FileAttributes.Hidden))
            {
                image = "Images/hidden.png";
            }            
            else
            {
                image = "Images/folder.png";
            }

            BitmapImage _image = new BitmapImage(new Uri(image, UriKind.Relative));
            _image.Freeze();
            return _image;
        }

        public BitmapSource SetFileImage()
        {
            string image = "Images/file.png";
            BitmapImage _image = new BitmapImage(new Uri(image, UriKind.Relative));
            _image.Freeze();
            return _image;
        }
        
    }
}
