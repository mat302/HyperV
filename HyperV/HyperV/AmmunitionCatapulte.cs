using Microsoft.Xna.Framework;
using System;


namespace HyperV
{
    public class AmmunitionCatapulte : CreateurModele
    {
        const float GRAVIT� = 9.81f;
        const float Poids = 0.1f;
        const float INTERVALLE_MAJ = 1 / 60f;

        float FrictionAir { get; set; }
        
        Vector3 D�placement { get; set; }

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
            Vector3 D�placement = PositionProjectile(0, 0.0001f, 1);
            
            Position += D�placement;
            if (Position.Y < -20)
            {
                Position = new Vector3(Position.X, -20, Position.Z);
            }
            base.Update(gameTime);
        }

        public Vector3 PositionProjectile(float angle, float vitesse, float temps)
        {
            return Position * new Vector3(D�placementXZ(angle, vitesse, temps), D�placementY(angle, vitesse, temps), D�placementXZ(angle, vitesse, temps));
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
