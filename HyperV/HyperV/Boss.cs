/*
Boss.cs
-------

By Mathieu Godin

Role : Used to create a non-playable
       boss rendered with a .fbx 
       3d model that can have a fight
       with the player

Created : 3/12/17
*/
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
    public class Boss : ObjetDeBase
    {
        float Interval { get; set; }
        float Radius { get; set; }
        public bool Dead { get; set; }
        BossLabel BossLabel { get; set; }
        string GaugeName { get; set; }
        string DockName { get; set; }
        string FontName { get; set; }
        string Name { get; set; }
        int MaxLife { get; set; }
        float Timer { get; set; }
        float LabelInterval { get; set; }
        InputManager InputManager { get; set; }
        Camera2 Camera { get; set; }
        List<Fireball> Fireballs { get; set; }
        Vector3 FireBallPosition { get; set; }

        public Boss(Game game, string name, int maxLife, string modelName, string gaugeName, string dockName, string fontName, float interval, float labelInterval, float startScale, Vector3 startRotation, Vector3 startPosition) : base(game, modelName, startScale, startRotation, startPosition)
        {
            Radius = 80;
            DockName = dockName;
            GaugeName = gaugeName;
            FontName = fontName;
            Name = name;
            Interval = interval;
            LabelInterval = labelInterval;
            MaxLife = maxLife;
            Fireballs = new List<Fireball>();
            Input = false;
            FireBallPosition = new Vector3(Position.X, Position.Y - 3, Position.Z);
        }

        public void AddFireball()
        {
            Fireballs.Add(new Fireball(Game, 1, new Vector3(0, MathHelper.ToRadians(180), 0), FireBallPosition, new Vector2(10, 10), "feufollet", new Vector2(20, 1), Interval, 0));
            Game.Components.Add(Fireballs[0]);
            Fireballs.Add(new Fireball(Game, 1, new Vector3(0, MathHelper.ToRadians(180), 0), FireBallPosition, new Vector2(10, 10), "feufollet", new Vector2(20, 1), Interval, 1));
            Game.Components.Add(Fireballs[1]);
            Fireballs.Add(new Fireball(Game, 1, new Vector3(0, MathHelper.ToRadians(180), 0), FireBallPosition, new Vector2(10, 10), "feufollet", new Vector2(20, 1), Interval, 2));
            Game.Components.Add(Fireballs[2]);
            Fireballs.Add(new Fireball(Game, 1, new Vector3(0, MathHelper.ToRadians(180), 0), FireBallPosition, new Vector2(10, 10), "feufollet", new Vector2(20, 1), Interval, 3));
            Game.Components.Add(Fireballs[3]);
            Fireballs.Add(new Fireball(Game, 1, new Vector3(0, MathHelper.ToRadians(180), 0), FireBallPosition, new Vector2(10, 10), "feufollet", new Vector2(20, 1), Interval, 4));
            Game.Components.Add(Fireballs[4]);




            //Fireballs.Add(new Fireball(Game, 1, new Vector3(0, MathHelper.ToRadians(180), 0), FireBallPosition, new Vector2(10, 10), "feufollet", new Vector2(20, 1), Interval, 5));
            //Game.Components.Add(Fireballs[5]);
            //Fireballs.Add(new Fireball(Game, 1, new Vector3(0, MathHelper.ToRadians(180), 0), FireBallPosition, new Vector2(10, 10), "feufollet", new Vector2(20, 1), Interval, 6));
            //Game.Components.Add(Fireballs[6]);
            //Fireballs.Add(new Fireball(Game, 1, new Vector3(0, MathHelper.ToRadians(180), 0), FireBallPosition, new Vector2(10, 10), "feufollet", new Vector2(20, 1), Interval, 7));
            //Game.Components.Add(Fireballs[7]);
            //Fireballs.Add(new Fireball(Game, 1, new Vector3(0, MathHelper.ToRadians(180), 0), FireBallPosition, new Vector2(10, 10), "feufollet", new Vector2(20, 1), Interval, 8));
            //Game.Components.Add(Fireballs[8]);
            //Fireballs.Add(new Fireball(Game, 1, new Vector3(0, MathHelper.ToRadians(180), 0), FireBallPosition, new Vector2(10, 10), "feufollet", new Vector2(20, 1), Interval, 9));
            //Game.Components.Add(Fireballs[9]);
        }

        public void RemoveFireball()
        {
            Game.Components.Remove(Fireballs[0]);
            Game.Components.Remove(Fireballs[1]);
        }

        protected override void LoadContent()
        {
            InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Camera = Game.Services.GetService(typeof(Caméra)) as Camera2;
            base.LoadContent();
        }

        public Vector3 GetPosition()
        {
            return new Vector3(Position.X, Position.Y, Position.Z);
        }

        public void AddLabel()
        {
            BossLabel = new BossLabel(Game, this, Name, MaxLife, GaugeName, DockName, FontName, LabelInterval);
            Game.Components.Add(BossLabel);
        }

        public void RemoveLabel()
        {
            Game.Components.Remove(BossLabel);
        }

        bool Input { get; set; }
        Vector3 Direction { get; set; }
        float Result { get; set; }

        /// <summary> 
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Input = InputManager.EstNouvelleTouche(Keys.E) ? true : Input;

            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Interval)
            {
                Direction = Camera.Position - Position;
                Result = Direction.Z / Direction.X;
                Rotation = new Vector3(0, -(float)Math.Atan(Result) + MathHelper.ToRadians(90) + (Direction.X >= 0 ? MathHelper.ToRadians(180) : 0), 0);
                UpdateWorld();
                //foreach (Fireball f in Fireballs)
                //{
                //    f.InitialPosition = CreateInitialPosition();
                //}
                if (Input)
                {
                    Input = false;
                    float? collision = Collision(new Ray(Camera.Position, Camera.Direction));
                    if (collision < 20)
                    {
                        BossLabel.Attack(1);
                    }
                }
                Timer = 0;
            }
        }

        public void CheckForAttack(int attack)
        {
            float? collision = Collision(new Ray(Camera.Position, Camera.Direction));
            if (collision < 20)
            {
                BossLabel.Attack(attack);
            }
        }

        public void CheckForArrowAttack(Vector3 position, Vector3 direction, int attack, Fleche arrow)
        {
            float? collision = Collision(new Ray(position, direction));
            if (collision < 20)
            {
                BossLabel.Attack(attack);
                Game.Components.Remove(arrow);
            }
        }

        void UpdateWorld()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(Échelle);
            Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            Monde *= Matrix.CreateTranslation(Position);
        }

        public float? Collision(Ray ray)
        {
            return BoundingSphere.Intersects(ray);
        }

        public BoundingSphere BoundingSphere { get { return new BoundingSphere(Position, Radius); } }
    }
}

