namespace SynchronousDemo
{
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DownloadImage(this.Image1, "https://icatcare.org/app/uploads/2018/07/Thinking-of-getting-a-cat.png");
            this.DownloadImage(this.Image2, "https://icatcare.org/app/uploads/2018/06/Layer-1704-1920x840.jpg");
            this.DownloadImage(this.Image3, "https://i.cbc.ca/1.5256404.1566499707!/fileImage/httpImage/cat-behaviour.jpg");
            this.DownloadImage(this.Image4, "https://www.advantagepetcare.com.au/sites/g/files/adhwdz311/files/styles/paragraph_image/public/2019-08/cat-eating-out-of-bowl.jpg");
            this.DownloadImage(this.Image5, "https://www.algonquinhotel.com/wp-content/uploads/sites/21/2019/07/the-algonquin-hotel-autograph-collection-cat-02-1440x900.jpg");
        }

        private void DownloadImage(Image image, string url)
        {
            var client = new HttpClient();
            Thread.Sleep(2000);
            var response = client.GetAsync(url).GetAwaiter().GetResult();
            var byteData = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
            image.Source = this.LoadImage(byteData);
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            var image = new BitmapImage();
            using (var memoryStream = new MemoryStream(imageData))
            {
                memoryStream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = memoryStream;
                image.EndInit();
            }

            image.Freeze();
            return image;
        }
    }
}
