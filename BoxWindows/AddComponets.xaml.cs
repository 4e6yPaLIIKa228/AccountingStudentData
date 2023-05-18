using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using AccountingStudentData.Connection;

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
                this.Height = 300;
            }
            else if (combtext == "Специальность")
            {
                StPnGrop.Visibility = Visibility.Collapsed;
                StPlSpeacial.Visibility = Visibility.Visible;
                this.Height = 500;
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
            CombKruterui.SelectedIndex = -1;
            TextComponet.Text = string.Empty;
        }

        public void AddComponent()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    String combtext = CombKruterui.Text;
                    if (combtext == "Группа")
                    {
                        if (String.IsNullOrEmpty(TextComponet.Text))
                        {
                            MessageBox.Show("Напшите названия критерия.");
                        }
                        else
                        {
                            string query = $@"SELECT count() Name from Groups where  Name = '{TextComponet.Text.ToUpper()}' ";
                            SQLiteCommand cmd = new SQLiteCommand(query, connection);
                            int ProverkaComponent = Convert.ToInt32(cmd.ExecuteScalar());
                            if (ProverkaComponent == 0)
                            {
                                query = $@"Insert Into Groups ('Name') Values ('{TextComponet.Text.ToUpper()}') ";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show($@"Группа: '{TextComponet.Text.ToUpper()}' доступна для использования.");
                            }
                            else
                            {

                                MessageBox.Show($@"Группа '{TextComponet.Text.ToUpper()}' уже использвутся.");
                            }
                            
                        }
                       
                    }
                    else if (combtext == "Специальность")
                    {
                        if (String.IsNullOrEmpty(NameSpecial.Text) || String.IsNullOrEmpty(KodSpecial.Text) || String.IsNullOrEmpty(KlassSpecial.Text))
                        {
                            MessageBox.Show("Заполните данные.");
                        }
                        else
                        {
                            string query = $@"SELECT count() Name,NumberSpecial,Class from Specialties where  Name = '{NameSpecial.Text.ToLower()}' and  NumberSpecial = '{KodSpecial.Text}' and  Class = '{KlassSpecial.Text}' ";
                            SQLiteCommand cmd = new SQLiteCommand(query, connection);
                            int ProverkaComponent = Convert.ToInt32(cmd.ExecuteScalar());
                            if (ProverkaComponent == 0)
                            {
                                query = $@"Insert Into Specialties ('Name','NumberSpecial','Class') Values ('{NameSpecial.Text.ToLower()}','{KodSpecial.Text}','{KlassSpecial.Text}') ";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show($@"Специальность  '{NameSpecial.Text.ToLower()}' c кодом '{KodSpecial.Text}' и классом '{KlassSpecial.Text}' доступлна для использования.");
                            }
                            else
                            {
                                MessageBox.Show($@"Специальность  '{NameSpecial.Text.ToLower()}' c кодом '{KodSpecial.Text}' и классом '{KlassSpecial.Text}' уже использвутся.");
                            }
                        }
                    }
                                        
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnAddcomponet_Click(object sender, RoutedEventArgs e)
        {
            AddComponent();
        }

        private void MnItClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }       

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch
            {

            }
        }
    }
}
