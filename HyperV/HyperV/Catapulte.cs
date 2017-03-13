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

        float Temps�coul�MAJ { get; set; }
        InputManager GestionInput { get; set; }
        Camera1 Camera { get; set; }
        AmmunitionCatapulte Ammunition { get; set; }
        bool EstActiv�e { get; set; }

        float angle;
        float Angle
        {
            get { return angle; }
            set
            {
                if(angle < 0)
                {
                    angle = 0;
                }
                if(angle > (Math.PI/2))
                {
                    angle = (float)(Math.PI / 2);
                }
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
                if(vitesse < 0)
                {
                    vitesse = 0;
                }
                if(vitesse > 100)
                {
                    vitesse = 100;
                }
            }
        }

        Vector2 AncienVecteur { get; set; }
        Point AnciennePositionSouris { get; set; }

        public Catapulte(Game game, string modele3D, Vector3 position, float homoth�sie, float rotation)
            : base(game, modele3D, position, homoth�sie, rotation)
        { }

        public override void Initialize()
        {
            Vitesse = 0;
            base.Initialize();
            EstActiv�e = true;
            AncienVecteur = new Vector2(Camera.Direction.X, Camera.Direction.Z);
            AnciennePositionSouris = GestionInput.GetPositionSouris();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Camera = Game.Services.GetService(typeof(Cam�ra)) as Camera1;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Ammunition = Game.Services.GetService(typeof(AmmunitionCatapulte)) as AmmunitionCatapulte;
        }

        public override void Update(GameTime gameTime)
        {
            if (GestionInput.EstNouveauClicGauche())
            {
                Camera.D�sactiverCam�ra();
                //Ammunition.TirerProjectile(Angle, Vitesse);
            }

            Temps�coul�MAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Temps�coul�MAJ >= INTERVALLE_MAJ)
            {
                if (EstActiv�e)
                {
                    TournerModele();
                    ModifierAngle();
                    ModifierForce();
                }
                Temps�coul�MAJ = 0;
            }
        }

        private void TournerModele()
        {                        
            Vector2 NouveauVecteur = new Vector2(Camera.Direction.X, Camera.Direction.Z);
            if(NouveauVecteur != AncienVecteur)
            {
                Rotation -= ((float)Math.Atan2(NouveauVecteur.Y - AncienVecteur.Y, NouveauVecteur.X - AncienVecteur.X))/50;
                AncienVecteur = NouveauVecteur;
            }
        }

        private void ModifierAngle()
        {
            Point NouvellePositionSouris = GestionInput.GetPositionSouris();
            if(NouvellePositionSouris != AnciennePositionSouris)
            {
                Angle -= NouvellePositionSouris.Y - AnciennePositionSouris.Y;                
            }
        }

        private void ModifierForce()
        {
            if (GestionInput.EstEnfonc�e(Keys.W))
            {
                Vitesse += 1;
            }
            if (GestionInput.EstEnfonc�e(Keys.S))
            {
                Vitesse -= 1;
            }
        }
    }
}
