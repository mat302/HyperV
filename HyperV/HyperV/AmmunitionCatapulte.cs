using Microsoft.Xna.Framework;
using System;


namespace HyperV
{
    public class AmmunitionCatapulte : CreateurModele
    {
        const float GRAVITÉ = 9.81f;
        const float Poids = 0.1f;
        const float INTERVALLE_MAJ = 1 / 60f;

        float FrictionAir { get; set; }
        
        Vector3 Déplacement { get; set; }

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
            Vector3 Déplacement = PositionProjectile(0, 0.0001f, 1);
            
            Position += Déplacement;
            if (Position.Y < -20)
            {
                Position = new Vector3(Position.X, -20, Position.Z);
            }
            base.Update(gameTime);
        }

        public Vector3 PositionProjectile(float angle, float vitesse, float temps)
        {
            return Position * new Vector3(DéplacementXZ(angle, vitesse, temps), DéplacementY(angle, vitesse, temps), DéplacementXZ(angle, vitesse, temps));
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
