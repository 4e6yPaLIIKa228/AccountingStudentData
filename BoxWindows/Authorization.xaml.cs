using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AccountingStudentData.Connection;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace AccountingStudentData.BoxWindows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        public void CheckerText()
        {
            SimpleComand.CheckTextBox(TextBoxLogin);
            SimpleComand.CheckPassBox(PassBox);
        } //Проверка пустых строк(подсветка)

        public void AuthorizationUser()
        {
            try
            {
               // SelectDb();
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {

                    if (String.IsNullOrEmpty(TextBoxLogin.Text) || String.IsNullOrEmpty(PassBox.Password))
                    {
                        CheckerText();
                        MessageBox.Show("Заполните обязательные поля ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {

                        connection.Open();
                        var Pass = SimpleComand.GetHash(PassBox.Password);
                        string LoginLower = TextBoxLogin.Text.ToLower();
                        string query = $@"SELECT  COUNT(1) FROM Users WHERE Login='{LoginLower}'";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        int UsersSearch = Convert.ToInt32(cmd.ExecuteScalar());
                        if (UsersSearch == 1)
                        {
                            query = $@"SELECT  COUNT(1) FROM Users WHERE Login='{LoginLower}' AND Password = @Password and IDStatus != 3";
                            cmd = new SQLiteCommand(query, connection);
                            cmd.Parameters.AddWithValue("@Password", Pass);
                            int UsersFound = Convert.ToInt32(cmd.ExecuteScalar());
                            if (UsersFound == 1)
                            {
                                query = $@"SELECT  COUNT(1) FROM Users WHERE Login='{LoginLower}' AND Password = @Password and IDStatus = 4";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.Parameters.AddWithValue("@Password", Pass);
                                int UsersFoundBan = Convert.ToInt32(cmd.ExecuteScalar());
                                if (UsersFoundBan == 0)
                                {
                                    query = $@"SELECT  COUNT(1) FROM Users WHERE Login='{LoginLower}' AND Password = @Password and IDStatus = 2";
                                    cmd = new SQLiteCommand(query, connection);
                                    cmd.Parameters.AddWithValue("@Password", Pass);
                                    int UsersFoundBanTime = Convert.ToInt32(cmd.ExecuteScalar());
                                    if (UsersFoundBanTime == 0)
                                    {

                                        query = $@"SELECT Users.ID,Users.IDAllowance, AllowanceUsers.Allowance FROM Users
                                        join AllowanceUsers on Users.IDAllowance = AllowanceUsers.ID 
                                        WHERE Login= '{LoginLower}'";
                                        Saver.LoginUser = LoginLower;
                                        SQLiteDataReader dr = null;
                                        SQLiteCommand cmd1 = new SQLiteCommand(query, connection);
                                        string IDAllowanceString = null;
                                        dr = cmd1.ExecuteReader();
                                        while (dr.Read())
                                        {
                                            Saver.IDUser = dr["ID"].ToString();
                                            IDAllowanceString = dr["Allowance"].ToString();
                                        }

                                        if (IDAllowanceString == "Администратор" || IDAllowanceString == "администратор")
                                        {
                                            MessageBox.Show("Добро пожаловать!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                                            Saver.IDAllowanceString = IDAllowanceString;
                                            UsersBase admpnl = new UsersBase();
                                            this.Close();
                                            admpnl.ShowDialog();
                                            connection.Close();
                                        }
                                        else if (IDAllowanceString == "Преподаватель" || IDAllowanceString == "преподаватель")
                                        {
                                            Saver.Visitor = 0;
                                            MessageBox.Show("Добро пожаловать!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                                            Saver.IDAllowanceString = IDAllowanceString;
                                            StudentBase menuinfor = new StudentBase();
                                            this.Close();
                                            menuinfor.ShowDialog();
                                            connection.Close();
                                        }                                       
                                    }
                                    else
                                    {
                                        MessageBox.Show("Ваш аккаунт неактивен, обратитесь к администратору системы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Ваш аккаунт заблокирован", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                }

                                connection.Close();
                            }
                            else
                            {
                                NotInvalidPass();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неверный пароль или логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        } //Функция авторизации пользователя 

        public void NotInvalidPass()
        {

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    var Pass = SimpleComand.GetHash(PassBox.Password);
                    string LoginLower = TextBoxLogin.Text.ToLower();
                    string query = $@"SELECT  COUNT(1) FROM Users WHERE Login = '{LoginLower}' and Password != @Password";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Password", Pass);
                    int proverkaInvalidPass = Convert.ToInt32(cmd.ExecuteScalar());
                    query = $@"SELECT  COUNT(1) FROM Users WHERE Login='{LoginLower}' AND Password = @Password and IDStatus = 3";
                    cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Password", Pass);
                    int UsersUnBlock = Convert.ToInt32(cmd.ExecuteScalar());
                    if (proverkaInvalidPass == 1)
                    {
                        query = $@"SELECT IDAllowance,IDProverka FROM Users WHERE Login= '{LoginLower}'";
                        Saver.LoginUser = LoginLower;
                        SQLiteDataReader dr = null;
                        SQLiteCommand cmd1 = new SQLiteCommand(query, connection);
                        int IDProverka = 0;
                        int IDAllowance = 0;
                        dr = cmd1.ExecuteReader();
                        while (dr.Read())
                        {
                            IDProverka = Convert.ToInt32(dr["IDProverka"].ToString());
                            IDAllowance = Convert.ToInt32(dr["IDAllowance"].ToString());
                        }
                        query = $@"SELECT AttemptNumber,TimeEnd,TimeBegin FROM Proverka WHERE ID = '{IDProverka}';";
                        cmd = new SQLiteCommand(query, connection);
                        string dateOpen = DateTime.Now.ToString("t");
                        TimeSpan s2 = TimeSpan.Parse(dateOpen);
                        dr = null;
                        string AttemptNumber = "";
                        string dateban = "00:00";
                        string dateBegin = "00:00";
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            AttemptNumber = dr["AttemptNumber"].ToString();
                            dateban = dr["TimeEnd"].ToString();
                            dateBegin = dr["TimeBegin"].ToString();

                        }
                        TimeSpan sban = TimeSpan.Parse(dateban);
                        TimeSpan bban = TimeSpan.Parse(dateBegin);
                        if (Convert.ToInt32(AttemptNumber) < 3)
                        {

                            int kolint = Convert.ToInt32(AttemptNumber);
                            kolint++;
                            TimeSpan s1 = TimeSpan.Parse("0:01");
                            TimeSpan s3 = s1 + s2;
                            string times3 = s3.ToString("hh':'mm");
                            query = $@"UPDATE Proverka SET AttemptNumber='{kolint}',TimeBegin='{dateOpen}',TimeEnd ='{times3}' WHERE ID ='{IDProverka}';";
                            cmd = new SQLiteCommand(query, connection);
                            cmd.ExecuteReader();
                            MessageBox.Show("Неверный пароль или логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                        else if (Convert.ToInt32(AttemptNumber) == 3)
                        {
                            if (Convert.ToInt32(AttemptNumber) == 3 && (TimeSpan.Parse(dateOpen) < TimeSpan.Parse(dateban)))
                            {
                                query = $@"UPDATE Users SET IDStatus='{3}' WHERE Login='{LoginLower}';";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.ExecuteReader();
                                MessageBox.Show("Ваша учетная запись временно заблокированна,попробуйте позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else if (Convert.ToInt32(AttemptNumber) == 3 && (TimeSpan.Parse(dateOpen) >= TimeSpan.Parse(dateban)))
                            {
                                query = $@"UPDATE Proverka SET AttemptNumber = '{0}',TimeBegin = '00:00',TimeEnd = '00:00' WHERE ID ='{IDProverka}';";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.ExecuteReader();
                                query = $@"UPDATE Users SET IDStatus='{1}' WHERE Login='{LoginLower}';";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.ExecuteReader();
                                MessageBox.Show("Неверный пароль или логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {


                            }
                        }
                    }
                    if (UsersUnBlock == 1)// снятие времменной блокировки при правельном пароле,при котором время блокировки еще не прошло
                    {
                        query = $@"SELECT IDAllowance,IDProverka FROM Users WHERE Login= '{LoginLower}'";
                        Saver.LoginUser = LoginLower;
                        SQLiteDataReader dr = null;
                        SQLiteCommand cmd1 = new SQLiteCommand(query, connection);
                        int IDAllowance = 0;
                        int IDProverka = 0;
                        dr = cmd1.ExecuteReader();
                        while (dr.Read())
                        {
                            IDAllowance = Convert.ToInt32(dr["IDAllowance"].ToString());
                            IDProverka = Convert.ToInt32(dr["IDProverka"].ToString());
                        }
                        query = $@"SELECT AttemptNumber,TimeEnd,TimeBegin FROM Proverka WHERE ID = '{IDProverka}';";
                        cmd = new SQLiteCommand(query, connection);
                        string dateOpen = DateTime.Now.ToString("t");
                        TimeSpan s2 = TimeSpan.Parse(dateOpen);
                        dr = null;
                        string AttemptNumber = "";
                        string dateBegin = "00:00";
                        string dateban = DateTime.Now.ToString("t");
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            AttemptNumber = dr["AttemptNumber"].ToString();
                            dateban = dr["TimeEnd"].ToString();
                            dateBegin = dr["TimeBegin"].ToString();
                        }

                        if (Convert.ToInt32(AttemptNumber) == 3 && (TimeSpan.Parse(dateOpen) > TimeSpan.Parse(dateban)))
                        {
                            query = $@"UPDATE Proverka SET AttemptNumber = '{0}',TimeBegin = '00:00',TimeEnd = '00:00' WHERE ID ='{IDProverka}';";
                            cmd = new SQLiteCommand(query, connection);
                            cmd.ExecuteReader();
                            query = $@"UPDATE Users SET IDStatus='{1}' WHERE Login='{LoginLower}';";
                            cmd = new SQLiteCommand(query, connection);
                            cmd.ExecuteReader();
                            query = $@"SELECT Users.ID ,Users.IDAllowance, AllowanceUsers.Allowance FROM Users
                                        join AllowanceUsers on Users.IDAllowance = AllowanceUsers.ID 
                                        WHERE Login= '{LoginLower}'";
                            Saver.LoginUser = LoginLower;
                            dr = null;
                            cmd1 = new SQLiteCommand(query, connection);
                            dr = cmd1.ExecuteReader();
                            string IDAllowanceString = null;
                            while (dr.Read())
                            {
                                Saver.IDUser = dr["ID"].ToString();
                                Saver.IDAllowanceString = IDAllowanceString;

                            }
                            if (IDAllowanceString == "Администратор" || IDAllowanceString == "администратор")
                            {
                                Saver.IDAllowanceString = IDAllowanceString;
                                MessageBox.Show("Добро пожаловать!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                                UsersBase admpnl = new UsersBase();
                                Saver.IDAllowance = IDAllowance;                                
                                this.Close();
                                admpnl.ShowDialog();
                                connection.Close();
                            }
                            else if (IDAllowanceString == "Преподватель" || IDAllowanceString == "преподаватель")
                            {
                                Saver.Visitor = 0;
                                Saver.IDAllowanceString = IDAllowanceString;
                                MessageBox.Show("Добро пожаловать!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                                StudentBase menuinfor = new StudentBase();
                                Saver.IDAllowance = IDAllowance;                               
                                this.Close();
                                menuinfor.ShowDialog();
                                connection.Close();
                            }                           
                            connection.Close();
                        }
                        else
                        {
                            MessageBox.Show("Ваша учетная запись временно заблокированна,попробуйте позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        } //Если пароль не правельный

        private void TextBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            SimpleComand.CheckTextBox(TextBoxLogin);
        }

        private void PassBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SimpleComand.CheckPassBox(PassBox);
        }

        private void BtnAvtoriz_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationUser();
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MnItClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public void SelectDb()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();//(*.bmp, *.jpg)|*.bmp;*.jpg|Все файлы (*.*)|*.*""
            openFileDialog.Filter = "Text files (*.db)|*.db|All files (*.*)|*.*"; //"Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {                
                Saver.NameDB = Path.GetFullPath(openFileDialog.FileName);
                MessageBox.Show(Saver.NameDB);
                DBConnection.myConn = $@"Data Source = {Saver.NameDB};Version=3;";
                return;
            }
            else
            {
                DBConnection.myConn = $@"Data Source = AccountingStudentData.db;Version=3;";
                Saver.NameDB = "AccountingStudentData.db";
                return;
            }
        }

        private void BtnSelectDB_Click(object sender, RoutedEventArgs e)
        {
            SelectDb();
        }
        
    }
}
