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

namespace WPF_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServerInterface foob;
        public MainWindow()
        {
            InitializeComponent();
            //This is a factory that generates remote connections to our remote class. This is what hides the RPC stuff!
            ChannelFactory<ServerInterface> foobFatory;
            NetTcpBinding tcp = new NetTcpBinding();
            tcp.MaxReceivedMessageSize = 2000000000;   

            //Set the URL and create the connection
            string URL = "net.tcp://localhost:8100/DataService";
            foobFatory = new ChannelFactory<ServerInterface>(tcp, URL);
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

        private void index_box_GotFocus(object sender, RoutedEventArgs e)
        {
            index_box.Text = "";
        }
    }
}
