using AccountingStudentData.Connection;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace AccountingStudentData.BoxWindows
{
    /// <summary>
    /// Логика взаимодействия для RegistrUser.xaml
    /// </summary>
    public partial class RegistrUser : Window
    {
        int CheckeLogin = 0, IDProverka = 0;
        public RegistrUser()
        {
            InitializeComponent();
            CombBoxDowmload();
        }

        public void CombBoxDowmload()  //Данные для комбобоксов 
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT * FROM AllowanceUsers";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable("AllowanceUsers");
                    SDA.Fill(dt);
                    CombAllowance.ItemsSource = dt.DefaultView;
                    CombAllowance.DisplayMemberPath = "Allowance";
                    CombAllowance.SelectedValuePath = "ID";

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            //AddProverka();
            AddUsers();
        } //Кнопка добавления пользователя

        public void CheckerText()
        {
            SimpleComand.CheckTextBox(TextBoxLogin);
            SimpleComand.CheckPassBox(PassBox);
            SimpleComand.CheckTextBox(TextFamili);
            SimpleComand.CheckTextBox(TextName);
            SimpleComand.CheckComboBox(CombAllowance);
        } //Проверка пустых строк(подсветка)

        private void TextBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            SimpleComand.CheckTextBox(TextBoxLogin);
        }

        private void PassBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SimpleComand.CheckPassBox(PassBox);
        }

        private void TextFamili_TextChanged(object sender, TextChangedEventArgs e)
        {
            SimpleComand.CheckTextBox(TextFamili);
        }

        private void TextName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SimpleComand.CheckTextBox(TextName);
        }

        private void CombAllowance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SimpleComand.CheckComboBox(CombAllowance);
        }

        public void AddUsers()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {

                    if (String.IsNullOrEmpty(TextBoxLogin.Text) || String.IsNullOrEmpty(PassBox.Password) || String.IsNullOrEmpty(TextFamili.Text) || String.IsNullOrEmpty(TextName.Text) || String.IsNullOrEmpty(CombAllowance.Text))
                    {
                        CheckerText();
                        MessageBox.Show("Заполните обязательные поля ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (MessageBox.Show("Вы уверены, что хотите добавить пользователя?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            CheckerLogin();
                            if (CheckeLogin == 0)
                            {
                                connection.Open();
                                var Login = TextBoxLogin.Text.ToLower();
                                var Pass = SimpleComand.GetHash(PassBox.Password);
                                //var Pass = SimpleComand.GetHash(txtpassreg.Password);
                                var DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                                bool resultType = int.TryParse(CombAllowance.SelectedValue.ToString(), out int IDCombAllowance);
                                string query = $@"INSERT INTO Users ('Login','Password','Surname',Name,MidleName,IDStatus,IDAllowance,DataRegist,IsDelet) VALUES ('{Login}',@Password,'{TextFamili.Text}','{TextName.Text}','{TextOthectbo.Text}','{1}','{IDCombAllowance}','{DateNow}',0)";
                                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                                cmd.Parameters.AddWithValue("@Password", Pass);
                                cmd.ExecuteScalar();
                                query = $@"SELECT ID FROM Users WHERE Login='{Login}';";
                                cmd = new SQLiteCommand(query, connection);
                                int IDProverka = Convert.ToInt32(cmd.ExecuteScalar());
                                AddProverka();
                                query = $@"UPDATE Users SET IDProverka = '{IDProverka}' WHERE Login= '{Login}'";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.ExecuteScalar();
                                connection.Close();
                                MessageBox.Show("Пользователь добавлен ", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Такой логин занят ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                connection.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        } //Функция добавления пользователя

        public void AddProverka()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    string query = $@"INSERT INTO Proverka ('TimeBegin','TimeEnd','AttemptNumber') VALUES ('00:00','00:00','{0}')";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    query = $@"SELECT ID FROM Proverka ORDER BY ID DESC;";
                    cmd = new SQLiteCommand(query, connection);
                    IDProverka = Convert.ToInt32(cmd.ExecuteScalar());
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        }


        public void CheckerLogin()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    var Login = TextBoxLogin.Text.ToLower();
                    string query = $@"SELECT  COUNT(1) FROM Users WHERE Login='{Login}'";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    CheckeLogin = Convert.ToInt32(cmd.ExecuteScalar());
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        } //Проверка на повторый логин

        private void InHome_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } //Кнопка назад

        private void BtnResize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnSazeMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MnItClose_Click(object sender, RoutedEventArgs e)
        {

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

