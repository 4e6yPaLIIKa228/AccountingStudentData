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
    public partial class StudentBase : Window
    {
        public StudentBase()
        {
            InitializeComponent();
            LoadBase();
            CheackListUser();
        }

        public void LoadBase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query = $@"                    					
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
                    Students.NumberPrigazKyrs4,Students.DataСreditedKyrs4,MestoBirthday					
					
                    from Students

                    LEFT JOIN Polls on Students.IDPoll = Polls.ID
                    LEFT JOIN Specialties on Students.IDSpecual = Specialties.ID
                    LEFT JOIN Groups on Students.IDGrop = Groups.ID
                    LEFT JOIN Users on Students.IDPyku = Users.ID
                    where Students.IsDelet = 0 
                    ORDER BY SurnameSt";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    DataTable DT = new DataTable("Students");
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

        private void MnItAddStudent_Click(object sender, RoutedEventArgs e)
        {

            AddStudents addst = new AddStudents();
            bool? result = addst.ShowDialog();
            switch (result)
            {
                default:
                    LoadBase();
                    break;
            }
        }
        public void EdditStudent()
        {
            if (GridBaseStudent.SelectedIndex != -1)
            {
                //LoadBase();
                EddStudents eddst = new EddStudents((DataRowView)GridBaseStudent.SelectedItem);
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
            EdditStudent();
        }

        private void MnItEddStudent_Click(object sender, RoutedEventArgs e)
        {
            EdditStudent();
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
                Excel.Range myRang3 = sheet1.get_Range("D1", "F1");
                myRang3.Value = "Руководитель";
                myRang3.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                myRang3.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                myRang3.Merge(Type.Missing);
                myRang3.Font.Name = "Times New Roman";
                myRang3.Cells.Font.Size = 14;
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
                    Students.NumberPrigazKyrs4,Students.DataСreditedKyrs4,MestoBirthday				
				
                    from Students

                    LEFT JOIN Polls on Students.IDPoll = Polls.ID
                    LEFT JOIN Specialties on Students.IDSpecual = Specialties.ID
                    LEFT JOIN Groups on Students.IDGrop = Groups.ID
                    LEFT JOIN Users on Students.IDPyku = Users.ID					
                    where Students.IsDelet != 1 
                    ";
                    string DBSearchExcel = $@"SELECT Students.Surname as SurnameSt, Students.Name as NameSt, Students.MidleName as MidleNameSt, Users.Surname as SurnamePyk ,Users.Name as NamePyk, Users.MidleName as MidleNamePyk,
                                        Polls.Name as PollSt, Students.Phone1 as Phone1St, Students.PocleKlass as KlassSt,
                                        Specialties.NumberSpecial as NumberSpecualSt,Groups.Name as GroupSt,                                       
                                        Students.DataСredited as DataPost,Students.DataEnd as DataOkon,
                                        Students.NumberPrikaz as NumberPrikazSt,Students.NumberDogovora as NumberDogovorSt from  Students
                                        LEFT JOIN Polls on Students.IDPoll = Polls.ID
                                        LEFT JOIN Specialties on Students.IDSpecual = Specialties.ID
                                        LEFT JOIN Groups on Students.IDGrop = Groups.ID
                                        LEFT JOIN Users on Students.IDPyku = Users.ID
                                        where Students.IsDelet != 1 ";                   
                    if (combtext == "Фамилия Ст")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}  and  Students.Surname like '%{TxtSearch.Text}%'";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   and  Students.Surname like '%{TxtSearch.Text}%'";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Имя Ст")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}   and Students.Name like '%{TxtSearch.Text}%'";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}  and Students.Name like '%{TxtSearch.Text}%' ORDER BY SurnameSt";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Отчество Ст")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}   and Students.MidleName like '%{TxtSearch.Text}%'";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   and Students.MidleName like '%{TxtSearch.Text}%' ORDER BY SurnameSt";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Код специальности")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}  and Specialties.NumberSpecial like '%{TxtSearch.Text}%'";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}  and Specialties.NumberSpecial  like '%{TxtSearch.Text}%' ORDER BY SurnameSt";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Группа")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}   and  Groups.Name  like '%{TxtSearch.Text}%'";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   and Groups.Name like '%{TxtSearch.Text}%' ORDER BY SurnameSt";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Фамилия Рук")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}   and Users.Surname like '%{TxtSearch.Text}%'";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}  and Users.Surname like '%{TxtSearch.Text}%' ORDER BY SurnameSt";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Имя Рук")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi}  and Users.Name like '%{TxtSearch.Text}%'";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   and Users.Name like '%{TxtSearch.Text}%' ORDER BY SurnameSt";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        return DT;
                    }
                    else if (combtext == "Отчество Рук")
                    {
                        GridBaseStudent.ItemsSource = null;
                        string query = $@"{DBSearchVisi} and  Users.MidleName like '%{TxtSearch.Text}%'";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        SDA.Fill(DT);
                        GridBaseStudent.ItemsSource = DT.DefaultView;
                        cmd.ExecuteNonQuery();
                        query = $@"{DBSearchExcel}   and  Users.MidleName like '%{TxtSearch.Text}%' ORDER BY SurnameSt";
                        cmd = new SQLiteCommand(query, connection);
                        DT = new DataTable("Students");
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

        public void KartochkaYchetSt()
        {
            try
            {
                if (GridBaseStudent.SelectedIndex != -1)
                {
                    Microsoft.Office.Interop.Word.Document doc = null;
                    Word._Application oWord = new Word.Application();
                    DataRowView drv = (DataRowView)GridBaseStudent.SelectedItem;
                    Microsoft.Office.Interop.Word.Application app = new Word.Application();
                    // Путь до шаблона документа
                    string source = System.IO.Path.Combine(Environment.CurrentDirectory, "Учётная карточка студента.doc");
                    // Открываем
                    doc = app.Documents.Open(source);
                    doc.Activate();
                    doc.Bookmarks["Surname"].Range.Text = drv["SurnameSt"].ToString();
                    doc.Bookmarks["Name"].Range.Text = drv["NameSt"].ToString();
                    doc.Bookmarks["FirstName"].Range.Text = drv["MidleNameSt"].ToString();
                    doc.Bookmarks["Birthday"].Range.Text = drv["DataBirthSt"].ToString();
                    doc.Bookmarks["Group"].Range.Text = drv["GroupSt"].ToString();
                    doc.Bookmarks["KodSpecial"].Range.Text = drv["NumberSpecualSt"].ToString();
                    doc.Bookmarks["NameSpecial"].Range.Text = drv["NameSpecial"].ToString();
                    doc.Bookmarks["NamePrikazePost"].Range.Text = drv["NumberPrikazSt"].ToString();
                    doc.Bookmarks["EndDate"].Range.Text = drv["DataOkon"].ToString();
                    doc.Bookmarks["Adress"].Range.Text = drv["AdressSt"].ToString();
                    doc.Bookmarks["RegistrAdress"].Range.Text = drv["AdressSt"].ToString();
                    using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                    {
                        connection.Open();
                        string pr = "0";
                        string IDSt = "0";
                        IDSt = drv["IDSt"].ToString();
                        for (int i = 1; i <= 4; i++)
                        {
                            var Surname = (UIElement)FindName("SurnameOtved" + i);
                            var Name = (UIElement)FindName("NameOtved" + i);
                            var MidleName = (UIElement)FindName("MideleNameOtved" + i);
                            var Pod = (UIElement)FindName("CmbRodOtved" + i);
                            string qwert = $@"Select ID,Surname,Name,MidleName,Pod,Work,WorkDol from Responsible where Responsible.IsDelet = 0 and  ID > '{pr}'  and {IDSt} ";
                            SQLiteCommand cmd = new SQLiteCommand(qwert, connection);
                            cmd.ExecuteNonQuery();
                            SQLiteDataReader dr = null;
                            dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                pr = dr["ID"].ToString();
                                doc.Bookmarks[$@"Otved{i}"].Range.Text = dr["Pod"].ToString() + ": " +
                                dr["Surname"].ToString() + " " + dr["Name"].ToString() + " " + dr["MidleName"].ToString()
                                + "\n" + "Место работы: " + dr["Work"].ToString() + "\n" + "Должность: " + dr["WorkDol"].ToString();
                                break;
                            }
                        }

                    }                    
                    string txtSurnKlss = drv["SurnamePyk"].ToString();
                    char chr = txtSurnKlss[0];
                    doc.Bookmarks["SurnameKlass"].Range.Text = chr.ToString();
                    string txtnameKlss = drv["NamePyk"].ToString();
                    char chr1 = txtnameKlss[0];
                    doc.Bookmarks["NameKlass"].Range.Text = chr1.ToString();
                    doc.Bookmarks["FirstNameKlass"].Range.Text = drv["MidleNamePyk"].ToString();
                    doc.Bookmarks["DateNow"].Range.Text = DateTime.Now.ToString("D");
                    string DirectoryFale = System.IO.Path.GetDirectoryName(source);
                    doc.SaveAs($@"{DirectoryFale}\{drv["SurnameSt"]} {drv["NameSt"]} {drv["MidleNameSt"]}");
                    doc.Close();
                    doc = null;
                    app.Quit();
                    MessageBox.Show($@"Отчет сформулирован и находится в {DirectoryFale}");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MnItYchetSt_Click(object sender, RoutedEventArgs e)
        {
            KartochkaYchetSt();
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

        private void MnItDelStudent_Click(object sender, RoutedEventArgs e)
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
                        ID = drv["IDSt"].ToString();
                        string query = $@"Update Students SET IsDelet = 1 WHERE ID = '{ID}'";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        cmd.ExecuteNonQuery();
                        LoadBase();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }                
            }
        }

        private void MnItExitUser_Click(object sender, RoutedEventArgs e)
        {
            Authorization eddst = new Authorization();
            eddst.Show();
            this.Close();
            Saver.IDUser = "0";
        }

        public void CheackListUser()
        {
            if (Saver.IDAllowanceString == "Администратор")
            {
                MnItListUsers.Visibility = Visibility.Visible;
            }
            else
            {
                MnItListUsers.Visibility = Visibility.Collapsed;
            }
        }
            
        private void MnItListUsers_Click(object sender, RoutedEventArgs e)
        {
            Authorization eddst = new Authorization();
            eddst.Show();
            this.Close();
           // Saver.IDUser = "0";
        }

        private void MnItUpdate_Click(object sender, RoutedEventArgs e)
        {
            LoadBase();
            CombSearchInfo.SelectedIndex = -1;
            TxtSearch.Text = string.Empty;
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
    }
}
