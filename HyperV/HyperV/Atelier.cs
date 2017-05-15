using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Diagnostics;
using AtelierXNA;
using System.Threading;
using System.Globalization;

namespace HyperV
{
   public enum Language
   {
      French, English, Spanish, Japanese
   }


   enum Input
   {
      Controller, Keyboard
   }

   public class Atelier : Microsoft.Xna.Framework.Game
   {
      bool PuzzleRuneCompletePremiereFois { get; set; }
      const float INTERVALLE_CALCUL_FPS = 1f;
      float FpsInterval { get; set; }
      GraphicsDeviceManager PériphériqueGraphique { get; set; }

      Caméra Camera { get; set; }
      List<Maze> Maze { get; set; }
      InputManager InputManager { get; set; }
      GamePadManager GamePadManager { get; set; }

      //GraphicsDeviceManager PériphériqueGraphique { get; set; }
      SpriteBatch SpriteBatch { get; set; }

      RessourcesManager<SpriteFont> FontManager { get; set; }
      RessourcesManager<Texture2D> TextureManager { get; set; }
      RessourcesManager<Model> ModelManager { get; set; }
      RessourcesManager<Song> SongManager { get; set; }
      Song Song { get; set; }
      PressSpaceLabel PressSpaceLabel { get; set; }
      //Caméra CaméraJeu { get; set; }

      public Atelier()
      {
         PériphériqueGraphique = new GraphicsDeviceManager(this);
         Content.RootDirectory = "Content";
         PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
         IsFixedTimeStep = false;
         IsMouseVisible = false;
         //PériphériqueGraphique.PreferredBackBufferHeight = 500;
         //PériphériqueGraphique.PreferredBackBufferWidth = 900;
         PériphériqueGraphique.PreferredBackBufferHeight = 800;
         PériphériqueGraphique.PreferredBackBufferWidth = 1500;
      }

      Gazon Gazon { get; set; }
      Grass Grass { get; set; }
      Ceiling Ceiling { get; set; }

      RessourcesManager<Video> VideoManager { get; set; }
      CutscenePlayer CutscenePlayer { get; set; }
      List<Walls> Walls { get; set; }
      Character Robot { get; set; }
      List<Character> Characters { get; set; }
      int SaveNumber { get; set; }
      int Level { get; set; }
      Vector3 Position { get; set; }
      Vector3 Direction { get; set; }
      TexteCentré Loading { get; set; }
      TexteCentré GameOver { get; set; }
      TexteCentré Success { get; set; }
      TimeSpan TimePlayed { get; set; }
      public Language Language { get; private set; }
      int RenderDistance { get; set; }
      bool FullScreen { get; set; }
      Input Input { get; set; }
      Sprite Crosshair { get; set; }
      RessourcesManager<SoundEffect> SoundManager { get; set; }
      List<House> Houses { get; set; }
      List<UnlockableWall> Unlockables { get; set; }

      void LoadSettings()
      {
         //StreamReader reader = new StreamReader("F:/programmation clg/quatrième session/WPFINTERFACE/Launching Interface/Saves/Settings.txt");
         //StreamReader reader = new StreamReader("C:/Users/Mathieu/Source/Repos/WPFINTERFACE/Launching Interface/Saves/Settings.txt");
         StreamReader reader = new StreamReader("../../../WPFINTERFACE/Launching Interface/Saves/Settings.txt");
         string line = reader.ReadLine();
         string[] parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
         MediaPlayer.Volume = int.Parse(parts[1]) / 100.0f;
         line = reader.ReadLine();
         parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
         SoundEffect.MasterVolume = int.Parse(parts[1]) / 100.0f;
         line = reader.ReadLine();
         parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
         Language = (Language)int.Parse(parts[1]);
         Services.RemoveService(typeof(Language));
         Services.AddService(typeof(Language), Language);
         line = reader.ReadLine();
         parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
         RenderDistance = int.Parse(parts[1]);
         if (Camera != null)
         {
            (Camera as CaméraJoueur).ÉtablirDistenceDeRendu(RenderDistance);
         }
         line = reader.ReadLine();
         parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
         FpsInterval = 1.0f / int.Parse(parts[1]);
         TargetElapsedTime = new TimeSpan((int)(FpsInterval * 10000000));
         line = reader.ReadLine();
         parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
         FullScreen = int.Parse(parts[1]) == 1;
         if (FullScreen != PériphériqueGraphique.IsFullScreen)
         {
            //PériphériqueGraphique.ToggleFullScreen();
         }
         line = reader.ReadLine();
         parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
         Input = (Input)int.Parse(parts[1]);
         reader.Close();
      }

