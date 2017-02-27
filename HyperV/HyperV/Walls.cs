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
        bool[,] Collisions { get; set; }
        Vector2[,] TexturePositions { get; set; }
        VertexPositionTexture[] Vertices { get; set; }
        BlendState BlendState { get; set; }
        BasicEffect BasicEffect { get; set; }
        Matrix World { get; set; }
        Caméra Camera { get; set; }
        List<float> Heights { get; set; }

        public Walls(Game game, float interval, string tileTextureName, string dataFileName) : base(game)
        {
            Interval = interval;
            TileTextureName = tileTextureName;
            DataFileName = dataFileName;
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
            Collisions = new bool[(int)FindMaxX() * 11 / 10, (int)FindMaxY() * 11 / 10];
            CreateCollisions();
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
                VerticesPositions[i] = new Vector3(FirstVertices[vertexIndex].X, 0, FirstVertices[vertexIndex].Y);
                VerticesPositions[i + 1] = new Vector3(SecondVertices[vertexIndex].X, -Heights[vertexIndex], SecondVertices[vertexIndex].Y);
                VerticesPositions[i + 2] = new Vector3(FirstVertices[vertexIndex].X, -Heights[vertexIndex], FirstVertices[vertexIndex].Y);
                VerticesPositions[i + 3] = new Vector3(FirstVertices[vertexIndex].X, 0, FirstVertices[vertexIndex].Y);
                VerticesPositions[i + 4] = new Vector3(SecondVertices[vertexIndex].X, 0, SecondVertices[vertexIndex].Y);
                VerticesPositions[i + 5] = new Vector3(SecondVertices[vertexIndex].X, -Heights[vertexIndex], SecondVertices[vertexIndex].Y);
            }
        }

        void CreateCollisions()
        {
            for (int i = 0; i < Collisions.GetLength(0); ++i)
            {
                for (int j = 0; j < Collisions.GetLength(1); ++j)
                {
                    Collisions[i, j] = false;
                }
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
            int startInd;
            float aXPosition, aYPosition;
            StreamReader reader = new StreamReader(DataFileName);
            char[] separator = new char[1] { ';' };
            FirstVertices = new List<Vector2>();
            SecondVertices = new List<Vector2>();
            Heights = new List<float>();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                vectors = line.Split(separator);

                startInd = vectors[0].IndexOf("X:") + 2;
                aXPosition = float.Parse(vectors[0].Substring(startInd, vectors[0].IndexOf(" Y") - startInd));
                startInd = vectors[0].IndexOf("Y:") + 2;
                aYPosition = float.Parse(vectors[0].Substring(startInd, vectors[0].IndexOf("}") - startInd));
                FirstVertices.Add(new Vector2(aXPosition, aYPosition));

                startInd = vectors[1].IndexOf("X:") + 2;
                aXPosition = float.Parse(vectors[1].Substring(startInd, vectors[1].IndexOf(" Y") - startInd));
                startInd = vectors[1].IndexOf("Y:") + 2;
                aYPosition = float.Parse(vectors[1].Substring(startInd, vectors[1].IndexOf("}") - startInd));
                SecondVertices.Add(new Vector2(aXPosition, aYPosition));

                Heights.Add(float.Parse(vectors[2]));
            }
            reader.Close();
            NumTriangles = FirstVertices.Count * NUM_TRIANGLES_PER_TILE;
            NumVertices = NumTriangles * NUM_VERTICES_PER_TRIANGLE;
        }

        void InitializeVertices()
        {
            int v;
            for (int i = 0; i < NumVertices; ++i)
            {
                v = i % 6;
                Vertices[i] = new VertexPositionTexture(VerticesPositions[i], v == 0 ? TexturePositions[0, 0] : v == 1 ? TexturePositions[1, 1] : v == 2 ? TexturePositions[0, 1] : v == 3 ? TexturePositions[0, 0] : v == 4 ? TexturePositions[1, 0] : TexturePositions[1, 1]);
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
