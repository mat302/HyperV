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
    public class Ennemy : ObjetDeBase
    {
        int Strength { get; set; }
        float Interval { get; set; }
        float Timer { get; set; }
        CaméraJoueur Camera { get; set; }
        Vector3 Adjustment { get; set; }
        Vector3 Shifting { get; set; }
        float Scale { get; set; }
        float Height { get; set; }

        public Ennemy(Game game, string name, float scale, Vector3 rotation, Vector3 position, int strength, float interval) : base(game, name, scale, rotation, position)
        {
            Scale = scale;
            Strength = strength;
            Interval = interval;
            Height = position.Y;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Camera = Game.Services.GetService(typeof(Caméra)) as CaméraJoueur;
            Adjustment = new Vector3(0, MathHelper.ToDegrees(180), 0);
            Shifting = Vector3.Normalize(Camera.Position - Position);
        }

        float r { get; set; }
        float theta { get; set; }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Interval)
            {
                //Game.Window.Title = Position.ToString();
                r = (float)Math.Sqrt(Camera.Direction.X * Camera.Direction.X + Camera.Direction.Y * Camera.Direction.Y + Camera.Direction.Z * Camera.Direction.Z);
                theta = -(float)Math.Acos(Camera.Direction.Z / r);
                Rotation = new Vector3(0, theta, 0) + Adjustment;
                Shifting = Vector3.Normalize(Camera.Position - Position);
                Position += Shifting;
                Position = new Vector3(Position.X, Height, Position.Z);
                CalculerMatriceMonde();
                if (CheckForCollision())
                {
                    Position -= Shifting;
                    Camera.Attack(2);
                }
                Timer = 0;
            }
        }

        protected virtual void CalculerMatriceMonde()
        {
            Monde = Matrix.Identity * Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);
        }

        const float MAX_DISTANCE = 8;

        bool CheckForCollision()
        {
            return Vector3.Distance(Camera.Position, Position) < MAX_DISTANCE;
        }
    }
}
