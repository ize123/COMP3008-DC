using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Tut_6.Models
{
    internal class DatabaseGenerator
    {
        public static Random random = new Random();
        public static string[] firstNames = { "Nicole","Mark","Treyvon","Kaliyah","Javion","Kerry","Marissa","Keven","Chase","Keara",
                "Andie","Sydnee","Kristen","Payton","Maura","Monet","Eric","Rex","Kareem","Abram",
                "Grace","Anissa","Rowan","Caitlyn","Isiah","Slade","Frida","Brandy","Kale","Nevin",
                "Valentin","Levi","Azaria","Eleni","Geneva","Genaro","Devontae","Odalis","Rayshawn","Ezra",
                "Loren","Ramsey","Nancy","Ashlin","Danielle","Donnie","Marlee","Kameron","Ayleen","Waylon" };
        public static string[] lastNames = { "Barragan", "Duggan", "Peacock", "Maples", "Kaplan", "McCormick", "Frederick", "Serrano", "Alcantar", "Lehman",
                "Corcoran", "Laird", "Hermann", "Blevins", "Ashford", "Holton", "Stevenson", "Dwyer", "Whaley",
                "Corrigan", "Linder", "Valentin", "Butler", "Wicks", "Meehan", "Stephens", "Reinhart", "Pickering", "Cronin",
                "Dent", "Langston", "Fair", "Gold", "Bales", "Do", "Muller", "Allred", "Hilliard", "Paquette",
                "McClelland", "Turpin", "Boothe", "Hightower", "Koch", "Schulte", "Isom", "Lindquist", "Xu", "Jeffers", "Orr" };
        public static Bitmap bmp1 = ConvertToBitmap("C:\\Users\\isaac\\OneDrive\\Documents\\UNI\\Uni Year 2 Sem 2\\DC - COMP3008\\Week 4 - Async Communication\\Class Library Project\\Class Library Project\\Res\\sponge1.jpg");
        public static Bitmap bmp3 = ConvertToBitmap("C:\\Users\\isaac\\OneDrive\\Documents\\UNI\\Uni Year 2 Sem 2\\DC - COMP3008\\Week 4 - Async Communication\\Class Library Project\\Class Library Project\\Res\\sponge2.jpg");
        public static Bitmap bmp4 = ConvertToBitmap("C:\\Users\\isaac\\OneDrive\\Documents\\UNI\\Uni Year 2 Sem 2\\DC - COMP3008\\Week 4 - Async Communication\\Class Library Project\\Class Library Project\\Res\\sponge3.jpg");
        public static Bitmap bmp2 = ConvertToBitmap("C:\\Users\\isaac\\OneDrive\\Documents\\UNI\\Uni Year 2 Sem 2\\DC - COMP3008\\Week 4 - Async Communication\\Class Library Project\\Class Library Project\\Res\\sponge4.jpg");
        public static Bitmap bmp5 = ConvertToBitmap("C:\\Users\\isaac\\OneDrive\\Documents\\UNI\\Uni Year 2 Sem 2\\DC - COMP3008\\Week 4 - Async Communication\\Class Library Project\\Class Library Project\\Res\\sponge5.jpg");
        public static Bitmap bmp6 = ConvertToBitmap("C:\\Users\\isaac\\OneDrive\\Documents\\UNI\\Uni Year 2 Sem 2\\DC - COMP3008\\Week 4 - Async Communication\\Class Library Project\\Class Library Project\\Res\\sponge6.jpg");
        public static Bitmap bmp7 = ConvertToBitmap("C:\\Users\\isaac\\OneDrive\\Documents\\UNI\\Uni Year 2 Sem 2\\DC - COMP3008\\Week 4 - Async Communication\\Class Library Project\\Class Library Project\\Res\\sponge7.jpg");
        public static Bitmap bmp8 = ConvertToBitmap("C:\\Users\\isaac\\OneDrive\\Documents\\UNI\\Uni Year 2 Sem 2\\DC - COMP3008\\Week 4 - Async Communication\\Class Library Project\\Class Library Project\\Res\\sponge8.jpg");
        public static Bitmap bmp9 = ConvertToBitmap("C:\\Users\\isaac\\OneDrive\\Documents\\UNI\\Uni Year 2 Sem 2\\DC - COMP3008\\Week 4 - Async Communication\\Class Library Project\\Class Library Project\\Res\\sponge9.jpg");
        private static string GetFirstName()
        {           
            int val = random.Next(firstNames.Length);
            return firstNames[val];
        }
        private static string GetLastName()
        {           
            int val = random.Next(lastNames.Length);
            return lastNames[val];
        }
        private static uint GetPin()
        {
            uint val =  (uint)random.Next(0, 9999);
            return val;
        }
        private static uint GetAcctNo()
        {
            uint val = (uint)(random.Next(10000, 100000));
            return val;
        }
        private static int GetBalance()
        {
            int val = random.Next(1, 1000000);
            return val;
        }

        private static Bitmap GetImage()
        {
            int choice = random.Next(0, 9);
            switch (choice)
            {
                case 0:
                    return bmp1;
                case 1:
                    return bmp2;
                case 2:
                    return bmp3;
                case 3:
                    return bmp4;
                case 4:
                    return bmp5;
                case 5:
                    return bmp6;
                case 6:
                    return bmp7;
                case 7:
                    return bmp8;
                case 8:
                    return bmp9;
                default:
                    return bmp1;
            }
        }

        public static Bitmap ConvertToBitmap(String filename)
        {
            Bitmap bm = null;
            using(Stream bmpStream = System.IO.File.Open(filename, System.IO.FileMode.Open))
            {
                Image img = Image.FromStream(bmpStream);
                bm = new Bitmap(img);
            }
            return bm;
        }

        public static void GetNextAccount(out uint pin, out uint AcctNo, out string FirstName, out string LastName, out int balance, out Bitmap img)
        {
            pin = GetPin();
            AcctNo = GetAcctNo();
            FirstName = GetFirstName();
            LastName = GetLastName();
            balance = GetBalance();
            img = GetImage();
        }
    }
}
