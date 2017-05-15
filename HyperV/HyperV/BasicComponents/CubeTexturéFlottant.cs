using System;
using Microsoft.Xna.Framework;


namespace AtelierXNA
{
    public class CubeTexturÈFlottant : CubeTexturÈ
    {
        float Temps…coulÈDepuisMAJ { get; set; }
        float IntervalleVariation { get; set; }
        float Variation { get; set; }

        public CubeTexturÈFlottant(Game game, float homothÈtieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, string nomTextureCube,
            Vector3 dimension, float intervalleMAJ) : base(game, homothÈtieInitiale, rotationInitiale, positionInitiale, nomTextureCube, dimension, intervalleMAJ)
        {
            IntervalleVariation = intervalleMAJ;
        }
        public override void Initialize()
        {
            Lacet = true;
            Variation = 0;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float Temps…coulÈ = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps…coulÈDepuisMAJ += Temps…coulÈ;
            if (Temps…coulÈDepuisMAJ >= IntervalleVariation)
            {
                VariationCube();
                Temps…coulÈDepuisMAJ = 0;
            }
        }
        private void VariationCube()
        {
            Position = new Vector3(Position.X, PositionInitiale.Y + (float)Math.Sin(Variation), Position.Z);
            Variation += 0.02f;
        }
    }
}
