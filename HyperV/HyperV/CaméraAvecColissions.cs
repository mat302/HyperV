using AtelierXNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;

namespace HyperV
{
    public class Cam�raAvecColissions : Cam�raJoueur
    {
        bool placerJoueur { get; set; }
        float Temps�coul�DepuisMAJ { get; set; }
        List<Maze> Maze { get; set; }
        List<Character> Characters { get; set; }
        Boss Boss { get; set; }
        List<HeightMap> HeightMap { get; set; }
        Water Water { get; set; }
        Grass Grass { get; set; }
        List<Walls> Walls { get; set; }
        List<Portal> Portals { get; set; }
        List<House> Houses { get; set; }

        public Cam�raAvecColissions(Game jeu, Vector3 positionCam�ra, Vector3 cible, Vector3 orientation, float intervalleMAJ, float renderDistance)
            : base(jeu, positionCam�ra, cible, orientation, intervalleMAJ, renderDistance)
        { }

        protected override void ChargerContenu()
        {
            base.ChargerContenu();
            Maze = Game.Services.GetService(typeof(List<Maze>)) as List<Maze>;
            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
            Boss = Game.Services.GetService(typeof(Boss)) as Boss;
            HeightMap = Game.Services.GetService(typeof(List<HeightMap>)) as List<HeightMap>;
            Grass = Game.Services.GetService(typeof(Grass)) as Grass;
            G�rerHauteur();
            Water = Game.Services.GetService(typeof(Water)) as Water;
            Walls = Game.Services.GetService(typeof(List<Walls>)) as List<Walls>;
            Houses = Game.Services.GetService(typeof(List<House>)) as List<House>;
            Portals = Game.Services.GetService(typeof(List<Portal>)) as List<Portal>;
        }
        
        protected override void G�rerHauteur()
        {
            if (HeightMap.Count > 0)
            {
                float height = 5;
                for(int i = 0; i < HeightMap.Count && height == 5; ++i)
                {
                    height = HeightMap[i].GetHeight(Position);
                }
                Height = height;
            }
            base.G�rerHauteur();
        }

        protected override void G�rerD�placement(float direction, float lat�ral)
        {
            base.G�rerD�placement(direction, lat�ral);

            if ((Maze.Count > 0 ? CheckForMazeCollision() : false) || (Walls.Count > 0 ? CheckForWallsCollision() : false) || (Characters.Count > 0 ? CheckForCharacterCollision() : false) || (Portals.Count > 0 ? CheckForPortalCollision() : false) /*|| (Boss != null ? CheckForBossCollision() : false)*/ || (Houses.Count > 0 ? CheckForHouseCollision() : false))
            {
                Position -= direction * VitesseTranslation * Direction;
                Position += lat�ral * VitesseTranslation * Lat�ral;
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
            Game.Window.Title = Position.ToString();
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

        public void D�sactiverCam�ra()
        {
            D�sactiverD�placement = !D�sactiverD�placement;
            Direction = new Vector3(1, 0, 0);
        }

        public override void Update(GameTime gameTime)
        {
            float Temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += Temps�coul�;
            if (Temps�coul�DepuisMAJ >= IntervalleMAJ)
            {
                {
                    if (!D�sactiverD�placement)
                    {
                        if (placerJoueur)
                        {
                            Height = 2;
                            placerJoueur = false;
                            Position = new Vector3(-27, 2, -28);
                        }
                    }
                    if (D�sactiverD�placement)
                    {
                        Height = 15;
                        Position = new Vector3(-57, 15, -52);
                        placerJoueur = true;
                    }
                    Position = new Vector3(Position.X, Height, Position.Z);
                }
                Temps�coul�DepuisMAJ = 0;
            }
            base.Update(gameTime);
        }        
    }
}

