/*
Walls.cs
--------

By Mathieu Godin

Role : Creates walls based on their
       positions written in the specified
       text file

Created : 2/26/17
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
using System.IO;

namespace HyperV
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Walls : DrawableGameComponent
    {
        const int NUM_TRIANGLES_PER_TILE = 2, NUM_VERTICES_PER_TRIANGLE = 3;

        RessourcesManager<Texture2D> TexturesManager { get; set; }
        string TileTextureName { get; set; }
        string DataFileName { get; set; }
        float Interval { get; set; }
        float Timer { get; set; }
        Texture2D TileTexture { get; set; }
        List<Vector2> FirstVertices { get; set; }
        List<Vector2> SecondVertices { get; set; }
        int NumTriangles { get; set; }
        int NumVertices { get; set; }
        Vector3[] VerticesPositions { get; set; }
        List<Vector3> PlaneEquations { get; set; }
        List<Vector3> PlanePoints { get; set; }
        List<float> Magnitudes { get; set; }
        Vector2[,] TexturePositions { get; set; }
        VertexPositionTexture[] Vertices { get; set; }
        BlendState BlendState { get; set; }
        BasicEffect BasicEffect { get; set; }
        Matrix World { get; set; }
        Caméra Camera { get; set; }
        List<float> Heights { get; set; }
        float YPosition { get; set; }

        public Walls(Game game, float interval, string tileTextureName, string dataFileName, float yPosition) : base(game)
        {
            Interval = interval;
            TileTextureName = tileTextureName;
            DataFileName = "../../../Walls/" + dataFileName;
            YPosition = yPosition;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            TexturesManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            TileTexture = TexturesManager.Find(TileTextureName);
            InitializeWallsData();
            VerticesPositions = new Vector3[NumVertices];
            //Collisions = new bool[(int)FindMaxX() * 11 / 10, (int)FindMaxY() * 11 / 10];
            //CreateCollisions();
            CreateVerticesPositions();
            CreateTexturePositions();
            InitializeVertices();
            CreateWorld();
            base.Initialize();
            InitializeBasicEffectParameters();
        }

        protected void InitializeBasicEffectParameters()
        {
            BasicEffect.TextureEnabled = true;
            BasicEffect.Texture = TileTexture;
            BlendState = BlendState.AlphaBlend;
        }

        void CreateTexturePositions()
        {
            TexturePositions = new Vector2[2, 2];
            Vertices = new VertexPositionTexture[NumVertices];
            TexturePositions[0, 0] = new Vector2(0, 0);
            TexturePositions[1, 0] = new Vector2(1, 0);
            TexturePositions[0, 1] = new Vector2(0, 1);
            TexturePositions[1, 1] = new Vector2(1, 1);
        }

        void CreateVerticesPositions()
        {
            int vertexIndex;
            for (int i = 0; i < NumVertices; i += 6)
            {
                vertexIndex = i / 6;
                VerticesPositions[i + 1] = new Vector3(FirstVertices[vertexIndex].X, YPosition, FirstVertices[vertexIndex].Y);
                VerticesPositions[i] = new Vector3(SecondVertices[vertexIndex].X, YPosition + Heights[vertexIndex], SecondVertices[vertexIndex].Y);
                VerticesPositions[i + 2] = new Vector3(FirstVertices[vertexIndex].X, YPosition + Heights[vertexIndex], FirstVertices[vertexIndex].Y);
                VerticesPositions[i + 5] = new Vector3(FirstVertices[vertexIndex].X, YPosition, FirstVertices[vertexIndex].Y);
                VerticesPositions[i + 4] = new Vector3(SecondVertices[vertexIndex].X, YPosition, SecondVertices[vertexIndex].Y);
                VerticesPositions[i + 3] = new Vector3(SecondVertices[vertexIndex].X, YPosition + Heights[vertexIndex], SecondVertices[vertexIndex].Y);
            }
        }

        float FindMaxX()
        {
            return FirstVertices.Select(v => v.X).Max();
        }

        float FindMaxY()
        {
            return FirstVertices.Select(v => v.Y).Max();
        }

        void InitializeWallsData()
        {
            string line;
            string[] vectors = new string[2];
            Vector2 u2, vector;
            Vector3 u, v;
            StreamReader reader = new StreamReader(DataFileName);
            char[] separator = new char[1] { ';' };
            FirstVertices = new List<Vector2>();
            SecondVertices = new List<Vector2>();
            PlaneEquations = new List<Vector3>();
            Magnitudes = new List<float>();
            PlanePoints = new List<Vector3>();
            Heights = new List<float>();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                vectors = line.Split(separator);
                if (vectors[0] != "#")
                {
                    FirstVertices.Add(Vector2Parse(vectors[0]));
                    vector = Vector2Parse(vectors[1]);
                    SecondVertices.Add(vector);

                    Heights.Add(float.Parse(vectors[2]));

                    PlanePoints.Add(new Vector3(vector.X, YPosition, vector.Y));
                    u2 = SecondVertices.Last() - FirstVertices.Last();
                    u = new Vector3(u2.X, 0, u2.Y);
                    v = new Vector3(0, Heights.Last(), 0);
                    PlaneEquations.Add(Vector3.Cross(u, v));
                    Magnitudes.Add(PlaneEquations.Last().Length());
                }
            }
            reader.Close();
            NumTriangles = FirstVertices.Count * NUM_TRIANGLES_PER_TILE;
            NumVertices = NumTriangles * NUM_VERTICES_PER_TRIANGLE;
        }

        Vector2 Vector2Parse(string parse)
        {
            int startInd = parse.IndexOf("X:") + 2;
            float aXPosition = float.Parse(parse.Substring(startInd, parse.IndexOf(" Y") - startInd));
            startInd = parse.IndexOf("Y:") + 2;
            float aYPosition = float.Parse(parse.Substring(startInd, parse.IndexOf("}") - startInd));
            return new Vector2(aXPosition, aYPosition);
        }

        void InitializeVertices()
        {
            int v;
            for (int i = 0; i < NumVertices; ++i)
            {
                v = i % 6;
                Vertices[i] = new VertexPositionTexture(VerticesPositions[i], v == 0 ? TexturePositions[1, 0] : v == 1 ? TexturePositions[0, 1] : v == 2 ? TexturePositions[0, 0] : v == 3 ? TexturePositions[1, 0] : v == 4 ? TexturePositions[1, 1] : TexturePositions[0, 1]);
            }
        }

        protected override void LoadContent()
        {
            BasicEffect = new BasicEffect(GraphicsDevice);
            Camera = Game.Services.GetService(typeof(Caméra)) as Caméra;
            base.LoadContent();
        }

        void CreateWorld()
        {
            World = Matrix.Identity * Matrix.CreateScale(1) * Matrix.CreateFromYawPitchRoll(0, 0, 0) * Matrix.CreateTranslation(Vector3.Zero);
        }

        Matrix GetWorld()
        {
            return World;
        }

        const int MAX_DISTANCE = 1;

        public bool CheckForCollisions(Vector3 PositionObtained)
        {
            Vector3 Position = new Vector3(PositionObtained.X, PositionObtained.Y - YPosition, PositionObtained.Z);
            Vector3 AP;
            bool result = false;
            int i;
            float wallDistance;

            for (i = 0; i < PlaneEquations.Count && !result; ++i)
            {
                AP = Position - PlanePoints[i];
                wallDistance = Vector2.Distance(FirstVertices[i], SecondVertices[i]);
                result = Math.Abs(Vector3.Dot(AP, PlaneEquations[i])) / Magnitudes[i] < MAX_DISTANCE && (Position - new Vector3(FirstVertices[i].X, Position.Y, FirstVertices[i].Y)).Length() < wallDistance && (Position - new Vector3(SecondVertices[i].X, Position.Y, SecondVertices[i].Y)).Length() < wallDistance;
            }
            //CreateNewDirection(result, i, Direction, ref newDirection);

            return result;
        }

        void CreateNewDirection(bool result, int i, Vector3 Direction, ref Vector3 newDirection)
        {
            if (result)
            {
                Vector2 wall = SecondVertices[i] - FirstVertices[i], direction = new Vector2(Direction.X, Direction.Z), projection = ((Vector2.Dot(direction, wall) / wall.LengthSquared()) * wall) * wall;
                newDirection = Direction + PlaneEquations[i] / (PlaneEquations[i].Length() * 1);//new Vector3(projection.X, 0, projection.Y);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            BasicEffect.World = GetWorld();
            BasicEffect.View = Camera.Vue;
            BasicEffect.Projection = Camera.Projection;
            foreach (EffectPass passeEffet in BasicEffect.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Vertices, 0, NumTriangles);
            }
        }
    }
}
