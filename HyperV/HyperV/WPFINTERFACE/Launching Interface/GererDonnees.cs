using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Launching_Interface
{
   public static class GererDonnees
   {
      const string CHEMIN_LECTURE = "../../Langues/";
      const string CHEMIN_ÉCRITURE = "../../FichiersModifies/";
      const string NOM_FICHIER_ENVOI = "versXna.txt";

      public static bool AAAA = true;

      const int LANGUE_BASE = 0; 
      const int FPS_BASE = 60;     
      const int RENDER_D_BASE = 500; 
      const int VOL_MUS_BASE = 50;   
      const int VOL_EFF_BASE = 50;   
      const int NB_NIVEAUX_BASE = 0; 
      static TimeSpan TEMPS_BASE = new TimeSpan(0,0,0);  //pour nous, constante

      public static int Langue { get; set; } // 0 = Francais || 1 = Anglais || 2 = Espagnol || 3 = japonais  
      public static int Fps { get; set; }    // 30,60,90,120
      public static int RenderDistance { get; set; } // 10,50,100,500,1000,5000,10000,50000,100000
      public static int VolMusique { get; set; } // de 0 à 100
      public static int VolEffets { get; set; }  // de 0 à 100
      public static int FullscreenMode { get; set; } // 0 = false || 1 = true
      public static int KeyboardMode { get; set; }   // 0 = false || 1 = true
      public static int NbNiveauxComplétés { get; set; } // 1,2,3
      public static bool PremierFichier { get; set; }
      
      public static TimeSpan Temps { get; set; }

      public static List<string> ListeFrancais { get; private set; }
      public static List<string> ListeAnglais  { get; private set; }
      public static List<string> ListeEspagnol { get; private set; }
      public static List<string> ListeJaponais { get; private set; }
      public static List<int>  ListeInfosRecus { get; private set; }

      static GererDonnees()
      {
         ListeFrancais = new List<string>();
         ListeAnglais  = new List<string>();
         ListeEspagnol = new List<string>();
         ListeJaponais = new List<string>();
         ListeInfosRecus = new List<int>();

         

         LireFichier("En.txt");
         LireFichier("Es.txt");
         LireFichier("Jp.txt");
         LireFichier("Fr.txt");
         if (File.Exists("../../Langues/versMenu.txt"))
         {
            PremierFichier = false;
            LireFichier("versMenu.txt");           
         }
         else
         {
            PremierFichier = true;
         }
        
         ChoisirRéglages();
      }

      static void LireFichier(string nomFichier)
      {
         StreamReader lecteurDonnées = new StreamReader(CHEMIN_LECTURE + nomFichier);
         while (!lecteurDonnées.EndOfStream)
         {
            switch (nomFichier)
            {
               case "Fr.txt":
                  ListeFrancais.Add(lecteurDonnées.ReadLine() + '\n');
                  break;
               case "En.txt":
                  ListeAnglais.Add(lecteurDonnées.ReadLine() + '\n');
                  break;
               case "Es.txt":
                  ListeEspagnol.Add(lecteurDonnées.ReadLine() + '\n');
                  break;
               case "Jp.txt":
                  ListeJaponais.Add(lecteurDonnées.ReadLine() + '\n');
                  break;
               case "versMenu.txt":
                  ListeInfosRecus.Add(int.Parse(lecteurDonnées.ReadLine() + '\n'));
                  break;
            }
         }
         lecteurDonnées.Close();
      }

      public static void ÉcrireFichier(List<int> listeInfo)
      {
         StreamWriter écrivainDonnées = new StreamWriter(CHEMIN_ÉCRITURE + NOM_FICHIER_ENVOI);

         foreach (int info in listeInfo)
         {
            écrivainDonnées.WriteLine(info.ToString());
         }
         écrivainDonnées.Close();
      }

      static void ChoisirRéglages()
      {
         if (PremierFichier == true)
         {
            RéglagesBase();
         }
         else
         {
            RéglagesModifiés();
         }
      }

      public static void RéglagesBase()
      {
         Langue = LANGUE_BASE;
         RenderDistance = RENDER_D_BASE;
         Fps = FPS_BASE;        
         VolMusique = VOL_MUS_BASE;
         VolEffets = VOL_EFF_BASE;
         FullscreenMode = 1;
         KeyboardMode = 1;      
         NbNiveauxComplétés = NB_NIVEAUX_BASE;
         Temps = TEMPS_BASE;   
      }

      static void RéglagesModifiés()
      {
         Langue = ListeInfosRecus[0];
         RenderDistance = ListeInfosRecus[2];
         Fps = ListeInfosRecus[1];
         VolMusique = ListeInfosRecus[3];
         VolEffets = ListeInfosRecus[4];
         FullscreenMode = ListeInfosRecus[5];
         KeyboardMode = ListeInfosRecus[6];
         NbNiveauxComplétés = ListeInfosRecus[7];
         Temps = new TimeSpan(ListeInfosRecus[8],ListeInfosRecus[9],0);
      }
   }
}
