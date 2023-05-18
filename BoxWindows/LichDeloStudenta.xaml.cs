using AccountingStudentData.Connection;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Table = Microsoft.Office.Interop.Word.Table;
using Window = System.Windows.Window;

namespace AccountingStudentData.BoxWindows
{
    /// <summary>
    /// Логика взаимодействия для TestAddTableWord.xaml
    /// </summary>
    public partial class LichDeloStudenta : Window
    {
        DataRowView drvnew = null;
        int count1 = 1;
        string[,] celltable = new string[,]
             {
                    { "Наименование предмета, модуля", "Вид испытания", "№", "Часы", "Оценка", "Дата сдачи", "1" },
                    { "Русский", "Зачет", "", "51", "", "", "0" },
                    { "Литература", "Зачет", "", "51", "", "", "0" },
                    { "Иностранный язык", "Зачет", "", "34", "", "", "0" },
                    { "История", "Зачет", "", "34", "", "", "0" },
                    { "Физическая культура", "Диф. зачет", "", "51", "", "", "0" },
                    { "Химия", "Зачет", "", "34", "", "", "0" },
                    { "Основы безопасности жизнедеятельности", "Зачет", "", "34", "", "", "0" },
                    { "Математика: алгебра и начала математического анализа, геометрия", "Зачет", "", "119", "", "", "0" },
                    { "Информатика", "Диф. зачет", "", "51", "", "", "0" },
                    { "Технология", "Зачет", "", "51", "", "", "0" },
                    { "Индивидуальный проект", "Зачет", "", "34", "", "", "0" },
                    { "Физика", "Зачет", "", "34", "", "", "0" },
             };
        public LichDeloStudenta(DataRowView drv)
        {
            InitializeComponent();
            drvnew = drv;
        }

