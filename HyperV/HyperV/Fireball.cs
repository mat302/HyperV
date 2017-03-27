using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using AtelierXNA;


namespace HyperV
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Fireball : TuileTexturée
    {
        Vector2 Description { get; set; }
        Vector2 Delta { get; set; }
        Rectangle SourceRectangle { get; set; }
        float Timer { get; set; }
        float Interval { get; set; }
        CaméraJoueur Camera { get; set; }
        Vector3 Rotation { get; set; }
        Vector3 Adjustment { get; set; }

        public Fireball(Game game, float scale, Vector3 rotation, Vector3 position, Vector2 range, string textureName, Vector2 description, float interval) : base(game, scale, rotation, position, range, textureName)
        {
            Interval = interval * 3;
            Description = description;
        }

        public override void Initialize()
        {
            base.Initialize();
            Delta = new Vector2(Texture.Width, Texture.Height) / Description;
            Camera = Game.Services.GetService(typeof(Caméra)) as CaméraJoueur;
            Adjustment = new Vector3(MathHelper.ToDegrees(-10), MathHelper.ToDegrees(180), MathHelper.ToDegrees(200.01f));
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Interval)
            {
                UpdateTexture();
                InitialiserSommets();
                Rotation = new Vector3(Camera.Direction.Z, Camera.Direction.Y, Camera.Direction.X) + Adjustment;
                CalculerMatriceMonde();
                Timer = 0;
            }
        }

        void UpdateTexture()
        {
            PtsTexture[0, 0] = new Vector2(((PtsTexture[0, 0].X * Texture.Width + Delta.X) % Texture.Width) / Texture.Width, 1);
            PtsTexture[1, 0] = new Vector2(PtsTexture[0, 0].X + Delta.X / Texture.Width, 1);
            PtsTexture[0, 1] = new Vector2(PtsTexture[0, 0].X, 0);
            PtsTexture[1, 1] = new Vector2(PtsTexture[1, 0].X, 0);
        }

        protected override void CalculerMatriceMonde()
        {
            Monde = Matrix.Identity * Matrix.CreateScale(HomothétieInitiale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(PositionInitiale);
        }
    }
}
