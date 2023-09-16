using BusinessTier;
using Class_Library_Project;
using server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceModel;
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
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Drawing;

namespace AsyncClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;
        private string searchValue;
        public MainWindow()
        {
            InitializeComponent();
            ChannelFactory<BusinessServerInterface> foobFatory;
            NetTcpBinding tcp = new NetTcpBinding();
            tcp.MaxReceivedMessageSize = 2000000000;
            //Set the URL and create the connection
            string URL = "net.tcp://localhost:8200/DataService";
            foobFatory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = foobFatory.CreateChannel();

            //Also tell me how many entries are in the DB.
            totalNum_label.Text = "Total Items: " + foob.GetNumEntries().ToString();
        }

        private void go_btn_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            string fname = "", lname = "";
            int bal = 0;
            uint acct = 0, pin = 0;
            MemoryStream image = null;
            error_badge.Text = "";

            //On Click, get the index....
            try
            {
                index = Int32.Parse(index_box.Text);
                try
                {
                    foob.GetValuesForEntry(index, out acct, out pin, out bal, out fname, out lname, out image);
                }
                catch (FaultException ex)
                {
                    image_box.Source = null;
                    error_badge.Text = ex.Message;
                }
            }
            catch (FormatException ex)
            {
                image_box.Source = null;
                error_badge.Text = "Illegal Argument: " + ex.Message;
            }

            fName_box.Text = fname;
            lName_box.Text = lname;
            acctNo_box.Text = acct.ToString();
            pin_box.Text = pin.ToString("D4");
            balance_box.Text = bal.ToString("C");
            if (image != null)
            {
                image_box.Source = StreamToImageSource(image);
            }
        }

        private async void search_btn_Click(object sender, RoutedEventArgs e)
        {
            searchValue = search_box.Text;
            int timeout = 10;
            DataStruct person = null;
            //Task<DataStruct> task = new Task<DataStruct>(SearchDb);
            //task.Start();
            search_lbl.Content = "Searching Starts.............";
            Task<DataStruct> task = SearchWithTimeout();
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                person = await task;
                if (person != null)
                    UpdateGUI(person);
                else
                    UpdateGUI();
                    error_badge.Text = "No result found in database for entry: " + searchValue;
                search_lbl.Content = "Searching ends.............";
                Console.WriteLine("Task success");
            }
            else
            {
                UpdateGUI();
                search_lbl.Content = "Searching ends.............";
                error_badge.Text = "Error no response from server";
            }       
        }

        private void UpdateGUI(DataStruct person = null)
        {
            if(person == null)
            {
                fName_box.Text = "";
                lName_box.Text = "";
                acctNo_box.Text = "";
                pin_box.Text = "";
                balance_box.Text = "";
                image_box.Source = null;
            }
            else
            {
                fName_box.Text = person.firstName;
                lName_box.Text = person.lastName;
                acctNo_box.Text = person.acctNo.ToString();
                pin_box.Text = person.pin.ToString("D4");
                balance_box.Text = person.balance.ToString("C");
                var image = person.image;
                if (image != null)
                {
                    image_box.Source = BitmapToBitmapImage(image);
                }
            }          
        }

        private DataStruct SearchDb()
        {           
            return foob.Search(searchValue);
        }

        private async Task<DataStruct> SearchWithTimeout()
        {
            Task<DataStruct> task = new Task<DataStruct>(SearchDb);
            task.Start();
            return await task;
        }

        BitmapImage StreamToImageSource(MemoryStream memoryStream)
        {
            BitmapImage bmpImg = new BitmapImage();

            bmpImg.BeginInit();
            bmpImg.StreamSource = memoryStream;
            bmpImg.CacheOption = BitmapCacheOption.OnLoad;
            bmpImg.EndInit();

            return bmpImg;
        }

        BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Save the Bitmap to the MemoryStream in the desired format (e.g., PNG, JPEG)
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                // Create a new BitmapImage
                BitmapImage bitmapImage = new BitmapImage();

                // Set BitmapImage properties
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Optional: Freeze the BitmapImage for better performance

                return bitmapImage;
            }
        }

        private void index_box_GotFocus(object sender, RoutedEventArgs e)
        {
            index_box.Text = string.Empty;
        }

        private void search_box_GotFocus(object sender, RoutedEventArgs e)
        {
            search_box.Text = string.Empty;
        }
    }
}
