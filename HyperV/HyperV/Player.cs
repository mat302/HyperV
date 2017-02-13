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


namespace HyperV
{    
    public class Player : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const float VITESSE_INITIALE_TRANSLATION = 0.5f;

        InputManager GestionInput { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        float IntervalleMAJ { get; set; }
        CaméraSubjective Camera { get; set; }
        float VitesseTranslation { get; set; }
        Point AnciennePositionSouris { get; set; }
        Point DeplacementSouris { get; set; }

        public Player(Game game, float intervalleMAJ)
            : base(game)
        {
            IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            base.Initialize();
            VitesseTranslation = VITESSE_INITIALE_TRANSLATION;
            AnciennePositionSouris = GestionInput.GetPositionSouris();
        }

        protected override void LoadContent()
        {
            Camera = Game.Services.GetService(typeof(CaméraSubjective)) as CaméraSubjective;
        }

        public override void Update(GameTime gameTime)
        {
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                GérerRotation();
                if (GestionInput.EstEnfoncée(Keys.W) || GestionInput.EstEnfoncée(Keys.S) || GestionInput.EstEnfoncée(Keys.A) || GestionInput.EstEnfoncée(Keys.D))
                {
                    GererDeplacement();
                    GererSprint();
                }
                TempsÉcouléDepuisMAJ = 0;
            }
            base.Update(gameTime);
        }

        private int GérerTouche(Keys touche)
        {
            return GestionInput.EstEnfoncée(touche) ? 1 : 0;
        }

        private void GererDeplacement()
        {
            float deplacementDirection = (GérerTouche(Keys.W) - GérerTouche(Keys.S)) * VitesseTranslation;
            float deplacementLatéral = (GérerTouche(Keys.A) - GérerTouche(Keys.D)) * VitesseTranslation;
            Camera.GererDeplacementCamera(deplacementDirection, deplacementLatéral);
        }

        private void GererSprint()
        {
            if(GestionInput.EstEnfoncée(Keys.LeftShift) || GestionInput.EstEnfoncée(Keys.RightShift))
            {
                Camera.GérerAccélération(0.5f);
            }
        }

        private void GérerRotation()
        {
            DeplacementSouris = new Point((GestionInput.GetPositionSouris().X - AnciennePositionSouris.X), GestionInput.GetPositionSouris().Y - AnciennePositionSouris.Y);
            if(DeplacementSouris.X != 0)
            {
                Camera.GererLacet(DeplacementSouris.X);
            }
            if(DeplacementSouris.Y != 0)
            {
                Camera.GererTangage(DeplacementSouris.Y);
            }
            AnciennePositionSouris = GestionInput.GetPositionSouris();
        }
    }
}
