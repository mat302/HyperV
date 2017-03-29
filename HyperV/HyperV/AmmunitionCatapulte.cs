using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HyperV
{
    public class AmmunitionCatapulte : CreateurModele
    {
        bool EstTirÈ { get; set; }
        public bool EstAmmunition { get; set; }

        const float GRAVIT… = -9.81f;
        const float Poids = 0.1f;
        const float INTERVALLE_MAJ = 1 / 60f;
        
        int VitesseY { get; set; }
        float Temps…coulÈ = 0;
        float Temps…coulÈMAJ = 0;
        Vector3 PositionInitiale { get; set; }

        float FrictionAir { get; set; }
        float Angle { get; set; }
                
        Vector3 DÈplacement { get; set; }
        Vector2 Vitesse { get; set; }

        public AmmunitionCatapulte(Game game, string modele3D, Vector3 position, float homothÈsie, float rotation)
            : base(game, modele3D, position, homothÈsie, rotation)
        { }

        public override void Initialize()
        {
            base.Initialize();
            DÈplacement = Vector3.Zero;
            PositionInitiale = Position;
            EstTirÈ = true;
            EstAmmunition = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (EstTirÈ)
            {
                Temps…coulÈMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(Temps…coulÈMAJ >= INTERVALLE_MAJ)
                {
                    Temps…coulÈ += INTERVALLE_MAJ;
                    Vector3 DÈplacement = PositionProjectile(Temps…coulÈ);
                    Position = PositionInitiale + DÈplacement;
                    base.Update(gameTime);
                    Temps…coulÈMAJ = 0;                    
                }
            }
            if(Position.Y < -100)
            {
                Game.Components.Remove(this);
                //Game.Components.RemoveAt(0);  //remove le Afficheur3D qui va etre a la roche -1 ... 
            }
            if (EstAmmunition)
            {
                GÈrerColision();
            }
        }

        public void TirerProjectile(float angle, Vector3 vitesse, int ModificateurVitesse)
        {
            EstTirÈ = true;
            Angle = angle;
            vitesse.Normalize();
            VitesseY = ModificateurVitesse;
            Vitesse = new Vector2((float)Math.Cos(vitesse.X), (float)Math.Sin(vitesse.Z)) * ModificateurVitesse;
        }

        private Vector3 PositionProjectile(float temps)
        {
            return new Vector3(DÈplacementX(Vitesse.X, temps), DÈplacementY(Angle, VitesseY, temps), DÈplacementZ(Vitesse.Y, temps));
        }

        private float DÈplacementX(float vitesse, float temps)
        {
            return vitesse * temps;
        }

        private float DÈplacementZ(float vitesse, float temps)
        {
            return vitesse * temps;
        }

        private float DÈplacementY(float angle, float vitesse, float temps)
        {
            return (vitesse * (float)Math.Sin(angle) * temps + 0.5f * GRAVIT… * temps * temps);
        }

        private void GÈrerColision()
        {
            bool aDetruire = false;
            List<CreateurModele> modeleDetruire = new List<CreateurModele>();
            foreach(CreateurModele modele in Game.Components.Where(x => x is CreateurModele))
            {
                if (modele.EstTour)
                {
                    if(Position.X < modele.GetPosition().X + 3 && Position.X > modele.GetPosition().X - 3) //test X
                    {
                        if(Position.Z < modele.GetPosition().Z + 3 && Position.Z > modele.GetPosition().Z - 3) //test z
                        {
                            if (Position.Y < modele.GetPosition().Y + 130 && Position.Y > modele.GetPosition().Y) //test y
                            {
                                modeleDetruire.Add(modele);
                                aDetruire = true;
                            }
                        }
                    }
                }
            }
            if (aDetruire)
            {
                Game.Components.Remove(this);
                foreach(CreateurModele modele in modeleDetruire)
                {
                    Game.Components.Remove(modele);
                }
            }            
        }
    }
}