      void LoadSave()
      {
         StreamReader reader = new StreamReader("../../../WPFINTERFACE/Launching Interface/Saves/save.txt");
         SaveNumber = int.Parse(reader.ReadLine());
         reader.Close();

         reader = new StreamReader("../../../WPFINTERFACE/Launching Interface/Saves/save" + SaveNumber + ".txt");
         string line = reader.ReadLine();
         string[] parts = line.Split(new char[] { ' ' });
         Level = int.Parse(parts[1]);

         line = reader.ReadLine();
         parts = line.Split(new string[] { "n: " }, StringSplitOptions.None);
         Position = Vector3Parse(parts[1]);
         line = reader.ReadLine();
         parts = line.Split(new string[] { "n: " }, StringSplitOptions.None);
         Direction = Vector3Parse(parts[1]);
         line = reader.ReadLine();
         parts = line.Split(new string[] { "d: " }, StringSplitOptions.None);
         TimePlayed = TimeSpan.Parse(parts[1]);
         line = reader.ReadLine();
         parts = line.Split(new string[] { "e: " }, StringSplitOptions.None);
         LifeBars[0] = new LifeBar(this, int.Parse(parts[1]), "Gauge", "Dock", new Vector2(30, Window.ClientBounds.Height - 70), FpsInterval);
         line = reader.ReadLine();
         parts = line.Split(new string[] { "k: " }, StringSplitOptions.None);
         LifeBars[0].Attack(int.Parse(parts[1]));
         LifeBars[1] = new LifeBar(this, 300, "StaminaGauge", "TiredGauge", "WaterGauge", "Dock", new Vector2(30, Window.ClientBounds.Height - 130), FpsInterval);
         Complete = new List<bool>();
         line = reader.ReadLine();
         parts = line.Split(new char[] { ';' });
         for (int i = 0; i < parts.Length; ++i)
         {
            Complete.Add(bool.Parse(parts[i]));
         }
         reader.Close();
      }

      Vector2 Vector2Parse(string parse)
      {
         int startInd = parse.IndexOf("X:") + 2;
         float aXPosition = float.Parse(parse.Substring(startInd, parse.IndexOf(" Y") - startInd));
         startInd = parse.IndexOf("Y:") + 2;
         float aYPosition = float.Parse(parse.Substring(startInd, parse.IndexOf("}") - startInd));
         return new Vector2(aXPosition, aYPosition);
      }

      Vector3 Vector3Parse(string parse)
      {
         int startInd = parse.IndexOf("X:") + 2;
         float aXPosition = float.Parse(parse.Substring(startInd, parse.IndexOf(" Y") - startInd));
         startInd = parse.IndexOf("Y:") + 2;
         float aYPosition = float.Parse(parse.Substring(startInd, parse.IndexOf(" Z") - startInd));
         startInd = parse.IndexOf("Z:") + 2;
         float aZPosition = float.Parse(parse.Substring(startInd, parse.IndexOf("}") - startInd));
         return new Vector3(aXPosition, aYPosition, aZPosition);
      }

      void SelectWorld(bool usePosition)
      {
         SelectLevel(usePosition, Level);
         //LevelPrison(usePosition);
         //RythmLevel();
         Save();
      }

