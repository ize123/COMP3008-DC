using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestSharp;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using System.Windows.Interop;
using Class_Library;

namespace WPF_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String URL;
        RestClient restClient;
        public MainWindow()
        {
            InitializeComponent();
            URL = "http://localhost:44671";
            restClient = new RestClient(URL);
        }

        private void go_btn_Click(object sender, RoutedEventArgs e)
        {
            int index = Int32.Parse(index_box.Text);
           
            RestRequest restRequest = new RestRequest("/person/getperson/" + index.ToString());
            RestResponse restResponse = restClient.Get(restRequest);

            Class_Library.DataIntermed person = JsonConvert.DeserializeObject<Class_Library.DataIntermed>(restResponse.Content);

            if(person != null)
                UpdateGUI(person);
        }

        private void search_btn_Click(object sender, RoutedEventArgs e)
        {
            // Build request with the json in the body
            RestRequest request = new RestRequest("/person/searchperson", Method.Post);

            //Make a search class
            SearchData mySearch = new SearchData();
            mySearch.searchStr = search_box.Text;

            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddBody(mySearch);

            // Do the request
            RestResponse response = restClient.Post(request);
            // Deserialize the result
            Class_Library.DataIntermed person = JsonConvert.DeserializeObject<Class_Library.DataIntermed>(response.Content);

            if (person != null)
                UpdateGUI(person);
        }

        private void BoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            tb.Clear();
        }

        private void UpdateGUI(DataIntermed person)
        {
            //Set the values in the GUI
            fName_box.Text = person.firstName;
            lName_box.Text = person.lastName;
            acctNo_box.Text = person.acctNo.ToString();
            pin_box.Text = person.pin.ToString("D4");
            balance_box.Text = person.balance.ToString("C");

            // Get the bitmap image out of the base64 string
            byte[] imageBytes = Convert.FromBase64String(person.image);

            // Create a MemoryStream from the byte array
            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                // Create a Bitmap from the MemoryStream
                Bitmap bitmap = new Bitmap(memoryStream);
                BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
                );

                image_box.Source = bitmapSource;
            }
        }
    }
}
