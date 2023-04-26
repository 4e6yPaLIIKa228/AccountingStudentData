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

namespace AccountingStudentData.BoxWindows
{
    /// <summary>
    /// Логика взаимодействия для AddComponets.xaml
    /// </summary>
    public partial class AddComponets : Window
    {
        //int checkopen1 = 0;
        public AddComponets()
        {
            InitializeComponent();
            //checkopen1 = checkopen;
        }

        private void CombSearchInfo_DropDownClosed(object sender, EventArgs e)
        {
            String combtext = CombKruterui.Text;
            if (combtext == "Группа")
            {
                StPnGrop.Visibility = Visibility.Visible;
                StPlSpeacial.Visibility = Visibility.Collapsed;
            }
            else if (combtext == "Специальность")
            {
                StPnGrop.Visibility = Visibility.Collapsed;
                StPlSpeacial.Visibility = Visibility.Visible;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            StudentBase stbs = new StudentBase();
            stbs.IsEnabled = true;
            this.Close();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddcomponet_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MnItClose_Click(object sender, RoutedEventArgs e)
        {
            //checkopen1 = 1;
            //StudentBase stbs = new StudentBase();
            //stbs.Activate();
            //stbs.IsEnabled = true;
            this.Close();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CombKruterui_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextComponet_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
