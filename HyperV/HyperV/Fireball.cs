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
        Vector3 Position { get; set; }
        SoundEffect SoundEffect { get; set; }
        RessourcesManager<SoundEffect> SoundManager { get; set; }
        Afficheur3D Display3D { get; set; }
        float WaitTime { get; set; }

        public Fireball(Game game, float scale, Vector3 rotation, Vector3 position, Vector2 range, string textureName, Vector2 description, float interval, float waitTime) : base(game, scale, rotation, position, range, textureName)
        {
            Interval = interval;
            Description = description;
            WaitTime = -waitTime;
        }

        public override void Initialize()
        {
            base.Initialize();
            Position = PositionInitiale;
            Delta = new Vector2(Texture.Width, Texture.Height) / Description;
            Camera = Game.Services.GetService(typeof(Caméra)) as CaméraJoueur;
            Adjustment = new Vector3(0, MathHelper.ToDegrees(180), 0);
            SoundManager = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
            SoundEffect = SoundManager.Find("explosion");
            Display3D = Game.Services.GetService(typeof(Afficheur3D)) as Afficheur3D;
            Shifting = 4 * Vector3.Normalize(Camera.Position - Position);
        }

        Vector3 Shifting { get; set; }
        float r { get; set; }
        float theta { get; set; }
        float ResetTimer { get; set; }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            ResetTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            WaitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Interval && WaitTime > 0)
            {
                if (ResetTimer > 5)
                {
                    ResetTimer = 0;
                    Position = PositionInitiale;
                    Shifting = 4 * Vector3.Normalize(Camera.Position - Position);
                }
                //Game.Window.Title = Position.ToString();
                UpdateTexture();
                InitialiserSommets();
                r = (float)Math.Sqrt(Camera.Direction.X * Camera.Direction.X + Camera.Direction.Y * Camera.Direction.Y + Camera.Direction.Z * Camera.Direction.Z);
                theta = -(float)Math.Acos(Camera.Direction.Z / r);
                Rotation = new Vector3(0, theta, 0) + Adjustment;
                Position += Shifting;
                CalculerMatriceMonde();
                if (CheckForCollision())
                {
                    SoundEffect.Play();
                    //Visible = false;
                    Position = PositionInitiale;
                    Camera.Attack(20);
                }
                Timer = 0;
            }
        }

        const float MAX_DISTANCE = 4;

        bool CheckForCollision()
        {
            return Vector3.Distance(Camera.Position, Position) < MAX_DISTANCE;
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
            Monde = Matrix.Identity * Matrix.CreateScale(HomothétieInitiale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);
        }

        public override void Draw(GameTime gameTime)
        {
            RasterizerState s;// = Display3D.JeuRasterizerState;
            s = new RasterizerState();
            s.CullMode = CullMode.None;
            s.FillMode = FillMode.Solid;
            Game.GraphicsDevice.RasterizerState = s;
            base.Draw(gameTime);
            Game.GraphicsDevice.RasterizerState = Display3D.JeuRasterizerState;
        }
    }
}
