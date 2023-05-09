using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
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
using AccountingStudentData.Connection;
using Microsoft.Win32;
using Image = System.Drawing.Image;

namespace AccountingStudentData.BoxWindows
{
    /// <summary>
    /// Логика взаимодействия для AddStudents.xaml
    /// </summary>
    public partial class AddStudents : Window
    {
        int CheckDad = 0, CheckMum = 0,Proverka1 = 0;
        byte[] image_bytes = null;
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
                    string query1 = $@"SELECT ID, Surname,Name,MidleName  FROM Users where IDAllowance = 2 ";
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
                }
                else if (SeriaPasportSt.Text.Length != 4)
                {
                    MessageBox.Show("В серии паспорта должно быть 4 цифры", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                    Proverka1 = 1;
                }
                else if (NumberPasportSt.Text.Length != 6)
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
                    }
                    else if (NumberAtestat.Text.Length != 14)
                    {
                        MessageBox.Show("В номер атестата должно быть 14 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        Proverka1 = 1;
                    }
                    else if (SNILSSt.Text.Length != 11)
                    {
                        MessageBox.Show("В номере СНИЛСа должно быть 11 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        Proverka1 = 1;
                    }
                    else if (OMSSt.Text.Length != 16)
                    {
                        MessageBox.Show("В номере ОМСа должно быть 16 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        Proverka1 = 1;
                    }
                    else if (PhoneSt1.Text.Length != 11)
                    {
                        MessageBox.Show("В номер телефона должно быть 11 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        Proverka1 = 1;
                    }
                    else if (PhoneSt2.Text.Length != 11 && PhoneSt2.Text != string.Empty)
                    {
                        MessageBox.Show("В номер телефона должно быть 11 цифр", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        Proverka1 = 1;
                    }
                    else if (NumberAtestat.Text.Length == 14 && OMSSt.Text.Length == 16 && PhoneSt1.Text.Length == 11 && PhoneSt1.Text.Length == 11)
                    {
                        if (checkBoxDad.IsChecked == true)
                        {
                            if (String.IsNullOrEmpty(SurnameDad.Text) || String.IsNullOrEmpty(NameDad.Text) || String.IsNullOrEmpty(PhoneDad.Text) || String.IsNullOrEmpty(PasportDad.Text) ||
                               String.IsNullOrEmpty(NumberPasportDad.Text) || String.IsNullOrEmpty(SeriaPasportDad.Text) || String.IsNullOrEmpty(VudanPasportDad.Text)
                               || String.IsNullOrEmpty(GrStudentDad.Text) || String.IsNullOrEmpty(DtpPasportDad.Text))
                            {
                                MessageBox.Show("Заполните информацию в данных родитель(Отец)", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                                CheckDad = 1;
                                // Proverka1 = 1;
                            }
                            else if (PhoneDad.Text.Length != 11)
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
                            else if (NumberPasportDad.Text.Length != 6)
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
                            else if (PhoneDad.Text.Length == 11 && NumberPasportDad.Text.Length == 6 && SeriaPasportDad.Text.Length == 4)
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
                        else if (CheckDad == 0 && CheckMum == 0)
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

        public void AddStudent()
        {
            try
            {
                if (Proverka1 == 0)
                {
                    using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                    {
                        connection.Open();
                        string query = $@"SELECT count () FROM Students WHERE PassportNumber = '{NumberPasportSt.Text}' and PassportSeria = '{SeriaPasportSt.Text}' ";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        int ProverkaPassportSt = Convert.ToInt32(cmd.ExecuteScalar());
                        if (ProverkaPassportSt == 0) //Проверка номера и серии паспорта у студента
                        {
                            query = $@"SELECT count () FROM Students WHERE SNILS = '{SNILSSt.Text}' or OMS = '{OMSSt.Text}' ";
                            cmd = new SQLiteCommand(query, connection);
                            int ProverkaMedSt= Convert.ToInt32(cmd.ExecuteScalar());
                            if (ProverkaMedSt == 0)//Проверка снилса и омс у студента
                            {
                                int ProverkaMum = 0;
                                int ProverkaDad = 0;
                                if (checkBoxMum.IsChecked == true)
                                {
                                    query = $@"SELECT count () FROM MumStudents WHERE PassportSeria = '{NumberPasportMum.Text}' and PassportNumber = '{NumberPasportMum.Text}' ";
                                    cmd = new SQLiteCommand(query, connection);
                                    ProverkaMum = Convert.ToInt32(cmd.ExecuteScalar());
                                }
                                if (checkBoxDad.IsChecked == true)
                                {
                                    query = $@"SELECT count () FROM DadStudents WHERE PassportSeria = '{NumberPasportDad.Text}' and PassportNumber = '{SeriaPasportDad.Text}' ";
                                    cmd = new SQLiteCommand(query, connection);
                                    ProverkaDad = Convert.ToInt32(cmd.ExecuteScalar());
                                }
                                if (ProverkaMum == 0 && ProverkaDad == 0) //Проверка номера и серии паспорта у родителей
                                {
                                    string IDMum = null;
                                    if (checkBoxMum.IsChecked == true)
                                    {
                                        if (String.IsNullOrEmpty(WorkMum.Text))
                                        {
                                            WorkMum.Text = "Не рабоает";
                                            WorkDolMum.Text = "Не рабоает";
                                        }
                                        if (String.IsNullOrEmpty(WorkDolMum.Text))
                                        {
                                            WorkDolMum.Text = "Нет данных";
                                        }
                                        query = $@"INSERT INTO MumStudents('Surname','Name','MidleName','Phone1','Phone2','PassportVID','PassportVidan','PassportNumber','PassportSeria','PassportData','PassportCountry','WorkMum','WorkDolMum')
                                        values ('{SurnameMum.Text}','{NameMum.Text}','{MideleNameMum.Text}','{PhoneMum.Text.ToLower()}','{PhoneMum2.Text.ToLower()}','{PasportMum.Text.ToLower()}',
                                        '{VudanPasportMum.Text.ToLower()}','{NumberPasportMum.Text.ToLower()}','{SeriaPasportMum.Text.ToLower()}','{DtpPasportMum.Text.ToLower()}','{GrStudentMum.Text.ToUpper()}','{WorkMum.Text}','{WorkDolMum.Text}')";
                                        cmd = new SQLiteCommand(query, connection);
                                        cmd.ExecuteScalar();
                                        query = $@"SELECT ID FROM MumStudents WHERE Surname = '{SurnameMum.Text}' and Name = '{NameMum.Text}' and MidleName = '{MideleNameMum.Text}' and  Phone1 = '{PhoneMum.Text.ToLower()}' and Phone2 = '{PhoneMum2.Text.ToLower()}' and PassportVID = '{PasportMum.Text.ToLower()}'
                                        and PassportVidan = '{VudanPasportMum.Text.ToLower()}' and  PassportNumber = '{NumberPasportMum.Text.ToLower()}' and  PassportSeria ='{SeriaPasportMum.Text.ToLower()}' and PassportData = '{DtpPasportMum.Text.ToLower()}' and WorkMum ='{WorkMum.Text}' and WorkDolMum ='{WorkDolMum.Text}' ";
                                        cmd = new SQLiteCommand(query, connection);
                                        int idmum = Convert.ToInt32(cmd.ExecuteScalar());
                                        IDMum = Convert.ToString(idmum);
                                    }
                                    string IDDad = null;
                                    if (checkBoxDad.IsChecked == true)
                                    {
                                        if (String.IsNullOrEmpty(WorkDad.Text))
                                        {
                                            WorkMum.Text = "Не рабоает";
                                            WorkDolMum.Text = "Не рабоает";
                                        }
                                        if (String.IsNullOrEmpty(WorkDolDad.Text))
                                        {
                                            WorkDolMum.Text = "Нет данных";
                                        }
                                        query = $@"INSERT INTO DadStudents('Surname','Name','MidleName','Phone1','Phone2','PassportVID','PassportVidan','PassportNumber','PassportSeria','PassportData','PassportCountry','WorkDad','WorkDolDad')
                                        values ('{SurnameDad.Text}','{NameDad.Text}','{MideleNameDad.Text}','{PhoneDad.Text.ToLower()}','{PhoneDad.Text.ToLower()}','{PasportDad.Text.ToLower()}',
                                        '{VudanPasportDad.Text.ToLower()}','{NumberPasportDad.Text.ToLower()}','{SeriaPasportDad.Text.ToLower()}','{DtpPasportDad.Text.ToLower()}','{GrStudentDad.Text.ToUpper()}','{WorkDad.Text}','{WorkDolDad.Text}')";
                                        cmd = new SQLiteCommand(query, connection);
                                        cmd.ExecuteScalar();
                                        query = $@"SELECT ID FROM DadStudents WHERE Surname = '{SurnameDad.Text}' and Name = '{NameDad.Text}' and MidleName = '{MideleNameDad.Text}' and  Phone1 = '{PhoneDad.Text.ToLower()}' and Phone2 = '{PhoneDad2.Text.ToLower()}' and PassportVID = '{PasportDad.Text.ToLower()}'
                                        and PassportVidan = '{VudanPasportDad.Text.ToLower()}' and  PassportNumber = '{NumberPasportDad.Text.ToLower()}' and  PassportSeria ='{SeriaPasportDad.Text.ToLower()}' and PassportData = '{DtpPasportDad.Text.ToLower()}' and WorkDad ='{WorkDad.Text}' and WorkDolDad ='{WorkDolDad.Text}' ";
                                        cmd = new SQLiteCommand(query, connection);
                                        int iddad = Convert.ToInt32(cmd.ExecuteScalar());
                                        IDDad = Convert.ToString(iddad);
                                    }
                                    bool result1 = int.TryParse(Poll.SelectedValue.ToString(), out int IDPoll);
                                    bool result2 = int.TryParse(CbmCpec.SelectedValue.ToString(), out int IDCpec);
                                    bool result3 = int.TryParse(CbmGroup.SelectedValue.ToString(), out int IDGroup);
                                    bool result4 = int.TryParse(CbmPyk.SelectedValue.ToString(), out int IDPyk);
                                    query = $@" INSERT INTO Students ('Surname','Name','MidleName','Phone1','Phone2','SNILS',
                                    'OMS','Adress','PassportVid','PassportVidan','PassportNumber','PassportSeria',
                                    'PassportData','IDPoll','IDSpecual','IDGrop','IDMum','IDDad',
                                    'IDPyku','PocleKlass','NameSchool','NumberAtect','DataPolycen','Foto','DataСredited','DataEnd','NumberPrikaz','NumberDogovora','DataBirth','PassportCountry','NumberZatechBook','NumberPrigazKyrs1','DataСreditedKyrs1','NumberPrigazKyrs2','DataСreditedKyrs2','NumberPrigazKyrs3','DataСreditedKyrs3','NumberPrigazKyrs4','DataСreditedKyrs4','MestoBirthday') 
                                    values ('{SurnameSt.Text}','{NameSt.Text}','{MideleNameSt.Text}','{PhoneSt1.Text.ToLower()}','{PhoneSt2.Text.ToLower()}','{SNILSSt.Text.ToLower()}',
                                        '{OMSSt.Text.ToLower()}','{AdressSt.Text.ToLower()}','{PasportSt.Text.ToLower()}','{VudanPasportSt.Text.ToLower()}','{NumberPasportSt.Text.ToLower()}','{SeriaPasportSt.Text.ToLower()}','{DtpPasportSt.Text.ToLower()}'
                                        ,'{IDPoll}','{IDCpec}','{IDGroup}','{IDMum}','{IDDad}','{IDPyk}','{LastObraz.Text.ToLower()}',
                                        '{OrganizStudent.Text.ToLower()}','{NumberAtestat.Text.ToLower()}','{DtnPolucheyne.Text.ToLower()}',@Foto,'{DataСredited.Text.ToLower()}','{DataEnd.Text.ToLower()}','{NumberPrigaz.Text.ToLower()}',@NumberDogovora,'{DtpSt.Text.ToLower()}','{GrStudent.Text.ToUpper()}','{TxtNumberzatechBook.Text}','{NumberPrigazKyrs1.Text}','{DataСreditedKyrs1.Text}','{NumberPrigazKyrs2.Text}','{DataСreditedKyrs2.Text}','{NumberPrigazKyrs3.Text}','{DataСreditedKyrs3.Text}','{NumberPrigazKyrs4.Text}','{DataСreditedKyrs4.Text}','{AdressStBirht.Text}')";
                                    cmd = new SQLiteCommand(query, connection);
                                    byte[] bytes = null;
                                    if (image_bytes == null)
                                    {
                                        bytes = File.ReadAllBytes("Foto/notfoto.jpg");
                                        cmd.Parameters.AddWithValue("@Foto", bytes);
                                    }
                                    else if (image_bytes != null)
                                    {
                                        cmd.Parameters.AddWithValue("@Foto", image_bytes);
                                    }
                                    if (ChBxPlatObych.IsChecked == true)
                                    {
                                        cmd.Parameters.AddWithValue("@NumberDogovora", NumberDogovora.Text);
                                    }
                                    else if (ChBxPlatObych.IsChecked == false)
                                    {
                                        cmd.Parameters.AddWithValue("@NumberDogovora", "Бюджет");
                                    }
                                    cmd.ExecuteScalar();
                                    MessageBox.Show("Студент добавлен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);

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

        private void PhoneMaskSt1(string PhoneStudent1)
        {
            var newVal = PhoneStudent1;
            PhoneStudent1 = string.Empty;
            switch (newVal.Length)
            {
                case 1:
                    PhoneStudent1 = Regex.Replace(newVal, @"(\d{1})", "+7(___)___-__-__");
                    break;
                case 2:
                    PhoneStudent1 = Regex.Replace(newVal, @"(\d{1})(\d{0,3})", "+7($2__)___-__-__");
                    break;
                case 3:
                    PhoneStudent1 = Regex.Replace(newVal, @"(\d{1})(\d{0,3})", "+7($2_)___-__-__");
                    break;
                case 4:
                    PhoneStudent1 = Regex.Replace(newVal, @"(\d{1})(\d{0,3})", "+7($2)___-__-__");
                    break;
                case 5:
                    PhoneStudent1 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})", "+7($2)$3__-__-__");
                    break;
                case 6:
                    PhoneStudent1 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})", "+7($2)$3_-__-__");
                    break;
                case 7:
                    PhoneStudent1 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})", "+7($2)$3-__-__");
                    break;
                case 8:
                    PhoneStudent1 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})", "+7($2)$3-$4_-__");
                    break;
                case 9:
                    PhoneStudent1 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})", "+7($2)$3-$4-__");
                    break;
                case 10:
                    PhoneStudent1 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "+7($2)$3-$4-$5_");
                    break;
                case 11:
                    PhoneStudent1 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "+7($2)$3-$4-$5");
                    break;
            }
            PhoneSt1.Text = PhoneStudent1;
        }
        private string ReplacenumberSt1()
        {
            string num = Regex.Replace(PhoneSt1.Text, @"[^0-9]", "");
            return num;
        }       
        private void ChangeCaretIndexSt1(string PhoneStudent1)
        {
            if (PhoneStudent1.Length <= 11)
            {
                PhoneMaskSt1(PhoneStudent1);
            }
            if (PhoneStudent1.Length <= 4)
            {
                PhoneSt1.CaretIndex = PhoneStudent1.Length + 2;
            }
            else if (PhoneStudent1.Length <= 7)
            {
                PhoneSt1.CaretIndex = PhoneStudent1.Length + 3;
            }
            else if (PhoneStudent1.Length <= 9)
            {
                PhoneSt1.CaretIndex = PhoneStudent1.Length + 4;
            }
            else if (PhoneStudent1.Length <= 11)
            {
                PhoneSt1.CaretIndex = PhoneStudent1.Length + 5;
            }

        }
        private void TbPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeCaretIndexSt1(ReplacenumberSt1());
        }
        private void TbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ChangeCaretIndexSt1(ReplacenumberSt1() + e.Text);
            e.Handled = true;
        }
        private void TbPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            ChangeCaretIndexSt1(ReplacenumberSt1());
        }

