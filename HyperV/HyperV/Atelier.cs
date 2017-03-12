using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using AtelierXNA;

namespace HyperV
{
    public class Atelier : Microsoft.Xna.Framework.Game
    {
        const float INTERVALLE_CALCUL_FPS = 1f;
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        GraphicsDeviceManager PériphériqueGraphique { get; set; }

        CaméraJoueur CaméraJeu { get; set; }                
        InputManager GestionInput { get; set; }
        GamePadManager GestionGamePad { get; set; }
        Gazon Gazon { get; set; }

        public Atelier()
        {
            PériphériqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            GestionInput = new InputManager(this);
            GestionGamePad = new GamePadManager(this);
            Gazon = new Gazon(this, 1f, Vector3.Zero, new Vector3(0,0,0), new Vector2(256, 256), "Grass", INTERVALLE_MAJ_STANDARD);

            Components.Add(Gazon);
            Services.AddService(typeof(Gazon), Gazon);
            CaméraJeu = new CaméraJoueur(this, Vector3.Zero, new Vector3(0, 0, 20), Vector3.Up, INTERVALLE_MAJ_STANDARD);


            Components.Add(CaméraJeu);
            Components.Add(GestionInput);
            Components.Add(GestionGamePad); 

            //Components.Add(new AfficheurFPS(this, "Arial", Color.Red, INTERVALLE_CALCUL_FPS));


            Components.Add(new Afficheur3D(this));
            Components.Add(new AfficheurFPS(this, "Arial", Color.Red, INTERVALLE_CALCUL_FPS));
            //Components.Add(new Jeu(this));

            //Components.Add(new ObjetDeDémo(this, "ship", 0.01f, Vector3.Zero, new Vector3(0, 0, 20), INTERVALLE_MAJ_STANDARD));
            

            //Components.Add(new SphèreTest(this, 1, new Vector3(0, 0, 0), new Vector3(10,0,10), 1f, new Vector2(20, 20), "BleuBlancRouge", INTERVALLE_MAJ_STANDARD));
            Components.Add(new SphèreRamassable(this, 1, new Vector3(0, 0, 0), Vector3.Zero, 1f, new Vector2(20, 20), "BleuBlancRouge", INTERVALLE_MAJ_STANDARD));



            Services.AddService(typeof(Random), new Random());
            Services.AddService(typeof(RessourcesManager<SpriteFont>), new RessourcesManager<SpriteFont>(this, "Fonts"));
            Services.AddService(typeof(RessourcesManager<Texture2D>), new RessourcesManager<Texture2D>(this, "Textures"));
            Services.AddService(typeof(RessourcesManager<Model>), new RessourcesManager<Model>(this, "Models"));
            Services.AddService(typeof(AtelierXNA.Caméra), CaméraJeu);
            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(GamePadManager), GestionGamePad);
            Services.AddService(typeof(SpriteBatch), new SpriteBatch(GraphicsDevice));



            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            GérerClavier();


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
            base.Draw(gameTime);
        }
    }
}