      void SelectLevel(bool usePosition, int level)
      {
         MediaPlayer.Stop();
         Components.Clear();

            if (level > 0)
            {
                switch (level)
                {
                    case 1:
                        Song = SongManager.Find("castle");
                        break;
                    case 2:
                        Song = SongManager.Find("Elf");
                        break;
                    case 3:
                        Song = SongManager.Find("Elf");
                        break;
                    case 4:
                        Song = SongManager.Find("King Arthur");
                        break;
                    case 5:
                        Song = SongManager.Find("Kingdom of Bards");
                        break;
                    case 6:
                        Song = SongManager.Find("Elf");
                        break;
                    case 7:
                        Song = SongManager.Find("exid");
                        break;
                    case 8:
                        Song = SongManager.Find("Elf");
                        break;
                    case 9:
                        Song = SongManager.Find("Elf");
                        break;
                }
                MediaPlayer.Play(Song);
            }



         StreamReader reader = new StreamReader("../../../Levels/Level" + level.ToString() + ".txt");
         string line;
         string[] parts;
         bool boss = false;
         Components.Add(InputManager);
         Components.Add(GamePadManager);
         while (!reader.EndOfStream)
         {
            line = reader.ReadLine();
            parts = line.Split(new char[] { ';' });
            switch (parts[0])
            {
               case "#":
                  break;
               case "SpaceBackground":
                  Components.Add(new ArrièrePlanSpatial(this, parts[1], FpsInterval));
                  break;
               case "Display3D":
                  Display3D = new Afficheur3D(this);
                  Components.Add(Display3D);
                  Services.RemoveService(typeof(Afficheur3D));
                  Services.AddService(typeof(Afficheur3D), Display3D);
                  break;
               case "Camera":
                  if (usePosition)
                  {
                     Camera = new Camera2(this, Position, new Vector3(20, 0, 0), Vector3.Up, FpsInterval, RenderDistance);
                     (Camera as Camera2).ÉtablirDirection(Direction);
                  }
                  else
                  {
                     Camera = new Camera2(this, Vector3Parse(parts[1]), Vector3Parse(parts[2]), Vector3.Up, FpsInterval, RenderDistance);
                     (Camera as Camera2).ÉtablirDirection(Vector3Parse(parts[3]));
                  }
                  //(Camera as Camera2).SetRenderDistance(RenderDistance);
                  Services.RemoveService(typeof(Caméra));
                  Services.AddService(typeof(Caméra), Camera);
                  break;
               case "Runes":
                  AjouterRunes();
                  break;
               case "Maze":
                  Maze.Add(new Maze(this, float.Parse(parts[1]), Vector3Parse(parts[2]), Vector3Parse(parts[3]), Vector3Parse(parts[4]), parts[5], FpsInterval, parts[6]));
                  Components.Add(Maze.Last());
                  Services.RemoveService(typeof(List<Maze>));
                  Services.AddService(typeof(List<Maze>), Maze);
                  break;
               case "Boss":
                  boss = true;
                  Boss = new Boss(this, parts[1], int.Parse(parts[2]), parts[3], parts[4], parts[5], parts[6], FpsInterval, FpsInterval, float.Parse(parts[7]), Vector3Parse(parts[8]), Vector3Parse(parts[9]));
                  Components.Add(Boss);
                  Services.RemoveService(typeof(Boss));
                  Services.AddService(typeof(Boss), Boss);
                  break;
               case "Mill":
                  Mill = new Mill(this, float.Parse(parts[1]), Vector3Parse(parts[2]), Vector3Parse(parts[3]), parts[4], Vector2Parse(parts[5]), FpsInterval, parts[6]);
                  Components.Add(Mill);
                  Mill.AddLabel();
                  Services.RemoveService(typeof(Mill));
                  Services.AddService(typeof(Mill), Mill);
                  break;
               case "Food":
                  Food.Add(new Food(this, parts[1], float.Parse(parts[2]), Vector3Parse(parts[3]), Vector3Parse(parts[4]), int.Parse(parts[5]), FpsInterval));
                  Components.Add(Food.Last());
                  Services.RemoveService(typeof(List<Food>));
                  Services.AddService(typeof(List<Food>), Food);
                  break;
               case "Enemy":
                  Enemy.Add(new Enemy(this, parts[1], float.Parse(parts[2]), Vector3Parse(parts[3]), Vector3Parse(parts[4]), int.Parse(parts[5]), int.Parse(parts[6]), float.Parse(parts[7]), FpsInterval));
                  Components.Add(Enemy.Last());
                  Services.RemoveService(typeof(List<Enemy>));
                  Services.AddService(typeof(List<Enemy>), Enemy);
                  break;
               case "Arc":
                  Components.Add(new Arc(this, parts[1], float.Parse(parts[2]), Vector3Parse(parts[3]), Vector3Parse(parts[4])));
                  break;
               case "Epee":
                  Épée = new Epee(this, parts[1], float.Parse(parts[2]), Vector3Parse(parts[3]), Vector3Parse(parts[4]), int.Parse(parts[5]));
                  Components.Add(Épée);
                  Services.RemoveService(typeof(Epee));
                  Services.AddService(typeof(Epee), Épée);
                  break;
               case "Character":
                  Characters.Add(new Character(this, parts[1], float.Parse(parts[2]), Vector3Parse(parts[3]), Vector3Parse(parts[4]), parts[5], parts[6], parts[7], parts[8], FpsInterval));
                  Components.Add(Characters.Last());
                  Services.RemoveService(typeof(List<Character>));
                  Services.AddService(typeof(List<Character>), Characters);
                  break;
               case "Grass":
                  Grass = new Grass(this, float.Parse(parts[1]), Vector3Parse(parts[2]), Vector3Parse(parts[3]), Vector2Parse(parts[4]), parts[5], Vector2Parse(parts[6]), FpsInterval);
                  Components.Add(Grass);
                  Services.RemoveService(typeof(Grass));
                  Services.AddService(typeof(Grass), Grass);
                  break;
               case "Ceiling":
                  Ceiling = new Ceiling(this, float.Parse(parts[1]), Vector3Parse(parts[2]), Vector3Parse(parts[3]), Vector2Parse(parts[4]), parts[5], Vector2Parse(parts[6]), FpsInterval);
                  Components.Add(Ceiling);
                  Services.RemoveService(typeof(Ceiling));
                  Services.AddService(typeof(Ceiling), Ceiling);
                  break;
               case "Walls":
                  Walls.Add(new Walls(this, FpsInterval, parts[1], parts[2], float.Parse(parts[3])));
                  Components.Add(Walls.Last());
                  Services.RemoveService(typeof(List<Walls>));
                  Services.AddService(typeof(List<Walls>), Walls);
                  break;
               case "Portal":
                  Portals.Add(new Portal(this, float.Parse(parts[1]), Vector3Parse(parts[2]), Vector3Parse(parts[3]), Vector2Parse(parts[4]), level == 1 && Complete[Portals.Count/*+ 2*/] ? "Complete" : parts[5], int.Parse(parts[6]), FpsInterval));
                  Components.Add(Portals.Last());
                  break;
               case "CutscenePlayer":
                  CutscenePlayer = new CutscenePlayer(this, parts[1], bool.Parse(parts[2]), parts[3]);
                  Components.Add(CutscenePlayer);
                  break;
               case "Catapulte":
                  Components.Add(new Catapulte(this, parts[1], Vector3Parse(parts[2]), float.Parse(parts[3]), float.Parse(parts[4])));
                  break;
               case "AjouterModeles":
                  AjouterModeles(parts[1]);
                  break;
               case "AjouterArbres":
                  AjouterArbres();
                  break;
               case "AjouterTours":
                  AjouterTours();
                  break;
               case "HeightMap":
                  HeightMap.Add(new HeightMap(this, float.Parse(parts[1]), Vector3Parse(parts[2]), Vector3Parse(parts[3]), Vector3Parse(parts[4]), parts[5], new string[] { parts[6], parts[7] }));
                  Components.Add(HeightMap.Last());
                  Services.RemoveService(typeof(List<HeightMap>));
                  Services.AddService(typeof(List<HeightMap>), HeightMap);
                  break;
               case "Livres":
                  AjouterLivres();
                  break;
               case "Boutons":
                  AjouterBoutons();
                  break;
               case "House":
                  Houses.Add(new House(this, parts[1], float.Parse(parts[2]), Vector3Parse(parts[3]), Vector3Parse(parts[4]), Vector3Parse(parts[5]), Vector3Parse(parts[6])));
                  Components.Add(Houses.Last());
                  Services.RemoveService(typeof(List<House>));
                  Services.AddService(typeof(List<House>), Houses);
                  break;
               case "Skybox":
                  Components.Add(new Skybox(this, parts[1]));
                  break;
               case "UnlockableWall":
                  Unlockables.Add(new UnlockableWall(this, float.Parse(parts[1]), Vector3Parse(parts[2]), Vector3Parse(parts[3]), Vector2Parse(parts[4]), parts[5], FpsInterval, int.Parse(parts[6]), CountComplete(), ListeRunes, SaveNumber));
                  Components.Add(Unlockables.Last());
                  Services.RemoveService(typeof(List<UnlockableWall>));
                  Services.AddService(typeof(List<UnlockableWall>), Unlockables);
                  break;
               case "Water":
                  Water.Add(new Water(this, float.Parse(parts[1]), Vector3Parse(parts[2]), Vector3Parse(parts[3]), Vector2Parse(parts[4]), FpsInterval));
                  Components.Add(Water.Last());
                  Services.RemoveService(typeof(List<Water>));
                  Services.AddService(typeof(List<Water>), Water);
                  break;
               case "Prison":
                  LevelPrison(false);
                  break;
               case "Rythm":
                  NiveauRythmé();
                  break;
            }
         }
         reader.Close();
         if (Level != 0)
         {
            Components.Add(PressSpaceLabel);
            PressSpaceLabel.Visible = false;
            if (boss)
            {
               Boss.AddFireball();
               Boss.AddLabel();
            }
            Components.Add(LifeBars[0]);
            Components.Add(LifeBars[1]);
            Services.RemoveService(typeof(LifeBar[]));
            Services.AddService(typeof(LifeBar[]), LifeBars);
            AddCharacterLabels();
            AddFoodLabels();
            Components.Add(Camera);
            Components.Remove(Loading);
         Components.Add(Crosshair);
            //LifeBars[0].Visible = false;
            //LifeBars[1].Visible = false;

            //Components.Add(FPSLabel);
         }
      }

