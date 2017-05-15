using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using AtelierXNA;


namespace HyperV
{
   public class Arc : ModeleRamassable
    {
        const float FPS_60_INTERVAL = 1f / 60f;

        bool LancerFleche { get; set; }
        InputManager GestionInput { get; set; }
        GamePadManager GestionGamePad { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }

        public Arc(Game jeu, string nomModèle, float échelleInitiale,
                    Vector3 rotationInitiale, Vector3 positionInitiale)
            : base(jeu, nomModèle, échelleInitiale, rotationInitiale, positionInitiale)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            LancerFleche = false;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionGamePad = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
        }

        public override void Update(GameTime gameTime)
        {
            LancerFleche = (GestionInput.EstEnfoncée(Keys.T) || GestionInput.EstNouveauClicDroit()) && EstRamassée;

            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= FPS_60_INTERVAL)
            {
                if (LancerFleche)
                {
                    Game.Components.Add(new Fleche(Game, "shop", 0.0025f, new Vector3(angleY, 1.57f/*angleX + (float)Math.PI / 2*/, 1.57f/*Rotation.Z*/),
                                                   CaméraJoueur.Position, CaméraJoueur.Direction));
                }
            }
            base.Update(gameTime);

        }
    }
}
