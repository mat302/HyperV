using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            ListeReçu.Add( lecteurDonnées.ReadLine());           
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
            //StreamReader reader = new StreamReader("F:/programmation clg/quatrième session/WPFINTERFACE/Launching Interface/Saves/Settings.txt");
            //StreamReader reader = new StreamReader("C:/Users/Mathieu/Source/Repos/WPFINTERFACE/Launching Interface/Saves/Settings.txt");
            StreamReader reader = new StreamReader("../../Saves/Settings.txt");
            string line = reader.ReadLine();
            string[] parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.VolMusique = int.Parse(parts[1]);
            line = reader.ReadLine();
            parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.VolEffets = int.Parse(parts[1]);
            line = reader.ReadLine();
            parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.Langue = int.Parse(parts[1]);
            line = reader.ReadLine();
            parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.RenderDistance = int.Parse(parts[1]);
            line = reader.ReadLine();
            parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.Fps = int.Parse(parts[1]);
            line = reader.ReadLine();
            parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.FullscreenMode = int.Parse(parts[1]);
            line = reader.ReadLine();
            parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.KeyboardMode = int.Parse(parts[1]);
            reader.Close();
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            MainFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
         
        }
    }
}