      void AddCharacterLabels()
      {
         foreach (Character e in Characters)
         {
            e.AddLabel();
         }
      }

      void AddFoodLabels()
      {
         foreach (Food e in Food)
         {
            e.AddLabel();
         }
      }

      Grass[,] GrassArray { get; set; }
      Ceiling[,] CeilingArray { get; set; }
      ArrièrePlanSpatial SpaceBackground { get; set; }
      AfficheurFPS FPSLabel { get; set; }
      List<Portal> Portals { get; set; }
      Boss Boss { get; set; }
      Mill Mill { get; set; }
      List<HeightMap> HeightMap { get; set; }
      LifeBar[] LifeBars { get; set; }
      Afficheur3D Display3D { get; set; }
      List<Water> Water { get; set; }
      List<Food> Food { get; set; }
      List<Enemy> Enemy { get; set; }

      private void AjouterModeles(string chemin)
      {
         StreamReader fichier = new StreamReader(chemin);
         fichier.ReadLine();
         while (!fichier.EndOfStream)
         {
            string ligneLu = fichier.ReadLine();
            string[] ligneSplit = ligneLu.Split(';');
            CreateurModele x = new CreateurModele(this, ligneSplit[0], new Vector3(int.Parse(ligneSplit[1]), int.Parse(ligneSplit[2]), int.Parse(ligneSplit[3])), int.Parse(ligneSplit[4]), int.Parse(ligneSplit[5]));
            Components.Add(new Afficheur3D(this));
            Components.Add(x);
         }
         fichier.Close();
      }

