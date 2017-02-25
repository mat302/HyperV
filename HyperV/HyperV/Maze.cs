/*
Maze.cs
-------

By Mathieu Godin

Role : Used to create a maze from its
       map in an image format

Created : 2/13/17
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
    public class Maze : PrimitiveDeBase
    {
        const float MAX_COLOR = 255f;

        Vector3 Position { get; set; }
        float IntervalleMAJ { get; set; }
        protected InputManager GestionInput { get; private set; }
        float TempsÉcouléDepuisMAJ { get; set; }

        const int NUM_TRIANGLES_PER_TILE = 2, NUM_VERTICES_PER_TRIANGLE = 3;
        protected Vector3[,] VerticesPositions { get; private set; }
        Vector3 Origin { get; set; }
        Vector2 Delta { get; set; }
        protected BasicEffect BasicEffect { get; private set; }

        //VertexPositionColor[] Vertices { get; set; }
        RessourcesManager<Texture2D> TextureManager;
        Texture2D TileTexture { get; set; }
        Texture2D WallTexture { get; set; }
        Texture2D MazeMap { get; set; }
        VertexPositionTexture[] Vertices { get; set; }
        BlendState BlendState { get; set; }

        Vector2[,] TileTexturePositions { get; set; }
        Vector2[,] WallTexturePositions { get; set; }
        string TileTextureName { get; set; }
        string MazeImageName { get; set; }
        int NumRows { get; set; }
        int NumColumns { get; set; }
        int NumTexels { get; set; }
        Color[] TextureData { get; set; }
        Vector3 Range { get; set; }
        bool[,] Collisions { get; set; }

        public Maze(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector3 range, string tileTextureName, float intervalleMAJ, string mazeImageName) : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale)
        {
            Range = range;
            IntervalleMAJ = intervalleMAJ;
            TileTextureName = tileTextureName;
            MazeImageName = mazeImageName;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            TileTexture = TextureManager.Find(TileTextureName);
            InitializeMazeData();
            Origin = new Vector3(/*-Range.X / 2, 0, -Range.Z / 2*/0, 0, 0);
            VerticesPositions = new Vector3[MazeMap.Width * 2 * 3, MazeMap.Height * 2 * 3];
            Collisions = new bool[MazeMap.Width, MazeMap.Height];
            CreateCollisions();
            CreateVerticesPositions();
            CreateTexturePositions();
            Position = PositionInitiale;
            base.Initialize();
            InitializeBasicEffectParameters();
        }

        void CreateTexturePositions()
        {
            TileTexturePositions = new Vector2[2, 2];
            WallTexturePositions = new Vector2[2, 2];
            Vertices = new VertexPositionTexture[NbSommets];
            TileTexturePositions[0, 0] = new Vector2(0, 0);
            TileTexturePositions[1, 0] = new Vector2(1, 0);
            TileTexturePositions[0, 1] = new Vector2(0, 0.5f);
            TileTexturePositions[1, 1] = new Vector2(1, 0.5f);

            WallTexturePositions[0, 0] = new Vector2(0, 257 / 512f);
            WallTexturePositions[1, 0] = new Vector2(1, 257 / 512f);
            WallTexturePositions[0, 1] = new Vector2(0, 1);
            WallTexturePositions[1, 1] = new Vector2(1, 1);
        }

        void CreateCollisions()
        {
            StreamWriter s = new StreamWriter("../../test.txt");
            for (int i = 0; i < Collisions.GetLength(0); ++i)
            {
                for (int j = 0; j < Collisions.GetLength(1); ++j)
                {
                    Collisions[i, j] = TextureData[j * MazeMap.Height + i].B == 0;
                    s.Write(Collisions[i, j] + " ");
                }
                s.WriteLine();
            }
            s.Close();
        }

        void CreateVerticesPositions()
        {
            //Delta = new Vector2(Range.X / NumRows, Range.Z / NumColumns);
            bool collision;
            Delta = new Vector2(5f / 3f, 5f / 3f);
            for (int i = 0; i < VerticesPositions.GetLength(0); i += 6)
            {
                for (int j = 0; j < VerticesPositions.GetLength(1); j += 6)
                {
                    collision = TextureData[j / 6 * MazeMap.Height + i / 6].B == 0;
                    VerticesPositions[i, j] = Origin + new Vector3(Delta.X * i - 5, collision ? 10 : 0, Delta.Y * j - 5);
                    VerticesPositions[i + 1, j] = Origin + new Vector3(Delta.X * i + 5, collision ? 10 : 0, Delta.Y * j - 5);
                    VerticesPositions[i, j + 1] = Origin + new Vector3(Delta.X * i - 5, collision ? 10 : 0, Delta.Y * j + 5);
                    VerticesPositions[i + 1, j + 1] = Origin + new Vector3(Delta.X * i + 5, collision ? 10 : 0, Delta.Y * j + 5);

                    VerticesPositions[i + 2, j] = Origin + new Vector3(Delta.X * i + 5, VerticesPositions[i, j].Y, Delta.Y * j - 5);
                    VerticesPositions[i + 3, j] = Origin + new Vector3(Delta.X * i + 5, VerticesPositions[i + 1, j].Y - 10, Delta.Y * j - 5);
                    VerticesPositions[i + 2, j + 1] = Origin + new Vector3(Delta.X * i + 5, VerticesPositions[i, j + 1].Y, Delta.Y * j + 5);
                    VerticesPositions[i + 3, j + 1] = Origin + new Vector3(Delta.X * i + 5, VerticesPositions[i + 1, j + 1].Y - 10, Delta.Y * j + 5);

                    VerticesPositions[i + 4, j] = Origin + new Vector3(Delta.X * i - 5, VerticesPositions[i, j].Y, Delta.Y * j + 5);
                    VerticesPositions[i + 5, j] = Origin + new Vector3(Delta.X * i - 5, VerticesPositions[i + 1, j].Y - 10, Delta.Y * j + 5);
                    VerticesPositions[i + 4, j + 1] = Origin + new Vector3(Delta.X * i - 5, VerticesPositions[i, j + 1].Y, Delta.Y * j - 5);
                    VerticesPositions[i + 5, j + 1] = Origin + new Vector3(Delta.X * i - 5, VerticesPositions[i + 1, j + 1].Y - 10, Delta.Y * j - 5);

                    VerticesPositions[i, j + 2] = Origin + new Vector3(Delta.X * i + 5, VerticesPositions[i, j].Y, Delta.Y * j + 5);
                    VerticesPositions[i, j + 3] = Origin + new Vector3(Delta.X * i + 5, VerticesPositions[i + 1, j].Y - 10, Delta.Y * j + 5);
                    VerticesPositions[i + 1, j + 2] = Origin + new Vector3(Delta.X * i - 5, VerticesPositions[i, j + 1].Y, Delta.Y * j + 5);
                    VerticesPositions[i + 1, j + 3] = Origin + new Vector3(Delta.X * i - 5, VerticesPositions[i + 1, j + 1].Y - 10, Delta.Y * j + 5);

                    VerticesPositions[i, j + 4] = Origin + new Vector3(Delta.X * i - 5, VerticesPositions[i, j].Y, Delta.Y * j - 5);
                    VerticesPositions[i, j + 5] = Origin + new Vector3(Delta.X * i - 5, VerticesPositions[i + 1, j].Y - 10, Delta.Y * j - 5);
                    VerticesPositions[i + 1, j + 4] = Origin + new Vector3(Delta.X * i + 5, VerticesPositions[i, j + 1].Y, Delta.Y * j - 5);
                    VerticesPositions[i + 1, j + 5] = Origin + new Vector3(Delta.X * i + 5, VerticesPositions[i + 1, j + 1].Y - 10, Delta.Y * j - 5);
                }
            }
        }
        
        protected override void LoadContent()
        {
            BasicEffect = new BasicEffect(GraphicsDevice);
            base.LoadContent();
        }

        protected void InitializeBasicEffectParameters()
        {
            //BasicEffect.VertexColorEnabled = true;
            BasicEffect.TextureEnabled = true;
            BasicEffect.Texture = TileTexture;
            BlendState = BlendState.AlphaBlend;
        }

        protected override void InitialiserSommets() // Est appelée par base.Initialize()
        {
            int cpt = -1, maxJ = VerticesPositions.GetLength(1), maxI = VerticesPositions.GetLength(0);
            for (int j = 0; j < NumColumns * 6; j += 6)
            {
                for (int i = 0; i < NumRows * 6; i += 6)
                {
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j], TileTexturePositions[0, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j], TileTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j + 1 == maxJ ? j : j + 1], TileTexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j], TileTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j + 1 == maxJ ? j : j + 1], TileTexturePositions[1, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j + 1 == maxJ ? j : j + 1], TileTexturePositions[1, 0]);

                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 2 == maxI ? i : i + 2, j], WallTexturePositions[0, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 3 == maxI ? i : i + 3, j], WallTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 2 == maxI ? i : i + 2, j + 1 == maxJ ? j : j + 1], WallTexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 3 == maxI ? i : i + 3, j], TileTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 3 == maxI ? i : i + 3, j + 1 == maxJ ? j : j + 1], WallTexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 2 == maxI ? i : i + 2, j + 1 == maxJ ? j : j + 1], WallTexturePositions[1, 1]);

                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 4 == maxI ? i : i + 4, j], WallTexturePositions[0, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 5 == maxI ? i : i + 5, j], WallTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 4 == maxI ? i : i + 4, j + 1 == maxJ ? j : j + 1], WallTexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 5 == maxI ? i : i + 5, j], TileTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 5 == maxI ? i : i + 5, j + 1 == maxJ ? j : j + 1], WallTexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 4 == maxI ? i : i + 4, j + 1 == maxJ ? j : j + 1], WallTexturePositions[1, 1]);

                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i == maxI ? i : i, j + 2 == maxJ ? j : j + 2], WallTexturePositions[0, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i == maxI ? i : i, j + 3 == maxJ ? j : j + 3], WallTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j + 2 == maxJ ? j : j + 2], WallTexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i == maxI ? i : i, j + 3 == maxJ ? j : j + 3], WallTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j + 3 == maxJ ? j : j + 3], WallTexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j + 2 == maxJ ? j : j + 2], WallTexturePositions[1, 1]);

                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i == maxI ? i : i, j + 4 == maxJ ? j : j + 4], WallTexturePositions[0, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i == maxI ? i : i, j + 5 == maxJ ? j : j + 5], WallTexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j + 4 == maxJ ? j : j + 4], WallTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i == maxI ? i : i, j + 5 == maxJ ? j : j + 5], WallTexturePositions[1, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j + 5 == maxJ ? j : j + 5], WallTexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j + 4 == maxJ ? j : j + 4], WallTexturePositions[0, 1]);
                }
            }
        }

        void InitializeMazeData()
        {
            MazeMap = TextureManager.Find(MazeImageName);
            NumTexels = MazeMap.Width * MazeMap.Height;
            TextureData = new Color[NumTexels];
            MazeMap.GetData<Color>(TextureData);
            NumRows = MazeMap.Width;
            NumColumns = MazeMap.Height;
            NbTriangles = NumRows * NumColumns * NUM_TRIANGLES_PER_TILE * 5;
            NbSommets = NbTriangles * NUM_VERTICES_PER_TRIANGLE;
        }

        public override void Draw(GameTime gameTime)
        {
            BasicEffect.World = GetMonde();
            BasicEffect.View = CaméraJeu.Vue;
            BasicEffect.Projection = CaméraJeu.Projection;
            foreach (EffectPass passeEffet in BasicEffect.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Vertices, 0, NbTriangles);
            }
            //GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Vertices, 0, NbTriangles);
        }

        public Vector3 GetPositionWithHeight(Vector3 position, int height)
        {
            Vector3 positionAvecHauteur;
            if (IsBetween(position.Z, VerticesPositions[0, 0].Z, VerticesPositions[VerticesPositions.GetLength(0) - 1, VerticesPositions.GetLength(1) - 1].Z) && IsBetween(position.X, VerticesPositions[0, 0].X, VerticesPositions[VerticesPositions.GetLength(0) - 1, VerticesPositions.GetLength(1) - 1].X))
            {
                positionAvecHauteur = new Vector3(position.X, VerticesPositions[0, 0].Y + height, position.Z);
            }
            else
            {
                positionAvecHauteur = position;
            }
            return positionAvecHauteur;
        }

        bool IsBetween(float valeur, float borneA, float borneB)
        {
            return (valeur >= borneA && valeur <= borneB || valeur <= borneA && valeur >= borneB);
        }

        public bool CheckForCollisions(Vector3 position)
        {
            Game.Window.Title = position.ToString();
            return Collisions[(int)((position.X + 5) / 10f), (int)((position.Z + 5) / 10f)]; //|| Collisions[(int)((position.X + 3) / 10f), (int)((position.Z + 5) / 10f)] || Collisions[(int)((position.X + 5) / 10f), (int)((position.Z + 3) / 10f)] || Collisions[(int)((position.X + 7) / 10f), (int)((position.Z + 5) / 10f)] || Collisions[(int)((position.X + 5) / 10f), (int)((position.Z + 7) / 10f)] || Collisions[(int)((position.X + 3) / 10f), (int)((position.Z + 3) / 10f)] || Collisions[(int)((position.X + 7) / 10f), (int)((position.Z + 7) / 10f)];
        }
    }
}
