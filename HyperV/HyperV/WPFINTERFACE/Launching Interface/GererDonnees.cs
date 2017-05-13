using System;
using System.Collections.Generic;
using System.IO;

namespace Launching_Interface
{
   public static class GererDonnees
   {
      const string CHEMIN_LECTURE_BASE = "../../";
      const string BACKSLASH = "/";
      const string MESSAGE_ERREUR_LECTURE = "Auncun fichier n'a été lu dans la classe statique";

      public static bool RenderDistenceModifiée = true;
      public const int NBRE_NIVEAUX = 8,
                       NBRE_SAUVEGARDES = 3,
                       LANGUE_BASE = 0,
                       FPS_BASE = 60,
                       RENDER_D_BASE = 500,
                       VOL_MUS_BASE = 50,
                       VOL_EFF_BASE = 50,
                       NB_NIVEAUX_BASE = 0,
                       NBRE_CARACTÉRISTIQUES_RÉGLAGES = 7,
                       NBRE_CARACTÉRISTIQUE_HYPERV = 6,
                       NBRE_RENDER_DISTANCE = 9,
                       NBRE_FPS = 4;

      static TimeSpan TEMPS_BASE = TimeSpan.Zero;

      public enum Langues { Francais, Anglais, Espagnol, Japonais }
      public enum Fullscreen { oui, non }
      public enum Controller { Manette, Clavier }

      static Langues langue;
      static Fullscreen fullscreen;
      static Controller controller;

      public static Langues Langue
      {
         get { return langue; }
         set
         {
            if (value >= Langues.Francais && value <= Langues.Japonais)
            {
               langue = value;
            }
            else
            {
               langue = (int)Langues.Francais;
            }
         }
      }
      public static int Fps;
      public static int RenderDistance;
      public static int VolMusique;
      public static int VolEffets;
      public static Fullscreen FullscreenMode
      {
         get { return fullscreen; }
         set
         {
            if (value == Fullscreen.non || value == Fullscreen.oui)
            {
               fullscreen = value;
            }
            else
            {
               fullscreen = Fullscreen.oui;
            }
         }
      }
      public static Controller KeyboardMode
      {
         get { return controller; }
         set
         {
            if (value == Controller.Manette || value == Controller.Clavier)
            {
               controller = value;
            }
            else
            {
               controller = Controller.Clavier;
            }
         }
      }
      public static bool PremierFichier;
      public static TimeSpan Temps;

      public static List<string> ListeFrancais { get; private set; }
      public static List<string> ListeAnglais { get; private set; }
      public static List<string> ListeEspagnol { get; private set; }
      public static List<string> ListeJaponais { get; private set; }
      public static List<string> ListeCaractéristiquesAAfficher0 { get; private set; }
      public static List<string> ListeCaractéristiquesAAfficher1 { get; private set; }
      public static List<string> ListeCaractéristiquesAAfficher2 { get; private set; }
      static List<bool>[] EstComplété { get; set; }

      public static List<Joueur> ListeJoueurs { get; private set; }

      public static bool[] JeuEstExistant;

      public static void InitialiserGererDonnees(StreamReader lecteurDonnées)
      {
         string[] parties;
         int[] caractéristiques = new int[NBRE_CARACTÉRISTIQUES_RÉGLAGES];
         string ligne;
         int cpt = 0;

         for (int i = 0; i < NBRE_CARACTÉRISTIQUES_RÉGLAGES; i++)
         {
            ligne = lecteurDonnées.ReadLine();
            parties = ligne.Split(new string[] { ": " }, StringSplitOptions.None);
            caractéristiques[i] = int.Parse(parties[1]);
         }

         VolMusique = caractéristiques[cpt]; cpt++;
         VolEffets = caractéristiques[cpt]; cpt++;
         Langue = (Langues)caractéristiques[cpt]; cpt++;
         RenderDistance = caractéristiques[cpt]; cpt++;
         Fps = caractéristiques[cpt]; cpt++;
         FullscreenMode = (Fullscreen)caractéristiques[cpt]; cpt++;
         KeyboardMode = (Controller)caractéristiques[cpt];
         lecteurDonnées.Close();
      }

