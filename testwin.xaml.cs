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
using System.Windows.Shapes;

namespace AccountingStudentData
{
    /// <summary>
    /// Логика взаимодействия для testwin.xaml
    /// </summary>
    public partial class testwin : Window
    {
        public testwin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var Expander = new Expander
            {
                Name = "expander",
                Header = "Данные родителей",
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(2),
                Height = 200,
            };

            var StackPanel1 = new StackPanel
            {
                Orientation = Orientation.Vertical,
            };
            var StackPanel2 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };
            var StackPanel3 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };
            var StackPanel4 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };
            var StackPanel5 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };
            var textBlock2_1 = new TextBlock
            {
                Text = "Фамилия",
            };

            var textBox2_1 = new TextBox
            {
                Name = "Surname",
                Width= 250,                
            };
            var textBlock3_2 = new TextBlock
            {
                Text = "Имя",
            };

            var textBox3_2 = new TextBox
            {
                Name = "Name",
                Width = 250,
            };

            var textBlock4_3 = new TextBlock
            {
                Text = "Отчество",
            };

            var textBox4_3 = new TextBox
            {
                Name = "FirstName",
                Width = 250,
            };
            var textBlock5_1 = new TextBlock
            {
                Text = "Родственник",
            };

            var cobbox5_2 = new ComboBox
            {
                Name = "TypeRod",
                
            };
            var textBlockcmb1 = new TextBlock
            {
                Text = "Мать",
            };
            var textBlockcmb2 = new TextBlock
            {
                Text = "Отец",
            };

            var textBox4_32 = new TextBox
            {
                Name = "FirstName",
                Width = 250,
            };

            myStackPanel.Children.Add(Expander);
            Expander.Content = StackPanel1;
            StackPanel1.Children.Add(StackPanel2);
            StackPanel1.Children.Add(StackPanel3);
            StackPanel1.Children.Add(StackPanel4);

            cobbox5_2.ItemsSource = new object[]
            {
                "Мать","Отец","Бабушка"
            };
            StackPanel2.Children.Add(textBlock2_1);
            StackPanel2.Children.Add(textBox2_1);
            StackPanel3.Children.Add(textBlock3_2);
            StackPanel3.Children.Add(textBox3_2);
            StackPanel4.Children.Add(textBlock4_3);
            StackPanel4.Children.Add(textBox4_3);


            // Expander.Content = textBlock;
            //for (var j = 0; j < 10; j++) // The 10 here could be any number
            //{
            //    sp.Children.Add(new Rectangle
            //    {
            //        Name = "myRec" + i + j,
            //        Fill = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString($@"#89000000")),
            //        Width = 20,
            //        Height = 20,
            //    });
            //}
            // }
           /* // myStackPanel.Children.Remove(Expander);// Удаление*/
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
        }

        private void NumberValidationNumberPassport(object sender, TextCompositionEventArgs e)
        {

        }

        private void TextValidationTextBox(object sender, KeyEventArgs e)
        {

        }

        private void TextValidationTextBox(object sender, TextCompositionEventArgs e)
        {

        }

        private void NumberValidationNumberDate(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
