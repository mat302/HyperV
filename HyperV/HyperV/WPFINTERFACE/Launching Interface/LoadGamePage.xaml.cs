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

namespace Launching_Interface
{
    /// <summary>
    /// Interaction logic for LoadGamePage.xaml
    /// </summary>
    public partial class LoadGamePage : Page
    {
      List<string> LangueOficielleLoadPage { get; set; }
      public LoadGamePage()
        {
         LangueOficielleLoadPage = new List<string>();
         InitializeComponent();
         if (GererDonnees.Langue == 0) { LangueOficielleLoadPage = GererDonnees.ListeFrancais; }
         if (GererDonnees.Langue == 1) { LangueOficielleLoadPage = GererDonnees.ListeAnglais; }
         if (GererDonnees.Langue == 2) { LangueOficielleLoadPage = GererDonnees.ListeEspagnol; }
         if (GererDonnees.Langue == 3) { LangueOficielleLoadPage = GererDonnees.ListeJaponais; }

         tbtitre.Text = LangueOficielleLoadPage[32];
         BackButton.Text = LangueOficielleLoadPage[0];

         saveA.Text = LangueOficielleLoadPage[2];
         timeA.Text = LangueOficielleLoadPage[3];
         doneA.Text = LangueOficielleLoadPage[4];

         saveB.Text = LangueOficielleLoadPage[5];
         timeB.Text = LangueOficielleLoadPage[6];
         doneB.Text = LangueOficielleLoadPage[7];

         saveC.Text = LangueOficielleLoadPage[8];
         timeC.Text = LangueOficielleLoadPage[9];
         doneC.Text = LangueOficielleLoadPage[10];
      }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage());
        }

      private void Load1Button_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Load2Button_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Load3Button_Click(object sender, RoutedEventArgs e)
      {

      }
   }
}
