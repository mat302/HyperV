using Microsoft.Xna.Framework;
using System;


namespace HyperV
{
    public class AmmunitionCatapulte : CreateurModele, ICollisionable
    {
        bool EstTir� = true;
        const float GRAVIT� = -9.81f;
        const float Poids = 0.1f;
        const float INTERVALLE_MAJ = 1 / 60f;
        float Temps�coul� = 0;
        float Temps�coul�MAJ = 0;
        Vector3 PositionInitiale { get; set; }

        float FrictionAir { get; set; }
        float Angle { get; set; }
                
        Vector3 D�placement { get; set; }
        Vector2 Vitesse { get; set; }

        public AmmunitionCatapulte(Game game, string modele3D, Vector3 position, float homoth�sie, float rotation)
            : base(game, modele3D, position, homoth�sie, rotation)
        { }

        public override void Initialize()
        {
            base.Initialize();
            D�placement = Vector3.Zero;
            PositionInitiale = Position;
        }

        public override void Update(GameTime gameTime)
        {
            if (EstTir�)
            {
                Temps�coul�MAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(Temps�coul�MAJ >= INTERVALLE_MAJ)
                {
                    Temps�coul� += INTERVALLE_MAJ;
                    Vector3 D�placement = PositionProjectile(Temps�coul�);
                    Position = PositionInitiale + D�placement;
                    base.Update(gameTime);
                    Temps�coul�MAJ = 0;                    
                }
            }
            if(Position.Y < -100)
            {
                Game.Components.Remove(this);
            }
        }

        public void TirerProjectile(float angle, Vector3 vitesse)
        {
            EstTir� = true;
            Angle = angle;
            vitesse.Normalize();
            Vitesse = new Vector2((float)Math.Cos(vitesse.X), (float)Math.Sin(vitesse.Z)) * 10;
        }

        private Vector3 PositionProjectile(float temps)
        {
            return new Vector3(D�placementX(Vitesse.X, temps), D�placementY(Angle, 50, temps), D�placementZ(Vitesse.Y, temps));
        }

        private float D�placementX(float vitesse, float temps)
        {
            return vitesse * temps;
        }

        private float D�placementZ(float vitesse, float temps)
        {
            return vitesse * temps;
        }

        private float D�placementY(float angle, float vitesse, float temps)
        {
            return (vitesse * temps + 0.5f * GRAVIT� * temps * temps) * (float)Math.Sin(angle);
        }
    }
}
