using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using AccountingStudentData.Connection;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;
using DataTable = System.Data.DataTable;
using Image = System.Drawing.Image;
using TextBox = System.Windows.Controls.TextBox;
using Window = System.Windows.Window;

namespace AccountingStudentData.BoxWindows
{
    /// <summary>
    /// Логика взаимодействия для EddStudents.xaml
    /// </summary>
    public partial class EddStudents : Window
    {
        int CheckDad = 0, CheckMum = 0, Proverka1 = 0,IDOved1=0, IDOved2 = 0, IDOved3 = 0, IDOved4 = 0;
        string IDSt = string.Empty, IDGroup = string.Empty, IDPyk = string.Empty, IDSpec = string.Empty, Foto = string.Empty,
        OldNumberPasportSt = null, OldSeriaPasportSt = null, OldSNILSSt = null, OldOMSSt = null;
        byte[] image_bytes = null;

        public EddStudents(DataRowView drv)
        {
            InitializeComponent();
            CobBoxLoadPoll();
            CobBoxLoadGroup();
            IDSt = drv["IDSt"].ToString();
            SurnameSt.Text = drv["SurnameSt"].ToString();
            NameSt.Text = drv["NameSt"].ToString();
            MideleNameSt.Text = drv["MidleNameSt"].ToString();
            DtpSt.Text = drv["DataBirthSt"].ToString();
            Poll.Text = drv["PollSt"].ToString();
            PasportSt.Text = drv["PassVIDSt"].ToString();
            OldSeriaPasportSt = drv["PassSeriaSt"].ToString();
            SeriaPasportSt.Text = drv["PassSeriaSt"].ToString();
            NumberPasportSt.Text = drv["PassNumSt"].ToString();
            OldNumberPasportSt = drv["PassNumSt"].ToString();
            DtpPasportSt.Text = drv["PassDataSt"].ToString();
            VudanPasportSt.Text = drv["PassVidanSt"].ToString();
            GrStudent.Text = drv["PassCountrySt"].ToString();
            DataСredited.Text = drv["DataPost"].ToString();
            DataEnd.Text = drv["DataOkon"].ToString();
            NumberPrigaz.Text = drv["NumberPrikazSt"].ToString();
            NumberDogovora.Text = drv["NumberDogovorSt"].ToString();
            TxtNumberzatechBook.Text = drv["NumberZatechBook"].ToString();
            NumberPrigazKyrs1.Text = drv["NumberPrigazKyrs1"].ToString();
            DataСreditedKyrs1.Text = drv["DataСreditedKyrs1"].ToString();
            NumberPrigazKyrs2.Text = drv["NumberPrigazKyrs2"].ToString();
            DataСreditedKyrs2.Text = drv["DataСreditedKyrs2"].ToString();
            NumberPrigazKyrs3.Text = drv["NumberPrigazKyrs3"].ToString();
            DataСreditedKyrs3.Text = drv["DataСreditedKyrs3"].ToString();
            NumberPrigazKyrs4.Text = drv["NumberPrigazKyrs4"].ToString();
            DataСreditedKyrs4.Text = drv["DataСreditedKyrs4"].ToString();
            AdressStBirht.Text = drv["MestoBirthday"].ToString();
            if (NumberDogovora.Text == "Бюджет")
            {
                ChBxPlatObych.IsChecked = false;
                NumberDogovora.Text = string.Empty;
            }
            else
            {
                ChBxPlatObych.IsChecked = true;
            }
            LastObraz.Text = drv["KlassSt"].ToString();
            OrganizStudent.Text = drv["NameSchoolSt"].ToString();
            NumberAtestat.Text = drv["AtectSt"].ToString();
            DtnPolucheyne.Text = drv["DataPolecenSt"].ToString();
            IDPyk = drv["IDPyk"].ToString();
            IDSpec =  drv["IDSpecSt"].ToString();
            CbmGroup.Text = drv["GroupSt"].ToString();
            IDGroup = drv["IDGropSt"].ToString();
            OldSNILSSt = drv["SNILSSt"].ToString();
            SNILSSt.Text = drv["SNILSSt"].ToString();
            OMSSt.Text = drv["OMSSt"].ToString();
            OldOMSSt = drv["OMSSt"].ToString();
            PhoneSt1.Text = drv["Phone1St"].ToString();
            PhoneSt2.Text = drv["Phone2St"].ToString();
            AdressSt.Text = drv["AdressSt"].ToString();
            LoadFamule();
            image_bytes = (byte[])drv["FotoSt"];           
            BitmapImage img = new BitmapImage();            
            img.BeginInit();
            img.CreateOptions = BitmapCreateOptions.None;
            img.CacheOption = BitmapCacheOption.Default;
            img.StreamSource = new MemoryStream(image_bytes);          
            img.EndInit();
            FotoStudenta.Source = img;            
            CombBoxDowmload();

        }

