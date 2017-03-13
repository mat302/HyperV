using Microsoft.Xna.Framework;
using System;


namespace HyperV
{
    public class AmmunitionCatapulte : CreateurModele
    {
        bool EstTiré = true;
        const float GRAVITÉ = 9.81f;
        const float Poids = 0.1f;
        const float INTERVALLE_MAJ = 1 / 60f;
        float TempsÉcoulé = 0;
        float TempsÉcouléMAJ = 0;

        float FrictionAir { get; set; }
        float Angle { get; set; }
                
        Vector3 Déplacement { get; set; }
        float Vitesse { get; set; }

        public AmmunitionCatapulte(Game game, string modele3D, Vector3 position, float homothésie, float rotation)
            : base(game, modele3D, position, homothésie, rotation)
        {
            Position = position;
        }

        public override void Initialize()
        {
            base.Initialize();
            Déplacement = Vector3.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            if (EstTiré)
            {
                TempsÉcouléMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(TempsÉcouléMAJ >= INTERVALLE_MAJ)
                {
                    TempsÉcoulé += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    //Vector3 Déplacement = PositionProjectile(0, new Vector3(0, 0.0000000001f, 0), TempsÉcoulé);
                    Vector3 Déplacement = new Vector3(0,-0.1f,0);
                    Position += Déplacement;
                    if (Position.Y < -20)
                    {
                        Position = new Vector3(Position.X, -20, Position.Z);
                    }
                    base.Update(gameTime);
                    TempsÉcouléMAJ = 0;
                }
            }
        }

        public void TirerProjectile(float angle, float vitesse)
        {
            EstTiré = true;
            Angle = angle;
            Vitesse = vitesse;
        }

        private Vector3 PositionProjectile(float temps)
        {
            return Position * new Vector3(DéplacementXZ(Angle, Vitesse, temps), DéplacementY(Angle, Vitesse, temps), DéplacementXZ(Angle, Vitesse, temps));
        }

        private float DéplacementXZ(float angle, float vitesse, float temps)
        {
            return vitesse * (float)(Math.Cos(angle)) * temps;
        }

        private float DéplacementY(float angle, float vitesse, float temps)
        {
            return vitesse * (float)Math.Sin(angle) * temps - 0.5f * GRAVITÉ * temps * temps;
        }
    }
}
