using System.Globalization;
using TreeSize.Models;

namespace TreeSize.Test
{
    [TestClass]
    public class CountingTests
    {
        Counting counting = new Counting();

        [TestInitialize]
        public void SetEnglishCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
        [TestMethod]
        public async Task GetSizeTest()
        {
            SetEnglishCulture(); 

            TreeListFolderItem item;

            item = new TreeListFolderItem
            {                
                FullName = @".\Images"
            };

            await counting.GetSize(item);
            
            Assert.AreEqual(item.Size, "Size: 5.7 Kb");            
        }

        [DataRow(25, "25 byte")]
        [DataRow(2300, "2.2 Kb")]
        [DataRow(1856200, "1.8 Mb")]
        [DataRow(5481856200, "5.1 Gb")]
        [TestMethod]
        public void StringSizeTest(long longSize, string result)
        {      
            SetEnglishCulture();

            string stringSize = counting.StringSize(longSize);

            Assert.AreEqual(stringSize, result);
        }

    }
}