        public void LoadFamule()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    string pr = "0";
                    for (int i = 1; i <= 4; i++)
                    {
                        var Surname = (UIElement)FindName("SurnameOtved" + i);
                        var Name = (UIElement)FindName("NameOtved" + i);
                        var MidleName = (UIElement)FindName("MideleNameOtved" + i);
                        var Pod = (UIElement)FindName("CmbRodOtved" + i);                       
                        var Phone1 = (UIElement)FindName("PhoneOtved" + i);
                        var Phone2 = (UIElement)FindName("PhoneDopOtved" + i);
                        var PassportVID = (UIElement)FindName("PasportOtved" + i);
                        var PassportVidan = (UIElement)FindName("VudanPasportOtved" + i);
                        var PassportNumber = (UIElement)FindName("NumberPasportOtved" + i);
                        var PassportSeria = (UIElement)FindName("SeriaPasportOtved" + i);
                        var PassportData = (UIElement)FindName("DtpPasportOtved" + i);
                        var PassportCountry = (UIElement)FindName("GrStudentOtved" + i);
                        var Work = (UIElement)FindName("WorkOtved" + i);
                        var WorkDol = (UIElement)FindName("WorkDolOtved" + i);
                        string qwert = $@"Select ID,Surname,Name,MidleName,Pod,Phone1,Phone2,PassportVID,PassportVidan,PassportNumber,PassportSeria,PassportData,
                        PassportCountry,Work,WorkDol from Responsible where Responsible.IsDelet = 0 and  ID > '{pr}' and IDStudent = {IDSt} ";
                        SQLiteCommand cmd = new SQLiteCommand(qwert, connection);                      
                        cmd.ExecuteNonQuery();
                        SQLiteDataReader dr = null;
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            pr = dr["ID"].ToString();                         
                            (Surname as TextBox).Text = dr["Surname"].ToString();
                            (Name as TextBox).Text = (string)dr["Name"];
                            (MidleName as TextBox).Text = (string)dr["MidleName"];
                            (Pod as ComboBox).Text = (string)dr["Pod"];
                            (Phone1 as TextBox).Text = (string)dr["Phone1"];
                            (Phone2 as TextBox).Text = (string)dr["Phone2"];
                            (PassportVID as TextBox).Text = (string)dr["PassportVID"];
                            (PassportVidan as TextBox).Text = (string)dr["PassportVidan"];
                            (PassportNumber as TextBox).Text = (string)dr["PassportNumber"];
                            (PassportSeria as TextBox).Text = (string)dr["PassportSeria"];
                            (PassportData as DatePicker).Text = (string)dr["PassportData"];
                            (PassportCountry as TextBox).Text = (string)dr["PassportCountry"];
                            (Work as TextBox).Text = (string)dr["Work"];
                            (WorkDol as TextBox).Text = (string)dr["WorkDol"];
                            if (i == 1)
                            {
                                IDOved1 = Convert.ToInt32(pr);
                            }
                            else if (i == 2)
                            {
                                IDOved2 = Convert.ToInt32(pr);
                            }
                            else if (i == 3)
                            {
                                IDOved3 = Convert.ToInt32(pr);
                            }
                            else if (i == 4)
                            {
                                IDOved4 = Convert.ToInt32(pr);
                            }
                            break;
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CobBoxLoadPoll()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT * FROM Polls";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable("Polls");
                    SDA.Fill(dt);
                    Poll.ItemsSource = dt.DefaultView;
                    Poll.DisplayMemberPath = "Name";
                    Poll.SelectedValuePath = "ID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void CobBoxLoadGroup()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT * FROM Groups";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable("Groups");
                    SDA.Fill(dt);
                    CbmGroup.ItemsSource = dt.DefaultView;
                    CbmGroup.DisplayMemberPath = "Name";
                    CbmGroup.SelectedValuePath = "ID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void CombBoxDowmload()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query1 = $@"SELECT ID, Surname,Name,MidleName  FROM Users where IDAllowance = 2 and ID = {IDPyk} ";
                    string query2 = $@"SELECT ID, Name as NameSpecial, NumberSpecial, Class from Specialties where ID = {IDSpec} ";;
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                    SQLiteDataAdapter SDA2 = new SQLiteDataAdapter(cmd2);
                    DataTable dt1 = new DataTable("Users");
                    DataTable dt2 = new DataTable("Specialties");;
                    SDA1.Fill(dt1);
                    SDA2.Fill(dt2);
                   
                    CbmPyk.ItemsSource = dt1.DefaultView;                   
                    CbmPyk.SelectedValuePath = "ID";
                    CbmPyk.SelectedIndex = 0;

                    CbmCpec.ItemsSource = dt2.DefaultView;
                    CbmCpec.SelectedValuePath = "ID";
                    CbmCpec.SelectedIndex = 0;


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void CheckText()
        {
            try
            {
                if (String.IsNullOrEmpty(SurnameSt.Text) || String.IsNullOrEmpty(NameSt.Text) || String.IsNullOrEmpty(DtpSt.Text) || String.IsNullOrEmpty(Poll.Text) ||
                                    String.IsNullOrEmpty(PasportSt.Text) || String.IsNullOrEmpty(NumberPasportSt.Text) || String.IsNullOrEmpty(SeriaPasportSt.Text) || String.IsNullOrEmpty(VudanPasportSt.Text) ||
                                    String.IsNullOrEmpty(GrStudent.Text) || String.IsNullOrEmpty(DataСredited.Text) || String.IsNullOrEmpty(DataEnd.Text) || String.IsNullOrEmpty(NumberPrigaz.Text))

                {
                    MessageBox.Show("Заполните информацию в вкладке: Основаня информация", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                    Proverka1 = 1;
                }else if (SeriaPasportSt.Text.Length != 4)
                {
                    MessageBox.Show("В серии паспорта должно быть 4 цифры", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                    Proverka1 = 1;
                }else if (NumberPasportSt.Text.Length != 6)
                {
                    MessageBox.Show("В номере паспорта должно быть 6 цифры", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                    Proverka1 = 1;
                }
                else if (ChBxPlatObych.IsChecked == true && NumberDogovora.Text == string.Empty)
                {
                    MessageBox.Show("Должен быть номер приказа при платном обучении", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                    Proverka1 = 1;
                }
                else if (SeriaPasportSt.Text.Length == 4 && NumberPasportSt.Text.Length == 6)
                {
                    if (String.IsNullOrEmpty(LastObraz.Text) || String.IsNullOrEmpty(OrganizStudent.Text) || String.IsNullOrEmpty(NumberAtestat.Text) || String.IsNullOrEmpty(DtnPolucheyne.Text)
                        || String.IsNullOrEmpty(CbmPyk.Text) || String.IsNullOrEmpty(CbmCpec.Text) || String.IsNullOrEmpty(CbmGroup.Text) || String.IsNullOrEmpty(SNILSSt.Text)
                        || String.IsNullOrEmpty(OMSSt.Text) || String.IsNullOrEmpty(PhoneSt1.Text) || String.IsNullOrEmpty(AdressSt.Text))
                    {
                        MessageBox.Show("Заполните информацию в вкладке: Доп. Информация", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        Proverka1 = 1;
                    }else if (NumberAtestat.Text.Length != 14)
                    {
                        MessageBox.Show("В номер атестата должно быть 14 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        Proverka1 = 1;
                    }else if (SNILSSt.Text.Length !=11)
                    {
                        MessageBox.Show("В номере СНИЛСа должно быть 11 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        Proverka1 = 1;
                    }
                    else if (OMSSt.Text.Length !=16)
                    {
                        MessageBox.Show("В номере ОМСа должно быть 16 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        Proverka1 = 1;
                    }
                    else if (PhoneSt1.Text.Length !=11)
                    {
                        MessageBox.Show("В номер телефона должно быть 11 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        Proverka1 = 1;
                    }
                    else if (PhoneSt2.Text.Length !=11 && PhoneSt2.Text != string.Empty)
                    {
                        MessageBox.Show("В номер телефона должно быть 11 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        Proverka1 = 1;
                    }
                    else if (NumberAtestat.Text.Length == 14 && OMSSt.Text.Length == 16 && PhoneSt1.Text.Length == 11 && PhoneSt1.Text.Length == 11)
                    {/*
                        if (checkBoxDad.IsChecked == true)
                        {
                            if (String.IsNullOrEmpty(SurnameDad.Text) || String.IsNullOrEmpty(NameDad.Text) || String.IsNullOrEmpty(PhoneDad.Text) || String.IsNullOrEmpty(PasportDad.Text) ||
                               String.IsNullOrEmpty(NumberPasportDad.Text) || String.IsNullOrEmpty(SeriaPasportDad.Text) || String.IsNullOrEmpty(VudanPasportDad.Text)
                               || String.IsNullOrEmpty(GrStudentDad.Text) || String.IsNullOrEmpty(DtpPasportDad.Text))
                            {
                                MessageBox.Show("Заполните информацию в данных родитель(Отец)", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckDad = 1;
                               // Proverka1 = 1;
                            }else if (PhoneDad.Text.Length != 11)
                            {
                                MessageBox.Show("В номере телефона должно быть 11 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckDad = 1;
                               // Proverka1 = 1;
                            }
                            else if (PhoneDad2.Text.Length != 11 && PhoneDad2.Text != string.Empty)
                            {
                                MessageBox.Show("В номере телефона должно быть 11 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckDad = 1;
                               // Proverka1 = 1;
                            }
                            else if (NumberPasportDad.Text.Length !=6)
                            {
                                MessageBox.Show("В номере паспорта должно быть 6 цифры", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckDad = 1;
                              //  Proverka1 = 1;
                            }
                            else if (SeriaPasportDad.Text.Length != 4)
                            {
                                MessageBox.Show("В серии паспорта должно быть 4 цифры", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckDad = 1;
                               // Proverka1 = 1;
                            }
                            else if (PhoneDad.Text.Length == 11  && NumberPasportDad.Text.Length == 6 && SeriaPasportDad.Text.Length == 4)
                            {
                                CheckDad = 0;
                            }
                            
                        }
                        if (checkBoxMum.IsChecked == true)
                        {
                            if (String.IsNullOrEmpty(SurnameMum.Text) || String.IsNullOrEmpty(NameMum.Text) || String.IsNullOrEmpty(PhoneMum.Text) || String.IsNullOrEmpty(PasportMum.Text) ||
                              String.IsNullOrEmpty(NumberPasportMum.Text) || String.IsNullOrEmpty(SeriaPasportMum.Text) || String.IsNullOrEmpty(VudanPasportMum.Text) || String.IsNullOrEmpty(DtpPasportMum.Text)
                              || String.IsNullOrEmpty(GrStudentMum.Text))
                            {
                                MessageBox.Show("Заполните информацию в данных родитель(Мать)", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckMum = 1;
                               // Proverka1 = 1;
                            }
                            else if (PhoneMum.Text.Length != 11)
                            {
                                MessageBox.Show("В номере телефона должно быть 11 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckMum = 1;
                               // Proverka1 = 1;
                            }
                            else if (PhoneMum2.Text.Length != 11 && PhoneMum2.Text != string.Empty)
                            {
                                MessageBox.Show("В номере телефона должно быть 11 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckMum = 1;
                                //Proverka1 = 1;
                            }
                            else if (NumberPasportMum.Text.Length != 6)
                            {
                                MessageBox.Show("В номере паспорта должно быть 6 цифры", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckMum = 1;
                               // Proverka1 = 1;
                            }
                            else if (SeriaPasportMum.Text.Length != 4)
                            {
                                MessageBox.Show("В серии паспорта должно быть 4 цифры", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckMum = 1;
                                //Proverka1 = 1;
                            }
                            else if (PhoneMum.Text.Length == 11 && NumberPasportMum.Text.Length == 6 && SeriaPasportMum.Text.Length == 4)
                            {
                                CheckMum = 0;
                            }                          
                        }
                        if (checkBoxDad.IsChecked == false && checkBoxMum.IsChecked == false)
                        {
                            MessageBox.Show("Выберите хотябы одного родителя", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                            Proverka1 = 1;
                        }
                        else*/ if (CheckDad == 0 && CheckMum == 0)
                        {
                            Proverka1 = 0;
                        }
                        else
                        {
                            Proverka1 = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void EddStudent()
        {
            try
            {
                if (Proverka1 == 0)
                {
                    using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                    {
                        connection.Open();
                        int ProverkaPassportSt = 0;
                        if (OldNumberPasportSt != NumberPasportSt.Text && OldSeriaPasportSt != SeriaPasportSt.Text)
                        {
                            string query = $@"SELECT count () FROM Students WHERE PassportNumber = '{NumberPasportSt.Text}' and PassportSeria = '{SeriaPasportSt.Text}' ";
                            SQLiteCommand cmd = new SQLiteCommand(query, connection);
                            ProverkaPassportSt = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        if (ProverkaPassportSt == 0) //Проверка номера и серии паспорта у студента
                        {
                            int ProverkaMedSt = 0;
                            if (OldOMSSt != OMSSt.Text && OldSNILSSt != SNILSSt.Text)
                            {
                                string query = $@"SELECT count () FROM Students WHERE SNILS = '{SNILSSt.Text}' or OMS = '{OMSSt.Text}' ";
                                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                                ProverkaMedSt = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            if (ProverkaMedSt == 0)//Проверка снилса и омс у студента
                            {

                                if (IDOved1 == 0)
                                {
                                    if (SurnameOtved1.Text != string.Empty)
                                    {
                                        String txtOtved1 = CmbRodOtved1.Text;
                                        string query = $@"INSERT INTO Responsible ('IDStudent','Surname','Name','MidleName','Pod','Phone1','Phone2','PassportVID','PassportVidan','PassportNumber','PassportSeria','PassportData','PassportCountry','Work','WorkDol',IsDelet)
                                        values ('{IDSt}','{SurnameOtved1.Text}','{NameOtved1.Text}','{MideleNameOtved1.Text}','{txtOtved1}','{PhoneOtved1.Text.ToLower()}','{PhoneDopOtved1.Text.ToLower()}','{PasportOtved1.Text.ToLower()}',
                                        '{VudanPasportOtved1.Text.ToLower()}','{NumberPasportOtved1.Text.ToLower()}','{SeriaPasportOtved1.Text.ToLower()}','{DtpPasportOtved1.Text.ToLower()}','{GrStudentOtved1.Text.ToUpper()}','{WorkOtved1.Text}','{WorkDolOtved1.Text}',0)";
                                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else if (IDOved1 != 0)
                                {
                                    if (SurnameOtved1.Text != string.Empty)
                                    {
                                        String txtOtved1 = CmbRodOtved1.Text;
                                        string qwert = $@"UPDATE Responsible SET Surname='{SurnameOtved1.Text}', Name='{NameOtved1.Text}',MidleName='{MideleNameOtved1.Text}', Pod = '{txtOtved1}',Phone1='{PhoneOtved1.Text.ToLower()}', Phone2='{PhoneDopOtved1.Text.ToLower()}', 
                                            PassportVID='{PasportOtved1.Text.ToLower()}',PassportVidan= '{VudanPasportOtved1.Text.ToLower()}', PassportNumber='{NumberPasportOtved1.Text}',PassportSeria='{SeriaPasportOtved1.Text}',PassportData='{DtpPasportOtved1.Text.ToLower()}',PassportCountry = '{GrStudentOtved1.Text.ToUpper()}' , Work ='{WorkOtved1.Text}', WorkDol ='{WorkDolOtved1.Text}' WHERE ID='{IDOved1}';";
                                        SQLiteCommand cmd = new SQLiteCommand(qwert, connection);
                                        cmd.ExecuteScalar();
                                    }
                                    else
                                    {
                                        string qwert = $@"UPDATE Responsible SET IsDelet = 1 WHERE ID='{IDOved1}';";
                                        SQLiteCommand cmd = new SQLiteCommand(qwert, connection);
                                        cmd.ExecuteScalar();
                                    }
                                }
                                if (IDOved2 == 0)
                                {
                                    if (SurnameOtved2.Text != string.Empty)
                                    {
                                        String txtOtved2 = CmbRodOtved2.Text;
                                        string query = $@"INSERT INTO Responsible ('IDStudent','Surname','Name','MidleName','Pod','Phone1','Phone2','PassportVID','PassportVidan','PassportNumber','PassportSeria','PassportData','PassportCountry','Work','WorkDol',IsDelet)
                                        values ('{IDSt}','{SurnameOtved2.Text}','{NameOtved2.Text}','{MideleNameOtved2.Text}','{txtOtved2}','{PhoneOtved2.Text.ToLower()}','{PhoneDopOtved2.Text.ToLower()}','{PasportOtved2.Text.ToLower()}',
                                        '{VudanPasportOtved2.Text.ToLower()}','{NumberPasportOtved2.Text.ToLower()}','{SeriaPasportOtved2.Text.ToLower()}','{DtpPasportOtved2.Text.ToLower()}','{GrStudentOtved2.Text.ToUpper()}','{WorkOtved2.Text}','{WorkDolOtved2.Text}',0)";
                                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else if (IDOved2 != 0)
                                {
                                    if (SurnameOtved2.Text != string.Empty)
                                    {
                                        String txtOtved2 = CmbRodOtved2.Text;
                                        string qwert = $@"UPDATE Responsible SET Surname='{SurnameOtved2.Text}', Name='{NameOtved2.Text}',MidleName='{MideleNameOtved2.Text}', Pod = '{txtOtved2}', Phone1='{PhoneOtved2.Text.ToLower()}', Phone2='{PhoneDopOtved2.Text.ToLower()}', 
                                            PassportVID='{PasportOtved2.Text.ToLower()}',PassportVidan= '{VudanPasportOtved2.Text.ToLower()}', PassportNumber='{NumberPasportOtved2.Text}',PassportSeria='{SeriaPasportOtved2.Text}',PassportData='{DtpPasportOtved2.Text.ToLower()}',PassportCountry = '{GrStudentOtved2.Text.ToUpper()}' , Work ='{WorkOtved2.Text}', WorkDol ='{WorkDolOtved2.Text}' WHERE ID='{IDOved2}';";
                                        SQLiteCommand cmd = new SQLiteCommand(qwert, connection);
                                        cmd.ExecuteScalar();
                                    }
                                    else
                                    {
                                        string qwert = $@"UPDATE Responsible SET IsDelet = 1 WHERE ID='{IDOved2}';";
                                        SQLiteCommand cmd = new SQLiteCommand(qwert, connection);
                                        cmd.ExecuteScalar();
                                    }
                                }
                                if (IDOved3 == 0)
                                {
                                    if (SurnameOtved3.Text != string.Empty)
                                    {
                                        String txtOtved3 = CmbRodOtved3.Text;
                                        string query = $@"INSERT INTO Responsible ('IDStudent','Surname','Name','MidleName','Pod','Phone1','Phone2','PassportVID','PassportVidan','PassportNumber','PassportSeria','PassportData','PassportCountry','Work','WorkDol',IsDelet)
                                        values ('{IDSt}','{SurnameOtved3.Text}','{NameOtved3.Text}','{MideleNameOtved3.Text}','{txtOtved3}','{PhoneOtved3.Text.ToLower()}','{PhoneDopOtved3.Text.ToLower()}','{PasportOtved3.Text.ToLower()}',
                                        '{VudanPasportOtved3.Text.ToLower()}','{NumberPasportOtved3.Text.ToLower()}','{SeriaPasportOtved3.Text.ToLower()}','{DtpPasportOtved3.Text.ToLower()}','{GrStudentOtved3.Text.ToUpper()}','{WorkOtved3.Text}','{WorkDolOtved3.Text}',0)";
                                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else if (IDOved3 != 0)
                                {
                                    if (SurnameOtved3.Text != string.Empty)
                                    {
                                        String txtOtved3 = CmbRodOtved3.Text;
                                        string qwert = $@"UPDATE Responsible SET Surname='{SurnameOtved3.Text}', Name='{NameOtved3.Text}',MidleName='{MideleNameOtved3.Text}', Pod = '{txtOtved3}', Phone1='{PhoneOtved3.Text.ToLower()}', Phone2='{PhoneDopOtved3.Text.ToLower()}', 
                                            PassportVID='{PasportOtved3.Text.ToLower()}',PassportVidan= '{VudanPasportOtved3.Text.ToLower()}', PassportNumber='{NumberPasportOtved3.Text}',PassportSeria='{SeriaPasportOtved3.Text}',PassportData='{DtpPasportOtved3.Text.ToLower()}',PassportCountry = '{GrStudentOtved3.Text.ToUpper()}' , Work ='{WorkOtved3.Text}', WorkDol ='{WorkDolOtved3.Text}' WHERE ID='{IDOved3}';";
                                        SQLiteCommand cmd = new SQLiteCommand(qwert, connection);
                                        cmd.ExecuteScalar();
                                    }
                                    else
                                    {
                                        string qwert = $@"UPDATE Responsible SET IsDelet = 1 WHERE ID='{IDOved3}';";
                                        SQLiteCommand cmd = new SQLiteCommand(qwert, connection);
                                        cmd.ExecuteScalar();
                                    }
                                }
                                if (IDOved4 == 0)
                                {
                                    if (SurnameOtved4.Text != string.Empty)
                                    {
                                        String txtOtved4 = CmbRodOtved4.Text;
                                        string query = $@"INSERT INTO Responsible ('IDStudent','Surname','Name','MidleName','Pod','Phone1','Phone2','PassportVID','PassportVidan','PassportNumber','PassportSeria','PassportData','PassportCountry','Work','WorkDol',IsDelet)
                                        values ('{IDSt}','{SurnameOtved4.Text}','{NameOtved4.Text}','{MideleNameOtved4.Text}','{txtOtved4}','{PhoneOtved4.Text.ToLower()}','{PhoneDopOtved4.Text.ToLower()}','{PasportOtved4.Text.ToLower()}',
                                        '{VudanPasportOtved4.Text.ToLower()}','{NumberPasportOtved4.Text.ToLower()}','{SeriaPasportOtved4.Text.ToLower()}','{DtpPasportOtved4.Text.ToLower()}','{GrStudentOtved4.Text.ToUpper()}','{WorkOtved4.Text}','{WorkDolOtved4.Text}',0)";
                                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else if (IDOved3 != 0)
                                {
                                    if (SurnameOtved4.Text != string.Empty)
                                    {
                                        String txtOtved4 = CmbRodOtved4.Text;
                                        string qwert = $@"UPDATE Responsible SET Surname='{SurnameOtved4.Text}', Name='{NameOtved4.Text}',MidleName='{MideleNameOtved4.Text}', Pod = '{txtOtved4}', Phone1='{PhoneOtved4.Text.ToLower()}', Phone2='{PhoneDopOtved4.Text.ToLower()}', 
                                            PassportVID='{PasportOtved4.Text.ToLower()}',PassportVidan= '{VudanPasportOtved4.Text.ToLower()}', PassportNumber='{NumberPasportOtved4.Text}',PassportSeria='{SeriaPasportOtved4.Text}',PassportData='{DtpPasportOtved4.Text.ToLower()}',PassportCountry = '{GrStudentOtved4.Text.ToUpper()}' , Work ='{WorkOtved4.Text}', WorkDol ='{WorkDolOtved4.Text}' WHERE ID='{IDOved4}';";
                                        SQLiteCommand cmd = new SQLiteCommand(qwert, connection);
                                        cmd.ExecuteScalar();
                                    }
                                    else
                                    {
                                        string qwert = $@"UPDATE Responsible SET IsDelet = 1 WHERE ID='{IDOved4}';";
                                        SQLiteCommand cmd = new SQLiteCommand(qwert, connection);
                                        cmd.ExecuteScalar();
                                    }
                                }                            
                               
                               
                                    
                                    bool result1 = int.TryParse(Poll.SelectedValue.ToString(), out int IDPoll);
                                    bool result2 = int.TryParse(CbmCpec.SelectedValue.ToString(), out int IDCpec);
                                    bool result3 = int.TryParse(CbmGroup.SelectedValue.ToString(), out int IDGroup);
                                    bool result4 = int.TryParse(CbmPyk.SelectedValue.ToString(), out int IDPyk);
                                    string query1 = $@"UPDATE  Students Set Surname='{SurnameSt.Text}',Name = '{NameSt.Text}',MidleName= '{MideleNameSt.Text}', Phone1= '{PhoneSt1.Text}', Phone2= '{PhoneSt2.Text}', SNILS = '{SNILSSt.Text}', OMS= '{OMSSt.Text}', 
                                    Adress= '{AdressSt.Text}', PassportVid= '{PasportSt.Text}', PassportVidan= '{VudanPasportSt.Text.ToUpper()}', PassportNumber= '{NumberPasportSt.Text}', PassportSeria= '{SeriaPasportSt.Text}',
                                    PassportData= '{DtpPasportSt.Text}', IDPoll= '{IDPoll}', IDSpecual= '{IDCpec}', IDGrop= '{IDGroup}', IDPyku= '{IDPyk}', PocleKlass= '{LastObraz.Text}', NameSchool= '{OrganizStudent.Text}', NumberAtect= '{NumberAtestat.Text}', DataPolycen= '{DtnPolucheyne.Text}', Foto=@Foto, DataСredited= '{DataСredited.Text}', DataEnd= '{DataEnd.Text}', NumberPrikaz = '{NumberPrigaz.Text}', NumberDogovora=@NumberDogovora,
                                    NumberzatechBook = '{TxtNumberzatechBook.Text}',NumberPrigazKyrs1 = '{NumberPrigazKyrs1.Text}' ,DataСreditedKyrs1 ='{DataСreditedKyrs1.Text}' ,NumberPrigazKyrs2 = '{NumberPrigazKyrs2.Text}',DataСreditedKyrs2 = '{DataСreditedKyrs1.Text}',NumberPrigazKyrs3 = '{NumberPrigazKyrs3.Text}',DataСreditedKyrs3= '{DataСreditedKyrs1.Text}',NumberPrigazKyrs4= '{NumberPrigazKyrs1.Text}',DataСreditedKyrs4 = '{DataСreditedKyrs1.Text}', MestoBirthday = '{AdressStBirht.Text}'
                                    where ID = {IDSt} ";
                                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                                    byte[] bytes = null;                                   
                                    if (image_bytes == null)
                                    {
                                        bytes = File.ReadAllBytes("Foto/notfoto.jpg");
                                        cmd1.Parameters.AddWithValue("@Foto", bytes);
                                    }
                                    else if (image_bytes != null)
                                    {
                                        cmd1.Parameters.AddWithValue("@Foto", image_bytes);
                                    }
                                    if (ChBxPlatObych.IsChecked == true)
                                    {
                                        cmd1.Parameters.AddWithValue("@NumberDogovora", NumberDogovora.Text);
                                    }
                                    else if (ChBxPlatObych.IsChecked == false)
                                    {
                                        cmd1.Parameters.AddWithValue("@NumberDogovora", "Бюджет");
                                    }
                                    cmd1.ExecuteScalar();
                                    MessageBox.Show("Данные студента изменены", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);                                                               
                            }
                            else
                            {
                                MessageBox.Show("Такой СНИЛС и ОМС уже используется у студента", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Такой номер и серия паспорта уже используется у студента", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void TextValidationTextBox(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationNumberDate(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationNumberPassport(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-ZА-яА-я]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationNumberClass(object sender, TextCompositionEventArgs e)
        {
            TextBox box = (TextBox)sender;
            Regex regex = new Regex("[^91]+");
            e.Handled = regex.IsMatch(e.Text);

        }
        private void TextInputForSeriaPassport(object sender, TextCompositionEventArgs e)
        {
            TextBox box = (TextBox)sender;
            e.Handled = box.Text.Length > 4;
        }
 
        private void checkBoxMum_Checked(object sender, RoutedEventArgs e)
        {
            StpMum.IsEnabled = true;
        }

        private void checkBoxMum_Unchecked(object sender, RoutedEventArgs e)
        {
            StpMum.IsEnabled = false;
        }

        private void checkBoxDad_Unchecked(object sender, RoutedEventArgs e)
        {
            //StpDad.IsEnabled = false;
        }

        private void checkBoxDad_Checked(object sender, RoutedEventArgs e)
        {
           // StpDad.IsEnabled = true;
        }

        public void AddFoto()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                image_bytes = File.ReadAllBytes(op.FileName); // получаем байты выбранного файла
                FotoStudenta.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void BtnAddFoto_Click(object sender, RoutedEventArgs e)
        {
            AddFoto();
        }

        private void ChBxPlatObych_Checked(object sender, RoutedEventArgs e)
        {
            NumberDogovora.IsEnabled = true;
        }
        private void ChBxPlatObych_Unchecked(object sender, RoutedEventArgs e)
        {
            NumberDogovora.IsEnabled = false;
        }

        private void BtnArxiv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    string query1 = $@"Update Students set IsDelet=1 WHERE ID='{IDSt}' ";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                    cmd1.ExecuteScalar();
                    MessageBox.Show("Студент отправлен в архив", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CbmCpec_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                String textcomb = CbmCpec.Text;
                if (textcomb == "")
                {
                    CbmCpec.SelectedIndex = 0;
                }
                else
                {
                    bool result1 = int.TryParse(CbmCpec.SelectedValue.ToString(), out int IDSpec);
                    using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                    {
                        string query1 = $@"SELECT ID, Name as NameSpecial, NumberSpecial, Class from Specialties where  ID = {IDSpec} ";
                        SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                        SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                        DataTable dt1 = new DataTable("Specialties");
                        SDA1.Fill(dt1);
                        CbmCpec.ItemsSource = dt1.DefaultView;
                        CbmCpec.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CbmCpec_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    string query1 = $@"SELECT ID, Name as NameSpecial, NumberSpecial, Class from Specialties ";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                    SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable("Specialties");
                    SDA1.Fill(dt1);
                    CbmCpec.ItemsSource = dt1.DefaultView;
                    CbmCpec.SelectedValuePath = "ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CbmPyk_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    string query1 = $@"SELECT ID, Surname,Name,MidleName  FROM Users where IDAllowance = 2 ";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                    SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                    DataTable dt1 = new DataTable("Users");
                    SDA1.Fill(dt1);
                    CbmPyk.ItemsSource = dt1.DefaultView;
                    CbmPyk.SelectedValuePath = "ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CbmPyk_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                String textcomb = CbmPyk.Text;
                if (textcomb == "")
                {
                    CbmPyk.SelectedIndex = 0;
                }
                else
                {
                    bool result1 = int.TryParse(CbmPyk.SelectedValue.ToString(), out int IDPyk);
                    using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                    {
                        string query1 = $@"SELECT ID, Surname,Name,MidleName  FROM Users where IDAllowance = 2 and ID = {IDPyk} ";
                        SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                        SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                        DataTable dt1 = new DataTable("Users");
                        SDA1.Fill(dt1);
                        CbmPyk.ItemsSource = dt1.DefaultView;
                        CbmPyk.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDellFoto_Click(object sender, RoutedEventArgs e)
        {
            var uriSource = new Uri(@"/Foto/notfoto.jpg", UriKind.Relative);
            FotoStudenta.Source = new BitmapImage(uriSource);
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            CheckText();
            EddStudent();
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

        private void Expander1_Expanded(object sender, RoutedEventArgs e)
        {
            Expander2.IsExpanded = false;
            Expander3.IsExpanded = false;
            Expander4.IsExpanded = false;
        }
        private void Expander2_Expanded(object sender, RoutedEventArgs e)
        {
            Expander1.IsExpanded = false;
            Expander3.IsExpanded = false;
            Expander4.IsExpanded = false;
        }
        private void Expander3_Expanded(object sender, RoutedEventArgs e)
        {
            Expander1.IsExpanded = false;
            Expander2.IsExpanded = false;
            Expander4.IsExpanded = false;
        }
        private void Expander4_Expanded(object sender, RoutedEventArgs e)
        {
            Expander1.IsExpanded = false;
            Expander2.IsExpanded = false;
            Expander3.IsExpanded = false;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MnItClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}