      private void AjouterArbres()
      {
         Random generateur = new Random();
         const int NB_ARBRES = 150;
         for (int i = 0; i < NB_ARBRES; ++i)
         {
            Components.Add(new Afficheur3D(this));
            Components.Add(new CreateurModele(this, "Models_Tree", new Vector3(generateur.Next(-300, 300), -70, generateur.Next(-300, 300)), 10, generateur.Next(0, 360)));
         }
      }

      private void AjouterTours()
      {
         Random generateur = new Random();
         const int NB_Tours = 10;
         for (int i = 0; i < NB_Tours; ++i)
         {
            Components.Add(new Afficheur3D(this));
            CreateurModele x = new CreateurModele(this, "Models_Tower", new Vector3(generateur.Next(50, 300), -70, generateur.Next(-300, 300)), 0.05f, generateur.Next(0, 360));
            Components.Add(x);
            x.EstTour = true;
         }

      }

      List<Rune> ListeRunes { get; set; }

      private void AjouterRunes()
      {
         StreamReader fichier = new StreamReader("../../../Monde1_Runes.txt");
         fichier.ReadLine();
         while (!fichier.EndOfStream)
         {
            string ligneLu = fichier.ReadLine();
            string[] ligneSplit = ligneLu.Split(';');
            Rune x = new Rune(this, 1f, new Vector3(-(MathHelper.ToRadians(90)), 0, 0), new Vector3(float.Parse(ligneSplit[0]), -19.8f, float.Parse(ligneSplit[1])), new Vector2(3, 3), Vector2.One, ligneSplit[2], FpsInterval);
            ListeRunes.Add(x);
            Components.Add(x);
         }
         fichier.Close();
      }

      private void AjouterLivres()
      {
         StreamReader fichier = new StreamReader("../../../Monde1_Modeles.txt");
         fichier.ReadLine();
         while (!fichier.EndOfStream)
         {
            string ligneLu = fichier.ReadLine();
            string[] ligneSplit = ligneLu.Split(';');
            Livre x = new Livre(this, ligneSplit[0], new Vector3(int.Parse(ligneSplit[1]), int.Parse(ligneSplit[2]), int.Parse(ligneSplit[3])), int.Parse(ligneSplit[4]), int.Parse(ligneSplit[5]), "Briques", "ImageLivre" + (ligneSplit[6]));
            Components.Add(new Afficheur3D(this));
            Components.Add(x);
         }
         fichier.Close();
      }

      private void AjouterBoutons()
      {
         Random générateur = new Random();
         int[] ordre = new int[4];
         for (int i = 0; i < ordre.Length; ++i)
         {
            ordre[i] = générateur.Next(0, 4);
         }
         PuzzleBouton PuzzleBouton = new PuzzleBouton(this, ordre, "../../../PositionBoutons.txt", SaveNumber);
         Services.RemoveService(typeof(PuzzleBouton));
         Services.AddService(typeof(PuzzleBouton), PuzzleBouton);
         Components.Add(PuzzleBouton);
      }

      const int NUM_LEVELS = 10;
      List<bool> Complete { get; set; }

      void Save()
      {
         StreamWriter writer = new StreamWriter("../../../WPFINTERFACE/Launching Interface/Saves/pendingsave.txt");

         writer.WriteLine("Level: " + Level.ToString());
         if (Camera != null)
         {
            writer.WriteLine("Position: " + Camera.Position.ToString());
            if (Level != 8)
            {
               writer.WriteLine("Direction: " + (Camera as CaméraJoueur).Direction.ToString());
            }
         }
         else
         {
            writer.WriteLine("Position: {X:5 Y:5 Z:5}");
            writer.WriteLine("Direction: {X:5 Y:5 Z:5}");
         }
         TimeSpan time = new TimeSpan(TimePlayed.Hours, TimePlayed.Minutes, TimePlayed.Seconds);
         writer.WriteLine("Time Played: " + time.ToString());
         writer.WriteLine("Max Life: " + LifeBars[0].MaxLife.ToString());
         writer.WriteLine("Attack: " + (LifeBars[0].MaxLife - LifeBars[0].Life).ToString());
         for (int i = 0; i < Complete.Count - 1; ++i)
         {
            writer.Write(Complete[i].ToString() + ";");
         }
         writer.Write(Complete.Last().ToString());
         writer.Close();
      }

