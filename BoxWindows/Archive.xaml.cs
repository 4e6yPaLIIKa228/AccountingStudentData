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
    public partial class Archive : Window
    {
        public Archive()
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
                    where Students.IsDelet = 1 
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




        private void MnItClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
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
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MnItListStudents_Click(object sender, RoutedEventArgs e)
        {
            StudentBase eddst = new StudentBase();
            eddst.Show();
            this.Close();
        }
    }
}
