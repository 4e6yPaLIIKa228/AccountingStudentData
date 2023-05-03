﻿using AccountingStudentData.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
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

namespace AccountingStudentData.BoxWindows
{
    /// <summary>
    /// Логика взаимодействия для AddComponets.xaml
    /// </summary>
    public partial class DellComponets : Window
    {
        //int checkopen1 = 0;
        public DellComponets()
        {
            InitializeComponent();
            LoadGroup();
            //checkopen1 = checkopen;
        }

        private void CombSearchInfo_DropDownClosed(object sender, EventArgs e)
        {
            String combtext = CombKruterui.Text;
            if (combtext == "Группа")
            {                
                StPnGrop.Visibility = Visibility.Visible;
                StPlSpeacial.Visibility = Visibility.Collapsed;
                this.Height = 300;
                LoadGroup();
            }
            else if (combtext == "Специальность")
            {
                StPnGrop.Visibility = Visibility.Collapsed;
                StPlSpeacial.Visibility = Visibility.Visible;
                this.Height = 400;
                LoadKodSpecial();
            }
        }

        public void LoadGroup()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                { 
                    connection.Open();
                    string query = $@"SELECT ID, Name from Groups ";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable("Groups");
                    SDA.Fill(dt);
                    CmbComponetGroup.ItemsSource = dt.DefaultView;
                    CmbComponetGroup.DisplayMemberPath = "Name";
                    CmbComponetGroup.SelectedValuePath = "ID";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void LoadKodSpecial()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    string query = $@"SELECT NumberSpecial from Specialties
                                      GROUP by NumberSpecial  ";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable("Specialties");
                    SDA.Fill(dt);
                    CmdKodSpecial.ItemsSource = dt.DefaultView;
                    CmdKodSpecial.DisplayMemberPath = "NumberSpecial";
                   // CmbComponet.SelectedValuePath = "ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void LoadKlassSpecial()
        {
            try
            {
                String textcomb = CmdKodSpecial.Text;
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    string query = $@"SELECT ID, Class from Specialties where NumberSpecial  = '{textcomb}'";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable("Specialties");
                    SDA.Fill(dt);
                    CmbKlassSpecial.ItemsSource = dt.DefaultView;
                    CmbKlassSpecial.DisplayMemberPath = "Class";
                    CmbKlassSpecial.SelectedValuePath = "ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CmdKodSpecial_DropDownClosed(object sender, EventArgs e)
        {
            if (CmdKodSpecial.SelectedIndex != -1)
            {
                LoadKlassSpecial();
                CmbKlassSpecial.IsEnabled = true;
            }
            else
            {
                CmbKlassSpecial.IsEnabled = false;
            }
        }

        public void DellComponent()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    if (CombKruterui.SelectedIndex == 0)
                    {
                        if (String.IsNullOrEmpty(CmbComponetGroup.Text))
                        {
                            MessageBox.Show("Выберите компонет.");
                        }
                        else
                        {
                            bool result1 = int.TryParse(CmbComponetGroup.SelectedValue.ToString(), out int IDGroup);
                            string query = $@"SELECT count()  from Students where  IDGrop = '{IDGroup}' ";
                            SQLiteCommand cmd = new SQLiteCommand(query, connection);
                            int ProverkaComponent = Convert.ToInt32(cmd.ExecuteScalar());
                            if (ProverkaComponent == 0)
                            {
                                query = $@"Delete  from Groups where  ID = '{IDGroup}' ";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Компанет удален.");
                                LoadGroup();
                            }
                            else
                            {
                                MessageBox.Show("Компанет используется, в удалении отказано.");
                            }
                        }
                    }
                    else if (CombKruterui.SelectedIndex == 1)
                    {
                        if (String.IsNullOrEmpty(CmdKodSpecial.Text) || String.IsNullOrEmpty(CmbKlassSpecial.Text))
                        {
                            MessageBox.Show("Выберите компонет.");
                        }
                        else
                        {
                            bool result1 = int.TryParse(CmbKlassSpecial.SelectedValue.ToString(), out int IDSpecial);
                            string query = $@"SELECT count()  from Students  where  IDSpecual = '{IDSpecial}' ";
                            SQLiteCommand cmd = new SQLiteCommand(query, connection);
                            int ProverkaComponent = Convert.ToInt32(cmd.ExecuteScalar());
                            if (ProverkaComponent == 0)
                            {
                                query = $@"Delete  from Specialties where  ID = '{IDSpecial}' ";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Компанет удален.");
                                LoadKodSpecial();
                                CmbKlassSpecial.IsEnabled = false;
                                CmbKlassSpecial.SelectedIndex = -1;
                            }
                            else
                            {
                                MessageBox.Show("Компанет используется, в удалении отказано.");
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            StudentBase stbs = new StudentBase();
            stbs.IsEnabled = true;
            this.Close();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddcomponet_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MnItClose_Click(object sender, RoutedEventArgs e)
        {
            //checkopen1 = 1;
            //StudentBase stbs = new StudentBase();
            //stbs.Activate();
            //stbs.IsEnabled = true;
            this.Close();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CombKruterui_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextComponet_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnDellcomponet_Click(object sender, RoutedEventArgs e)
        {
            DellComponent();
        }
    }
}