      int CountComplete()
      {
         int numComplete = 0;

         foreach (bool e in Complete)
         {
            if (e)
            {
               ++numComplete;
            }
         }
         return numComplete;
      }

      protected override void Initialize()
      {
         Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

         PuzzleRuneCompletePremiereFois = true;
         Sleep = false;
         Random = new Random();
         Services.AddService(typeof(Random), Random);
         FirstGameOver = true;
         FpsInterval = 1f / 60f;
         SongManager = new RessourcesManager<Song>(this, "Songs");
         Services.AddService(typeof(RessourcesManager<Song>), SongManager);
         TextureManager = new RessourcesManager<Texture2D>(this, "Textures");
         Services.AddService(typeof(RessourcesManager<TextureCube>), new RessourcesManager<TextureCube>(this, "Textures"));
         Services.AddService(typeof(RessourcesManager<Effect>), new RessourcesManager<Effect>(this, "Effects"));
         Services.AddService(typeof(RessourcesManager<Texture2D>), TextureManager);
         ModelManager = new RessourcesManager<Model>(this, "Models");
         Services.AddService(typeof(RessourcesManager<Model>), ModelManager);
         FontManager = new RessourcesManager<SpriteFont>(this, "Fonts");
         SpaceBackground = new ArrièrePlanSpatial(this, "CielÉtoilé", FpsInterval);
         FPSLabel = new AfficheurFPS(this, "Arial", Color.Tomato, INTERVALLE_CALCUL_FPS);
         UpdateText();
         InputManager = new InputManager(this);
         Services.AddService(typeof(RessourcesManager<SpriteFont>), FontManager);
         Services.AddService(typeof(InputManager), InputManager);
         GamePadManager = new GamePadManager(this);
         Services.AddService(typeof(GamePadManager), GamePadManager);
         SpriteBatch = new SpriteBatch(GraphicsDevice);
         Services.AddService(typeof(SpriteBatch), SpriteBatch);
         VideoManager = new RessourcesManager<Video>(this, "Videos");
         Services.AddService(typeof(RessourcesManager<Video>), VideoManager);
         SoundManager = new RessourcesManager<SoundEffect>(this, "Sounds");
         Services.AddService(typeof(RessourcesManager<SoundEffect>), SoundManager);
         ResetLists();
         PressSpaceLabel = new PressSpaceLabel(this);
         LifeBars = new LifeBar[2];
         Crosshair = new Sprite(this, "crosshair", new Vector2(Window.ClientBounds.Width / 2 - 18, Window.ClientBounds.Height / 2 - 18));
         LoadSave();
         LoadSettings();
         //Level = 0;
         SelectWorld(true);
         base.Initialize();
      }

      public void UpdateText()
      {
         Loading = new TexteCentré(this, DéterminerTextes(0), "Arial", new Rectangle(Window.ClientBounds.Width / 2 - 200, Window.ClientBounds.Height / 2 - 40, 400, 80), Color.White, 0);
         GameOver = new TexteCentré(this, DéterminerTextes(1), "Arial", new Rectangle(Window.ClientBounds.Width / 2 - 200, Window.ClientBounds.Height / 2 - 40, 400, 80), Color.White, 0);
         Success = new TexteCentré(this, DéterminerTextes(2), "Arial", new Rectangle(Window.ClientBounds.Width / 2 - 200, Window.ClientBounds.Height / 2 - 40, 400, 80), Color.White, 0);
      }

      string DéterminerTextes(int i)
      {
         const int NBRE_TEXTES = 3;
         string[] tableauxTextes = new string[NBRE_TEXTES] { "Loading ...", "Game Over", "Success!" };
         switch (Language)
         {
            case Language.French:
               tableauxTextes = new string[NBRE_TEXTES] { "Chargement ...", "Fin de partie", "Réussi!" };
               break;
            case Language.Spanish:
               tableauxTextes = new string[NBRE_TEXTES] { "Cargando ...", "Juego terminado", "¡Éxito!" };
               break;
            case Language.Japanese:
               tableauxTextes = new string[NBRE_TEXTES] { "読み込んでいます...", "ゲームオーバー", "成功！" };
               break;
         }
         return tableauxTextes[i];
      }


