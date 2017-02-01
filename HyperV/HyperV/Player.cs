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
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        InputManager GestionInput { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        float IntervalleMAJ { get; set; }

        public Player(Game game, float intervalleMAJ)
            : base(game)
        {
            IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            base.Initialize();
        }
        
        public override void Update(GameTime gameTime)
        {
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                if(GestionInput.EstEnfoncée(Keys.W) || GestionInput.EstEnfoncée(Keys.S) || GestionInput.EstEnfoncée(Keys.A) || GestionInput.EstEnfoncée(Keys.D))
                {
                    GererDeplacement();
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
            float deplacementDirection = (GérerTouche(Keys.W) - GérerTouche(Keys.S));
            float deplacementLatéral = (GérerTouche(Keys.A) - GérerTouche(Keys.D));
            CaméraSubjective Camera = Game.Services.GetService(typeof(CaméraSubjective)) as CaméraSubjective;
            Camera.DeplacerCamera(deplacementDirection, deplacementLatéral, 0.5f, false);
        }
    }
}
