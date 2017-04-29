using System;
using System.Collections.Generic;
using System.IO;

namespace Launching_Interface
{
   public static class GererDonnees
   {
      const string CHEMIN_LECTURE_BASE = "../../";

      public static bool RD = true;
      //public const int NBRE_NIVEAUX = 8;
      const int LANGUE_BASE = 0;
      const int FPS_BASE = 60;
      const int RENDER_D_BASE = 500;
      const int VOL_MUS_BASE = 50;
      const int VOL_EFF_BASE = 50;
      const int NB_NIVEAUX_BASE = 0;
      const int NBRE_CARACTÉRISTIQUES = 7;
      static TimeSpan TEMPS_BASE = new TimeSpan(0,0,0);  //pour nous, constante

      enum Langues { Francais, Anglais, Espagnol, japonais }
      enum Fullscreen { oui,non }
      enum Controller { Manette, Clavier}
      enum NbreNiveauxFinis { Auncun,Un,Deux,Trois,Quatres,Cinq,Six,Sept,Huit}

      static int langue;
      static int fullscreen;
      static int controller;
      static int nbreNiveauxFinis;
  //    static TimeSpan temps;

      public static int Langue
      {
         get { return langue; }
         set
         {
            if (value >= (int)Langues.Francais && value <= (int)Langues.japonais)
            {
               langue = value;
            }
            else
            {
               //throw new ArgumentException("Langue invalide");
               langue = (int)Langues.Francais;
            }
         }
      }
      public static int Fps;    // 30,60,90,120
      public static int RenderDistance; // 10,50,100,500,1000,5000,10000,50000,100000
      public static int VolMusique; // de 0 à 100
      public static int VolEffets; // de 0 à 100
      public static int FullscreenMode // 0 = false || 1 = true
      {
         get { return fullscreen; }
         set
         {
            if (value == (int)Fullscreen.non || value == (int)Fullscreen.oui)
            {
               fullscreen = value;
            }
            else
            {
               fullscreen = (int)Fullscreen.oui;
            }
         }
      } 
      public static int KeyboardMode // 0 = false || 1 = true
      {
         get { return controller; }
         set
         {
            if (value == (int)Controller.Manette || value == (int)Controller.Clavier)
            {
               controller = value;
            }
            else
            {
               controller = (int)Controller.Clavier;
            }
         }
      }
      public static int NbNiveauxComplétés // 1,2,3,4,5,6,7,8
      {
         get { return nbreNiveauxFinis; }
         set
         {
            if (value >= (int)NbreNiveauxFinis.Auncun && value <= (int)NbreNiveauxFinis.Huit)
            {
               nbreNiveauxFinis = value;
            }
            else
            {
               nbreNiveauxFinis = (int)Langues.Francais;
            }
         }
      }     
      public static bool PremierFichier;
      public static TimeSpan Temps;
      //{
      //   get { return temps; }
      //   set
      //   {
      //      value = new TimeSpan(value.Hours, value.Minutes,int.Parse( string.Format("00", value.Seconds)));
      //      temps = value;
      //      //a.Seconds = string.Format("00", value.Seconds);
      //      //if (value = strSeconds )
      //      //{
      //      //   langue = value;
      //      //}
      //      //else
      //      //{
      //      //   //throw new ArgumentException("Langue invalide");
      //      //   langue = (int)Langues.Francais;
      //      //}
      //   }
      //}

      public static List<string> ListeFrancais { get; private set; }
      public static List<string> ListeAnglais  { get; private set; }
      public static List<string> ListeEspagnol { get; private set; }
      public static List<string> ListeJaponais { get; private set; }

      public static List<string> ListeCaractéristiquesAAfficher0 { get; private set; }
      public static List<string> ListeCaractéristiquesAAfficher1 { get; private set; }
      public static List<string> ListeCaractéristiquesAAfficher2 { get; private set; }
      public static bool[] GameExists;