        private void PhoneMaskSt2(string PhoneStudent2)
        {
            var newVal = PhoneStudent2;
            PhoneStudent2 = string.Empty;
            switch (newVal.Length)
            {
                case 1:
                    PhoneStudent2 = Regex.Replace(newVal, @"(\d{1})", "+7(___)___-__-__");
                    break;
                case 2:
                    PhoneStudent2 = Regex.Replace(newVal, @"(\d{1})(\d{0,3})", "+7($2__)___-__-__");
                    break;
                case 3:
                    PhoneStudent2 = Regex.Replace(newVal, @"(\d{1})(\d{0,3})", "+7($2_)___-__-__");
                    break;
                case 4:
                    PhoneStudent2 = Regex.Replace(newVal, @"(\d{1})(\d{0,3})", "+7($2)___-__-__");
                    break;
                case 5:
                    PhoneStudent2 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})", "+7($2)$3__-__-__");
                    break;
                case 6:
                    PhoneStudent2 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})", "+7($2)$3_-__-__");
                    break;
                case 7:
                    PhoneStudent2 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})", "+7($2)$3-__-__");
                    break;
                case 8:
                    PhoneStudent2 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})", "+7($2)$3-$4_-__");
                    break;
                case 9:
                    PhoneStudent2 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})", "+7($2)$3-$4-__");
                    break;
                case 10:
                    PhoneStudent2 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "+7($2)$3-$4-$5_");
                    break;
                case 11:
                    PhoneStudent2 = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "+7($2)$3-$4-$5");
                    break;
            }
            PhoneSt2.Text = PhoneStudent2;
        }
        private string ReplacenumberSt2()
        {
            string num = Regex.Replace(PhoneSt2.Text, @"[^0-9]", "");
            return num;
        }
        private void ChangeCaretIndexSt2(string PhoneStudent2)
        {
            if (PhoneStudent2.Length <= 11)
            {
                PhoneMaskSt2(PhoneStudent2);
            }
            if (PhoneStudent2.Length <= 4)
            {
                PhoneSt1.CaretIndex = PhoneStudent2.Length + 2;
            }
            else if (PhoneStudent2.Length <= 7)
            {
                PhoneSt1.CaretIndex = PhoneStudent2.Length + 3;
            }
            else if (PhoneStudent2.Length <= 9)
            {
                PhoneSt1.CaretIndex = PhoneStudent2.Length + 4;
            }
            else if (PhoneStudent2.Length <= 11)
            {
                PhoneSt1.CaretIndex = PhoneStudent2.Length + 5;
            }
        }
        private void TbPhoneSt2_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeCaretIndexSt2(ReplacenumberSt2());
        }
        private void TbPhoneSt2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ChangeCaretIndexSt2(ReplacenumberSt2() + e.Text);
            e.Handled = true;
        }
        private void TbPhoneSt2_GotFocus(object sender, RoutedEventArgs e)
        {
            ChangeCaretIndexSt2(ReplacenumberSt2());
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

        private void checkBoxDad_Checked(object sender, RoutedEventArgs e)
        {
            StpDad.IsEnabled = true;
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
            NumberDogovora.IsEnabled= true;            
        }
        private void ChBxPlatObych_Unchecked(object sender, RoutedEventArgs e)
        {
            NumberDogovora.IsEnabled = false;
        }
        private void BtnDellFoto_Click(object sender, RoutedEventArgs e)
        {
            var uriSource = new Uri(@"/Foto/notfoto.jpg", UriKind.Relative);
            FotoStudenta.Source = new BitmapImage(uriSource);
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            CheckText();            
            AddStudent();            
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