      void ResetLists()
      {
         Characters = new List<Character>();
         Enemy = new List<Enemy>();
         Maze = new List<Maze>();
         Houses = new List<House>();
         HeightMap = new List<HeightMap>();
         Portals = new List<Portal>();
         Walls = new List<Walls>();
         Unlockables = new List<UnlockableWall>();
         Water = new List<Water>();
            ListeRunes = new List<Rune>();
            Food = new List<Food>();
         Services.RemoveService(typeof(List<Rune>));
            Services.AddService(typeof(List<Rune>), ListeRunes);
            Services.RemoveService(typeof(List<Character>));
         Services.AddService(typeof(List<Character>), Characters);
         Services.RemoveService(typeof(List<Enemy>));
         Services.AddService(typeof(List<Enemy>), Enemy);
         Services.RemoveService(typeof(List<Maze>));
         Services.AddService(typeof(List<Maze>), Maze);
         Services.RemoveService(typeof(List<House>));
         Services.AddService(typeof(List<House>), Houses);
         Services.RemoveService(typeof(List<HeightMap>));
         Services.AddService(typeof(List<HeightMap>), HeightMap);
         Services.RemoveService(typeof(List<Portal>));
         Services.AddService(typeof(List<Portal>), Portals);
         Services.RemoveService(typeof(List<UnlockableWall>));
         Services.AddService(typeof(List<UnlockableWall>), Unlockables);
         Services.RemoveService(typeof(List<Water>));
         Services.AddService(typeof(List<Water>), Water);
         Services.RemoveService(typeof(List<Walls>));
         Services.AddService(typeof(List<Walls>), Walls);
         Services.RemoveService(typeof(List<Food>));
         Services.AddService(typeof(List<Food>), Food);
      }

      float Timer { get; set; }

      protected override void Update(GameTime gameTime)
      {
         if (!Sleep)
         {
            if (Camera != null)
            {
               Window.Title = Camera.Position.ToString();
            }
            ManageKeyboard(gameTime);
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimePlayed = TimePlayed.Add(gameTime.ElapsedGameTime);
            if (Timer >= FpsInterval)
            {
               //Window.Title = Camera.Position.ToString();
               switch (Level)
               {
                  case 0:
                     CheckForCutscene();
                     break;
                  case 6:
                     CheckForUnlockableWallBouncingBalls();
                     break;
               }
               CheckForPortal();
               CheckForGameOver();
               Cheat();
               Timer = 0;
            }
            base.Update(gameTime);
         }
         if(Level == 1)
            {
                if (PuzzleRunesComplete() && PuzzleRuneCompletePremiereFois)
                {
                    StreamWriter writer = new StreamWriter("../../../WPFINTERFACE/Launching Interface/Saves/SavePuzzleRunes" + SaveNumber + ".txt");
                    writer.WriteLine(true);
                    writer.Close();
                    PuzzleRuneCompletePremiereFois = false;
                }
            }
      }

        void Cheat()
        {
            if (InputManager.EstEnfoncée(Keys.V) && InputManager.EstEnfoncée(Keys.E))
            {
                for (int i = 0; i < Complete.Count; ++i)
                {
                    Complete[i] = true;
                }
                StreamWriter writer = new StreamWriter("../../../WPFINTERFACE/Launching Interface/Saves/SavePuzzleRunes" + SaveNumber + ".txt");
                writer.WriteLine(true);
                writer.Close();
                PuzzleRuneCompletePremiereFois = false;
                Save();
                Components.Add(Loading);
                Level = 1;
                ResetLists();
                SelectWorld(false);
            }
        }


      bool FirstGameOver { get; set; }

      void CheckForGameOver()
      {
         if (LifeBars[0].Dead && FirstGameOver)
         {
            FirstGameOver = false;
            Components.Clear();
            Components.Add(GameOver);
            LaunchPause();
         }
         else if (Boss != null && Boss.Dead && FirstGameOver)
         {
            FirstGameOver = false;
            Components.Clear();
            Components.Add(Success);
         }
      }

      private bool PuzzleRunesComplete()
      {
         return ListeRunes[0].EstActivée && !ListeRunes[1].EstActivée && ListeRunes[2].EstActivée && !ListeRunes[3].EstActivée && !ListeRunes[4].EstActivée && ListeRunes[5].EstActivée;
      }


      protected override void OnActivated(object sender, EventArgs args)
      {
         Sleep = false;
         base.OnActivated(sender, args);
         if (Camera != null)
         {
            (Camera as CaméraJoueur).EstCaméraSourisActivée = true;
         }
         IsMouseVisible = false;
         LoadSettings();
         UpdateLanguages();
         UpdateText();
      }

      void UpdateLanguages()
      {
         foreach (Character c in Characters)
         {
            c.UpdateLanguage();
         }
         foreach (PressSpaceLabel e in Components.Where(a => a is PressSpaceLabel))
         {
            e.DéterminerMesage();
         }
         foreach(SkipCutsceneLabel s in Components.Where(a => a is SkipCutsceneLabel))
            {
                s.MAJLangue();
            }
      }

      protected override void OnDeactivated(object sender, EventArgs args)
      {
         Sleep = true;
         base.OnDeactivated(sender, args);
         if (Camera != null)
         {
            if (Level != 8)
            {
               (Camera as CaméraJoueur).EstCaméraSourisActivée = false;
            }
         }
         IsMouseVisible = true;
      }

