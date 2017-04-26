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
using AtelierXNA;

namespace HyperV
{
    public class Livre : CreateurModele
    {
        float DISTANCE_MINIMALE = 10;
        string ImageLivre { get; set; }

        InputManager GestionInputs { get; set; }
        Camera2 Caméra { get; set; }
        Sprite Texte { get; set; }


        public Livre(Game game, string modele3D, Vector3 position, float homothésie, float rotation, string nomModele2D, string imageLivre)
            : base(game, modele3D, position, homothésie, rotation, nomModele2D)
        {
            ImageLivre = imageLivre;
        }

        public override void Initialize()
        {           
            base.Initialize();
        }

        float? TrouverDistance(Ray autreObjet, BoundingSphere SphèreDeCollision)
        {
            return SphèreDeCollision.Intersects(autreObjet);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            GestionInputs = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Caméra = Game.Services.GetService(typeof(Caméra)) as Camera2;
        }

        public override void Update(GameTime gameTime)        
        {
            if (GestionInputs.EstNouveauClicGauche())
            {                
                if (EstABonneDistance(this))
                {
                    if (Game.Components.Contains(Texte))
                    {
                        Game.Components.Remove(Texte);
                    }
                    Texte = new Sprite(Game, ImageLivre, new Vector2(GraphicsDevice.DisplayMode.Width / 2 - 450, GraphicsDevice.DisplayMode.Height / 2 - 350));
                    Game.Components.Add(Texte);
                }
            }
            if (GestionInputs.EstAncienClicDroit())
            {
                Game.Components.Remove(Texte);
            }
        }

        bool EstABonneDistance(CreateurModele modele)
        {
            float? minDistance = float.MaxValue;
            BoundingSphere sphère = new BoundingSphere(modele.GetPosition(), 3f);
            float? distance = TrouverDistance(new Ray(Caméra.Position, Caméra.Direction), sphère);
            if (minDistance > distance)
            {
                minDistance = distance;
            }
            return minDistance < DISTANCE_MINIMALE;
        }
    }
}
