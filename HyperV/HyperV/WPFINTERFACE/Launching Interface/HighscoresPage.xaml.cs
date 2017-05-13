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
using System.Windows.Shapes;

namespace Launching_Interface
{
    /// <summary>
    /// Logique d'interaction pour HighscoresPage.xaml
    /// </summary>
    public partial class HighscoresPage : Page
    {
        List<string> LangueOficielleHighscoresPage { get; set; }
        List<Joueur> ListeJoueursChangeante { get; set; }

        public HighscoresPage()
        {
            LangueOficielleHighscoresPage = new List<string>();
            ListeJoueursChangeante = new List<Joueur>();
            ListeJoueursChangeante = GererDonnees.ListeJoueurs.OrderBy(x => x.TempsTotal).ToList(); ;

            InitializeComponent();
            ChangerTableau(100);

            OrganiserMargesDesCaractéristiques();
            NommerBoutons();

        }
        void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage());
        }

        void OrganiserMargesDesCaractéristiques()
        {
            switch (GererDonnees.Langue)
            {
                case GererDonnees.Langues.Francais:
                    LangueOficielleHighscoresPage = GererDonnees.ListeFrancais;
                    HStitre.Margin = new Thickness(-31, 13, 40, 5);
                    BackButtonText.Margin = new Thickness(36, 17, 105, 50);
                    ButNiveau1Text.Margin = new Thickness(120, 17, 35, 29);
                    ButNiveau5Text.Margin = new Thickness(35, 17, 120, 29);
                    break;
                case GererDonnees.Langues.Anglais:
                    LangueOficielleHighscoresPage = GererDonnees.ListeAnglais;
                    HStitre.Margin = new Thickness(-31, 13, 40, 5);
                    BackButtonText.Margin = new Thickness(36, 17, 105, 50);
                    ButNiveau1Text.Margin = new Thickness(120, 17, 35, 29);
                    ButNiveau5Text.Margin = new Thickness(35, 17, 120, 29);
                    break;
                case GererDonnees.Langues.Espagnol:
                    LangueOficielleHighscoresPage = GererDonnees.ListeEspagnol;
                    HStitre.Margin = new Thickness(-36, 13, 40, 5);
                    BackButtonText.Margin = new Thickness(33, 17, 107, 52);
                    ButNiveau1Text.Margin = new Thickness(120, 17, 35, 29);
                    ButNiveau5Text.Margin = new Thickness(35, 17, 120, 29);
                    break;
                case GererDonnees.Langues.Japonais:
                    LangueOficielleHighscoresPage = GererDonnees.ListeJaponais;
                    HStitre.Margin = new Thickness(-20, 13, 53, 5);
                    BackButtonText.Margin = new Thickness(36, 17, 105, 52);
                    ButNiveau1Text.Margin = new Thickness(120, 17, 35, 29);
                    ButNiveau5Text.Margin = new Thickness(35, 17, 120, 29);
                    break;
            }
            ButNiveau4Text.Margin = ButNiveau3Text.Margin = ButNiveau2Text.Margin = ButNiveau1Text.Margin;
            ButNiveau8Text.Margin = ButNiveau7Text.Margin = ButNiveau6Text.Margin = ButNiveau5Text.Margin;
        }

        void NommerBoutons()
        {
            HStitre.Text = LangueOficielleHighscoresPage[28];
            BackButtonText.Text = LangueOficielleHighscoresPage[0];

            ButNiveau1Text.Text = LangueOficielleHighscoresPage[46];
            ButNiveau2Text.Text = LangueOficielleHighscoresPage[47];
            ButNiveau3Text.Text = LangueOficielleHighscoresPage[48];
            ButNiveau4Text.Text = LangueOficielleHighscoresPage[49];
            ButNiveau5Text.Text = LangueOficielleHighscoresPage[50];
            ButNiveau6Text.Text = LangueOficielleHighscoresPage[51];
            ButNiveau7Text.Text = LangueOficielleHighscoresPage[52];
            ButNiveau8Text.Text = LangueOficielleHighscoresPage[53];

            ButTotalText.Text = LangueOficielleHighscoresPage[54];
            Rang.Text = LangueOficielleHighscoresPage[9];
            Noms.Text = LangueOficielleHighscoresPage[45];
            Temps.Text = LangueOficielleHighscoresPage[10];
        }

        List<Joueur> ListeOrdonnéeSelonNiveau(int niveau)
        {
            return GererDonnees.ListeJoueurs.OrderBy(x => x.ListeTempsDuJoueur[niveau - 1]).ToList();
        }



        // Boutons Niveau X
        #region

        private void ButNiveau1_Click(object sender, RoutedEventArgs e)
        {
            ListeJoueursChangeante = ListeOrdonnéeSelonNiveau(1);
            ChangerTableau(1);

        }

        private void ButNiveau2_Click(object sender, RoutedEventArgs e)
        {
            ListeJoueursChangeante = ListeOrdonnéeSelonNiveau(2);
            ChangerTableau(2);
        }

        private void ButNiveau3_Click(object sender, RoutedEventArgs e)
        {
            ListeJoueursChangeante = ListeOrdonnéeSelonNiveau(3);
            ChangerTableau(3);
        }

        private void ButNiveau4_Click(object sender, RoutedEventArgs e)
        {
            ListeJoueursChangeante = ListeOrdonnéeSelonNiveau(4);
            ChangerTableau(4);
        }

        private void ButNiveau8_Click(object sender, RoutedEventArgs e)
        {
            ListeJoueursChangeante = ListeOrdonnéeSelonNiveau(8);
            ChangerTableau(8);
        }

        private void ButNiveau7_Click(object sender, RoutedEventArgs e)
        {
            ListeJoueursChangeante = ListeOrdonnéeSelonNiveau(7);
            ChangerTableau(7);
        }

        private void ButNiveau6_Click(object sender, RoutedEventArgs e)
        {
            ListeJoueursChangeante = ListeOrdonnéeSelonNiveau(6);
            ChangerTableau(6);
        }

        private void ButNiveau5_Click(object sender, RoutedEventArgs e)
        {
            ListeJoueursChangeante = ListeOrdonnéeSelonNiveau(5);
            ChangerTableau(5);
        }

        private void ButTotal_Click(object sender, RoutedEventArgs e)
        {
            //ListeJoueursChangeante = ListeJoueursChangeante.OrderBy(x => x.TempsTotal).ToList();

            ChangerTableau(100);

        }

        #endregion

        void ChangerTableau(int niveau)
        {
            niveau -= 1;
            while (ListeJoueursChangeante.Count < 10)
            {
                ListeJoueursChangeante.Add(new Joueur("-------------------", TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                                                                             TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                                                                             TimeSpan.Zero, TimeSpan.Zero));
            }

            nom1.Text = ListeJoueursChangeante[0].Nom;
            nom2.Text = ListeJoueursChangeante[1].Nom;
            nom3.Text = ListeJoueursChangeante[2].Nom;
            nom4.Text = ListeJoueursChangeante[3].Nom;
            nom5.Text = ListeJoueursChangeante[4].Nom;
            nom6.Text = ListeJoueursChangeante[5].Nom;
            nom7.Text = ListeJoueursChangeante[6].Nom;
            nom8.Text = ListeJoueursChangeante[7].Nom;
            nom9.Text = ListeJoueursChangeante[8].Nom;
            nom10.Text = ListeJoueursChangeante[9].Nom;

            if (niveau != 99)
            {
                temps1.Text = ListeJoueursChangeante[0].ListeTempsDuJoueur[niveau].ToString();
                temps2.Text = ListeJoueursChangeante[1].ListeTempsDuJoueur[niveau].ToString();
                temps3.Text = ListeJoueursChangeante[2].ListeTempsDuJoueur[niveau].ToString();
                temps4.Text = ListeJoueursChangeante[3].ListeTempsDuJoueur[niveau].ToString();
                temps5.Text = ListeJoueursChangeante[4].ListeTempsDuJoueur[niveau].ToString();
                temps6.Text = ListeJoueursChangeante[5].ListeTempsDuJoueur[niveau].ToString();
                temps7.Text = ListeJoueursChangeante[6].ListeTempsDuJoueur[niveau].ToString();
                temps8.Text = ListeJoueursChangeante[7].ListeTempsDuJoueur[niveau].ToString();
                temps9.Text = ListeJoueursChangeante[8].ListeTempsDuJoueur[niveau].ToString();
                temps10.Text = ListeJoueursChangeante[9].ListeTempsDuJoueur[niveau].ToString();
            }
            else
            {
                ChangerTempsTotaux();
            }
        }

        void ChangerTempsTotaux()
        {


            temps1.Text = ListeJoueursChangeante[0].TempsTotal.ToString();
            temps2.Text = ListeJoueursChangeante[1].TempsTotal.ToString();
            temps3.Text = ListeJoueursChangeante[2].TempsTotal.ToString();
            temps4.Text = ListeJoueursChangeante[3].TempsTotal.ToString();
            temps5.Text = ListeJoueursChangeante[4].TempsTotal.ToString();
            temps6.Text = ListeJoueursChangeante[5].TempsTotal.ToString();
            temps7.Text = ListeJoueursChangeante[6].TempsTotal.ToString();
            temps8.Text = ListeJoueursChangeante[7].TempsTotal.ToString();
            temps9.Text = ListeJoueursChangeante[8].TempsTotal.ToString();
            temps10.Text = ListeJoueursChangeante[9].TempsTotal.ToString();

        }
    }
}
