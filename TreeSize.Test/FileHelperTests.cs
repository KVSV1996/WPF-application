namespace TreeSize.Test
{
    [TestClass]
    public class FileHelperTests
    {
        FileHelper fileHelper = new FileHelper();

        [DataRow(@".\Models")]
        [DataRow(@".\.vs")]
        [DataRow(@"C:\")]
        [TestMethod]
        public void SetFolderImageTest(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            var image = fileHelper.SetFolderImage(directoryInfo);

            Assert.AreEqual(image.PixelWidth, 32);
        }

        [TestMethod]
        public void SetFileImageTests()
        {
            var image = fileHelper.SetFileImage();

            Assert.AreEqual(image.PixelWidth, 32);
        }
    }
}
