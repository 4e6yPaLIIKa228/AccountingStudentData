using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
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
using Image = System.Drawing.Image;

namespace AccountingStudentData.BoxWindows
{
    /// <summary>
    /// Логика взаимодействия для AddStudents.xaml
    /// </summary>
    public partial class AddStudents : Window
    {
        int CheckDad = 0, CheckMum = 0,Proverka1 = 0, ProverkaFoto = 0;
        public AddStudents()
        {
            InitializeComponent();
            CombBoxDowmload();
        }
        public void CombBoxDowmload()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT * FROM Polls";
                    string query1 = $@"SELECT ID, Surname,Name,MiddleName  FROM Users where IDAllowance = 2 ";
                    string query2 = $@"SELECT ID, Name as NameSpecial, NumberSpecial, Class from Specialties ";
                    string query3 = $@"SELECT ID, Name from Groups ";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    SQLiteCommand cmd3 = new SQLiteCommand(query3, connection);
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                    SQLiteDataAdapter SDA2 = new SQLiteDataAdapter(cmd2);
                    SQLiteDataAdapter SDA3 = new SQLiteDataAdapter(cmd3);
                    DataTable dt = new DataTable("Polls");
                    DataTable dt1 = new DataTable("Users");
                    DataTable dt2 = new DataTable("Specialties");
                    DataTable dt3 = new DataTable("Groups");
                    SDA.Fill(dt);
                    SDA1.Fill(dt1);
                    SDA2.Fill(dt2);
                    SDA3.Fill(dt3);
                    Poll.ItemsSource = dt.DefaultView;
                    Poll.DisplayMemberPath = "Name";
                    Poll.SelectedValuePath = "ID";
                    CbmPyk.ItemsSource = dt1.DefaultView;
                    CbmPyk.SelectedValuePath = "ID";                  
                    CbmCpec.ItemsSource = dt2.DefaultView;
                    CbmCpec.SelectedValuePath = "ID";
                    CbmGroup.ItemsSource = dt3.DefaultView;
                    CbmGroup.SelectedValuePath = "ID";

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void CheackText()
        {
            try
            {
                if (String.IsNullOrEmpty(SurnameSt.Text) || String.IsNullOrEmpty(NameSt.Text) || String.IsNullOrEmpty(DtpSt.Text) || String.IsNullOrEmpty(Poll.Text) ||
                                    String.IsNullOrEmpty(PasportSt.Text) || String.IsNullOrEmpty(NumberPasportSt.Text) || String.IsNullOrEmpty(SeriaPasportSt.Text) || String.IsNullOrEmpty(VudanPasportSt.Text) ||
                                    String.IsNullOrEmpty(GrStudent.Text) || String.IsNullOrEmpty(DataСredited.Text) || String.IsNullOrEmpty(DataEnd.Text) || String.IsNullOrEmpty(NumberPrigaz.Text))
                                  
                {
                    MessageBox.Show("Заполните информацию в вкладке: Основаня информация", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (String.IsNullOrEmpty(LastObraz.Text) || String.IsNullOrEmpty(OrganizStudent.Text) || String.IsNullOrEmpty(NumberAtestat.Text) || String.IsNullOrEmpty(DtnPolucheyne.Text)
                        || String.IsNullOrEmpty(CbmPyk.Text) || String.IsNullOrEmpty(CbmCpec.Text) || String.IsNullOrEmpty(CbmGroup.Text) || String.IsNullOrEmpty(SNILSSt.Text)
                        || String.IsNullOrEmpty(OMSSt.Text) || String.IsNullOrEmpty(PhoneSt1.Text) || String.IsNullOrEmpty(AdressSt.Text))
                    {
                        MessageBox.Show("Заполните информацию в вкладке: Доп. Информация", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (checkBoxDad.IsChecked == true)
                        {
                            if (String.IsNullOrEmpty(SurnameDad.Text) || String.IsNullOrEmpty(NameDad.Text) || String.IsNullOrEmpty(PhoneDad.Text) || String.IsNullOrEmpty(PasportDad.Text) ||
                               String.IsNullOrEmpty(NumberPasportDad.Text) || String.IsNullOrEmpty(SeriaPasportDad.Text) || String.IsNullOrEmpty(VudanPasportDad.Text)
                               || String.IsNullOrEmpty(GrStudentDad.Text))
                            {
                                MessageBox.Show("Заполните информацию в данных родитель(Отец)", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckDad = 1;
                                Proverka1 = 1;
                            }
                            else
                            {
                                CheckDad = 0;
                                Proverka1=0;
                            }
                        }
                        if (checkBoxMum.IsChecked == true)
                        {
                            if (String.IsNullOrEmpty(SurnameMum.Text) || String.IsNullOrEmpty(NameMum.Text) || String.IsNullOrEmpty(PhoneMum.Text) || String.IsNullOrEmpty(PasportMum.Text) ||
                              String.IsNullOrEmpty(NumberPasportMum.Text) || String.IsNullOrEmpty(SeriaPasportMum.Text) || String.IsNullOrEmpty(VudanPasportMum.Text)
                              || String.IsNullOrEmpty(GrStudentMum.Text))
                            {
                                MessageBox.Show("Заполните информацию в данных родитель(Мать)", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckMum = 1;
                                Proverka1=1;
                            }
                            else
                            {
                                CheckMum = 0;
                                Proverka1 = 0;
                            }
                        }
                        if (checkBoxDad.IsChecked == false && checkBoxMum.IsChecked == false)
                        {
                            MessageBox.Show("Выберите хотябы одного родителя", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddStudent()
        {
            try
            {
                if (Proverka1 == 0)
                {
                    using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                    {
                        connection.Open();
                        string query = $@"SELECT count (Number) FROM Students WHERE PassportNumber = '{NumberPasportSt.Text}' and PasssportSeria = '{SeriaPasportSt.Text}' ";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        int ProverkaPassportSt = Convert.ToInt32(cmd.ExecuteScalar());
                        if (ProverkaPassportSt == 0) //Проверка номера и серии паспорта у студента
                        {
                            query = $@"SELECT count (Number) FROM Students WHERE Polus = '{SNILSSt.Text}' and OMS = '{OMSSt.Text}' ";
                            cmd = new SQLiteCommand(query, connection);
                            int ProverkaMedSt= Convert.ToInt32(cmd.ExecuteScalar());
                            if (ProverkaMedSt == 0)//Проверка снилса и омс у студента
                            {
                                query = $@"SELECT count () FROM MumStudents WHERE PasspoortSeria = '{NumberPasportMum.Text}' and PassportNumber = '{NumberPasportMum.Text}' ";
                                cmd = new SQLiteCommand(query, connection);
                                int ProverkaMum = Convert.ToInt32(cmd.ExecuteScalar());
                                query = $@"SELECT count () FROM DadStudents WHERE PasspoortSeria = '{NumberPasportDad.Text}' and PassportNumber = '{SeriaPasportDad.Text}' ";
                                cmd = new SQLiteCommand(query, connection);
                                int ProverkaDad= Convert.ToInt32(cmd.ExecuteScalar());
                                if (ProverkaMum == 0 && ProverkaDad == 0) //Проверка номера и серии паспорта у родителей
                                {
                                    int IDMum = 0; 
                                    if (checkBoxMum.IsChecked == true)
                                    {
                                        query = $@"INSERT INTO MumStudents('Surname','Name','MidleName','Phone1','Phone2','PassportVID','PassportVidan','PassportNumber','PassportSeria','PassportData')
                                        values ('{SurnameMum.Text.ToLower()}','{NameMum.Text.ToLower()}','{MideleNameMum.Text.ToLower()}','{PhoneMum.Text.ToLower()}',{PhoneMum2.Text.ToLower()}','{PasportMum.Text.ToLower()}',
                                        '{VudanPasportMum.Text.ToLower()}','{NumberPasportMum.Text.ToLower()}','{SeriaPasportMum.Text.ToLower()}','{DtpPasportMum.Text.ToLower()}')";
                                        cmd = new SQLiteCommand(query, connection);
                                        query = $@"SELECT ID FROM MumStudents WHERE Surname = '{SurnameMum.Text.ToLower()}' and Name = '{NameMum.Text.ToLower()}' and MidleName = '{MideleNameMum.Text.ToLower()}' and  Phone1 = '{PhoneMum.Text.ToLower()}' and Phone2 = '{PhoneMum2.Text.ToLower()}' and PassportVID = '{PasportMum.Text.ToLower()}'
                                        and PassportVidan = '{VudanPasportMum.Text.ToLower()}' and  PassportNumber = '{NumberPasportMum.Text.ToLower()}' and  PassportSeria ='{SeriaPasportMum.Text.ToLower()}' and PassportData = '{DtpPasportMum.Text.ToLower()}' ";
                                        cmd = new SQLiteCommand(query, connection);
                                        IDMum = Convert.ToInt32(cmd.ExecuteScalar());
                                    }
                                    int IDDad = 0;
                                    if (checkBoxDad.IsChecked == true)
                                    {
                                        query = $@"INSERT INTO DadStudents('Surname','Name','MidleName','Phone1','Phone2','PassportVID','PassportVidan','PassportNumber','PassportSeria','PassportData')
                                        values ('{SurnameDad.Text.ToLower()}','{NameDad.Text.ToLower()}','{MideleNameDad.Text.ToLower()}','{PhoneDad.Text.ToLower()}',{PhoneDad.Text.ToLower()}','{PasportDad.Text.ToLower()}',
                                        '{VudanPasportDad.Text.ToLower()}','{NumberPasportDad.Text.ToLower()}','{SeriaPasportDad.Text.ToLower()}','{DtpPasportDad.Text.ToLower()}')";
                                        cmd = new SQLiteCommand(query, connection);
                                        query = $@"SELECT ID FROM DadStudents WHERE Surname = '{SurnameDad.Text.ToLower()}' and Name = '{NameDad.Text.ToLower()}' and MidleName = '{MideleNameDad.Text.ToLower()}' and  Phone1 = '{PhoneDad.Text.ToLower()}' and Phone2 = '{PhoneDad2.Text.ToLower()}' and PassportVID = '{PasportDad.Text.ToLower()}'
                                        and PassportVidan = '{VudanPasportDad.Text.ToLower()}' and  PassportNumber = '{NumberPasportDad.Text.ToLower()}' and  PassportSeria ='{SeriaPasportDad.Text.ToLower()}' and PassportData = '{DtpPasportDad.Text.ToLower()}' ";
                                        cmd = new SQLiteCommand(query, connection);
                                        IDDad = Convert.ToInt32(cmd.ExecuteScalar());
                                    }
                                    bool result1= int.TryParse(Poll.SelectedValue.ToString(), out int IDPoll);
                                    bool result2 = int.TryParse(CbmCpec.SelectedValue.ToString(), out int IDCpec); 
                                    bool result3 = int.TryParse(CbmGroup.SelectedValue.ToString(), out int IDGroup);
                                    query = $@"INSERT INTO Students('Surname','Name','MidleName','Phone1','Phone2','SNILS','OMS','Adress','PassportVid','PassportVidan','PassportNumber','PasssportSeria','PassportData',
                                            'IDPoll','IDSpecual','IDGrop','IDMum','IDDad','IDPyku','PocleKlass','NameSchool','NumberAtect','DataPolycen','Foto')
                                        values ('{SurnameSt.Text.ToLower()}','{NameSt.Text.ToLower()}','{MideleNameSt.Text.ToLower()}','{PhoneSt1.Text.ToLower()}',{PhoneSt2.Text.ToLower()}','{SNILSSt.Text.ToLower()}',
                                        '{OMSSt.Text.ToLower()}','{AdressSt.Text.ToLower()}','{PasportSt.Text.ToLower()}','{VudanPasportSt.Text.ToLower()}','{NumberPasportSt.Text.ToLower()}','{SeriaPasportSt.Text.ToLower()}'
                                        ,'{IDPoll}','{IDCpec}','{IDGroup}','{IDMum}','{IDDad},'{LastObraz.Text.ToLower()},'{OrganizStudent.Text.ToLower()},'{NumberAtestat.Text.ToLower()},'{DtnPolucheyne.Text.ToLower()}, @Foto";
                                    cmd = new SQLiteCommand(query, connection);                                
                                    byte[] bytes = null;
                                    if (ProverkaFoto != 0)
                                    {
                                        bytes = File.ReadAllBytes("Foto/notfoto.jpg");
                                        cmd.Parameters.AddWithValue("@Foto", bytes);
                                    }
                                    else
                                    {

                                    }
                                    

                                }
                                else
                                {
                                    if (ProverkaMum == 1)
                                    {
                                        MessageBox.Show("Такой номер и серия паспорта уже используется(Мать)", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else if (ProverkaDad ==1)
                                    {
                                        MessageBox.Show("Такой номер и серия паспорта уже используется(Отец)", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }else if (ProverkaMum == 1 && ProverkaDad == 1)
                                    {
                                        MessageBox.Show("Такой номер и серия паспорта уже используется(Мать и Отец)", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Такой СНИЛС и ОМС уже используется", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Такой номер и серия паспорта уже используется", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static byte[] converterDemo(Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }

        private void checkBoxMum_Checked(object sender, RoutedEventArgs e)
        {
            StpMum.IsEnabled= true;
        }

        private void checkBoxMum_Unchecked(object sender, RoutedEventArgs e)
        {
            StpMum.IsEnabled = false;
        }

        private void checkBoxDad_Unchecked(object sender, RoutedEventArgs e)
        {
            StpDad.IsEnabled = false;
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            //Image x = new Image(FotoStudenta);
            //converterDemo();
           // byte[] bytes = File.ReadAllBytes("Foto/notfoto.jpg");
           // MessageBox.Show(Convert.ToString(bytes));
        }

        private void checkBoxDad_Checked(object sender, RoutedEventArgs e)
        {
            StpDad.IsEnabled = true;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            CheackText();
        }       
    }
}
