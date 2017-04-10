// By Mathieu Godin
// Created on January 2017

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Diagnostics;
using AtelierXNA;

namespace HyperV
{
   enum Language
   {
      French, English, Spanish, Japanese
   }

   enum Input
   {
      Controller, Keyboard
   }

   public class Atelier : Microsoft.Xna.Framework.Game
   {
      const float INTERVALLE_CALCUL_FPS = 1f;
      float FpsInterval { get; set; }
      GraphicsDeviceManager PériphériqueGraphique { get; set; }

      Caméra Camera { get; set; }
      Maze Maze { get; set; }
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
         //PériphériqueGraphique.PreferredBackBufferHeight = 800;
         //PériphériqueGraphique.PreferredBackBufferWidth = 1500;
         PériphériqueGraphique.PreferredBackBufferHeight = 500;
         PériphériqueGraphique.PreferredBackBufferWidth = 1000;
      }

      Gazon Gazon { get; set; }
      Grass Grass { get; set; }


      Random GénérateurAléatoire { get; set; }

      LifeBar[] LifeBars { get; set; }
      RessourcesManager<Video> VideoManager { get; set; }
      CutscenePlayer CutscenePlayer { get; set; }
      Walls Murs { get; set; }
      Character Robot { get; set; }
      List<Character> Characters { get; set; }
      int SaveNumber { get; set; }
      int Level { get; set; }
      Vector3 Position { get; set; }
      Vector3 Direction { get; set; }
      Portal Portal { get; set; }
      TexteCentré Loading { get; set; }
      TimeSpan TimePlayed { get; set; }
      Language Language { get; set; }
      int RenderDistance { get; set; }
      bool FullScreen { get; set; }
      Input Input { get; set; }

