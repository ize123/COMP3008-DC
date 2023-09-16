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

using System.ServiceModel;
using Server;
using System.Security.Principal;
using System.Runtime.Serialization.Formatters;
using server;
using System.Drawing;
using System.IO;
using BusinessTier;
using Class_Library_Project;
using System.Windows.Interop;
using System.Runtime.Remoting.Messaging;

namespace WPF_Client
{
    public delegate DataStruct SearchOp(string value);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;


        public MainWindow()
        {
            InitializeComponent();            

            //This is a factory that generates remote connections to our remote class. This is what hides the RPC stuff!
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
                catch (FaultException<ServerError> ex)
                {
                    image_box.Source = null;
                    error_badge.Text = ex.Detail.ProblemType;
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
            if(image != null)
            {
                image_box.Source = StreamToImageSource(image);
            }         
        }

        //Bitmap
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
            index_box.Text = "";
        }

        private void search_btn_Click(object sender, RoutedEventArgs e)
        {
            string searchString = "";
            DataStruct student = new DataStruct();
            SearchOp searchOp;
            AsyncCallback callBackSearch;

            try
            {
                searchString = search_box.Text;
                try
                {
                    searchOp = foob.Search;
                    callBackSearch = this.OnSearchComplete;
                    searchOp.BeginInvoke(searchString, callBackSearch, null);
                }
                catch (FaultException<ServerError> ex)
                {
                    image_box.Source = null;
                    error_badge.Text = ex.Detail.ProblemType;
                }
            }
            catch (FormatException ex)
            {
                image_box.Source = null;
                error_badge.Text = "Illegal Argument: " + ex.Message;
            }
        }

        private void search_box_GotFocus(object sender, RoutedEventArgs e)
        {
            search_box.Text = "";
        }

        private void OnSearchComplete(IAsyncResult asyncResult)
        {
            DataStruct iSearchResult;
            SearchOp searchOp;
            AsyncResult asyncObj = (AsyncResult)asyncResult;

            if(asyncObj.EndInvokeCalled == false) 
            {
                searchOp = (SearchOp)asyncObj.AsyncDelegate;

                //What to do when the task finishes
                iSearchResult = searchOp.EndInvoke(asyncObj);

                fName_box.Text = iSearchResult.firstName;
                lName_box.Text = iSearchResult.lastName;
                acctNo_box.Text = iSearchResult.acctNo.ToString();
                pin_box.Text = iSearchResult.pin.ToString("D4");
                balance_box.Text = iSearchResult.balance.ToString("C");
                if (iSearchResult.image != null)
                {
                    image_box.Source = BitmapToBitmapImage(iSearchResult.image);
                }
            }
            asyncObj.AsyncWaitHandle.Close();
        }
    }
}
