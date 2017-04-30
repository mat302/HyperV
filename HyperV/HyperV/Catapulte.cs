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
    public class Catapulte : CreateurModele
    {
        const float INTERVALLE_MAJ = 1 / 60f;

        float TempsÉcouléMAJ { get; set; }
        float TempsÉcouléMAJ2 { get; set; }
        float CooldownTir { get; set; }
        InputManager GestionInput { get; set; }
        Camera2 Camera { get; set; }
        AmmunitionCatapulte Ammunition { get; set; }
        bool EstActivée { get; set; }
        RessourcesManager<SoundEffect> SoundManager { get; set; }
        SoundEffect CatapulteTirée { get; set; }

        float angle_;
        float Angle
        {
            get { return angle_; }
            set
            {
                if (angle_ < 0)
                {
                    value = 0;
                }
                if (angle_ > 90)
                {
                    value = 90;
                }
                angle_ = value;
            }
        }

        int vitesse_;
        int Vitesse
        {
            get { return vitesse_; }
            set
            {
                if (vitesse_ < 10)
                {
                    value = 10;
                }
                if (vitesse_ > 150)
                {
                    value = 150;
                }
                vitesse_ = value;
            }
        }

        Vector2 AncienVecteur { get; set; }

        public Catapulte(Game game, string modele3D, Vector3 position, float homothésie, float rotation)
            : base(game, modele3D, position, homothésie, rotation)
        { }

        public override void Initialize()
        {
            CooldownTir = 5;
            Vitesse = 20;
            Angle = 45;
            base.Initialize();
            EstActivée = false;
            AncienVecteur = new Vector2(Camera.Direction.X, Camera.Direction.Z);
            CatapulteTirée = SoundManager.Find("CatapulteTirée");

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Camera = Game.Services.GetService(typeof(Caméra)) as Camera2;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            SoundManager = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
        }

        public override void Update(GameTime gameTime)
        {
            if (GestionInput.EstNouveauClicGauche())
            {
                Camera.DésactiverCaméra();
                EstActivée = !EstActivée;
                Rotation = 0;
            }

            GérerTrajectoire(gameTime);
            GérerTir(gameTime);
        }

        private void TournerModele()
        {
            Vector2 NouveauVecteur = new Vector2(Camera.Direction.X, Camera.Direction.Z);
            if (NouveauVecteur != AncienVecteur)
            {
                Rotation -= Camera.DéplacementSouris.X * MathHelper.Pi / 180 * 0.1f;
                AncienVecteur = NouveauVecteur;
            }
        }

        private void ModifierAngle()
        {
            if (GestionInput.EstEnfoncée(Keys.W))
            {
                Angle += 1;
            }
            if (GestionInput.EstEnfoncée(Keys.S))
            {
                Angle -= 1;
            }
        }
        private void ModifierVitesse()
        {
            if (GestionInput.EstEnfoncée(Keys.A))
            {
                Vitesse -= 1;
            }
            if (GestionInput.EstEnfoncée(Keys.D))
            {
                Vitesse += 1;
            }
        }

        private void GérerTrajectoire(GameTime gameTime)
        {
            TempsÉcouléMAJ2 += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TempsÉcouléMAJ2 >= 0.5f)
            {
                if (EstActivée)
                {
                    AmmunitionCatapulte Trajectoire = new AmmunitionCatapulte(Game, "Models_Ammunition", new Vector3(Position.X, Position.Y + 15, Position.Z), 0.4f, 180);
                    Game.Components.Add(new Afficheur3D(Game));
                    Game.Components.Add(Trajectoire);
                    Trajectoire.TirerProjectile(MathHelper.ToRadians(Angle), Camera.Direction, Vitesse);
                }
                TempsÉcouléMAJ2 = 0;
            }
        }

        private void GérerTir(GameTime gameTime)
        {
            CooldownTir += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléMAJ >= INTERVALLE_MAJ)
            {
                if (EstActivée)
                {
                    TournerModele();
                    ModifierAngle();
                    ModifierVitesse();
                    if (GestionInput.EstEnfoncée(Keys.Space))
                    {
                        if (CooldownTir >= 5)
                        {
                            AmmunitionCatapulte Ammunition = new AmmunitionCatapulte(Game, "Models_Ammunition", new Vector3(Position.X, Position.Y + 15, Position.Z), 2, 180);
                            Game.Components.Add(new Afficheur3D(Game));
                            Game.Components.Add(Ammunition);
                            Ammunition.TirerProjectile(MathHelper.ToRadians(Angle), Camera.Direction, Vitesse);
                            Ammunition.EstAmmunition = true;
                            CooldownTir = 0;
                            CatapulteTirée.Play();
                        }
                    }
                }
                TempsÉcouléMAJ = 0;
            }
        }
    }
}
