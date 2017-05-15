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
        GamePadManager GestionGamePad { get; set; }
        Camera2 Caméra { get; set; }
        Sprite Texte { get; set; }
        public PressSpaceLabel PressSpaceLabel { get; private set; }

        public Livre(Game game, string modele3D, Vector3 position, float homothésie, float rotation, string nomModele2D, string imageLivre)
            : base(game, modele3D, position, homothésie, rotation, nomModele2D)
        {
            ImageLivre = imageLivre;
            Shown = false;
        }

        public override void Initialize()
        {           
            base.Initialize();
            PressSpaceLabel = new PressSpaceLabel(Game);
            PressSpaceLabel.Visible = false;
            PressSpaceLabel.DrawOrder = 1000;
            Game.Components.Add(PressSpaceLabel);
            Shown = !Shown;
        }

        float? TrouverDistance(Ray autreObjet, BoundingSphere SphèreDeCollision)
        {
            return SphèreDeCollision.Intersects(autreObjet);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            GestionInputs = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionGamePad = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
            Caméra = Game.Services.GetService(typeof(Caméra)) as Camera2;
        }

        bool Shown { get; set; }

        public override void Update(GameTime gameTime)        
        {
            if (EstABonneDistance(this) && !Shown)
            {
                PressSpaceLabel.Visible = true;
            }
            else
            {
                PressSpaceLabel.Visible = false;
            }
            if (GestionInputs.EstNouvelleTouche(Keys.R)/*EstNouveauClicGauche()*/|| GestionGamePad.EstNouveauBouton(Buttons.Y))
            {
                if (Shown)
                {
                    Game.Components.Remove(Texte);
                }
                else
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
                Shown = !Shown;              
                //if (EstABonneDistance(this))
                //{
                //    if (Game.Components.Contains(Texte))
                //    {
                //        Game.Components.Remove(Texte);
                //    }
                //    Texte = new Sprite(Game, ImageLivre, new Vector2(GraphicsDevice.DisplayMode.Width / 2 - 450, GraphicsDevice.DisplayMode.Height / 2 - 350));
                //    Game.Components.Add(Texte);
                //}
            }
            //if (GestionInputs./*EstAncienClicDroit()*/)
            //{
            //    Game.Components.Remove(Texte);
            //}
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
