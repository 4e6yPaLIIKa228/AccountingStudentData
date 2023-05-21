using System;
using System.Data.SQLite;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Window = System.Windows.Window;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;
using DBConnection = AccountingStudentData.Connection.DBConnection;
using XlLineStyle = Microsoft.Office.Interop.Excel.XlLineStyle;

namespace AccountingStudentData.BoxWindows
{
    /// <summary>
    /// Логика взаимодействия для UsersBase.xaml
    /// </summary>
    public partial class UsersBase : Window
    {
        public UsersBase()
        {
            InitializeComponent();
            LoadBase();
        }

        public void LoadBase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query = $@"                    					
                     SELECT  Users.ID,Users.Login, Users.Password,Users.Surname,Users.Name,Users.MidleName,Users.DataRegist, StatusUsers.NameStatus, AllowanceUsers.Allowance  FROM Users
                               LEFT JOIN StatusUsers on Users.IDStatus = StatusUsers.ID
							   LEFT JOIN AllowanceUsers on Users.IDAllowance = AllowanceUsers.ID			
                    where Users.IsDelet = 0  and Users.ID != '{Saver.IDUser}'
                    ORDER BY Surname";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    DataTable DT = new DataTable("Users");
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    SDA.Fill(DT);
                    GridBaseStudent.ItemsSource = DT.DefaultView;
                    cmd.ExecuteNonQuery();
                    SQLiteDataReader dr = null;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                      
                    }  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }  

        private void MnItAddUser_Click(object sender, RoutedEventArgs e)
        {

            RegistrUser addst = new RegistrUser();
            bool? result = addst.ShowDialog();
            switch (result)
            {
                default:
                    LoadBase();
                    break;
            }
        }
        public void EdditUser()
        {
            if (GridBaseStudent.SelectedIndex != -1)
            {
                EdditUser eddst = new EdditUser((DataRowView)GridBaseStudent.SelectedItem);
                eddst.Owner = this;
                bool? result = eddst.ShowDialog();
                switch (result)
                {
                    default:
                        LoadBase();
                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку с данными,чтобы ее изменить");
            }
        }

        private void GridBaseStudent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EdditUser();
        }

        private void MnItEddUser_Click(object sender, RoutedEventArgs e)
        {
            EdditUser();
        }

        private void MnItAddComponet_Click(object sender, RoutedEventArgs e)
        {
            AddComponets addst = new AddComponets();
            bool? result = addst.ShowDialog();
            switch (result)
            {
                default:
                    LoadBase();
                    break;
            }
        }

        private void MnItDellComponet_Click(object sender, RoutedEventArgs e)
        {
            DellComponets addst = new DellComponets();
            bool? result = addst.ShowDialog();
            switch (result)
            {
                default:
                    LoadBase();
                    break;
            }
        }

        public void ExportToExcel()
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Microsoft.Office.Interop.Excel.Worksheet sheet1 = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                excel.Visible = true;
                excel.Interactive = false;
                excel.ScreenUpdating = false;
                excel.UserControl = false;
                excel.DisplayAlerts = false;        
                for (int j = 2; j < GridBaseStudent.Columns.Count+1; j++) //Столбцы
                {
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[1, j+2];
                    sheet1.Columns[j].NumberFormat = "@";
                    myRange.Value2 = GridBaseStudent.Columns[j-1].Header;
                    myRange.Font.Name = "Times New Roman";
                    myRange.Cells.Font.Size = 16;
                    myRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    myRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    myRange.Style.WrapText = false;
                    myRange.Columns.ColumnWidth = 200;

                }
                Excel.Range myRang2 = sheet1.get_Range("A1", "C1");
                myRang2.Value = "ФИО";
                myRang2.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                myRang2.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                myRang2.Merge(Type.Missing);
                myRang2.Font.Name = "Times New Roman";
                myRang2.Font.Bold = true;
                myRang2.Cells.Font.Size = 16;
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    string query = $@"SELECT Users.Surname,Users.Name,Users.MidleName, Users.Login,Users.DataRegist, StatusUsers.NameStatus, AllowanceUsers.Allowance  FROM Users
                               LEFT JOIN StatusUsers on Users.IDStatus = StatusUsers.ID
							   LEFT JOIN AllowanceUsers on Users.IDAllowance = AllowanceUsers.ID
                               where Users.IsDelet = 0  and Users.ID != '{Saver.IDUser}' 
                               ORDER BY Surname";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    DataTable DT = new DataTable("Users");
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    SDA.Fill(DT);
                    if (CombSearchInfo.SelectedIndex != -1 && String.IsNullOrEmpty(TxtSearch.Text) == false)
                    {
                        DT = SerchInfo();
                    }
                    DataTable dt = DT;
                    int collInd = 0;
                    int rowInd = 0;
                    string data = "";
                    for (rowInd = 0; rowInd < dt.Rows.Count; rowInd++)
                    {
                        for (collInd = 0; collInd < dt.Columns.Count; collInd++)
                        {
                            data = dt.Rows[rowInd].ItemArray[collInd].ToString();
                            sheet1.Cells[rowInd + 2, collInd + 1] = data;
                            Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[rowInd + 2, collInd + 1];
                            myRange.Font.Name = "Times New Roman";
                            myRange.Cells.Font.Size = 14;
                            myRange = sheet1.UsedRange;
                            myRange.Borders.LineStyle = XlLineStyle.xlContinuous;
                        }
                    }
                }
                sheet1.Columns.AutoFit();
                sheet1.Rows.AutoFit();
                excel.Interactive = true;
                excel.ScreenUpdating = true;
                excel.UserControl = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public DataTable SerchInfo()
        {
            DataTable DT = new DataTable();
            try
            {
               
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    
                    connection.Open();
                    String combtext = CombSearchInfo.Text;
                    string DBSearchVisi = $@"                    					
                    SELECT  Users.ID,Users.Login, Users.Password,Users.Surname,Users.Name,Users.MidleName,Users.DataRegist, StatusUsers.NameStatus, AllowanceUsers.Allowance  FROM Users
                               LEFT JOIN StatusUsers on Users.IDStatus = StatusUsers.ID
							   LEFT JOIN AllowanceUsers on Users.IDAllowance = AllowanceUsers.ID			
                    where Users.IsDelet = 0  and Users.ID != '{Saver.IDUser}'  ";
                    string DBSearchExcel = $@"SELECT Users.Surname,Users.Name,Users.MidleName, Users.Login,Users.DataRegist, StatusUsers.NameStatus, AllowanceUsers.Allowance  FROM Users
                               LEFT JOIN StatusUsers on Users.IDStatus = StatusUsers.ID
							   LEFT JOIN AllowanceUsers on Users.IDAllowance = AllowanceUsers.ID
                               where Users.IsDelet = 0  and Users.ID != '{Saver.IDUser}' ";                   
                    if (combtext == "Фамилия")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}   and  Users.Surname like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   and Users.Surname  like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Имя")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}   and Users.Name like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   and Users.Name  like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Отчество")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}   and Users.MidleName like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   and Users.MidleName  like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Логин")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}   and Users.Login like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   and Users.Login  like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Дата регистрации")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}   and Users.DataRegist like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   and Users.DataRegist  like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Статус")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}   and  StatusUsers.NameStatus like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   and  StatusUsers.NameStatus  like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Доступ")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}   and AllowanceUsers.Allowance like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   and AllowanceUsers.Allowance  like '%{TxtSearch.Text}%'  ORDER BY Surname";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Users");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return DT;          
        }

        private void MnItClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MnItExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SerchInfo();
        }      

        private void MnItYchetSt_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void MnItLichSt_Click(object sender, RoutedEventArgs e)
        {
            if (GridBaseStudent.SelectedIndex != -1)
            {
                LichDeloStudenta eddst = new LichDeloStudenta((DataRowView)GridBaseStudent.SelectedItem);
                eddst.Owner = this;
                bool? result = eddst.ShowDialog();
                switch (result)
                {
                    default:
                        LoadBase();
                        break;
                }
            }               
        }

        private void MnItDelUser_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите добавить пользователя?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (GridBaseStudent.SelectedIndex != -1)
                {
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                        {
                            connection.Open();
                            string ID;
                            DataRowView drv = GridBaseStudent.SelectedItem as DataRowView;
                            ID = drv["ID"].ToString();
                            string query = $@"Update Users SET IsDelet = 1 WHERE ID = '{ID}'";
                            SQLiteCommand cmd = new SQLiteCommand(query, connection);
                            cmd.ExecuteNonQuery();
                            LoadBase();
                            MessageBox.Show("Аккаунт удален", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }            
            }
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

        private void MnItSize_Click(object sender, RoutedEventArgs e)
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

        private void MnItUpdate_Click(object sender, RoutedEventArgs e)
        {
            LoadBase();
            CombSearchInfo.SelectedIndex = -1;
            TxtSearch.Text = string.Empty;
        }       

        private void MnItListStudents_Click(object sender, RoutedEventArgs e)
        {
            StudentBase eddst = new StudentBase();
            eddst.Show();
            this.Close();
        }

        private void MnItExitUser_Click(object sender, RoutedEventArgs e)
        {
            Authorization eddst = new Authorization();
            eddst.Show();
            this.Close();
            Saver.IDUser = "0";
        }

        private void MnItArchive_Click(object sender, RoutedEventArgs e)
        {
            Archive eddst = new Archive();
            eddst.Show();
            this.Close();
        }
    }
}
