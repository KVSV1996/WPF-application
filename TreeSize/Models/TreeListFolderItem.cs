using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TreeSize.Models
{
    public class TreeListFolderItem : INotifyPropertyChanged
    {
        protected ObservableCollection<TreeListFolderItem> _items;       

        public ObservableCollection<TreeListFolderItem> Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("Children"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        protected string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }          

        protected string fullname;
        public string FullName
        {
            get { return fullname; }
            set { fullname = value; OnPropertyChanged("FullName"); }
        }              

        protected int hierarchylevel;
        public int HierarchyLevel
        {
            get { return hierarchylevel; }
            set
            {
                SetMargin = (value * 15).ToString();
                hierarchylevel = value;
                OnPropertyChanged("HierarchyLevel");
            }
        }

        protected string size;
        public string Size
        {
            get { return size; }
            set { size = $"Size: {value}"; OnPropertyChanged("Size"); }
        }

        protected BitmapSource image;
        public BitmapSource Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged("Image"); }
        }

        protected string setmargin;
        public string SetMargin
        {
            get { return setmargin; }
            set
            {
                setmargin = $"{value}, 0, 0, 0";
                OnPropertyChanged("SetMargin");
            }
        }                     
    }
}
