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
    public class Enemy : ObjetDeBase
    {
        int Strength { get; set; }
        float Interval { get; set; }
        float Timer { get; set; }
        CaméraJoueur Camera { get; set; }
        Vector3 Adjustment { get; set; }
        Vector3 Shifting { get; set; }
        float Scale { get; set; }
        float Height { get; set; }
        int MaxLife { get; set; }
        float Speed { get; set; }
        int Life { get; set; }
        bool Dead { get; set; }
        List<Character> Characters { get; set; }
        Boss Boss { get; set; }
        List<HeightMap> HeightMap { get; set; }
        List<Walls> Walls { get; set; }
        List<Portal> Portals { get; set; }
        List<House> Houses { get; set; }
        List<Maze> Maze { get; set; }

        public Enemy(Game game, string name, float scale, Vector3 rotation, Vector3 position, int strength, int maxLife, float speed, float interval) : base(game, name, scale, rotation, position)
        {
            Scale = scale;
            Strength = strength;
            Interval = interval;
            Height = position.Y;
            MaxLife = maxLife;
            Speed = speed;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Life = MaxLife;
            Dead = false;
            Camera = Game.Services.GetService(typeof(Caméra)) as CaméraJoueur;
            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
            Boss = Game.Services.GetService(typeof(Boss)) as Boss;
            HeightMap = Game.Services.GetService(typeof(List<HeightMap>)) as List<HeightMap>;
            Walls = Game.Services.GetService(typeof(List<Walls>)) as List<Walls>;
            Houses = Game.Services.GetService(typeof(List<House>)) as List<House>;
            Portals = Game.Services.GetService(typeof(List<Portal>)) as List<Portal>;
            Maze = Game.Services.GetService(typeof(List<Maze>)) as List<Maze>;
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
                r = (float)Math.Sqrt(Camera.Direction.X * Camera.Direction.X + Camera.Direction.Y * Camera.Direction.Y + Camera.Direction.Z * Camera.Direction.Z);
                theta = -(float)Math.Acos(Camera.Direction.Z / r);
                Rotation = new Vector3(0, theta, 0) + Adjustment;
                Shifting = Vector3.Normalize(Camera.Position - Position);
                if (Vector3.Distance(Camera.Position, Position) < 200)
                {
                    Position += Shifting * Speed;
                    if ((Maze.Count > 0 ? CheckForMazeCollision() : false) || (Walls.Count > 0 ? CheckForWallsCollision() : false) || (Characters.Count > 0 ? CheckForCharacterCollision() : false) || (Portals.Count > 0 ? CheckForPortalCollision() : false) || (Boss != null ? CheckForBossCollision() : false) || (Houses.Count > 0 ? CheckForHouseCollision() : false))
                    {
                        Position -= Shifting * Speed;
                    }
                }
                if (HeightMap.Count > 0)
                {
                    float height = 5;
                    for (int i = 0; i < HeightMap.Count && height == 5; ++i)
                    {
                        height = HeightMap[i].GetHeight(Position) - 5;
                    }
                    Height = height;
                }
                Position = new Vector3(Position.X, Height, Position.Z);
                CalculerMatriceMonde();
                if (CheckForCollision(MAX_DISTANCE_ENEMY))
                {
                    Position -= Shifting * Speed;
                    Camera.Attack(2);
                }
                if (Dead)
                {
                    Game.Components.Remove(this);
                }
                Timer = 0;
            }
        }

        protected virtual void CalculerMatriceMonde()
        {
            Monde = Matrix.Identity * Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);
        }

        const float MAX_DISTANCE_ENEMY = 8;
        const float MAX_DISTANCE_PLAYER = 10;

        bool CheckForCollision(float distance)
        {
            return Vector3.Distance(Camera.Position, Position) < distance;
        }

        public void CheckForAttack(int attack)
        {
            if (CheckForCollision(MAX_DISTANCE_PLAYER))
            {
                Attack(attack);
            }
        }

        public void CheckForArrowAttack(Vector3 position, int attack, Fleche arrow)
        {
            if (CheckForArrowCollision(position, MAX_DISTANCE_PLAYER))
            {
                Attack(attack);
                Game.Components.Remove(arrow);
            }
        }

        bool CheckForArrowCollision(Vector3 position, float distance)
        {
            return Vector3.Distance(position, Position) < distance;
        }

        void Attack(int attackPts)
        {
            int newLife = Life - attackPts;
            if (newLife <= 0)
            {
                Life = 0;
                Dead = true;
            }
            else if (newLife > MaxLife)
            {
                Life = MaxLife;
            }
            else
            {
                Life = newLife;
            }
        }

        bool CheckForWallsCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Walls.Count && !result; ++i)
            {
                result = Walls[i].CheckForCollisions(Position);
            }

            return result;
        }

        bool CheckForMazeCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Maze.Count && !result; ++i)
            {
                result = Maze[i].CheckForCollisions(Position);
            }

            return result;
        }

        const float MAX_DISTANCE = 5.5f, MAX_DISTANCE_BOSS = 80f;

        bool CheckForBossCollision()
        {
            return Vector3.Distance(Boss.GetPosition(), Position) < MAX_DISTANCE_BOSS;
        }

        bool CheckForPortalCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Portals.Count && !result; ++i)
            {
                result = Portals[i].CheckForCollisions(Position);
            }

            return result;
        }

        bool CheckForCharacterCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Characters.Count && !result; ++i)
            {
                result = Vector3.Distance(Characters[i].GetPosition(), Position) < MAX_DISTANCE;
            }

            return result;
        }

        bool CheckForHouseCollision()
        {
            bool result = false;
            float? d;
            int i;

            for (i = 0; i < Houses.Count && !result; ++i)
            {
                result = Houses[i].Collision(new BoundingSphere(Position, 7));
            }

            return result;
        }
    }
}
