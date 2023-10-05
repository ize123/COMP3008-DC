using RestSharp;
using Newtonsoft.Json;
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
using ClassLibraryDLL;
using System.IO;
using System.Windows.Interop;
using System.Drawing;
using System.Reflection;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string URL;
        RestClient restClient;
        public MainWindow()
        {
            InitializeComponent();
            URL = "http://localhost:5091";
            restClient = new RestClient(URL);
            getNumEntries();
        }

        private void BoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            tb.Clear();
        }

        private async void search_btn_Click(object sender, RoutedEventArgs e)
        {
            //Make a search class
            SearchData mySearch = new SearchData();
            mySearch.searchStr = search_box.Text;
         
            var restResponse = await SearchPersonAsync(mySearch);

            if(restResponse.IsSuccessStatusCode)
            {
                // Deserialize the result
                DataIntermed person = JsonConvert.DeserializeObject<DataIntermed>(restResponse.Content);

                if (person != null)
                    UpdateGUI(person);
                UpdateErrorLable("");
            }
            else
            {
                ApiException apiException = JsonConvert.DeserializeObject<ApiException>(restResponse.Content);
                UpdateErrorLable(apiException.ErrorCode);
            }   
        }

        private async void go_btn_Click(object sender, RoutedEventArgs e)
        {
            int index = Int32.Parse(index_box.Text);

            RestResponse restResponse = await GetPersonAsync(index);
            // Handle the errors
            if(!restResponse.IsSuccessStatusCode)
            {
                ApiException apiException = JsonConvert.DeserializeObject<ApiException>(restResponse.Content);
                UpdateErrorLable(apiException.ErrorCode);
            }
            else
            {
                DataIntermed person = JsonConvert.DeserializeObject<DataIntermed>(restResponse.Content);

                if (person != null)
                    UpdateGUI(person);
                UpdateErrorLable("");
            }      
        }

        private async void getNumEntries()
        {

            var restResponse = await GetNumEntriesAsync();

            int number = JsonConvert.DeserializeObject<int>(restResponse.Content);
            totalNum_label.Text = "Total Items: " + number;
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

        private void UpdateErrorLable(string errorMessage)
        {
            error_badge.Text = errorMessage;
        }

        private async Task<RestResponse> GetPersonAsync(int index)
        {
            // Create a RestRequest with the URL
            var restRequest = new RestRequest("/getperson/" + index.ToString());

            // Perform the REST call asynchronously
            var restResponse = await restClient.GetAsync(restRequest);

            return restResponse;
        }

        private async Task<RestResponse> GetNumEntriesAsync()
        {
            // Create a RestRequest with the URL
            var restRequest = new RestRequest("/getvalues");

            // Perform the REST call asynchronously
            var restResponse = await restClient.GetAsync(restRequest);

            return restResponse;
        }

        private async Task<RestResponse> SearchPersonAsync(SearchData searchData)
        {
            // Create a RestRequest with the URL
            var restRequest = new RestRequest("/search", Method.Post);           

            restRequest.RequestFormat = RestSharp.DataFormat.Json;
            // Add the body to the request
            restRequest.AddBody(searchData);

            // Perform the REST call asynchronously
            var response = await restClient.PostAsync(restRequest);
            return response;
        }
    }
}