      static GererDonnees()
      {
         ListeFrancais = new List<string>();
         ListeAnglais = new List<string>();
         ListeEspagnol = new List<string>();
         ListeJaponais = new List<string>();
         ListeCaractéristiquesAAfficher0 = new List<string>();
         ListeCaractéristiquesAAfficher1 = new List<string>();
         ListeCaractéristiquesAAfficher2 = new List<string>();
         LireFichiers("Langues", "En.txt");
         LireFichiers("Langues", "Es.txt");
         LireFichiers("Langues", "Jp.txt");
         LireFichiers("Langues", "Fr.txt");
         RafraichirSauvegardes();
      }

      static void InitialisationEstComplété()
      {
         EstComplété = new List<bool>[NBRE_SAUVEGARDES];
         for (int i = 0; i < NBRE_SAUVEGARDES; ++i)
         {
            EstComplété[i] = new List<bool>();
         }
      }

      public static void RafraichirSauvegardes()
      {
         InitialisationEstComplété();
         VérifieExistanceParties();

         for (int i = 0; i < NBRE_SAUVEGARDES; i++)
         {
            if (JeuEstExistant[i])
            {
               LireFichiers("Saves", "save" + i + ".txt");
            }
         }
      }

      static void LireFichiers(string nomDossier, string nomFichier)
      {
         StreamReader lecteurDonnées = new StreamReader(CHEMIN_LECTURE_BASE + nomDossier + BACKSLASH + nomFichier);
         while (!lecteurDonnées.EndOfStream)
         {
            switch (nomFichier)
            {
               case "Fr.txt":
                  ListeFrancais.Add(lecteurDonnées.ReadLine());
                  break;
               case "En.txt":
                  ListeAnglais.Add(lecteurDonnées.ReadLine());
                  break;
               case "Es.txt":
                  ListeEspagnol.Add(lecteurDonnées.ReadLine());
                  break;
               case "Jp.txt":
                  ListeJaponais.Add(lecteurDonnées.ReadLine());
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
                  throw new Exception(MESSAGE_ERREUR_LECTURE);
            }
         }
         lecteurDonnées.Close();
      }

      static void GérerFichiersSaves(StreamReader lecteurDonnées, int i)
      {
         List<string> listeCaractéristiquestemporaire = new List<string>();

         string[] parties;
         string[] symboleQuiSépare = new string[NBRE_CARACTÉRISTIQUE_HYPERV] { "l: ", "n: ", "n: ", "d: ", "e: ", "k: " };
         string ligne;

         for (int j = 0; j < NBRE_CARACTÉRISTIQUE_HYPERV; j++)
         {
            ligne = lecteurDonnées.ReadLine();
            parties = ligne.Split(new string[] { symboleQuiSépare[j] }, StringSplitOptions.None);
            listeCaractéristiquestemporaire.Add(parties[1]);
         }

         string line = lecteurDonnées.ReadLine();
         string[] parts = line.Split(new char[] { ';' });
         for (int j = 0; j < parts.Length; ++j)
         {
            EstComplété[i].Add(bool.Parse(parts[j]));
         }

         AssocierBonneListeAfficher(i, listeCaractéristiquestemporaire);
      }

      public static int NbreNiveauxComplétés(int i)
      {
         int nbreNiveaux = 0;

         foreach (bool e in EstComplété[i])
         {
            if (e)
            {
               ++nbreNiveaux;
            }
         }
         return nbreNiveaux;
      }

      public static int NombreNiveaux(int i)
      {
         return EstComplété[i].Count;
      }

      static void AssocierBonneListeAfficher(int i, List<string> listeCaractéristiquestemporaire)
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
         FullscreenMode = Fullscreen.oui;
         KeyboardMode = Controller.Manette;
         Temps = TEMPS_BASE;
      }

      static void VérifieExistanceParties()
      {
         StreamReader lecteurDonnées;
         JeuEstExistant = new bool[NBRE_SAUVEGARDES];
         for (int i = 0; i < NBRE_SAUVEGARDES; ++i)
         {
            lecteurDonnées = new StreamReader("../../Saves/save" + i + ".txt");
            JeuEstExistant[i] = lecteurDonnées.ReadLine() != "";
            lecteurDonnées.Close();
         }
      }

   }
}

