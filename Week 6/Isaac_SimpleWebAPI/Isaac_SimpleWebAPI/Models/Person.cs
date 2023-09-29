using System.Drawing;

namespace Isaac_SimpleWebAPI.Models
{
    public class Person
    {
        public uint acctNo { get; set; }
        public uint pin { get; set; }
        public int balance { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public Bitmap? image { get; set; }


        public Person()
        {
            acctNo = 0;
            pin = 0;
            balance = 0;
            firstName = "";
            lastName = "";
            image = null;
        }

        public Person(uint acctNo, uint pin, int balance, string firstName, string lastName, Bitmap image)
        {
            this.acctNo = acctNo;
            this.pin = pin;
            this.balance = balance;
            this.firstName = firstName;
            this.lastName = lastName;
            this.image = image;
        }
    }
}
