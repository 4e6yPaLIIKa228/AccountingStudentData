using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
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
using Microsoft.Office.Interop.Excel;
using Window = System.Windows.Window;
using DataTable = System.Data.DataTable;
using System.Windows.Markup;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.Common;
using Application = Microsoft.Office.Interop.Excel.Application;
using Spire.Xls;
using DBConnection = AccountingStudentData.Connection.DBConnection;
using Microsoft.Win32;
using Spire.Xls.Core;
using System.IO;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using XlLineStyle = Microsoft.Office.Interop.Excel.XlLineStyle;
using static System.Net.Mime.MediaTypeNames;
using Spire.Pdf.Exporting.XPS.Schema;
using System.Xml.Linq;
using System.Reflection;

namespace AccountingStudentData.BoxWindows
{
    /// <summary>
    /// Логика взаимодействия для StudentBase.xaml
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
                    ORDER BY Login";
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
                //LoadBase();
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
                for (int j = 2; j < GridBaseStudent.Columns.Count - 1; j++) //Столбцы
                {
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[1, j + 5];
                    sheet1.Columns[j + 1].NumberFormat = "@";
                    myRange.Value2 = GridBaseStudent.Columns[j].Header;
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
                //Excel.Range myRang3 = sheet1.get_Range("D1", "F1");
                //myRang3.Value = "Руководитель";
                //myRang3.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                //myRang3.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                //myRang3.Merge(Type.Missing);
                //myRang3.Font.Name = "Times New Roman";
                //myRang3.Cells.Font.Size = 14;
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    string query = $@"SELECT Students.Surname as SurnameSt, Students.Name as NameSt, Students.MidleName as MidleNameSt, Users.Surname as SurnamePyk ,Users.Name as NamePyk, Users.MidleName as MidleNamePyk,
                                        Polls.Name as PollSt, Students.Phone1 as Phone1St, Students.PocleKlass as KlassSt,
                                        Specialties.NumberSpecial as NumberSpecualSt,Groups.Name as GroupSt,                                       
                                        Students.DataСredited as DataPost,Students.DataEnd as DataOkon,
                                        Students.NumberPrikaz as NumberPrikazSt,Students.NumberDogovora as NumberDogovorSt from  Students
                                        LEFT JOIN Polls on Students.IDPoll = Polls.ID
                                        LEFT JOIN Specialties on Students.IDSpecual = Specialties.ID
                                        LEFT JOIN Groups on Students.IDGrop = Groups.ID
                                        LEFT JOIN Users on Students.IDPyku = Users.ID";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    DataTable DT = new DataTable("Students");
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
                    SELECT Students.ID as IDSt,Students.Surname as SurnameSt, Students.Name as NameSt, Students.MidleName as MidleNameSt,Students.Phone1 as Phone1St,Students.Phone2 as Phone2St,Students.DataBirth as DataBirthSt,
                    Polls.Name as PollSt,Specialties.NumberSpecial as NumberSpecualSt,Specialties.Name as NameSpecial, Groups.Name as GroupSt,Students.PocleKlass as KlassSt,
                    Users.ID as IDPyk,Users.Surname as SurnamePyk ,Users.Name as NamePyk, Users.MidleName as MidleNamePyk,
					Students.NumberPrikaz as NumberPrikazSt,Students.NumberDogovora as NumberDogovorSt,Students.NumberAtect as AtectSt,Students.DataPolycen as  DataPolecenSt,
					Students.DataСredited as DataPost, Students.DataEnd as DataOkon, Students.Foto as FotoSt,Students.NameSchool as NameSchoolSt,
					Students.SNILS as SNILSSt, Students.OMS as OMSSt, Students.Adress as AdressSt,
					Students.PassportData as PassDataSt, Students.PassportNumber as PassNumSt,Students.PassportSeria as PassSeriaSt,
					Students.PassportVID as PassVIDSt,Students.PassportVidan as PassVidanSt,Students.PassportCountry as PassCountrySt,
                    Students.IDSpecual as IDSpecSt,Students.IDGrop as IDGropSt,
                    Students.NumberZatechBook,Students.NumberPrigazKyrs1,Students.DataСreditedKyrs1,Students.NumberPrigazKyrs2,Students.DataСreditedKyrs2,Students.NumberPrigazKyrs3,Students.DataСreditedKyrs3,
                    Students.NumberPrigazKyrs4,Students.DataСreditedKyrs4,MestoBirthday,				
				
                    from Students

                    LEFT JOIN Polls on Students.IDPoll = Polls.ID
                    LEFT JOIN Specialties on Students.IDSpecual = Specialties.ID
                    LEFT JOIN Groups on Students.IDGrop = Groups.ID
                    LEFT JOIN Users on Students.IDPyku = Users.ID					
                    where Students.Delete != 1;
                    ";
                    string DBSearchExcel = $@"SELECT Students.Surname as SurnameSt, Students.Name as NameSt, Students.MidleName as MidleNameSt, Users.Surname as SurnamePyk ,Users.Name as NamePyk, Users.MidleName as MidleNamePyk,
                                        Polls.Name as PollSt, Students.Phone1 as Phone1St, Students.PocleKlass as KlassSt,
                                        Specialties.NumberSpecial as NumberSpecualSt,Groups.Name as GroupSt,                                       
                                        Students.DataСredited as DataPost,Students.DataEnd as DataOkon,
                                        Students.NumberPrikaz as NumberPrikazSt,Students.NumberDogovora as NumberDogovorSt from  Students
                                        LEFT JOIN Polls on Students.IDPoll = Polls.ID
                                        LEFT JOIN Specialties on Students.IDSpecual = Specialties.ID
                                        LEFT JOIN Groups on Students.IDGrop = Groups.ID
                                        LEFT JOIN Users on Students.IDPyku = Users.ID";                   
                    if (combtext == "Фамилия")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}   WHERE Students.Surname like '%{TxtSearch.Text}%'";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("MenuPerTech");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   WHERE Students.Surname like '%{TxtSearch.Text}%' ORDER BY SurnameSt";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("MenuPerTech");
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
    }
}
