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
    public class Atelier : Microsoft.Xna.Framework.Game
    {
        const float INTERVALLE_CALCUL_FPS = 1f;
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        GraphicsDeviceManager PériphériqueGraphique { get; set; }

        Caméra Camera { get; set; }
        Maze Maze { get; set; }
        InputManager InputManager { get; set; }

        //GraphicsDeviceManager PériphériqueGraphique { get; set; }
        SpriteBatch SpriteBatch { get; set; }

        RessourcesManager<SpriteFont> FontManager { get; set; }
        RessourcesManager<Texture2D> TextureManager { get; set; }
        RessourcesManager<Model> ModelManager { get; set; }
        //Caméra CaméraJeu { get; set; }

        public Atelier()
        {
            PériphériqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = false;
            PériphériqueGraphique.PreferredBackBufferHeight = 800;
            PériphériqueGraphique.PreferredBackBufferWidth = 1500;

        }

        Gazon Gazon { get; set; }

        RessourcesManager<Video> VideoManager { get; set; }
        CutscenePlayer CutscenePlayer { get; set; }
        Walls Walls { get; set; }
        Character Robot { get; set; }
        List<Character> Characters { get; set; }
        int SaveNumber { get; set; }
        int Level { get; set; }
        Vector3 Position { get; set; }

        void LoadSave()
        {
            //StreamReader reader = new StreamReader("F:/programmation clg/quatrième session/WPFINTERFACE/Launching Interface/Saves/save.txt");
            StreamReader reader = new StreamReader("C:/Users/Mathieu/Source/Repos/WPFINTERFACE/Launching Interface/Saves/save.txt");
            SaveNumber = int.Parse(reader.ReadLine());
            reader.Close();
            //reader = new StreamReader("F:/programmation clg/quatrième session/WPFINTERFACE/Launching Interface/Saves/save" + SaveNumber.ToString() + ".txt");
            reader = new StreamReader("C:/Users/Mathieu/Source/Repos/WPFINTERFACE/Launching Interface/Saves/save" + SaveNumber.ToString() + ".txt");
            string line = reader.ReadLine();
            char[] separator = new char[] { ' ' };
            string[] parts = line.Split(separator);
            Level = int.Parse(parts[1]);
            line = reader.ReadLine();
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

        void SelectWorld()
        {
            switch (Level)
            {
                case 0:
                    Level0();
                    break;
                case 1:
                    Level1();
                    break;
            }
        }

        void Level1()
        {
            Components.Add(new ArrièrePlanSpatial(this, "CielÉtoilé", INTERVALLE_MAJ_STANDARD));
            Components.Add(new Afficheur3D(this));
            Camera = new CaméraJoueur(this, new Vector3(0, -16, 60), new Vector3(20, 0, 0), Vector3.Up, INTERVALLE_MAJ_STANDARD);
            Services.AddService(typeof(Caméra), Camera);
            Robot = new Character(this, "Robot", 0.02f, new Vector3(0, MathHelper.PiOver2, 0), new Vector3(-50, -20, 60), "../../../CharacterScripts/Robot.txt", "FaceImages/Robot", "ScriptRectangle");
            Characters.Add(Robot);
            Grass grass = new Grass(this, 1f, Vector3.Zero, new Vector3(20, -20, 50), new Vector2(20, 20), "Grass", INTERVALLE_MAJ_STANDARD);
            Components.Add(grass);
            Services.AddService(typeof(Grass), grass);
            Walls = new Walls(this, INTERVALLE_MAJ_STANDARD, "Rockwall", "../../../Data.txt");
            Components.Add(Walls);
            Services.AddService(typeof(Walls), Walls);
            Components.Add(Camera);
            for (int i = 0; i < 30; ++i)
            {
                for (int j = 0; j < 30; ++j)
                {
                    Components.Add(new Grass(this, 1f, Vector3.Zero, new Vector3(60 - i * 20, -20, -30 + j * 20), new Vector2(20, 20), "Ceiling", INTERVALLE_MAJ_STANDARD));
                }
            }
            for (int i = 0; i < 30; ++i)
            {
                for (int j = 0; j < 30; ++j)
                {
                    Components.Add(new Ceiling(this, 1f, Vector3.Zero, new Vector3(60 - i * 20, 0, -30 + j * 20), new Vector2(20, 20), "Ceiling", INTERVALLE_MAJ_STANDARD));
                }
            }
            Components.Add(Robot);
            Robot.AddLabel();
            Components.Remove(CutscenePlayer.Loading);
            //Components.Add(new AfficheurFPS(this, "Arial", Color.Tomato, INTERVALLE_CALCUL_FPS));
        }

        void Level0()
        {
            CutscenePlayer = new CutscenePlayer(this, "test1", false);
            Components.Add(CutscenePlayer);
        }

        protected override void Initialize()
        {
            TextureManager = new RessourcesManager<Texture2D>(this, "Textures");
            Services.AddService(typeof(RessourcesManager<Texture2D>), TextureManager);
            ModelManager = new RessourcesManager<Model>(this, "Models");
            Services.AddService(typeof(RessourcesManager<Model>), ModelManager);
            FontManager = new RessourcesManager<SpriteFont>(this, "Fonts");
            InputManager = new InputManager(this);
            Components.Add(InputManager);
            Services.AddService(typeof(RessourcesManager<SpriteFont>), FontManager);
            Services.AddService(typeof(InputManager), InputManager);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), SpriteBatch);
            VideoManager = new RessourcesManager<Video>(this, "Videos");
            Services.AddService(typeof(RessourcesManager<Video>), VideoManager);
            Characters = new List<Character>();
            Services.AddService(typeof(List<Character>), Characters);
            LoadSave();
            SelectWorld();

            //const float ÉCHELLE_OBJET = 0.02f;
            //Vector3 positionObjet = new Vector3(-50, -20, 60);
            //Vector3 rotationObjet = new Vector3(0, MathHelper.PiOver2, 0);
            ////CaméraJeu = new CaméraFixe(this, Vector3.Zero, positionObjet, Vector3.Up);
            ////CaméraJeu = new CaméraSubjective(this, new Vector3(0, 0, 0), positionObjet, Vector3.Up, INTERVALLE_MAJ_STANDARD);
            //Components.Add(new ArrièrePlanSpatial(this, "CielÉtoilé", INTERVALLE_MAJ_STANDARD));
            ////Components.Add(new ObjetDeBase(this, "Robot", ÉCHELLE_OBJET, rotationObjet, positionObjet));
            
            //Robot = new Character(this, "Robot", ÉCHELLE_OBJET, rotationObjet, positionObjet, "../../../CharacterScripts/Robot.txt", "FaceImages/Robot", "ScriptRectangle");
            //Components.Add(Robot);
            //Characters.Add(Robot);
            //Services.AddService(typeof(List<Character>), Characters);
            ////Components.Add(new PlanTexturé(this, 1f, Vector3.Zero, new Vector3(4, 4, -5), new Vector2(20, 20), new Vector2(40, 40), "Grass", INTERVALLE_MAJ_STANDARD));
            
            //Grass grass = new Grass(this, 1f, Vector3.Zero, new Vector3(20, -20, 50), new Vector2(20, 20), "Grass", INTERVALLE_MAJ_STANDARD);
            //Components.Add(grass);
            //for (int i = 0; i < 15; ++i)
            //{
            //    for (int j = 0; j < 15; ++j)
            //    {
            //        Components.Add(new Grass(this, 1f, Vector3.Zero, new Vector3(60 - i * 20, -20, 10 + j * 20), new Vector2(20, 20), "Grass", INTERVALLE_MAJ_STANDARD));
            //    }
            //}
            //for (int i = 0; i < 15; ++i)
            //{
            //    for (int j = 0; j < 15; ++j)
            //    {
            //        Components.Add(new Ceiling(this, 1f, Vector3.Zero, new Vector3(60 - i * 20, 0, 10 + j * 20), new Vector2(20, 20), "Grass", INTERVALLE_MAJ_STANDARD));
            //    }
            //}
            //Services.AddService(typeof(RessourcesManager<TextureCube>), new RessourcesManager<TextureCube>(this, "Textures"));
            //Services.AddService(typeof(RessourcesManager<Effect>), new RessourcesManager<Effect>(this, "Effects"));
            //Maze = new Maze(this, 1f, Vector3.Zero, new Vector3(0, 0, 0), new Vector3(256, 5, 256), "GrassFence", INTERVALLE_MAJ_STANDARD, "Maze");
            //Walls = new Walls(this, INTERVALLE_MAJ_STANDARD, "Rockwall", "../../../Data.txt");
            //Components.Add(Walls);
            //Services.AddService(typeof(Walls), Walls);
            ////Components.Add(Maze);
            ////Services.AddService(typeof(Maze), Maze);
            //Services.AddService(typeof(Grass), grass);
            //Camera = new CaméraJoueur(this, new Vector3(0, -16, 60), new Vector3(20, 0, 0), Vector3.Up, INTERVALLE_MAJ_STANDARD);
            //Services.AddService(typeof(Caméra), Camera);
            //Components.Add(Camera);
            //Services.AddService(typeof(RessourcesManager<Model>), ModelManager);
            //////Components.Add(new Skybox(this, "Texture_Skybox"));

            //Components.Add(new AfficheurFPS(this, "Arial", Color.Tomato, INTERVALLE_CALCUL_FPS));
            //Services.AddService(typeof(RessourcesManager<SpriteFont>), GestionnaireDeFonts);
            //Services.AddService(typeof(InputManager), GestionInput);
            //GestionSprites = new SpriteBatch(GraphicsDevice);
            //Services.AddService(typeof(SpriteBatch), GestionSprites);
            //VideoManager = new RessourcesManager<Video>(this, "Videos");
            //Services.AddService(typeof(RessourcesManager<Video>), VideoManager);
            //CutscenePlayer = new CutscenePlayer(this, "test1");
            ////Components.Add(CutscenePlayer);
            base.Initialize();
        }

        float Timer { get; set; }

        protected override void Update(GameTime gameTime)
        {
            ManageKeyboard();
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= INTERVALLE_MAJ_STANDARD)
            {
                CheckForCutscene();
                Timer = 0;
            }
            //Window.Title = CaméraJeu.Position.ToString();
            base.Update(gameTime);
        }

        void CheckForCutscene()
        {
            if (CutscenePlayer.CutsceneFinished)
            {
                ++Level;
                SelectWorld();
                CutscenePlayer.ResetCutsceneFinished();
            }
        }

        void ManageKeyboard()
        {
            if (InputManager.EstEnfoncée(Keys.Escape))
            {
                //string path = "F:/programmation clg/quatrième session/WPFINTERFACE/Launching Interface/bin/Debug/Launching Interface.exe";
                string path = "C:/Users/Mathieu/Source/Repos/WPFINTERFACE/Launching Interface/bin/Debug/Launching Interface.exe";
                ProcessStartInfo p = new ProcessStartInfo();
                p.FileName = path;
                p.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
                Process.Start(p);
                Exit();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}