      void CheckForPortal()
      {
         foreach (Portal p in Portals)
         {
            float? collision = p.Collision(new Ray(Camera.Position, (Camera as Camera2).Direction));
            if (collision < 30 && collision != null)
            {
               PressSpaceLabel.Visible = true;
               if (InputManager.EstEnfoncée(/*Keys.Space*/Keys.R) || GamePadManager.EstEnfoncé(Buttons.Y))
               {
                  if (Level > 1)
                  {
                     Complete[Level - 2] = true;
                  }
                  Save();
                  Components.Add(Loading);
                  Level = p.Level;
                  ResetLists();
                  SelectWorld(false);
               }
               break;
            }
            else
            {
               PressSpaceLabel.Visible = false;
            }
         }
      }

      void CheckForCutscene()
      {
         if (CutscenePlayer.CutsceneFinished)
         {
            ++Level;
            ResetLists();
            SelectWorld(false);
            CutscenePlayer.ResetCutsceneFinished();
         }
      }

      public void AddLoading()
      {
         Components.Add(Loading);
      }

      bool Sleep { get; set; }

      void ManageKeyboard(GameTime gameTime)
      {
         if (InputManager.EstNouvelleTouche(Keys.P) || InputManager.EstNouvelleTouche(Keys.Escape) || GamePadManager.EstNouveauBouton(Buttons.Start) && Level > 0)
         {
            LaunchPause();
               
         }
      }

      void LaunchPause()
      {
         Sleep = true;
         Save();
         TakeAScreenshot();
         string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\WPFINTERFACE\Launching Interface\bin\Debug\Launching Interface.exe");
         ProcessStartInfo p = new ProcessStartInfo();
         p.FileName = path;
         p.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
         Process.Start(p);
         //(Camera as CaméraJoueur).EstCaméraSourisActivée = false;
         //Exit();
      }

      Texture2D Screenshot { get; set; }

      void TakeAScreenshot()
      {
         int w = GraphicsDevice.PresentationParameters.BackBufferWidth;
         int h = GraphicsDevice.PresentationParameters.BackBufferHeight;
         Draw(new GameTime());
         int[] backBuffer = new int[w * h];
         GraphicsDevice.GetBackBufferData(backBuffer);
         Screenshot = new Texture2D(GraphicsDevice, w, h, false, GraphicsDevice.PresentationParameters.BackBufferFormat);
         Screenshot.SetData(backBuffer);
         Stream stream;
         while (true)
         {
            try
            {
               stream = File.OpenWrite("../../../WPFINTERFACE/Launching Interface/Saves/pendingscreenshot.png");
            }
            catch (Exception)
            {
               continue;
            }
            break;
         }
         Screenshot.SaveAsPng(stream, w, h);
         stream.Dispose();
         stream.Close();
         Screenshot.Dispose();
      }

      protected override void Draw(GameTime gameTime)
      {
         GraphicsDevice.Clear(Color.Black);
         base.Draw(gameTime);
      }

      Walls Wall { get; set; }

      void NiveauRythmé()
      {
         NiveauRythmé circuit = new NiveauRythmé(this, "Fil Electrique", "../../../Data3.txt",
                                                 3, "Blanc", "Rouge",
                                                 "Vert", "BleuBlancRouge", "Arial50",
                                                 Color.Black, 100, 1,
                                                 FpsInterval);
         Components.Add(circuit);
         Services.AddService(typeof(NiveauRythmé), circuit);
      }

      // LevelPrison
      #region

      BalleRebondissante Balle { get; set; }
      Epee Épée { get; set; }
      Random Random { get; set; }

      const int LARGEUR_TUILE = 20, NBRE_BALLES_DÉSIRÉS = 20, NBRE_LIMITE_BALLES = 4;
      const float ÉCHELLE_ÉPÉE = 0.009f;
      const string NOM_MODÈLE_ÉPÉE = "robot";
      List<BalleRebondissante> ListeBalles { get; set; }

      void LevelPrison(bool usePosition)
      {

         ListeBalles = new List<BalleRebondissante>();
         Services.RemoveService(typeof(List<BalleRebondissante>));
         Services.AddService(typeof(List<BalleRebondissante>), ListeBalles);

         for (int i = 0; i < NBRE_BALLES_DÉSIRÉS; i++)
         {
            Balle = new BalleRebondissante(this, 1f, Vector3.Zero, CalculerPositionInitiale(), 5f, new Vector2(50), "Balle_Bois", FpsInterval);
            ListeBalles.Add(Balle);
            Components.Add(Balle);
         }

      }
      Vector3 CalculerPositionInitiale()
      {
         float x = Random.Next(-190, 70);
         float z = Random.Next(-40, 220);
         float y = Random.Next(-35, -15);
         return new Vector3(x, y, z);
      }


      void CheckForUnlockableWallBouncingBalls()
      {

         if (BalleRebondissante.Count < NBRE_LIMITE_BALLES)
         {
            for (int i = 0; i < ListeBalles.Count; i++)
            {
               Components.Remove(ListeBalles[i]);
            }

            Components.Remove(Unlockables[0]);
         }
      }
      #endregion
   }
}



