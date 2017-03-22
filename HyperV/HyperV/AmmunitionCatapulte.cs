using Microsoft.Xna.Framework;
using System;


namespace HyperV
{
    public class AmmunitionCatapulte : CreateurModele, ICollisionable
    {
        bool EstTirÈ = true;
        const float GRAVIT… = -9.81f;
        const float Poids = 0.1f;
        const float INTERVALLE_MAJ = 1 / 60f;
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
            }
        }

        public void TirerProjectile(float angle, Vector3 vitesse)
        {
            EstTirÈ = true;
            Angle = angle;
            vitesse.Normalize();
            Vitesse = new Vector2((float)Math.Cos(vitesse.X), (float)Math.Sin(vitesse.Z)) * 10;
        }

        private Vector3 PositionProjectile(float temps)
        {
            return new Vector3(DÈplacementX(Vitesse.X, temps), DÈplacementY(Angle, 50, temps), DÈplacementZ(Vitesse.Y, temps));
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
            return (vitesse * temps + 0.5f * GRAVIT… * temps * temps) * (float)Math.Sin(angle);
        }
    }
}
