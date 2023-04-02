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
                    SELECT Students.Surname as SurnameSt, Students.Name as NameSt, Students.MidleName as MidleNameSt,Students.Phone1 as Phone1St,
                    Polls.Name as PollSt,Specialties.NumberSpecial as NumberSpecualSt, Groups.Name as GroupSt,Students.PocleKlass as KlassSt,
                    Users.ID as IDPyk,Users.Surname as SurnamePyk ,Users.Name as NamePyk, Users.MidleName as MidleNamePyk,
					Students.NumberPrikaz as NumberPrikazSt,Students.NumberDogovora as NumberDogovorSt,
					Students.DataСredited as DataPost, Students.DataEnd as DataOkon, Students.Foto,
					Students.Phone2 as Phone2St, Students.SNILS as SNILSSt, Students.OMS as OMSSt, Students.Adress as AdressSt,
					Students.PassportData as PassDataSt, Students.PassportNumber as PassNumSt,Students.PassportSeria as PassSeriaSt,
					Students.PassportVID as PassVIDSt,Students.PassportVidan as PassVidanSt,
					
					MumStudents.ID as IDMumSt, MumStudents.Surname as SurnameMum, MumStudents.Name as NameMum, MumStudents.MidleName as MidleNameMum,
					MumStudents.PassportData as PassDataMum, MumStudents.PassportNumber as PassNumMum, MumStudents.PassportSeria as PassSeriaMum,
					MumStudents.PassportVID as PassVIDMum,MumStudents.PassportVidan as PassVidanMum, MumStudents.Phone1 as Phone1Mum, MumStudents.Phone2 as Phone2Mum,
					
					DadStudents.ID as IDDadSt, DadStudents.Surname as SurnameDad, DadStudents.Name as NameDad, DadStudents.MidleName as MidleNameDad,
					DadStudents.PassportData as PassDataDad, DadStudents.PassportNumber as PassNumDad, DadStudents.PassportSeria as PassSeriaDad,
					DadStudents.PassportVID as PassVIDDad,DadStudents.PassportVidan as PassVidanDad, DadStudents.Phone1 as Phone1Dad, DadStudents.Phone2 as Phone2Dad

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
        
    }
}
