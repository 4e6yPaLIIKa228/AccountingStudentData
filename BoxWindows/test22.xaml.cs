using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace AccountingStudentData.BoxWindows
{
    /// <summary>
    /// Логика взаимодействия для test22.xaml
    /// </summary>
    public partial class test22 : Window
    {
        string GPSFailNew, GPSFailOld, NameDB;

        public test22()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();//(*.bmp, *.jpg)|*.bmp;*.jpg|Все файлы (*.*)|*.*""
            openFileDialog.Filter = "Text files (*.db)|*.db|All files (*.*)|*.*"; //"Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                GPSFailNew = Path.GetFullPath(openFileDialog.FileName);
                if (GPSFailNew == null)
                {
                    GPSFailOld = Path.GetFullPath(openFileDialog.FileName);
                }
                else
                {
                    GPSFailOld = GPSFailNew;
                }
                Saver.NameDB = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                MessageBox.Show(Saver.NameDB);
                MessageBox.Show(GPSFailOld);
            }

        }
    }
}
