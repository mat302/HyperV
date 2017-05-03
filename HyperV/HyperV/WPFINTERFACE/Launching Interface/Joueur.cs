using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace Launching_Interface
{
    public class Joueur
    {
        public TimeSpan TempsTotal;


        public string Nom { get; private set; }
        public List<TimeSpan> ListeTempsDuJoueur { get; private set; }


        public Joueur(string nom, TimeSpan tempsNiveau1, TimeSpan tempsNiveau2, TimeSpan tempsNiveau3,
                                  TimeSpan tempsNiveau4, TimeSpan tempsNiveau5, TimeSpan tempsNiveau6,
                                  TimeSpan tempsNiveau7, TimeSpan tempsNiveau8)
        {
            ListeTempsDuJoueur = new List<TimeSpan>();
            Nom = nom;
            ListeTempsDuJoueur.Add(tempsNiveau1);
            ListeTempsDuJoueur.Add(tempsNiveau2);
            ListeTempsDuJoueur.Add(tempsNiveau3);
            ListeTempsDuJoueur.Add(tempsNiveau4);
            ListeTempsDuJoueur.Add(tempsNiveau5);
            ListeTempsDuJoueur.Add(tempsNiveau6);
            ListeTempsDuJoueur.Add(tempsNiveau7);
            ListeTempsDuJoueur.Add(tempsNiveau8);

            TempsTotal = DéterminerTempsTotal();
        }

        TimeSpan DéterminerTempsTotal()
        {
            TimeSpan tempsTot = TimeSpan.Zero;
            for (int i = 0; i < 8; i++)
            {
                tempsTot += ListeTempsDuJoueur[i];
            }
            return tempsTot;
        }


    }
}
