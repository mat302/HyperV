using AtelierXNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HyperV
{
    public class AmmunitionCatapulte : CreateurModele
    {
        bool EstTiré { get; set; }
        public bool EstAmmunition { get; set; }

        const float GRAVITÉ = -9.81f;
        const float Poids = 0.1f;
        const float INTERVALLE_MAJ = 1 / 60f;

        int VitesseY { get; set; }
        float TempsÉcoulé = 0;
        float TempsÉcouléMAJ = 0;
        Vector3 PositionInitiale { get; set; }

        float FrictionAir { get; set; }
        float Angle { get; set; }

        Vector3 Déplacement { get; set; }
        Vector2 Vitesse { get; set; }
        SoundEffect TourDétruite { get; set; }
        RessourcesManager<SoundEffect> SoundManager { get; set; }



        public AmmunitionCatapulte(Game game, string modele3D, Vector3 position, float homothésie, float rotation)
            : base(game, modele3D, position, homothésie, rotation)
        { }

        public override void Initialize()
        {
            base.Initialize();
            Déplacement = Vector3.Zero;
            PositionInitiale = Position;
            EstTiré = true;
            EstAmmunition = false;
            TourDétruite = SoundManager.Find("TourDétruite");
        }

        public override void Update(GameTime gameTime)
        {
            if (EstTiré)
            {
                TempsÉcouléMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (TempsÉcouléMAJ >= INTERVALLE_MAJ)
                {
                    TempsÉcoulé += INTERVALLE_MAJ;
                    Vector3 Déplacement = PositionProjectile(TempsÉcoulé);
                    Position = PositionInitiale + Déplacement;
                    base.Update(gameTime);
                    TempsÉcouléMAJ = 0;
                }
            }
            if (Position.Y < -100)
            {
                Game.Components.Remove(this);
                //Game.Components.RemoveAt(0);  //remove le Afficheur3D qui va etre a la roche -1 ... 
            }
            if (EstAmmunition)
            {
                GérerColision();
            }
        }

        protected override void LoadContent()
        {
            SoundManager = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
            base.LoadContent();
        }

        public void TirerProjectile(float angle, Vector3 vitesse, int ModificateurVitesse)
        {
            EstTiré = true;
            Angle = angle;
            vitesse.Normalize();
            VitesseY = ModificateurVitesse;
            Vitesse = new Vector2((float)Math.Cos(vitesse.X), (float)Math.Sin(vitesse.Z)) * ModificateurVitesse;
        }

        private Vector3 PositionProjectile(float temps)
        {
            return new Vector3(DéplacementX(Vitesse.X, temps), DéplacementY(Angle, VitesseY, temps), DéplacementZ(Vitesse.Y, temps));
        }

        private float DéplacementX(float vitesse, float temps)
        {
            return vitesse * temps;
        }

        private float DéplacementZ(float vitesse, float temps)
        {
            return vitesse * temps;
        }

        private float DéplacementY(float angle, float vitesse, float temps)
        {
            return (vitesse * (float)Math.Sin(angle) * temps + 0.5f * GRAVITÉ * temps * temps);
        }

        private void GérerColision()
        {
            bool aDetruire = false;
            List<CreateurModele> modeleDetruire = new List<CreateurModele>();
            foreach (CreateurModele modele in Game.Components.Where(x => x is CreateurModele))
            {
                if (modele.EstTour)
                {
                    if (Position.X < modele.GetPosition().X + 8 && Position.X > modele.GetPosition().X - 8) //test X
                    {
                        if (Position.Z < modele.GetPosition().Z + 8 && Position.Z > modele.GetPosition().Z - 8) //test z
                        {
                            if (Position.Y < modele.GetPosition().Y + 120 && Position.Y > modele.GetPosition().Y) //test y
                            {
                                modeleDetruire.Add(modele);
                                aDetruire = true;
                                TourDétruite.Play();
                            }
                        }
                    }
                }
            }
            if (aDetruire)
            {
                Game.Components.Remove(this);
                foreach (CreateurModele modele in modeleDetruire)
                {
                    Game.Components.Remove(modele);
                }
            }
        }
    }
}
