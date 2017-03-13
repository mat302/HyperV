using Microsoft.Xna.Framework;
using System;


namespace HyperV
{
    public class AmmunitionCatapulte : CreateurModele
    {
        bool EstTir� = true;
        const float GRAVIT� = 9.81f;
        const float Poids = 0.1f;
        const float INTERVALLE_MAJ = 1 / 60f;
        float Temps�coul� = 0;
        float Temps�coul�MAJ = 0;

        float FrictionAir { get; set; }
        float Angle { get; set; }
                
        Vector3 D�placement { get; set; }
        float Vitesse { get; set; }

        public AmmunitionCatapulte(Game game, string modele3D, Vector3 position, float homoth�sie, float rotation)
            : base(game, modele3D, position, homoth�sie, rotation)
        {
            Position = position;
        }

        public override void Initialize()
        {
            base.Initialize();
            D�placement = Vector3.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            if (EstTir�)
            {
                Temps�coul�MAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(Temps�coul�MAJ >= INTERVALLE_MAJ)
                {
                    Temps�coul� += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    //Vector3 D�placement = PositionProjectile(0, new Vector3(0, 0.0000000001f, 0), Temps�coul�);
                    Vector3 D�placement = new Vector3(0,-0.1f,0);
                    Position += D�placement;
                    if (Position.Y < -20)
                    {
                        Position = new Vector3(Position.X, -20, Position.Z);
                    }
                    base.Update(gameTime);
                    Temps�coul�MAJ = 0;
                }
            }
        }

        public void TirerProjectile(float angle, float vitesse)
        {
            EstTir� = true;
            Angle = angle;
            Vitesse = vitesse;
        }

        private Vector3 PositionProjectile(float temps)
        {
            return Position * new Vector3(D�placementXZ(Angle, Vitesse, temps), D�placementY(Angle, Vitesse, temps), D�placementXZ(Angle, Vitesse, temps));
        }

        private float D�placementXZ(float angle, float vitesse, float temps)
        {
            return vitesse * (float)(Math.Cos(angle)) * temps;
        }

        private float D�placementY(float angle, float vitesse, float temps)
        {
            return vitesse * (float)Math.Sin(angle) * temps - 0.5f * GRAVIT� * temps * temps;
        }
    }
}
