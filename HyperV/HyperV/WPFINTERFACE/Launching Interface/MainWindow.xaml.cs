using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;

namespace Launching_Interface
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            List<string> ListeReçu = new List<string>();

            StreamReader lecteurDonnées = new StreamReader("../../Saves/save.txt");
            while (!lecteurDonnées.EndOfStream)
            {
                ListeReçu.Add(lecteurDonnées.ReadLine());
            }
            lecteurDonnées.Close();

            RefreshData();

            if (ListeReçu[1] == "true")
            {
                MainFrame.Navigate(new MenuDansJeu());
            }
            else
            {
                MainFrame.Navigate(new MainPage());
            }
        }

        private void RefreshData()
        {
         StreamReader reader = new StreamReader("../../Saves/Settings.txt");
         GererDonnees.InitialiserGererDonnees(reader);

      }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            MainFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }
    }
}