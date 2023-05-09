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
                    Students.NumberPrigazKyrs4,Students.DataСreditedKyrs4,MestoBirthday,
					
					MumStudents.ID as IDMumSt, MumStudents.Surname as SurnameMum, MumStudents.Name as NameMum, MumStudents.MidleName as MidleNameMum,
					MumStudents.PassportData as PassDataMum, MumStudents.PassportNumber as PassNumMum, MumStudents.PassportSeria as PassSeriaMum,
					MumStudents.PassportVID as PassVIDMum,MumStudents.PassportVidan as PassVidanMum, MumStudents.Phone1 as Phone1Mum, MumStudents.Phone2 as Phone2Mum,MumStudents.PassportCountry as PassCountryMum,
                    MumStudents.WorkMum,MumStudents.WorkDolMum,
					
					DadStudents.ID as IDDadSt, DadStudents.Surname as SurnameDad, DadStudents.Name as NameDad, DadStudents.MidleName as MidleNameDad,
					DadStudents.PassportData as PassDataDad, DadStudents.PassportNumber as PassNumDad, DadStudents.PassportSeria as PassSeriaDad,
					DadStudents.PassportVID as PassVIDDad,DadStudents.PassportVidan as PassVidanDad, DadStudents.Phone1 as Phone1Dad, DadStudents.Phone2 as Phone2Dad,DadStudents.PassportCountry as PassCountryDad,
                    DadStudents.WorkDad,DadStudents.WorkDolDad

                    from Students

                    LEFT JOIN Polls on Students.IDPoll = Polls.ID
                    LEFT JOIN Specialties on Students.IDSpecual = Specialties.ID
                    LEFT JOIN Groups on Students.IDGrop = Groups.ID
                    LEFT JOIN Users on Students.IDPyku = Users.ID
					LEFT JOIN MumStudents on Students.IDMum = MumStudents.ID
					LEFT JOIN DadStudents on Students.IDDad = DadStudents.ID
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
                    Students.NumberPrigazKyrs4,Students.DataСreditedKyrs4,MestoBirthday,
					
					MumStudents.ID as IDMumSt, MumStudents.Surname as SurnameMum, MumStudents.Name as NameMum, MumStudents.MidleName as MidleNameMum,
					MumStudents.PassportData as PassDataMum, MumStudents.PassportNumber as PassNumMum, MumStudents.PassportSeria as PassSeriaMum,
					MumStudents.PassportVID as PassVIDMum,MumStudents.PassportVidan as PassVidanMum, MumStudents.Phone1 as Phone1Mum, MumStudents.Phone2 as Phone2Mum,MumStudents.PassportCountry as PassCountryMum,
                    MumStudents.WorkMum,MumStudents.WorkDolMum,
					
					DadStudents.ID as IDDadSt, DadStudents.Surname as SurnameDad, DadStudents.Name as NameDad, DadStudents.MidleName as MidleNameDad,
					DadStudents.PassportData as PassDataDad, DadStudents.PassportNumber as PassNumDad, DadStudents.PassportSeria as PassSeriaDad,
					DadStudents.PassportVID as PassVIDDad,DadStudents.PassportVidan as PassVidanDad, DadStudents.Phone1 as Phone1Dad, DadStudents.Phone2 as Phone2Dad,DadStudents.PassportCountry as PassCountryDad,
                    DadStudents.WorkDad,DadStudents.WorkDolDad

                    from Students

                    LEFT JOIN Polls on Students.IDPoll = Polls.ID
                    LEFT JOIN Specialties on Students.IDSpecual = Specialties.ID
                    LEFT JOIN Groups on Students.IDGrop = Groups.ID
                    LEFT JOIN Users on Students.IDPyku = Users.ID
					LEFT JOIN MumStudents on Students.IDMum = MumStudents.ID
					LEFT JOIN DadStudents on Students.IDDad = DadStudents.ID
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
                    doc.Bookmarks["MumSt"].Range.Text = drv["SurnameMum"].ToString() + " " + drv["NameMum"].ToString() + " " + drv["MidleNameMum"].ToString();
                    doc.Bookmarks["WorkMum"].Range.Text = drv["WorkMum"].ToString() + " " + drv["WorkDolMum"].ToString();
                    doc.Bookmarks["DadSt"].Range.Text = drv["SurnameDad"].ToString() + " " + drv["NameDad"].ToString() + " " + drv["MidleNameDad"].ToString();
                    doc.Bookmarks["WorkDad"].Range.Text = drv["WorkDad"].ToString() + " " + drv["WorkDolDad"].ToString();
                    string txtSurnKlss = drv["SurnamePyk"].ToString();
                    char chr = txtSurnKlss[0];
                    doc.Bookmarks["SurnameKlass"].Range.Text = chr.ToString();
                    string txtnameKlss = drv["NamePyk"].ToString();
                    char chr1 = txtnameKlss[0];
                    doc.Bookmarks["NameKlass"].Range.Text = chr1.ToString();
                    doc.Bookmarks["FirstNameKlass"].Range.Text = drv["MidleNamePyk"].ToString();
                    doc.Bookmarks["DateNow"].Range.Text = DateTime.Now.ToString("D");
                    string DirectoryFale = System.IO.Path.GetDirectoryName(source);
                    doc.SaveAs($@"{DirectoryFale}\{drv["SurnameSt"]}{drv["NameSt"]}{drv["MidleNameSt"]}");
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

        //public void KartochkaLichSt()
        //{
        //    Microsoft.Office.Interop.Word.Document doc = null;
        //    Word._Application oWord = new Word.Application();
        //    try
        //    {
        //        if (GridBaseStudent.SelectedIndex != -1)
        //        {
        //            DataRowView drv = (DataRowView)GridBaseStudent.SelectedItem;
        //            // Создаём объект приложения
        //            Microsoft.Office.Interop.Word.Application app = new Word.Application();
        //            // Путь до шаблона документа
        //            // string source = @"/AccountingStudent/Test.docx";
        //            string source = System.IO.Path.Combine(Environment.CurrentDirectory, "Личная карточка студента  NEW.docx");
        //            // Открываем
        //            doc = app.Documents.Open(source);
        //            doc.Activate();
        //            //Заполение данных
        //            byte[] image_bytes = (byte[])drv["FotoSt"];
        //            BitmapImage img = new BitmapImage();
        //            img.BeginInit();
        //            img.CreateOptions = BitmapCreateOptions.None;
        //            img.CacheOption = BitmapCacheOption.Default;
        //            img.DecodePixelWidth = 600;
        //            img.DecodePixelHeight = 750;
        //            img.StreamSource = new MemoryStream(image_bytes);
        //            img.EndInit();
        //            Clipboard.SetImage(img);
        //            doc.Bookmarks.get_Item("Foto").Range.Paste();
        //            doc.Bookmarks["NumberZatetku"].Range.Text = drv["NumberZatechBook"].ToString();
        //            doc.Bookmarks["Surname"].Range.Text = drv["SurnameSt"].ToString() + " " + drv["NameSt"].ToString() + " " + drv["MidleNameSt"].ToString(); ;
        //            doc.Bookmarks["Birthday"].Range.Text = drv["DataBirthSt"].ToString();
        //            doc.Bookmarks["MestoBirthday"].Range.Text = "заглушка";
        //            doc.Bookmarks["GroupSt"].Range.Text = drv["GroupSt"].ToString();
        //            doc.Bookmarks["SpecialSt"].Range.Text = drv["NumberSpecualSt"].ToString() + " " + drv["NameSpecial"].ToString();
        //            doc.Bookmarks["NumberPrikaz"].Range.Text = drv["NumberPrikazSt"].ToString();
        //            doc.Bookmarks["DataPrikazwPostyplenuy"].Range.Text = drv["DataPost"].ToString();
        //            doc.Bookmarks["FIOMum"].Range.Text = drv["SurnameMum"].ToString() + " " + drv["NameMum"].ToString() + " " + drv["MidleNameMum"].ToString();
        //            doc.Bookmarks["MestoWorkMum"].Range.Text = drv["WorkMum"].ToString();
        //            doc.Bookmarks["DolWorkMum"].Range.Text = drv["WorkDolMum"].ToString();
        //            doc.Bookmarks["FIODad"].Range.Text = drv["SurnameDad"].ToString() + " " + drv["NameDad"].ToString() + " " + drv["MidleNameDad"].ToString();
        //            doc.Bookmarks["MestoWorkDad"].Range.Text = drv["WorkDad"].ToString();
        //            doc.Bookmarks["DolWorkDad"].Range.Text = drv["WorkDolDad"].ToString();
        //            doc.Bookmarks["NameSchool"].Range.Text = drv["NameSchoolSt"].ToString();
        //            doc.Bookmarks["DateEndSchool"].Range.Text = drv["DataPolecenSt"].ToString();
        //            doc.Bookmarks["AdressSt"].Range.Text = drv["AdressSt"].ToString();
        //            doc.Bookmarks["PhoneSt"].Range.Text = drv["Phone1St"].ToString();
        //            doc.Bookmarks["VIDPassporta"].Range.Text = drv["PassVIDSt"].ToString();
        //            doc.Bookmarks["SeriaPassport"].Range.Text = drv["PassSeriaSt"].ToString();
        //            doc.Bookmarks["NumberPassport"].Range.Text = drv["PassNumSt"].ToString();
        //            doc.Bookmarks["DatePolychPassport"].Range.Text = drv["PassDataSt"].ToString();
        //            doc.Bookmarks["KemVudanPass"].Range.Text = drv["PassVidanSt"].ToString();
        //            doc.Bookmarks["SNILS"].Range.Text = drv["SNILSSt"].ToString();
        //            doc.Bookmarks["OMS"].Range.Text = drv["OMSSt"].ToString();
        //            doc.Bookmarks["DateNow"].Range.Text = DateTime.Now.ToString("yyyy");
        //            doc.Bookmarks["DateNow1"].Range.Text = DateTime.Now.AddYears(1).ToString("yyyy");
        //            doc.Bookmarks["DateNow2"].Range.Text = DateTime.Now.AddYears(1).ToString("yyyy");
        //            doc.Bookmarks["DateNow3"].Range.Text = DateTime.Now.AddYears(2).ToString("yyyy");
        //            doc.Bookmarks["DateNow4"].Range.Text = DateTime.Now.AddYears(2).ToString("yyyy");
        //            doc.Bookmarks["DateNow5"].Range.Text = DateTime.Now.AddYears(3).ToString("yyyy");
        //            doc.Bookmarks["DateNow6"].Range.Text = DateTime.Now.AddYears(3).ToString("yyyy");
        //            doc.Bookmarks["DateNow7"].Range.Text = DateTime.Now.AddYears(4).ToString("yyyy");
        //            doc.Bookmarks["NumberPrigazKyrs1"].Range.Text = drv["NumberPrigazKyrs1"].ToString();
        //            doc.Bookmarks["NumberPrigazKyrs2"].Range.Text = drv["NumberPrigazKyrs2"].ToString();
        //            doc.Bookmarks["NumberPrigazKyrs3"].Range.Text = drv["NumberPrigazKyrs3"].ToString();
        //            doc.Bookmarks["NumberPrigazKyrs4"].Range.Text = drv["NumberPrigazKyrs4"].ToString();
        //            doc.Bookmarks["DataСreditedKyrs1"].Range.Text = drv["DataСreditedKyrs1"].ToString();
        //            doc.Bookmarks["DataСreditedKyrs2"].Range.Text = drv["DataСreditedKyrs2"].ToString();
        //            doc.Bookmarks["DataСreditedKyrs3"].Range.Text = drv["DataСreditedKyrs3"].ToString();
        //            doc.Bookmarks["DataСreditedKyrs4"].Range.Text = drv["DataСreditedKyrs4"].ToString();
        //            // Закрываем документ
        //            string DirectoryFale = System.IO.Path.GetDirectoryName(source);
        //            doc.SaveAs($@"{DirectoryFale}\Личная карточка студента_{drv["SurnameSt"]}_{drv["NameSt"]}_{drv["MidleNameSt"]}");
        //            doc.Close();
        //            doc = null;
        //            app.Quit();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        // Если произошла ошибка, то
        //        // закрываем документ и выводим информацию
        //        // doc.Close();
        //        // doc = null;
        //        Console.WriteLine("Во время выполнения произошла ошибка!");
        //        Console.ReadLine();
        //    }
        //}

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
    }
}
