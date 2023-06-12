using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AccountingStudentData.Connection;
using Microsoft.Win32;

namespace AccountingStudentData.BoxWindows
{
    /// <summary>
    /// Логика взаимодействия для AddStudents.xaml
    /// </summary>
    public partial class AddStudents : Window
    {
        int Proverka1 = 0;
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
                    string query1 = $@"SELECT ID, Surname,Name,MiddleName   FROM Users where IDAllowance = 2 ";
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
                Proverka1 = 0;
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
                               
                                    bool result1 = int.TryParse(Poll.SelectedValue.ToString(), out int IDPoll);
                                    bool result2 = int.TryParse(CbmCpec.SelectedValue.ToString(), out int IDCpec);
                                    bool result3 = int.TryParse(CbmGroup.SelectedValue.ToString(), out int IDGroup);
                                    bool result4 = int.TryParse(CbmPyk.SelectedValue.ToString(), out int IDPyk);
                                    query = $@" INSERT INTO Students ('Surname','Name','MiddleName','Phone1','Phone2','SNILS',
                                    'OMS','Adress','PassportVid','PassportVidan','PassportNumber','PassportSeria',
                                    'PassportData','IDPoll','IDSpecual','IDGrop',
                                    'IDPyku','PocleKlass','NameSchool','NumberAtect','DataPolycen','Foto','DataСredited','DataEnd','NumberPrikaz','NumberDogovora','DataBirth','PassportCountry','NumberZatechBook','NumberPrigazKyrs1','DataСreditedKyrs1','NumberPrigazKyrs2','DataСreditedKyrs2','NumberPrigazKyrs3','DataСreditedKyrs3','NumberPrigazKyrs4','DataСreditedKyrs4','MestoBirthday','IsDelet') 
                                    values ('{SurnameSt.Text}','{NameSt.Text}','{MideleNameSt.Text}','{PhoneSt1.Text.ToLower()}','{PhoneSt2.Text.ToLower()}','{SNILSSt.Text.ToLower()}',
                                        '{OMSSt.Text.ToLower()}','{AdressSt.Text.ToLower()}','{PasportSt.Text.ToLower()}','{VudanPasportSt.Text.ToLower()}','{NumberPasportSt.Text.ToLower()}','{SeriaPasportSt.Text.ToLower()}','{DtpPasportSt.Text.ToLower()}'
                                        ,'{IDPoll}','{IDCpec}','{IDGroup}','{IDPyk}','{LastObraz.Text.ToLower()}',
                                        '{OrganizStudent.Text.ToLower()}','{NumberAtestat.Text.ToLower()}','{DtnPolucheyne.Text.ToLower()}',@Foto,'{DataСredited.Text.ToLower()}','{DataEnd.Text.ToLower()}','{NumberPrigaz.Text.ToLower()}',@NumberDogovora,'{DtpSt.Text.ToLower()}','{GrStudent.Text.ToUpper()}','{TxtNumberzatechBook.Text}','{NumberPrigazKyrs1.Text}','{DataСreditedKyrs1.Text}','{NumberPrigazKyrs2.Text}','{DataСreditedKyrs2.Text}','{NumberPrigazKyrs3.Text}','{DataСreditedKyrs3.Text}','{NumberPrigazKyrs4.Text}','{DataСreditedKyrs4.Text}','{AdressStBirht.Text}','0')";
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
                                    AddAndUpdateResosible();
                                    MessageBox.Show("Студент добавлен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);                            
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
        
        public void AddAndUpdateResosible()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                { 
                    connection.Open();
                    string qwert = $@"SELECT ID FROM Students WHERE Surname = '{SurnameSt.Text}' and Name = '{NameSt.Text}' and MiddleName  = '{MideleNameSt.Text}' and  Phone1 = '{PhoneSt1.Text.ToLower()}' and Phone2 = '{PhoneSt2.Text.ToLower()}' and PassportVid = '{PasportSt.Text.ToLower()}'
                                        and PassportVidan = '{VudanPasportSt.Text.ToLower()}' and  PassportNumber = '{NumberPasportSt.Text.ToLower()}' and  PassportSeria ='{SeriaPasportSt.Text.ToLower()}' and PassportData = '{DtpPasportSt.Text.ToLower()}'";
                    SQLiteCommand cmd = new SQLiteCommand(qwert, connection);
                    int idmum = Convert.ToInt32(cmd.ExecuteScalar());
                    string IDSt = null;
                    IDSt = Convert.ToString(idmum);
                    if (SurnameOtved1.Text != string.Empty)
                    {
                        String txtOtved1 = CmbRodOtved1.Text;
                        qwert = $@"INSERT INTO Responsible ('IDStudent','Surname','Name','MiddleName','Pod','Phone1','Phone2','PassportVID','PassportVidan','PassportNumber','PassportSeria','PassportData','PassportCountry','Work','WorkDol','IsDelet')
                                        values ('{IDSt}','{SurnameOtved1.Text}','{NameOtved1.Text}','{MideleNameOtved1.Text}','{txtOtved1}','{PhoneOtved1.Text.ToLower()}','{PhoneDopOtved1.Text.ToLower()}','{PasportOtved1.Text.ToLower()}',
                                        '{VudanPasportOtved1.Text.ToLower()}','{NumberPasportOtved1.Text.ToLower()}','{SeriaPasportOtved1.Text.ToLower()}','{DtpPasportOtved1.Text.ToLower()}','{GrStudentOtved1.Text.ToUpper()}','{WorkOtved1.Text}','{WorkDolOtved1.Text}',0)";
                        cmd = new SQLiteCommand(qwert, connection);
                        cmd.ExecuteNonQuery();
                        if (SurnameOtved2.Text != string.Empty)
                        {
                            String txtOtved2 = CmbRodOtved2.Text;
                            qwert = $@"INSERT INTO Responsible ('IDStudent','Surname','Name','MiddleName','Pod','Phone1','Phone2','PassportVID','PassportVidan','PassportNumber','PassportSeria','PassportData','PassportCountry','Work','WorkDol','IsDelet')
                                        values ('{IDSt}','{SurnameOtved2.Text}','{NameOtved2.Text}','{MideleNameOtved2.Text}','{txtOtved2}','{PhoneOtved2.Text.ToLower()}','{PhoneDopOtved2.Text.ToLower()}','{PasportOtved2.Text.ToLower()}',
                                        '{VudanPasportOtved2.Text.ToLower()}','{NumberPasportOtved2.Text.ToLower()}','{SeriaPasportOtved2.Text.ToLower()}','{DtpPasportOtved2.Text.ToLower()}','{GrStudentOtved2.Text.ToUpper()}','{WorkOtved2.Text}','{WorkDolOtved2.Text}',0)";
                            cmd = new SQLiteCommand(qwert, connection);
                            cmd.ExecuteNonQuery();
                        }
                        if (SurnameOtved3.Text != string.Empty)
                        {
                            String txtOtved3 = CmbRodOtved3.Text;
                            qwert = $@"INSERT INTO Responsible('IDStudent','Surname','Name','MiddleName','Pod','Phone1','Phone2','PassportVID','PassportVidan','PassportNumber','PassportSeria','PassportData','PassportCountry','Work','WorkDol','IsDelet')
                                        values ('{IDSt}','{SurnameOtved3.Text}','{NameOtved3.Text}','{MideleNameOtved2.Text}','{txtOtved3}','{PhoneOtved3.Text.ToLower()}','{PhoneDopOtved3.Text.ToLower()}','{PasportOtved3.Text.ToLower()}',
                                        '{VudanPasportOtved3.Text.ToLower()}','{NumberPasportOtved3.Text.ToLower()}','{SeriaPasportOtved3.Text.ToLower()}','{DtpPasportOtved3.Text.ToLower()}','{GrStudentOtved3.Text.ToUpper()}','{WorkOtved3.Text}','{WorkDolOtved3.Text}',0)";
                            cmd = new SQLiteCommand(qwert, connection);
                            cmd.ExecuteNonQuery();
                        }
                        if (SurnameOtved4.Text != string.Empty)
                        {
                            String txtOtved4 = CmbRodOtved3.Text;
                            qwert = $@"INSERT INTO Responsible('IDStudent','Surname','Name','MiddleName','Pod','Phone1','Phone2','PassportVID','PassportVidan','PassportNumber','PassportSeria','PassportData','PassportCountry','Work','WorkDol','IsDelet')
                                        values ('{IDSt}','{SurnameOtved4.Text}','{NameOtved4.Text}','{MideleNameOtved4.Text}','{txtOtved4}','{PhoneOtved4.Text.ToLower()}','{PhoneDopOtved4.Text.ToLower()}','{PasportOtved4.Text.ToLower()}',
                                        '{VudanPasportOtved4.Text.ToLower()}','{NumberPasportOtved4.Text.ToLower()}','{SeriaPasportOtved4.Text.ToLower()}','{DtpPasportOtved4.Text.ToLower()}','{GrStudentOtved4.Text.ToUpper()}','{WorkOtved4.Text}','{WorkDolOtved4.Text}',0)";
                            cmd = new SQLiteCommand(qwert, connection);
                            cmd.ExecuteNonQuery();
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
            StpMum.IsEnabled= true;
        }

        private void checkBoxMum_Unchecked(object sender, RoutedEventArgs e)
        {
            StpMum.IsEnabled = false;
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
