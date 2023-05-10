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
            //for (var i = 0; i < 5; i++) // The 5 here could be any number
            //{
            //StackPanel sp = new StackPanel
            //{
            //    Name = "myPane22l" ,
            //    Orientation = Orientation.Horizontal,
            //    Background  = new SolidColorBrush(Colors.Black),
            //};
            //myStackPanel.Children.Add(sp);

          var  Expander =  new Expander
            {
                Name = "expander",
                Header = "Слот оперативной память 1",
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(2),
            };

          var   textBlock = new TextBlock
            {
                Name = "myPanel",
                Text = "3123",
                Background = new SolidColorBrush(Colors.Black),
                
            };

            myStackPanel.Children.Add(Expander);
            Expander.Content = textBlock;
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
            myStackPanel.Children.Remove(Expander);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
