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
                    Polls.Name as PollSt,Specialties.NumberSpecial as NumberSpecualSt, Groups.Name as GroupSt,Students.PocleKlass as KlassSt,
                    Users.ID as IDPyk,Users.Surname as SurnamePyk ,Users.Name as NamePyk, Users.MidleName as MidleNamePyk,
					Students.NumberPrikaz as NumberPrikazSt,Students.NumberDogovora as NumberDogovorSt,Students.NumberAtect as AtectSt,Students.DataPolycen as  DataPolecenSt,
					Students.DataСredited as DataPost, Students.DataEnd as DataOkon, Students.Foto as FotoSt,Students.NameSchool as NameSchoolSt,
					Students.SNILS as SNILSSt, Students.OMS as OMSSt, Students.Adress as AdressSt,
					Students.PassportData as PassDataSt, Students.PassportNumber as PassNumSt,Students.PassportSeria as PassSeriaSt,
					Students.PassportVID as PassVIDSt,Students.PassportVidan as PassVidanSt,Students.PassportCountry as PassCountrySt,
                    Students.IDSpecual as IDSpecSt,Students.IDGrop as IDGropSt,
					
					MumStudents.ID as IDMumSt, MumStudents.Surname as SurnameMum, MumStudents.Name as NameMum, MumStudents.MidleName as MidleNameMum,
					MumStudents.PassportData as PassDataMum, MumStudents.PassportNumber as PassNumMum, MumStudents.PassportSeria as PassSeriaMum,
					MumStudents.PassportVID as PassVIDMum,MumStudents.PassportVidan as PassVidanMum, MumStudents.Phone1 as Phone1Mum, MumStudents.Phone2 as Phone2Mum,MumStudents.PassportCountry as PassCountryMum,
					
					DadStudents.ID as IDDadSt, DadStudents.Surname as SurnameDad, DadStudents.Name as NameDad, DadStudents.MidleName as MidleNameDad,
					DadStudents.PassportData as PassDataDad, DadStudents.PassportNumber as PassNumDad, DadStudents.PassportSeria as PassSeriaDad,
					DadStudents.PassportVID as PassVIDDad,DadStudents.PassportVidan as PassVidanDad, DadStudents.Phone1 as Phone1Dad, DadStudents.Phone2 as Phone2Dad,DadStudents.PassportCountry as PassCountryDad

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
                        //Componets, ProccesID, MaterPlatID, VideCardID, IDRAM, Slot1ID1, Slot1ID2, Slot1ID3, Slot1ID4;
                        //Saver.IDMenuPerPC = dr["IDMenuPer"].ToString();
                        //Saver.IDComponets = dr["IDComponets"].ToString();
                        //Saver.ProccesID = dr["ProccesID"].ToString();
                        //Saver.MaterPlatID = dr["MaterPlatID"].ToString();
                        //Saver.VideCardID = dr["VideoCardID"].ToString();
                        //Saver.IDRAM = dr["IDRAM"].ToString();
                        //Saver.SlotID1 = dr["SlotID1"].ToString();
                        //Saver.SlotID2 = dr["SlotID2"].ToString();
                        //Saver.SlotID3 = dr["SlotID3"].ToString();
                        //Saver.SlotID4 = dr["SlotID4"].ToString();
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
            // Microsoft.Office.Interop.Excel.Application app = null;
            // Microsoft.Office.Interop.Excel.Workbook wb = null;
            //Microsoft.Office.Interop.Excel.Worksheet ws = null;
            //app = new Microsoft.Office.Interop.Excel.Application();
            //app.Visible = true;
            //app.DisplayAlerts = false;
            //wb = app.Workbooks.Add();
            //ws = wb.ActiveSheet;
            //GridBaseStudent.SelectAllCells();
            //GridBaseStudent.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            //ApplicationCommands.Copy.Execute(null, GridBaseStudent);
            //ws.Paste();
            //ws.Range["A1", "H1"].Font.Bold = true;
            //int number1 = ws.UsedRange.Rows.Count;
            //Microsoft.Office.Interop.Excel.Range myRange = ws.Range["A1", "H" + number1];
            //myRange.Borders.LineStyle = XlLineStyle.xlContinuous;
            //myRange.WrapText = false;
            //ws.Columns.EntireColumn.AutoFit();
            //Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            //ExcelApp.Application.Workbooks.Add(Type.Missing);
            //for (int i = 0; i < GridBaseStudent.Columns.Count; i++)
            //{
            //    ExcelApp.Cells[i + 1, 1] = GridBaseStudent[i].date;
            //    ExcelApp.Cells[i + 1, 2] = GridBaseStudent[i].bat;
            //}
            //ExcelApp.Visible = true;
            //       using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            //       {
            //           try
            //           {
            //               connection.Open();
            //               string query = $@"                    					
            //               SELECT Students.Surname as SurnameSt, Students.Name as NameSt, Students.MidleName as MidleNameSt,Students.Phone1 as Phone1St,
            //               Polls.Name as PollSt,Specialties.NumberSpecial as NumberSpecualSt, Groups.Name as GroupSt,Students.PocleKlass as KlassSt,
            //               Users.ID as IDPyk,Users.Surname as SurnamePyk ,Users.Name as NamePyk, Users.MidleName as MidleNamePyk,
            //Students.NumberPrikaz as NumberPrikazSt,Students.NumberDogovora as NumberDogovorSt,
            //Students.DataСredited as DataPost, Students.DataEnd as DataOkon, Students.Foto,
            //Students.Phone2 as Phone2St, Students.SNILS as SNILSSt, Students.OMS as OMSSt, Students.Adress as AdressSt,
            //Students.PassportData as PassDataSt, Students.PassportNumber as PassNumSt,Students.PassportSeria as PassSeriaSt,
            //Students.PassportVID as PassVIDSt,Students.PassportVidan as PassVidanSt,

            //MumStudents.ID as IDMumSt, MumStudents.Surname as SurnameMum, MumStudents.Name as NameMum, MumStudents.MidleName as MidleNameMum,
            //MumStudents.PassportData as PassDataMum, MumStudents.PassportNumber as PassNumMum, MumStudents.PassportSeria as PassSeriaMum,
            //MumStudents.PassportVID as PassVIDMum,MumStudents.PassportVidan as PassVidanMum, MumStudents.Phone1 as Phone1Mum, MumStudents.Phone2 as Phone2Mum,

            //DadStudents.ID as IDDadSt, DadStudents.Surname as SurnameDad, DadStudents.Name as NameDad, DadStudents.MidleName as MidleNameDad,
            //DadStudents.PassportData as PassDataDad, DadStudents.PassportNumber as PassNumDad, DadStudents.PassportSeria as PassSeriaDad,
            //DadStudents.PassportVID as PassVIDDad,DadStudents.PassportVidan as PassVidanDad, DadStudents.Phone1 as Phone1Dad, DadStudents.Phone2 as Phone2Dad

            //               from Students

            //               LEFT JOIN Polls on Students.IDPoll = Polls.ID
            //               LEFT JOIN Specialties on Students.IDSpecual = Specialties.ID
            //               LEFT JOIN Groups on Students.IDGrop = Groups.ID
            //               LEFT JOIN Users on Students.IDPyku = Users.ID
            //LEFT JOIN MumStudents on Students.IDMum = MumStudents.ID
            //LEFT JOIN DadStudents on Students.IDDad = DadStudents.ID
            //               ORDER BY SurnameSt";
            //               SQLiteCommand cmd = new SQLiteCommand(query, connection);
            //               DataTable DT = new DataTable("Students");
            //               DataTable dt = new DataTable();
            //               SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
            //               SDA.Fill(DT);
            //               GridBaseStudent.ItemsSource = DT.DefaultView;
            //               DataSet ds = new DataSet();
            //               cmd.ExecuteNonQuery();
            //               SDA.Fill(ds);
            //               dt = ds.Tables[0];

            //               SQLiteDataReader dr = null;
            //               dr = cmd.ExecuteReader();


            //               //Третий способ
            //               //app = new Excel.Application();
            //               //app.Visible = true;
            //               //app.DisplayAlerts = false;
            //               //wb = app.Workbooks.Add();
            //               //ws = wb.ActiveSheet;
            //               //GridBaseStudent.SelectAllCells();
            //               //GridBaseStudent.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            //               //ApplicationCommands.Copy.Execute(null, GridBaseStudent);
            //               //ws.Paste();
            //               //ws.Range["A1", "G1"].Font.Bold = true;
            //               //int number1 = ws.UsedRange.Rows.Count;
            //               //Microsoft.Office.Interop.Excel.Range myRange = ws.Range["A1", "G" + number1];
            //               //myRange.Borders.LineStyle = XlLineStyle.xlContinuous;
            //               //myRange.WrapText = false;
            //               //ws.Columns.EntireColumn.AutoFit();





            //               //Второй способ
            //               //Excel.Application excel = new Excel.Application();
            //               //excel.Visible = true;
            //               //Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            //               //Excel.Worksheet sheet1 = (Excel.Worksheet)workbook.Sheets[1];
            //               //int collInd = 0;
            //               //int rowInd = 0;
            //               //string data = "";
            //               //for (int i = 0; i < dt.Columns.Count; i++)
            //               //{
            //               //    data = dt.Columns[i].ColumnName.ToString();
            //               //    sheet1.Cells[1, i + 1] = data;

            //               //    //выделяем первую строку
            //               //    Excel.Range myRange = (Excel.Range)sheet1.get_Range("A1:Z1");

            //               //    //делаем полужирный текст и перенос слов
            //               //    myRange.WrapText = true;
            //               //    myRange.Font.Bold = true;
            //               //}
            //               //for (rowInd = 0; rowInd < dt.Rows.Count; rowInd++)
            //               //{
            //               //    for (collInd = 0; collInd < dt.Columns.Count; collInd++)
            //               //    {
            //               //        data = dt.Rows[rowInd].ItemArray[collInd].ToString();
            //               //        sheet1.Cells[rowInd + 2, collInd + 1] = data;
            //               //    }
            //               //}


            //               //Первый способ(надо докачивать)
            //               //Spire.Xls.Workbook book = new Spire.Xls.Workbook();
            //               //Spire.Xls.Worksheet sheet = book.Worksheets[0];
            //               //book.Worksheets[0].InsertDataTable(dt,true,1,1);
            //               //book.SaveToFile("sample.xlsx");
            //               //System.Diagnostics.Process.Start("sample.xlsx");
            //           }
            //           catch (Exception ex)
            //           {
            //               MessageBox.Show(ex.Message);
            //           }
            //       }
            //var options = new ExcelExportingOptions();
            //options.ExcelVersion = ExcelVersion.Excel2013;
            //var excelEngine = sfDataGrid.ExportToExcel(sfDataGrid.View, options);
            //var workBook = excelEngine.Excel.Workbooks[0];

            //IWorksheet sheet = workBook.Worksheets[0];

            //sheet.InsertColumn(1, 1, ExcelInsertOptions.FormatDefault);
            //var rowcount = this.sfDataGrid.RowGenerator.Items.Count;

            //for (int i = 1; i < rowcount; i++)
            //{
            //    sheet.Range["A" + (i + 1).ToString()].Number = i;
            //}

            //SaveFileDialog sfd = new SaveFileDialog
            //{
            //    FilterIndex = 2,
            //    Filter = "Excel 97 to 2003 Files(*.xls)|*.xls|Excel 2007 to 2010 Files(*.xlsx)|*.xlsx|Excel 2013 File(*.xlsx)|*.xlsx"
            //};

            //if (sfd.ShowDialog() == true)
            //{
            //    using (Stream stream = sfd.OpenFile())
            //    {
            //        if (sfd.FilterIndex == 1)
            //            workBook.Version = ExcelVersion.Excel97to2003;
            //        else if (sfd.FilterIndex == 2)
            //            workBook.Version = ExcelVersion.Excel2010;
            //        else
            //            workBook.Version = ExcelVersion.Excel2013;
            //        workBook.SaveAs(stream);
            //    }

            //    //Message box confirmation to view the created workbook.
            //    if (MessageBox.Show("Do you want to view the workbook?", "Workbook has been created",
            //                        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            //    {

            //        //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
            //        System.Diagnostics.Process.Start(sfd.FileName);
            //    }
            //}

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
    }
}
