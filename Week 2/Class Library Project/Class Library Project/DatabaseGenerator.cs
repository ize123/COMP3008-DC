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

namespace Class_Library_Project
{
    internal class DatabaseGenerator
    {
        Random random = new Random();
        private string GetFirstName()
        {
            string[] firstNames = { "Nicole","Mark","Treyvon","Kaliyah","Javion","Kerry","Marissa","Keven","Chase","Keara",
                "Andie","Sydnee","Kristen","Payton","Maura","Monet","Eric","Rex","Kareem","Abram",
                "Grace","Anissa","Rowan","Caitlyn","Isiah","Slade","Frida","Brandy","Kale","Nevin",
                "Valentin","Levi","Azaria","Eleni","Geneva","Genaro","Devontae","Odalis","Rayshawn","Ezra",
                "Loren","Ramsey","Nancy","Ashlin","Danielle","Donnie","Marlee","Kameron","Ayleen","Waylon" };
            int val = random.Next(firstNames.Length);
            return firstNames[val];
        }
        private string GetLastName()
        {
            string[] lastNames = { "Barragan", "Duggan", "Peacock", "Maples", "Kaplan", "McCormick", "Frederick", "Serrano", "Alcantar", "Lehman",
                "Corcoran", "Laird", "Hermann", "Blevins", "Ashford", "Holton", "Stevenson", "Dwyer", "Whaley", 
                "Corrigan", "Linder", "Valentin", "Butler", "Wicks", "Meehan", "Stephens", "Reinhart", "Pickering", "Cronin",
                "Dent", "Langston", "Fair", "Gold", "Bales", "Do", "Muller", "Allred", "Hilliard", "Paquette", 
                "McClelland", "Turpin", "Boothe", "Hightower", "Koch", "Schulte", "Isom", "Lindquist", "Xu", "Jeffers", "Orr" };
            int val = random.Next(lastNames.Length);
            return lastNames[val];
        }
        private uint GetPin()
        {
            uint val =  (uint)random.Next(0, 9999);
            return val;
        }
        private uint GetAcctNo()
        {
            uint val = (uint)(random.Next(10000, 100000));
            return val;
        }
        private int GetBalance()
        {
            int val = random.Next(1, 1000000);
            return val;
        }

        private Bitmap GetImage()
        {
            Bitmap bmp1 = ConvertToBitmap("C:\\Users\\isaac\\Desktop\\Hard Drive\\Uni Year 2 Sem 2\\DC\\Week 2\\Class Library Project\\Class Library Project\\Res\\sponge1.jpg");
            Bitmap bmp3 = ConvertToBitmap("C:\\Users\\isaac\\Desktop\\Hard Drive\\Uni Year 2 Sem 2\\DC\\Week 2\\Class Library Project\\Class Library Project\\Res\\sponge2.jpg");
            Bitmap bmp4 = ConvertToBitmap("C:\\Users\\isaac\\Desktop\\Hard Drive\\Uni Year 2 Sem 2\\DC\\Week 2\\Class Library Project\\Class Library Project\\Res\\sponge3.jpg");
            Bitmap bmp2 = ConvertToBitmap("C:\\Users\\isaac\\Desktop\\Hard Drive\\Uni Year 2 Sem 2\\DC\\Week 2\\Class Library Project\\Class Library Project\\Res\\sponge4.jpg");
            Bitmap bmp5 = ConvertToBitmap("C:\\Users\\isaac\\Desktop\\Hard Drive\\Uni Year 2 Sem 2\\DC\\Week 2\\Class Library Project\\Class Library Project\\Res\\sponge5.jpg");
            Bitmap bmp6 = ConvertToBitmap("C:\\Users\\isaac\\Desktop\\Hard Drive\\Uni Year 2 Sem 2\\DC\\Week 2\\Class Library Project\\Class Library Project\\Res\\sponge6.jpg");
            Bitmap bmp7 = ConvertToBitmap("C:\\Users\\isaac\\Desktop\\Hard Drive\\Uni Year 2 Sem 2\\DC\\Week 2\\Class Library Project\\Class Library Project\\Res\\sponge7.jpg");
            Bitmap bmp8 = ConvertToBitmap("C:\\Users\\isaac\\Desktop\\Hard Drive\\Uni Year 2 Sem 2\\DC\\Week 2\\Class Library Project\\Class Library Project\\Res\\sponge8.jpg");
            Bitmap bmp9 = ConvertToBitmap("C:\\Users\\isaac\\Desktop\\Hard Drive\\Uni Year 2 Sem 2\\DC\\Week 2\\Class Library Project\\Class Library Project\\Res\\sponge9.jpg");

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

        public Bitmap ConvertToBitmap(String filename)
        {
            Bitmap bm = null;
            using(Stream bmpStream = System.IO.File.Open(filename, System.IO.FileMode.Open))
            {
                Image img = Image.FromStream(bmpStream);
                bm = new Bitmap(img);
            }
            return bm;
        }

        public void GetNextAccount(out uint pin, out uint AcctNo, out string FirstName, out string LastName, out int balance, out Bitmap img)
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
