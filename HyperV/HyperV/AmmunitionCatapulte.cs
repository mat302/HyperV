using Microsoft.Xna.Framework;
using System;


namespace HyperV
{
    public class AmmunitionCatapulte : CreateurModele, ICollisionable
    {
        bool EstTiré = true;
        const float GRAVITÉ = -9.81f;
        const float Poids = 0.1f;
        const float INTERVALLE_MAJ = 1 / 60f;
        float TempsÉcoulé = 0;
        float TempsÉcouléMAJ = 0;
        Vector3 PositionInitiale { get; set; }

        float FrictionAir { get; set; }
        float Angle { get; set; }
                
        Vector3 Déplacement { get; set; }
        Vector2 Vitesse { get; set; }

        public AmmunitionCatapulte(Game game, string modele3D, Vector3 position, float homothésie, float rotation)
            : base(game, modele3D, position, homothésie, rotation)
        { }

        public override void Initialize()
        {
            base.Initialize();
            Déplacement = Vector3.Zero;
            PositionInitiale = Position;
        }

        public override void Update(GameTime gameTime)
        {
            if (EstTiré)
            {
                TempsÉcouléMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(TempsÉcouléMAJ >= INTERVALLE_MAJ)
                {
                    TempsÉcoulé += INTERVALLE_MAJ;
                    Vector3 Déplacement = PositionProjectile(TempsÉcoulé);
                    Position = PositionInitiale + Déplacement;
                    base.Update(gameTime);
                    TempsÉcouléMAJ = 0;                    
                }
            }
            if(Position.Y < -100)
            {
                Game.Components.Remove(this);
            }
        }

        public void TirerProjectile(float angle, Vector3 vitesse)
        {
            EstTiré = true;
            Angle = angle;
            vitesse.Normalize();
            Vitesse = new Vector2((float)Math.Cos(vitesse.X), (float)Math.Sin(vitesse.Z)) * 10;
        }

        private Vector3 PositionProjectile(float temps)
        {
            return new Vector3(DéplacementX(Vitesse.X, temps), DéplacementY(Angle, 50, temps), DéplacementZ(Vitesse.Y, temps));
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
            return (vitesse * temps + 0.5f * GRAVITÉ * temps * temps) * (float)Math.Sin(angle);
        }
    }
}
