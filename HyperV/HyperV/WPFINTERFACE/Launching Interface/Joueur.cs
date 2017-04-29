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
   class Joueur
   {
      string Nom { get; set; }
      public TimeSpan TempsNiveau1 { get; private set; }
      public TimeSpan TempsNiveau2 { get; private set; }
      public TimeSpan TempsNiveau3 { get; private set; }

      
      public Joueur(string nom, TimeSpan tempsNiveau1, TimeSpan tempsNiveau2, TimeSpan tempsNiveau3)
      {
         Nom = nom;
         TempsNiveau1 = tempsNiveau1;
         TempsNiveau2 = tempsNiveau2;
         TempsNiveau3 = tempsNiveau3;
      }

      void LireFichierDuJoueur(int i)
      {
         StreamReader lecteurDonnées = new StreamReader("../../Saves/save" + i + ".txt");
         while (!lecteurDonnées.EndOfStream)
         {
            for (int j = 0; j < 5; j++)
            {
               lecteurDonnées.ReadLine();
            }
            

            string Lignelue = lecteurDonnées.ReadLine();
           

            //Lignelue = lecteurDonnées.ReadLine();
            //séparateur = Lignelue.Split(new string[] { "k: " }, StringSplitOptions.None);
            //ListeÉlémentsAAfficher.Add(séparateur[1]);
         }
         lecteurDonnées.Close();
      }

   }
}
