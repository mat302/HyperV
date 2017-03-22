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
    public class Catapulte : CreateurModele, ICollisionable
    {
        const float INTERVALLE_MAJ = 1 / 60f;

        float Temps…coulÈMAJ { get; set; }
        float Temps…coulÈMAJ2 { get; set; }
        float CooldownTir { get; set; }
        InputManager GestionInput { get; set; }
        Camera1 Camera { get; set; }
        AmmunitionCatapulte Ammunition { get; set; }
        bool EstActivÈe { get; set; }

        float angle;
        float Angle
        {
            get { return angle; }
            set
            {
                if (angle < 0)
                {
                    value = 0;
                }
                if (angle > 90)
                {
                    value = 90;
                }
                angle = value;
            }
        }

        float vitesse;
        float Vitesse
        {
            get
            {
                return vitesse;
            }
            set
            {
                if (vitesse < 0)
                {
                    vitesse = 0;
                }
                if (vitesse > 100)
                {
                    vitesse = 100;
                }
            }
        }

        Vector2 AncienVecteur { get; set; }

        public Catapulte(Game game, string modele3D, Vector3 position, float homothÈsie, float rotation)
            : base(game, modele3D, position, homothÈsie, rotation)
        { }

        public override void Initialize()
        {
            CooldownTir = 5;
            Vitesse = 0;
            Angle = 45;
            base.Initialize();
            EstActivÈe = false;
            AncienVecteur = new Vector2(Camera.Direction.X, Camera.Direction.Z);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Camera = Game.Services.GetService(typeof(CamÈra)) as Camera1;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        public override void Update(GameTime gameTime)
        {
            if (GestionInput.EstNouveauClicGauche())
            {
                Camera.DÈsactiverCamÈra();
                EstActivÈe = !EstActivÈe;
            }

            GÈrerTrajectoire(gameTime);
            GÈrerTir(gameTime);           
        }

        private void TournerModele()
        {
            Vector2 NouveauVecteur = new Vector2(Camera.Direction.X, Camera.Direction.Z);
            if (NouveauVecteur != AncienVecteur)
            {
                Rotation -= Camera.DÈplacementSouris.X * MathHelper.Pi / 180 * 0.1f;
                AncienVecteur = NouveauVecteur;
            }
        }

        private void ModifierAngle()
        {
            if (GestionInput.EstEnfoncÈe(Keys.W))
            {
                Angle += 1;
            }
            if (GestionInput.EstEnfoncÈe(Keys.S))
            {
                Angle -= 1;
            }
            Game.Window.Title = Angle.ToString();
        }

        private void GÈrerTrajectoire(GameTime gameTime)
        {
            Temps…coulÈMAJ2 += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Temps…coulÈMAJ2 >= 0.5f)
            {
                if (EstActivÈe)
                {
                    AmmunitionCatapulte Ammunition = new AmmunitionCatapulte(Game, "Models_Ammunition", new Vector3(Position.X, Position.Y + 15, Position.Z), 1, 180);
                    Game.Components.Add(Ammunition);
                    Ammunition.TirerProjectile(MathHelper.ToRadians(Angle), Camera.Direction);
                }
                Temps…coulÈMAJ2 = 0;
            }
        }

        private void GÈrerTir(GameTime gameTime)
        {
            CooldownTir += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps…coulÈMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Temps…coulÈMAJ >= INTERVALLE_MAJ)
            {
                if (EstActivÈe)
                {
                    TournerModele();
                    ModifierAngle();
                    if (GestionInput.EstEnfoncÈe(Keys.Space))
                    {
                        if (CooldownTir >= 5)
                        {
                            AmmunitionCatapulte Ammunition = new AmmunitionCatapulte(Game, "Models_Ammunition", new Vector3(Position.X, Position.Y + 15, Position.Z), 1, 180);
                            Ammunition.DrawOrder = 3;
                            Game.Components.Add(Ammunition);

                        
                            Ammunition.TirerProjectile(MathHelper.ToRadians(Angle), Camera.Direction);
                            CooldownTir = 0;
                        }
                    }
                }
                Temps…coulÈMAJ = 0;
            }
        }
    }     
}