      public static void InitGererDonnees(StreamReader reader)
      {
         for (int i = 0; i < NBRE_CARACTÉRISTIQUES; i++)
         {
            string line = reader.ReadLine();
            string[] parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            int caractéristique = int.Parse(parts[1]);

            switch (i)
            {
               case 0:
                  VolMusique = caractéristique;
                  break;
               case 1:
                  VolEffets = caractéristique;
                  break;
               case 2:
                  Langue = caractéristique;
                  break;
               case 3:
                  RenderDistance = caractéristique;
                  break;
               case 4:
                  Fps = caractéristique;
                  break;
               case 5:
                  FullscreenMode = caractéristique;
                  break;
               case 6:
                  KeyboardMode = caractéristique;
                  break;
            }          
         }
         reader.Close();
      }

      static GererDonnees()
      {
         ListeFrancais = new List<string>();
         ListeAnglais  = new List<string>();
         ListeEspagnol = new List<string>();
         ListeJaponais = new List<string>();
         ListeCaractéristiquesAAfficher0 = new List<string>();
         ListeCaractéristiquesAAfficher1 = new List<string>();
         ListeCaractéristiquesAAfficher2 = new List<string>();
            //InitializeComplete();
         LireFichiers("Langues","En.txt");
         LireFichiers("Langues","Es.txt");
         LireFichiers("Langues","Jp.txt");
         LireFichiers("Langues","Fr.txt");


            RefreshSaves();
      }

        static void InitializeComplete()
        {
            Complete = new List<bool>[3];
            for (int i = 0; i < 3; ++i)
            {
                Complete[i] = new List<bool>();
            }
        }

        public static void RefreshSaves()
        {
            InitializeComplete();
            CheckForExistingGames();
            if (GameExists[0])
            {
                LireFichiers("Saves", "save0.txt");
            }

            if (GameExists[1])
            {
                LireFichiers("Saves", "save1.txt");
            }
            if (GameExists[2])
            {
                LireFichiers("Saves", "save2.txt");
            }
        }