      void LoadSettings()
      {
         //StreamReader reader = new StreamReader("F:/programmation clg/quatrième session/WPFINTERFACE/Launching Interface/Saves/Settings.txt");
         StreamReader reader = new StreamReader("C:/Users/PC Raph/Downloads/WPFINTERFACE-master/WPFINTERFACE-master/Launching Interface/Saves/Settings.txt");
         string line = reader.ReadLine();
         string[] parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
         MediaPlayer.Volume = int.Parse(parts[1]) / 100.0f;
         line = reader.ReadLine();
         parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
         SoundEffect.MasterVolume = int.Parse(parts[1]) / 100.0f;
         line = reader.ReadLine();
         parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
         Language = (Language)int.Parse(parts[1]);
         line = reader.ReadLine();
         parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
         RenderDistance = int.Parse(parts[1]);
         if (Camera != null)
         {
            (Camera as CaméraJoueur).SetRenderDistance(RenderDistance);
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
         //StreamReader reader = new StreamReader("F:/programmation clg/quatrième session/WPFINTERFACE/Launching Interface/Saves/save.txt");
         //StreamReader reader = new StreamReader("C:/Users/Mathieu/Source/Repos/WPFINTERFACE/Launching Interface/Saves/save.txt");
         StreamReader reader = new StreamReader("C:/Users/PC Raph/Downloads/WPFINTERFACE-master/WPFINTERFACE-master/Launching Interface/Saves/save.txt");
         SaveNumber = int.Parse(reader.ReadLine());
         reader.Close();
         //reader = new StreamReader("F:/programmation clg/quatrième session/WPFINTERFACE/Launching Interface/Saves/save" + SaveNumber.ToString() + ".txt");
         //reader = new StreamReader("C:/Users/Mathieu/Source/Repos/WPFINTERFACE/Launching Interface/Saves/save" + SaveNumber.ToString() + ".txt");
         reader = new StreamReader("C:/Users/PC Raph/Downloads/WPFINTERFACE-master/WPFINTERFACE-master/Launching Interface/Saves/save" + SaveNumber.ToString() + ".txt");
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
         reader.Close();
         //parts = line.Split(separator);
         //int startInd = parts[1].IndexOf("X:") + 2;
         //float aXPosition = float.Parse(parts[1].Substring(startInd, parts[1].IndexOf(" Y") - startInd));
         //startInd = parts[1].IndexOf("Y:") + 2;
         //float aYPosition = float.Parse(parts[1].Substring(startInd, parts[1].IndexOf(" Z") - startInd));
         //startInd = parts[1].IndexOf("Z:") + 2;
         //float aZPosition = float.Parse(parts[1].Substring(startInd, parts[1].IndexOf("}") - startInd));
         //Position = new Vector3(aXPosition, aYPosition, aZPosition);
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
         switch (Level)
         {
            case 0:
               Level0();
               break;
            case 1:
               Level1(usePosition);
               break;
            case 2:
               Level2(usePosition);
               break;
            case 3:
               LevelPrison(usePosition);
               break;
         }
         Save();
      }

      void Save()
      {
         //StreamWriter writer = new StreamWriter("F:/programmation clg/quatrième session/WPFINTERFACE/Launching Interface/Saves/pendingsave.txt");
         StreamWriter writer = new StreamWriter("C:/Users/PC Raph/Downloads/WPFINTERFACE-master/WPFINTERFACE-master/Launching Interface/Saves/pendingsave.txt");

         writer.WriteLine("Level: " + Level.ToString());
         if (Camera != null)
         {
            writer.WriteLine("Position: " + Camera.Position.ToString());
            writer.WriteLine("Direction: " + (Camera as CaméraJoueur).Direction.ToString());
         }
         else
         {
            writer.WriteLine("Position: {X:5 Y:5 Z:5}");
            writer.WriteLine("Direction: {X:5 Y:5 Z:5}");
         }
         writer.WriteLine("Time Played: " + TimePlayed.ToString());
         writer.Close();
      }

      Boss Boss { get; set; }
      Mill Mill { get; set; }
      HeightMap HeightMap { get; set; }

      void Level2(bool usePosition)
      {
         Components.Add(SpaceBackground);
         Components.Add(new Afficheur3D(this));
         if (usePosition)
         {
            Camera = new Camera2(this, Position, new Vector3(20, 0, 0), Vector3.Up, FpsInterval, RenderDistance);
            (Camera as Camera2).InitializeDirection(Direction);
         }
         else
         {
            Camera = new Camera2(this, new Vector3(0, 4, 60), new Vector3(20, 0, 0), Vector3.Up, FpsInterval, RenderDistance);
         }
         //(Camera as Camera2).SetRenderDistance(RenderDistance);
         Services.AddService(typeof(Caméra), Camera);
         Maze = new Maze(this, 1f, Vector3.Zero, new Vector3(0, 0, 0), new Vector3(256, 5, 256), "GrassFence", FpsInterval, "Maze1");
         Components.Add(Maze);
         Services.AddService(typeof(Maze), Maze);
         Boss = new Boss(this, "Great Bison", 100, "Robot", "Gauge", "Dock", "Arial", FpsInterval, FpsInterval, 1, Vector3.Zero, new Vector3(300, 30, 200));
         Components.Add(Boss);
         Services.AddService(typeof(Boss), Boss);
         Mill = new Mill(this, 1, Vector3.Zero, new Vector3(300, 10, 100), new Vector2(50, 50), "Fence", FpsInterval);
         Components.Add(Mill);
         Services.AddService(typeof(Mill), Mill);
         HeightMap = new HeightMap(this, 1, Vector3.Zero, Vector3.Zero, new Vector3(10000, 100, 10000), "HeightMap", "Ceiling");
         Components.Add(HeightMap);
         Services.AddService(typeof(HeightMap), HeightMap);
         Boss.AddLabel();
         Components.Add(Camera);
         Components.Remove(Loading);
         Components.Add(FPSLabel);
         base.Initialize();
      }

      Grass[,] GrassArray { get; set; }
      Ceiling[,] CeilingArray { get; set; }
      ArrièrePlanSpatial SpaceBackground { get; set; }
      AfficheurFPS FPSLabel { get; set; }

      void Level1(bool usePosition)
      {
         //Song = SongManager.Find("castle");
         //MediaPlayer.Play(Song);
         Components.Add(SpaceBackground);
         Components.Add(new Afficheur3D(this));
         Services.AddService(typeof(List<Character>), Characters);
         if (usePosition)
         {
            Camera = new Camera1(this, Position, new Vector3(20, 0, 0), Vector3.Up, FpsInterval, RenderDistance);
            (Camera as Camera1).InitializeDirection(Direction);
         }
         else
         {
            Camera = new Camera1(this, new Vector3(0, -16, 60), new Vector3(20, 0, 0), Vector3.Up, FpsInterval, RenderDistance);
         }
         //(Camera as Camera1).SetRenderDistance(RenderDistance);
         Services.AddService(typeof(Caméra), Camera);
         Robot = new Character(this, "Robot", 0.02f, new Vector3(0, MathHelper.PiOver2, 0), new Vector3(-50, -20, 60), "../../../CharacterScripts/Robot.txt", "FaceImages/Robot", "ScriptRectangle", "Arial", FpsInterval);
         Characters.Add(Robot);
         Grass = new Grass(this, 1f, Vector3.Zero, new Vector3(20, -20, 50), new Vector2(40, 40), "Ceiling", FpsInterval);
         Components.Add(Grass);
         Services.AddService(typeof(Grass), Grass);
         Murs = new Walls(this, FpsInterval, "Rockwall", "../../../Data.txt");
         Components.Add(Murs);
         Services.AddService(typeof(Walls), Murs);
         Components.Add(Camera);
         GrassArray = new Grass[11, 7];
         CeilingArray = new Ceiling[11, 7];
         for (int i = 0; i < 11; ++i)
         {
            for (int j = 0; j < 7; ++j)
            {
               GrassArray[i, j] = new Grass(this, 1f, Vector3.Zero, new Vector3(100 - i * 40, -20, -30 + j * 40), new Vector2(40, 40), "Ceiling", FpsInterval);
               Components.Add(GrassArray[i, j]);
            }
         }
         for (int i = 0; i < 11; ++i)
         {
            for (int j = 0; j < 7; ++j)
            {
               CeilingArray[i, j] = new Ceiling(this, 1f, Vector3.Zero, new Vector3(100 - i * 40, 0, -30 + j * 40), new Vector2(40, 40), "Ceiling", FpsInterval);
               Components.Add(CeilingArray[i, j]);
            }
         }
         Portal = new Portal(this, 1f, Vector3.Zero, new Vector3(-345, -10, 170), new Vector2(30, 20), "Garden", FpsInterval);
         Components.Add(Portal);
         Services.AddService(typeof(Portal), Portal);
         Components.Add(Robot);
         Robot.AddLabel();
         Components.Add(PressSpaceLabel);
         PressSpaceLabel.Visible = false;
         Components.Remove(Loading);
         Components.Add(FPSLabel);
      }

      void Level0()
      {
         CutscenePlayer = new CutscenePlayer(this, "test1", false, "Arial");
         Components.Add(CutscenePlayer);
      }


      protected override void Initialize()
      {
         FpsInterval = 1f / 60f;
         SongManager = new RessourcesManager<Song>(this, "Songs");
         Services.AddService(typeof(RessourcesManager<Song>), SongManager);
         TextureManager = new RessourcesManager<Texture2D>(this, "Textures");
         Services.AddService(typeof(RessourcesManager<Texture2D>), TextureManager);
         ModelManager = new RessourcesManager<Model>(this, "Models");
         Services.AddService(typeof(RessourcesManager<Model>), ModelManager);
         FontManager = new RessourcesManager<SpriteFont>(this, "Fonts");
         SpaceBackground = new ArrièrePlanSpatial(this, "CielÉtoilé", FpsInterval);
         FPSLabel = new AfficheurFPS(this, "Arial", Color.Tomato, INTERVALLE_CALCUL_FPS);
         Loading = new TexteCentré(this, "Loading . . .", "Arial", new Rectangle(Window.ClientBounds.Width / 2 - 200, Window.ClientBounds.Height / 2 - 40, 400, 80), Color.White, 0);
         InputManager = new InputManager(this);
         Components.Add(InputManager);
         Services.AddService(typeof(RessourcesManager<SpriteFont>), FontManager);
         Services.AddService(typeof(InputManager), InputManager);
         GamePadManager = new GamePadManager(this);
         Components.Add(GamePadManager);
         Services.AddService(typeof(GamePadManager), GamePadManager);
         SpriteBatch = new SpriteBatch(GraphicsDevice);
         Services.AddService(typeof(SpriteBatch), SpriteBatch);
         VideoManager = new RessourcesManager<Video>(this, "Videos");
         Services.AddService(typeof(RessourcesManager<Video>), VideoManager);
         Characters = new List<Character>();
         PressSpaceLabel = new PressSpaceLabel(this);
         Services.AddService(typeof(Random), new Random());

         GénérateurAléatoire = new Random();
         LifeBars = new LifeBar[2];

         LifeBars[0] = new LifeBar(this, 300, "Gauge", "Dock", new Vector2(30, Window.ClientBounds.Height - 70), FpsInterval);
         LifeBars[1] = new LifeBar(this, 300, "StaminaGauge", "TiredGauge", "WaterGauge", "Dock", new Vector2(30, Window.ClientBounds.Height - 130), FpsInterval);

         LoadSave();
         LoadSettings();
         Level = 3;
         SelectWorld(true);

         base.Initialize();
      }

      float Timer { get; set; }

      protected override void Update(GameTime gameTime)
      {
         ManageKeyboard(gameTime);
         Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
         TimePlayed = TimePlayed.Add(gameTime.ElapsedGameTime);
         //Window.Title = Input.ToString();
         if (Timer >= FpsInterval)
         {
            switch (Level)
            {
               case 0:
                  CheckForCutscene();
                  break;
               case 1:
                  CheckForPortal();
                  break;
            }
            Timer = 0;
            
         }

         
         base.Update(gameTime);
      }

      protected override void OnActivated(object sender, EventArgs args)
      {
         base.OnActivated(sender, args);
         if (Camera != null)
         {
            (Camera as CaméraJoueur).EstCaméraSourisActivée = true;
         }
         IsMouseVisible = false;
         LoadSettings();
      }

      protected override void OnDeactivated(object sender, EventArgs args)
      {
         base.OnDeactivated(sender, args);
         if (Camera != null)
         {
            (Camera as CaméraJoueur).EstCaméraSourisActivée = false;
         }
         IsMouseVisible = true;
      }

      void CheckForPortal()
      {
         float? collision = Portal.Collision(new Ray(Camera.Position, (Camera as Camera1).Direction));
         if (collision < 30 && collision != null)
         {
            PressSpaceLabel.Visible = true;
            if (InputManager.EstEnfoncée(Keys.Space))
            {
               Components.Add(Loading);
               ++Level;
               MediaPlayer.Stop();
               Robot.RemoveLabel();
               Components.Remove(Camera);
               Services.RemoveService(typeof(Caméra));
               Components.Remove(Grass);
               Services.RemoveService(typeof(Grass));
               Components.Remove(Murs);
               Services.RemoveService(typeof(Walls));
               for (int i = 0; i < 11; ++i)
               {
                  for (int j = 0; j < 7; ++j)
                  {
                     Components.Remove(GrassArray[i, j]);
                  }
               }
               for (int i = 0; i < 11; ++i)
               {
                  for (int j = 0; j < 7; ++j)
                  {
                     Components.Remove(CeilingArray[i, j]);
                  }
               }
               Components.Remove(Portal);
               Services.RemoveService(typeof(Portal));
               Characters.Remove(Robot);
               Services.RemoveService(typeof(List<Character>));
               Components.Remove(Robot);
               Components.Remove(SpaceBackground);
               Components.Remove(PressSpaceLabel);
               Components.Remove(FPSLabel);
               SelectWorld(false);
            }
         }
         else
         {
            PressSpaceLabel.Visible = false;
         }
      }

      void CheckForCutscene()
      {
         if (CutscenePlayer.CutsceneFinished)
         {
            ++Level;
            SelectWorld(false);
            CutscenePlayer.ResetCutsceneFinished();
         }
      }

      public void AddLoading()
      {
         Components.Add(Loading);
      }

      void ManageKeyboard(GameTime gameTime)
      {
         if (InputManager.EstNouvelleTouche(Keys.Escape))
         {
            Save();
            TakeAScreenshot();
            //string path = "F:/programmation clg/quatrième session/WPFINTERFACE/Launching Interface/bin/Debug/Launching Interface.exe";
            string path = "C:/Users/PC Raph/Downloads/WPFINTERFACE-master/WPFINTERFACE-master/Launching Interface/bin/Debug/Launching Interface.exe";
            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = path;
            p.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
            Process.Start(p);

            //(Camera as CaméraJoueur).EstCaméraSourisActivée = false;
            //Exit();
         }
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
               //stream = File.OpenWrite("F:/programmation clg/quatrième session/WPFINTERFACE/Launching Interface/Saves/pendingscreenshot.png");
               stream = File.OpenWrite("C:/Users/PC Raph/Downloads/WPFINTERFACE/Launching Interface/Saves/pendingscreenshot.png");
            }
            catch (IOException e)
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



      // LevelPrison
      #region

      BalleRebondissante Balle { get; set; }
      Epee Épée { get; set; }
      


      void LevelPrison(bool usePosition)
      {
         const int LARGEUR_TUILE = 20,
                   NBRE_BALLES_DÉSIRÉS = 20;
         const float ÉCHELLE_ÉPÉE = 0.009f;
         const string NOM_MODÈLE_ÉPÉE = "robot";

         usePosition = false;

         Services.AddService(typeof(List<Character>), Characters);


         CréationCaméra(usePosition);
         CréationMurs("imagePrisonMur", "DataPrison.txt");
         CréerPlancherEtPlafond(LARGEUR_TUILE);

         for (int i = 0; i < NBRE_BALLES_DÉSIRÉS; i++)
         {
            Balle = new BalleRebondissante(this, 1f, Vector3.Zero, CalculerPositionInitiale(), 5f, new Vector2(50), "Balle_Bois",FpsInterval);
            Components.Add(Balle);
         }
         CréerÉpée(NOM_MODÈLE_ÉPÉE, ÉCHELLE_ÉPÉE);
         Components.Add(LifeBars[0]);
         Components.Add(LifeBars[1]);
         Services.AddService(typeof(LifeBar[]), LifeBars);
      }
      Vector3 CalculerPositionInitiale()
      {
         float x = GénérateurAléatoire.Next(-190, 70);
         float z = GénérateurAléatoire.Next(-40, 220);
         float y = GénérateurAléatoire.Next(-35, -15);
         return new Vector3(x, y, z);
      }

      void CréerPlancherEtPlafond(int largeurTuiles)
      {
         GrassArray = new Grass[largeurTuiles, largeurTuiles];
         CeilingArray = new Ceiling[largeurTuiles, largeurTuiles];
         for (int i = 0; i < largeurTuiles; ++i)
         {
            for (int j = 0; j < largeurTuiles; ++j)
            {
               GrassArray[i, j] = new Grass(this, 1f, Vector3.Zero, new Vector3(100 - i * 40, -40, -30 + j * 40), new Vector2(40, 40), "imagePrisonMur", FpsInterval);
               Components.Add(GrassArray[i, j]);

               CeilingArray[i, j] = new Ceiling(this, 1f, Vector3.Zero, new Vector3(100 - i * 40, 0, -30 + j * 40), new Vector2(40, 40), "imagePrisonMur", FpsInterval);
               Components.Add(CeilingArray[i, j]);
            }
         }
      }

      void CréationCaméra(bool usePosition)
      {
         if (usePosition)
         {
            Camera = new Camera1(this, Position, new Vector3(20, 0, 0), Vector3.Up, FpsInterval, RenderDistance);
            (Camera as Camera1).InitializeDirection(Direction);
         }
         else
         {
            Camera = new Camera1(this, new Vector3(76, -20, -45), new Vector3(20, 0, 0), Vector3.Up, FpsInterval, RenderDistance);
         }
         
         Services.AddService(typeof(Caméra), Camera);
         
         Components.Add(Camera);
      }

      void CréationMurs(string nomTexture, string nomFichierTexte)
      {
         Murs = new Walls(this, FpsInterval, nomTexture, "../../../" + nomFichierTexte);
         Components.Add(Murs);
         Services.AddService(typeof(Walls), Murs);
      }

      void CréerÉpée(string nomModèle,float échelle)
      {
         Épée = new Epee(this, nomModèle, échelle, Vector3.Zero, Camera.Position);
         Épée.EstRamassée = true;

         Components.Add(Épée);
         Services.AddService(typeof(Epee), Épée);
      }

      #endregion
   }
}



