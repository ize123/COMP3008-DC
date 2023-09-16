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

using ConsoleApp1;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int value = Int32.Parse(student_id.Text);
                List<Student> studentlist = StudentList.Students();

                first_name.Text = "none";
                uni_name.Text = "none";

                foreach (Student student in studentlist)
                {
                    if (student.Id == value)
                    {
                        first_name.Text = student.Name;
                        uni_name.Text = student.University;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            

            
           
        }

        private void student_id_GotFocus(object sender, RoutedEventArgs e)
        {
            student_id.Clear();
        }
    }
}
