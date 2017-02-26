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

        Caméra CaméraJeu { get; set; }
        Maze Maze { get; set; }
        InputManager GestionInput { get; set; }

        //GraphicsDeviceManager PériphériqueGraphique { get; set; }
        SpriteBatch GestionSprites { get; set; }

        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        RessourcesManager<Model> GestionnaireDeModèles { get; set; }
        //Caméra CaméraJeu { get; set; }

        public Atelier()
        {
            PériphériqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = false;
            //PériphériqueGraphique.PreferredBackBufferHeight = 1080;
            //PériphériqueGraphique.PreferredBackBufferWidth = 1920;

        }

        Gazon Gazon { get; set; }

        RessourcesManager<Video> VideoManager { get; set; }
        CutscenePlayer CutscenePlayer { get; set; }

        protected override void Initialize()
        {
            //const float ÉCHELLE_OBJET = 0.01f;
            //Vector3 positionObjet = new Vector3(0, -10, -50);
            //Vector3 rotationObjet = new Vector3(0, MathHelper.PiOver2, 0);
            GestionnaireDeFonts = new RessourcesManager<SpriteFont>(this, "Fonts");
            GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
            GestionnaireDeModèles = new RessourcesManager<Model>(this, "Models");
            ////CaméraJeu = new CaméraFixe(this, Vector3.Zero, positionObjet, Vector3.Up);
            ////CaméraJeu = new CaméraSubjective(this, new Vector3(0, 0, 0), positionObjet, Vector3.Up, INTERVALLE_MAJ_STANDARD);
            GestionInput = new InputManager(this);
            Components.Add(GestionInput);
            //Components.Add(new ArrièrePlanSpatial(this, "CielÉtoilé", INTERVALLE_MAJ_STANDARD));
            //Components.Add(new Afficheur3D(this));
            //Components.Add(new ObjetDeBase(this, "ship", ÉCHELLE_OBJET, rotationObjet, positionObjet));
            ////Components.Add(new PlanTexturé(this, 1f, Vector3.Zero, new Vector3(4, 4, -5), new Vector2(20, 20), new Vector2(40, 40), "Grass", INTERVALLE_MAJ_STANDARD));
            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
            ////Grass gazon = new Grass(this, 1f, Vector3.Zero, new Vector3(0, 0, 0), new Vector2(256, 256), "Grass", INTERVALLE_MAJ_STANDARD);
            ////Components.Add(gazon);
            Services.AddService(typeof(RessourcesManager<TextureCube>), new RessourcesManager<TextureCube>(this, "Textures"));
            Services.AddService(typeof(RessourcesManager<Effect>), new RessourcesManager<Effect>(this, "Effects"));
            //Maze = new Maze(this, 1f, Vector3.Zero, new Vector3(0, 0, 0), new Vector3(256, 5, 256), "Grass", INTERVALLE_MAJ_STANDARD, "Maze");
            //Components.Add(Maze);
            //Services.AddService(typeof(Maze), Maze);
            ////Services.AddService(typeof(Grass), gazon);
            //CaméraJeu = new CaméraJoueur(this, new Vector3(0, 4, 60), new Vector3(20, 0, 0), Vector3.Up, INTERVALLE_MAJ_STANDARD);
            //Services.AddService(typeof(Caméra), CaméraJeu);
            //Components.Add(CaméraJeu);
            Services.AddService(typeof(RessourcesManager<Model>), GestionnaireDeModèles);
            ////Components.Add(new Skybox(this, "Texture_Skybox"));
            
            Components.Add(new AfficheurFPS(this, "Arial", Color.Tomato, INTERVALLE_CALCUL_FPS));
            Services.AddService(typeof(RessourcesManager<SpriteFont>), GestionnaireDeFonts);
            Services.AddService(typeof(InputManager), GestionInput);
            GestionSprites = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), GestionSprites);
            VideoManager = new RessourcesManager<Video>(this, "Videos");
            Services.AddService(typeof(RessourcesManager<Video>), VideoManager);
            CutscenePlayer = new CutscenePlayer(this, "test1");
            Components.Add(CutscenePlayer);
            //vidPlayer = new VideoPlayer();
            base.Initialize();
        }

        //Video vid;
        //VideoPlayer vidPlayer;
        //Texture2D vidTexture;
        //Rectangle vidRectangle;

        //protected override void LoadContent()
        //{
        //    vid = Content.Load<Video>("Videos\\test1");
        //    vidRectangle = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        //    vidPlayer.Play(vid);
        //    base.LoadContent();
        //}

        protected override void Update(GameTime gameTime)
        {
            GérerClavier();
            //Window.Title = CaméraJeu.Position.ToString();
            base.Update(gameTime);
        }

        private void GérerClavier()
        {
            if (GestionInput.EstEnfoncée(Keys.Escape))
            {
                Exit();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            //vidTexture = vidPlayer.GetTexture();
            //GestionSprites.Begin();
            //GestionSprites.Draw(vidTexture, vidRectangle, Color.White);
            //GestionSprites.End();
            base.Draw(gameTime);
        }
    }
}