      static void LireFichiers(string nomDossier,string nomFichier)
      {
         StreamReader lecteurDonnées = new StreamReader(CHEMIN_LECTURE_BASE + nomDossier+"/" + nomFichier);
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
               case "save0.txt":
                  GérerFichiersSaves(lecteurDonnées, 0);
                  break;
               case "save1.txt":
                  GérerFichiersSaves(lecteurDonnées, 1);
                  break;
               case "save2.txt":
                  GérerFichiersSaves(lecteurDonnées, 2);
                  break;
               default:
                  throw new Exception("Auncun fichier n'a été lu dans la classe statique");                  
            }
         }
         lecteurDonnées.Close();
      }

      static void GérerFichiersSaves(StreamReader lecteurDonnées,int i)
      {
         List<string> listeCaractéristiquestemporaire = new List<string>();

         for (int j = 0; j < 6; j++)
         {
            string line_ = lecteurDonnées.ReadLine();
            string symboleQuiSépare = " ";
            

            switch (j)
            {
               case 0:
                  symboleQuiSépare = "l: ";
                  break;
               case 1:
                  symboleQuiSépare = "n: ";
                  break;
               case 2:
                  symboleQuiSépare = "n: ";
                  break;
               case 3:
                  symboleQuiSépare = "d: ";
                  break;
               case 4:
                  symboleQuiSépare = "e: ";
                  break;
               case 5:
                  symboleQuiSépare = "k: ";
                  break;
               //case 6:
               //   symboleQuiSépare = ";";
               //   break;
            }
            string[] parts_ = line_.Split(new string[] { symboleQuiSépare }, StringSplitOptions.None);
            listeCaractéristiquestemporaire.Add(parts_[1]);

            //if (j == 3)
            //{

            //   string[] séparateur = parts[1].Split(new string[] { ":" }, StringSplitOptions.None);
            //   string tempsFormater = séparateur[2].Remove(2);
            //   string aa = parts[1].Remove(6) + tempsFormater;
            //   listeCaractéristiquestemporaire.Add(aa);
            //}
            //else
            //{

            //}



         }

            //listeCaractéristiquestemporaire.Add(lecteurDonnées.ReadLine());   //  nom  (#8)
            string line = lecteurDonnées.ReadLine();
            string[] parts = line.Split(new char[] { ';' });
            for (int j = 0; j < parts.Length; ++j)
            {
                Complete[i].Add(bool.Parse(parts[j]));
            }
            //   string ligneLue = lecteurDonnées.ReadLine();
            //   string[] séparateurTemps = ligneLue.Split(new string[] { ";" }, StringSplitOptions.None);
            //   for (int k = 0; k < NBRE_NIVEAUX; k++)
            //{
            //   //string ligneLue = lecteurDonnées.ReadLine();
            //   //string[] séparateurTemps = ligneLue.Split(new string[] { ";" }, StringSplitOptions.None);
            //   listeCaractéristiquestemporaire.Add(séparateurTemps[/*1*/k]);                       
            //}

            AssocierBonneListeAfficher(i,listeCaractéristiquestemporaire);
      }

        static List<bool>[] Complete { get; set; }

        public static int CountComplete(int i)
        {
            int numComplete = 0;

            foreach (bool e in Complete[i])
            {
                if (e)
                {
                    ++numComplete;
                }
            }
            return numComplete;
        }

        public static int CountLevels(int i)
        {
            return Complete[i].Count;
        }

        static void AssocierBonneListeAfficher(int i,List<string> listeCaractéristiquestemporaire)
      {
         switch (i)
         {
            case 0:
               ListeCaractéristiquesAAfficher0 = listeCaractéristiquestemporaire;
               break;
            case 1:
               ListeCaractéristiquesAAfficher1 = listeCaractéristiquestemporaire;
               break;
            case 2:
               ListeCaractéristiquesAAfficher2 = listeCaractéristiquestemporaire;
               break;
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

      static void CheckForExistingGames()
      {
         StreamReader r;

         GererDonnees.GameExists = new bool[3];
         for (int i = 0; i < 3; ++i)
         {
            r = new StreamReader("../../Saves/save" + i + ".txt");
            GererDonnees.GameExists[i] = r.ReadLine() != "";
            r.Close();
         }
      }


      //string Lignelue = lecteurDonnées.ReadLine();
      //string[] séparateur = Lignelue.Split(new string[] { "l: " }, StringSplitOptions.None);
      //listeCaractéristiquestemporaire.Add(séparateur[1]);

      //Lignelue = lecteurDonnées.ReadLine();
      //séparateur = Lignelue.Split(new string[] { "n: " }, StringSplitOptions.None);
      //listeCaractéristiquestemporaire.Add(séparateur[1]);

      //Lignelue = lecteurDonnées.ReadLine();
      //séparateur = Lignelue.Split(new string[] { "n: " }, StringSplitOptions.None);
      //listeCaractéristiquestemporaire.Add(séparateur[1]);

      //Lignelue = lecteurDonnées.ReadLine();
      //séparateur = Lignelue.Split(new string[] { "d: " }, StringSplitOptions.None);
      //listeCaractéristiquestemporaire.Add(séparateur[1]);

      //Lignelue = lecteurDonnées.ReadLine();
      //séparateur = Lignelue.Split(new string[] { "e: " }, StringSplitOptions.None);   //   #5
      //listeCaractéristiquestemporaire.Add(séparateur[1]);

      //Lignelue = lecteurDonnées.ReadLine();
      //séparateur = Lignelue.Split(new string[] { "k: " }, StringSplitOptions.None);
      //listeCaractéristiquestemporaire.Add(séparateur[1]);


      //Lignelue = lecteurDonnées.ReadLine();
      //séparateur = Lignelue.Split(new string[] { ";" }, StringSplitOptions.None);    //   #7
      //listeCaractéristiquestemporaire.Add(séparateur[1]);

      //-----------------------------------------------------------------------------------------------------------------
      // Caractéristiques du participant (1 nom,8 temps)
      //    listeCaractéristiquestemporaire.Add(lecteurDonnées.ReadLine());   //  false;false;false
   }
}

