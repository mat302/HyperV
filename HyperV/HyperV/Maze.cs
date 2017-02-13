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
        Texture2D TileTexture;
        Texture2D MazeMap;
        VertexPositionTexture[] Vertices { get; set; }
        BlendState BlendState { get; set; }

        Vector2[,] TexturePositions { get; set; }
        string TileTextureName { get; set; }
        string MazeImageName { get; set; }
        int NumRows { get; set; }
        int NumColumns { get; set; }
        int NumTexels { get; set; }
        Color[] TextureData { get; set; }
        Vector3 Range { get; set; }

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
            Origin = new Vector3(-Range.X / 2, 0, -Range.Z / 2);
            VerticesPositions = new Vector3[MazeMap.Width, MazeMap.Height];
            CreateVerticesPositions();
            CreateTexturePositions();
            Position = PositionInitiale;
            base.Initialize();
            InitializeBasicEffectParameters();
        }

        void CreateTexturePositions()
        {
            TexturePositions = new Vector2[2, 2];
            Vertices = new VertexPositionTexture[NbSommets];
            TexturePositions[0, 0] = new Vector2(0, 1);
            TexturePositions[1, 0] = new Vector2(1, 1);
            TexturePositions[0, 1] = new Vector2(0, 0);
            TexturePositions[1, 1] = new Vector2(1, 0);
        }

        void CreateVerticesPositions()
        {
            Delta = new Vector2(Range.X / NumRows, Range.Z / NumColumns);
            for (int i = 0; i < VerticesPositions.GetLength(0); ++i)
            {
                for (int j = 0; j < VerticesPositions.GetLength(1); ++j)
                {
                    VerticesPositions[i, j] = Origin + new Vector3(Delta.X * i, TextureData[i * VerticesPositions.GetLength(1) + j].B == 0 ? 0 : 5, Delta.Y * j);
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
            int cpt = -1;
            for (int j = 0; j < NumColumns; ++j)
            {
                for (int i = 0; i < NumRows; ++i)
                {
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j], TexturePositions[0, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == NumRows ? i : i + 1, j], TexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j + 1 == NumColumns ? j : j + 1], TexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == NumRows ? i : i + 1, j], TexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == NumRows ? i : i + 1, j + 1 == NumColumns ? j : j + 1], TexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j + 1 == NumColumns ? j : j + 1], TexturePositions[1, 1]);
                }
            }
        }

        void InitializeMazeData()
        {
            MazeMap = TextureManager.Find(MazeImageName);
            NumRows = MazeMap.Width;
            NumColumns = MazeMap.Height;
            NbTriangles = NumRows * NumColumns * NUM_TRIANGLES_PER_TILE;
            NbSommets = NbTriangles * NUM_VERTICES_PER_TRIANGLE;
            NumTexels = MazeMap.Width * MazeMap.Height;
            TextureData = new Color[NumTexels];
            MazeMap.GetData<Color>(TextureData);
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
    }
}