        public void KartochkaLichSt()
        {
            Microsoft.Office.Interop.Word.Document doc = null;           ;
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            try
            {
                // Создаём объект приложения
                // Путь до шаблона документа
                // string source = @"/AccountingStudent/Test.docx";
                string source = System.IO.Path.Combine(Environment.CurrentDirectory, "Личная карточка студента  NEW.docx");
                // Открываем
                doc = app.Documents.Open(source);
                doc.Activate();
                //Заполение данных
                byte[] image_bytes = (byte[])drvnew["FotoSt"];
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CreateOptions = BitmapCreateOptions.None;
                img.CacheOption = BitmapCacheOption.Default;
                img.DecodePixelWidth = 600;
                img.DecodePixelHeight = 750;
                img.StreamSource = new MemoryStream(image_bytes);
                img.EndInit();
                Clipboard.SetImage(img);
                doc.Bookmarks.get_Item("Foto").Range.Paste();
                doc.Bookmarks["NumberZatetku"].Range.Text = drvnew["NumberZatechBook"].ToString();
                doc.Bookmarks["Surname"].Range.Text = drvnew["SurnameSt"].ToString() + " " + drvnew["NameSt"].ToString() + " " + drvnew["MidleNameSt"].ToString(); ;
                doc.Bookmarks["Birthday"].Range.Text = drvnew["DataBirthSt"].ToString();
                doc.Bookmarks["MestoBirthday"].Range.Text = drvnew["MestoBirthday"].ToString(); 
                doc.Bookmarks["GroupSt"].Range.Text = drvnew["GroupSt"].ToString();
                doc.Bookmarks["SpecialSt"].Range.Text = drvnew["NumberSpecualSt"].ToString() + " " + drvnew["NameSpecial"].ToString();
                doc.Bookmarks["NumberPrikaz"].Range.Text = drvnew["NumberPrikazSt"].ToString();
                doc.Bookmarks["DataPrikazwPostyplenuy"].Range.Text = drvnew["DataPost"].ToString();
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    string pr = "0";
                    string IDSt = "0";
                    IDSt = drvnew["IDSt"].ToString();
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
                            doc.Bookmarks[$@"NameOtved{i}"].Range.Text = dr["Pod"].ToString() + ": " +
                            dr["Surname"].ToString() + " " + dr["Name"].ToString() + " " + dr["MidleName"].ToString()
                            + "\n" + "Место работы: " + dr["Work"].ToString() + "\n" + "Должность: " + dr["WorkDol"].ToString();
                            break;
                        }
                    }

                }
                doc.Bookmarks["DateEndSchool"].Range.Text = drvnew["DataPolecenSt"].ToString();
                doc.Bookmarks["AdressSt"].Range.Text = drvnew["AdressSt"].ToString();
                doc.Bookmarks["PhoneSt"].Range.Text = drvnew["Phone1St"].ToString();
                doc.Bookmarks["VIDPassporta"].Range.Text = drvnew["PassVIDSt"].ToString();
                doc.Bookmarks["SeriaPassport"].Range.Text = drvnew["PassSeriaSt"].ToString();
                doc.Bookmarks["NumberPassport"].Range.Text = drvnew["PassNumSt"].ToString();
                doc.Bookmarks["DatePolychPassport"].Range.Text = drvnew["PassDataSt"].ToString();
                doc.Bookmarks["KemVudanPass"].Range.Text = drvnew["PassVidanSt"].ToString();
                doc.Bookmarks["SNILS"].Range.Text = drvnew["SNILSSt"].ToString();
                doc.Bookmarks["OMS"].Range.Text = drvnew["OMSSt"].ToString();
                doc.Bookmarks["DateNow"].Range.Text = DateTime.Now.ToString("yyyy");
                doc.Bookmarks["DateNow1"].Range.Text = DateTime.Now.AddYears(1).ToString("yyyy");
                doc.Bookmarks["DateNow2"].Range.Text = DateTime.Now.AddYears(1).ToString("yyyy");
                doc.Bookmarks["DateNow3"].Range.Text = DateTime.Now.AddYears(2).ToString("yyyy");
                doc.Bookmarks["DateNow4"].Range.Text = DateTime.Now.AddYears(2).ToString("yyyy");
                doc.Bookmarks["DateNow5"].Range.Text = DateTime.Now.AddYears(3).ToString("yyyy");
                doc.Bookmarks["DateNow6"].Range.Text = DateTime.Now.AddYears(3).ToString("yyyy");
                doc.Bookmarks["DateNow7"].Range.Text = DateTime.Now.AddYears(4).ToString("yyyy");
                doc.Bookmarks["NumberPrigazKyrs1"].Range.Text = drvnew["NumberPrigazKyrs1"].ToString();
                doc.Bookmarks["NumberPrigazKyrs2"].Range.Text = drvnew["NumberPrigazKyrs2"].ToString();
                doc.Bookmarks["NumberPrigazKyrs3"].Range.Text = drvnew["NumberPrigazKyrs3"].ToString();
                doc.Bookmarks["NumberPrigazKyrs4"].Range.Text = drvnew["NumberPrigazKyrs4"].ToString();
                doc.Bookmarks["DataСreditedKyrs1"].Range.Text = drvnew["DataСreditedKyrs1"].ToString();
                doc.Bookmarks["DataСreditedKyrs2"].Range.Text = drvnew["DataСreditedKyrs2"].ToString();
                doc.Bookmarks["DataСreditedKyrs3"].Range.Text = drvnew["DataСreditedKyrs3"].ToString();
                doc.Bookmarks["DataСreditedKyrs4"].Range.Text = drvnew["DataСreditedKyrs4"].ToString();
                doc.Words.Last.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);
               //фомриуем таблицу
                Microsoft.Office.Interop.Word.Paragraph textparag = doc.Content.Paragraphs.Add();
                Table tebletext = doc.Tables.Add(textparag.Range, 1, 1);
                foreach (Row row in tebletext.Rows)
                {
                    foreach (Cell cell in row.Cells)
                    {
                        if (cell.RowIndex == 1)
                        {
                            cell.Range.Text = "1 курс 1 семестр";
                            cell.Range.Font.Bold = 1;
                        }
                    }
                }
                doc.Content.Paragraphs.Add();
                if (count1 >= 2)
                {
                    GlobalSearch();
                    Microsoft.Office.Interop.Word.Paragraph para1 = doc.Content.Paragraphs.Add();
                    Table firstttable = doc.Tables.Add(para1.Range, count1, 6);
                    firstttable.Borders.Enable = 1;
                    int i = 0;
                    int j = 1;
                    int k = 0;
                    foreach (Row row in firstttable.Rows)
                    {
                        foreach (Cell cell in row.Cells)
                        {
                            if (cell.RowIndex == 1)
                            {
                                cell.Range.Text = celltable[0, i];
                                //cell.Range.Font.Bold = 1;
                                i++;
                                if (i == 6)
                                {
                                    i = 0;
                                }
                            }
                            else
                            {
                                for (k = 1; k < 13; k++)
                                {
                                    if (celltable[k, 6] == "1")
                                    {
                                        j = k;
                                        break;
                                    }
                                }
                                if (celltable[j, 6] == "1")
                                {
                                    cell.Range.Text = celltable[j, i];
                                    //cell.Range.Font.Bold = 1;
                                    i++;
                                    if (i == 6)
                                    {
                                        i = 0;
                                        celltable[j, 6] = "0";
                                    }
                                }
                            }

                        }
                    }
                } //1k1c

                // Закрываем документ
                string DirectoryFale = System.IO.Path.GetDirectoryName(source);
                doc.SaveAs($@"{DirectoryFale}\Личная карточка студента_{drvnew["SurnameSt"]} {drvnew["NameSt"]} {drvnew["MidleNameSt"]}");
                doc.Close();
                doc = null;
                app.Quit();
                MessageBox.Show($@"Отчет сформулирован и находится в {DirectoryFale}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);               
            }
        }

        public  void Test()
        {
            Microsoft.Office.Interop.Word.Document doc = null;
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            // Путь до шаблона документа
            string source = System.IO.Path.Combine(Environment.CurrentDirectory, "Личная карточка студента  NEW.docx");
            // Открываем
            doc = app.Documents.Open(source);
            doc.Activate();
            Microsoft.Office.Interop.Word.Paragraph textparag = doc.Content.Paragraphs.Add();
            Table tebletext = doc.Tables.Add(textparag.Range, 1, 1);
            foreach (Row row in tebletext.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    if (cell.RowIndex == 1)
                    {
                        cell.Range.Text = "1 курс 1 семестр";
                        cell.Range.Font.Bold = 1;
                    }
                }
            }

            doc.Content.Paragraphs.Add();
            doc.Content.Paragraphs.Add();
            if (count1 >= 2)
            {
                GlobalSearch();
                Microsoft.Office.Interop.Word.Paragraph para1 = doc.Content.Paragraphs.Add();
                Table firstttable = doc.Tables.Add(para1.Range, count1, 6);
                firstttable.Borders.Enable = 1;
                int i = 0;
                int j = 1;
                int k = 0;
                foreach (Row row in firstttable.Rows)
                {
                    foreach (Cell cell in row.Cells)
                    {
                        if (cell.RowIndex == 1)
                        {
                            cell.Range.Text = celltable[0, i];
                            //cell.Range.Font.Bold = 1;
                            i++;
                            if (i == 6)
                            {
                                i = 0;
                            }
                        }
                        else
                        {    
                            for(k= 1; k < 13;k++)
                            {
                                if (celltable[k, 6] == "1")
                                {
                                    j = k;
                                    break;
                                }
                            }
                            if (celltable[j, 6] == "1")
                            {
                                cell.Range.Text = celltable[j, i];
                                //cell.Range.Font.Bold = 1;
                                i++;
                                if (i == 6)
                                {
                                    i = 0;
                                    celltable[j, 6] = "0";
                                }
                            }
                        }
                       
                    }
                }
            } //1k1c

            // TextBox textBlock = parag.AppendTextBox(300, 300);
            //doc.Content.Paragraphs.Add();
            //Microsoft.Office.Interop.Word.Paragraph para2 = doc.Content.Paragraphs.Add();
            //Table firstttable2 = doc.Tables.Add(para2.Range, 5, 6);
            //firstttable2.Borders.Enable = 1;
            //int j = 0;
            //foreach (Row row in firstttable2.Rows)
            //{
            //    foreach (Cell cell in row.Cells)
            //    {
            //        if (cell.RowIndex == 1)
            //        {
            //            cell.Range.Text = celltable[0, i];
            //            cell.Range.Font.Bold = 1;
            //            i++;
            //        }
            //        else
            //        {
            //            if (celltable[1, 6] == "1")
            //            {
            //                cell.Range.Text = celltable[1, i];
            //                cell.Range.Font.Bold = 1;
            //                i++;
            //                return;
            //            }
            //            //if (celltable[1, 6] == "1")
            //            //{
                            
            //            //}
            //            //if (celltable[2, 6] == "1")
            //            //{
            //            //    cell.Range.Text = celltable[2, i];
            //            //    cell.Range.Font.Bold = 1;
            //            //    i++;
            //            //    return;
            //            //}
            //        }
            //        if (i == 6)
            //        {
            //            i = 0;
            //        }
            //        j++;
            //    }
            //}
            string DirectoryFale = System.IO.Path.GetDirectoryName(source);
            doc.SaveAs($@"{DirectoryFale}\1234");
            doc.Close();
            doc = null;
            app.Quit();
        }

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Test();
        }

        public void GlobalSearch()
        {
            if (rus1.IsChecked == true)
            {
                celltable[1, 6] = "1";
            }
            if (luter1.IsChecked == true)
            {
                celltable[2, 6] = "1";
            }
            if (englh1.IsChecked == true)
            {
                celltable[3, 6] = "1";
            }
            if (history1.IsChecked == true)
            {
                celltable[4, 6] = "1";
            }
            if (fiz1.IsChecked == true)
            {
                celltable[5, 6] = "1";
            }
            if (xumui1.IsChecked == true)
            {
                celltable[6, 6] = "1";
            }
            if (obw1.IsChecked == true)
            {
                celltable[7, 6] = "1";
            }
            if (math1.IsChecked == true)
            {
                celltable[8, 6] = "1";
            }
            if (info1.IsChecked == true)
            {
                celltable[9, 6] = "1";
            }
            if (tech1.IsChecked == true)
            {
                celltable[10, 6] = "1";
            }
            if (undpr1.IsChecked == true)
            {
                celltable[11, 6] = "1";
            }
            if (fuzik1.IsChecked == true)
            {
                celltable[12, 6] = "1";
            }
        }
        
        private void rus1_Checked(object sender, RoutedEventArgs e)
        {
            count1++;
            celltable[1, 6] = "1";
        }
        private void luter1_Checked(object sender, RoutedEventArgs e)
        {
            count1++;
            celltable[2, 6] = "1";
        }      

        private void englh1_Checked(object sender, RoutedEventArgs e)
        {
            count1++;
            celltable[3, 6] = "1";
        }       

        private void history1_Checked(object sender, RoutedEventArgs e)
        {
            count1++;
            celltable[4, 6] = "1";
        }

        private void fiz1_Checked(object sender, RoutedEventArgs e)
        {
            count1++;
            celltable[5, 6] = "1";
        }

        private void xumui1_Checked(object sender, RoutedEventArgs e)
        {
            count1++;
            celltable[6, 6] = "1";
        }

        private void obw1_Checked(object sender, RoutedEventArgs e)
        {
            count1++;
            celltable[7, 6] = "1";
        }
        private void math1_Checked(object sender, RoutedEventArgs e)
        {
            count1++;
            celltable[8, 6] = "1";
        }

        private void info1_Checked(object sender, RoutedEventArgs e)
        {
            count1++;
            celltable[9, 6] = "1";
        }
       
        private void tech1_Checked(object sender, RoutedEventArgs e)
        {
            count1++;
            celltable[10, 6] = "1";
        }
        private void undpr1_Checked(object sender, RoutedEventArgs e)
        {
            count1++;
            celltable[11, 6] = "1";
        }
        private void fuzik1_Checked(object sender, RoutedEventArgs e)
        {
            count1++;
            celltable[12, 6] = "1";
        }
        private void rus1_Unchecked(object sender, RoutedEventArgs e)
        {
            count1--;
            celltable[1, 6] = "0";
        }
        private void luter1_Unchecked(object sender, RoutedEventArgs e)
        {
            count1--;
            celltable[2, 6] = "0";
        }
        private void englh1_Unchecked(object sender, RoutedEventArgs e)
        {
            count1--;
            celltable[3, 6] = "0";
        }
        private void history1_Unchecked(object sender, RoutedEventArgs e)
        {
            count1--;
            celltable[4, 6] = "0";
        }
        private void fiz1_Unchecked(object sender, RoutedEventArgs e)
        {
            count1--;
            celltable[5, 6] = "0";
        }
        private void xumui1_Unchecked(object sender, RoutedEventArgs e)
        {
            count1--;
            celltable[6, 6] = "0";
        }
        private void obw1_Unchecked(object sender, RoutedEventArgs e)
        {
            count1--;
            celltable[7, 6] = "0";
        }
        private void math1_Unchecked(object sender, RoutedEventArgs e)
        {
            count1--;
            celltable[8, 6] = "0";
        }
        private void info1_Unchecked(object sender, RoutedEventArgs e)
        {
            count1--;
            celltable[9, 6] = "0";
        }      
        private void tech1_Unchecked(object sender, RoutedEventArgs e)
        {
            count1--;
            celltable[10, 6] = "0";
        }
        private void undpr1_Unchecked(object sender, RoutedEventArgs e)
        {
            count1--;
            celltable[11, 6] = "0";
        }
        private void fuzik1_Unchecked(object sender, RoutedEventArgs e)
        {
            count1--;
            celltable[12, 6] = "0";
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAddDelo_Click(object sender, RoutedEventArgs e)
        {
            KartochkaLichSt();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MnItClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
