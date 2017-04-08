/*
Grass.cs
--------

By Mathieu Godin

Role : Used to create a flat grass surface

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
    public class Grass : PrimitiveDeBase
    {
        const int NUM_TRIANGLES_PER_TILE = 2, NUM_VERTICES_PER_TRIANGLE = 3;

        Vector3 Position { get; set; }
        float IntervalleMAJ { get; set; }
        protected InputManager GestionInput { get; private set; }
        float TempsÉcouléDepuisMAJ { get; set; }

        const int NB_TRIANGLES = 2;
        protected Vector3[,] VerticesPositions { get; private set; }
        Vector3 Origin { get; set; }
        Vector2 Delta { get; set; }
        protected BasicEffect BasicEffect { get; private set; }

        //VertexPositionColor[] Sommets { get; set; }
        RessourcesManager<Texture2D> TextureManager;
        Texture2D TileTexture;
        VertexPositionTexture[] Vertices { get; set; }
        BlendState GestionAlpha { get; set; }

        Vector2[,] TileTexturePositions { get; set; }
        string NomTextureTuile { get; set; }

        int NumRows { get; set; }
        int NumColumns { get; set; }

        public Vector3 GetPositionAvecHauteur(Vector3 position, int hauteur)
        {
            Vector3 positionAvecHauteur;
            if(EstEntre(position.Z, VerticesPositions[0,0].Z, VerticesPositions[VerticesPositions.GetLength(0)-1, VerticesPositions.GetLength(1)-1].Z) &&
                EstEntre(position.X, VerticesPositions[0, 0].X, VerticesPositions[VerticesPositions.GetLength(0) - 1, VerticesPositions.GetLength(1) - 1].X))
            {
                positionAvecHauteur = new Vector3(position.X, VerticesPositions[0, 0].Y + hauteur, position.Z);
            }
            else
            {
                positionAvecHauteur = position;
            }
            return positionAvecHauteur;
        }

        private bool EstEntre(float valeur, float borneA, float borneB)
        {
            return (valeur >= borneA && valeur <= borneB || valeur <= borneA && valeur >= borneB);
        }

        public Grass(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomTextureTuile, Vector2 numbers, float intervalleMAJ) : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale)
        {
            NomTextureTuile = nomTextureTuile;
            IntervalleMAJ = intervalleMAJ;
            Delta = new Vector2(étendue.X, étendue.Y);
            //Origine = new Vector3(-Delta.X / 2, 0, -Delta.Y / 2); //pour centrer la primitive au point (0,0,0)
            NumRows = (int)numbers.X;
            NumColumns = (int)numbers.Y;
        }

        public override void Initialize()
        {
            NbTriangles = NumRows * NumColumns * NUM_TRIANGLES_PER_TILE;
            NbSommets = NbTriangles * NUM_VERTICES_PER_TRIANGLE;
            Origin = new Vector3(0, 0, 0);
            VerticesPositions = new Vector3[NumRows * 2, NumColumns * 2];
            CreateVerticesPositions();
            TileTexturePositions = new Vector2[2, 2];
            Vertices = new VertexPositionTexture[NbSommets];
            CreateTexturePositions();
            Position = PositionInitiale;
            base.Initialize();
        }

        private void CreateVerticesPositions()
        {
            for (int i = 0; i < VerticesPositions.GetLength(0); i += 2)
            {
                for (int j = 0; j < VerticesPositions.GetLength(1); j += 2)
                {
                    VerticesPositions[i, j] = Origin + new Vector3(Delta.X * i - Delta.X, Origin.Y, Delta.Y * j - Delta.Y);
                    VerticesPositions[i + 1, j] = Origin + new Vector3(Delta.X * i + Delta.X, Origin.Y, Delta.Y * j - Delta.Y);
                    VerticesPositions[i, j + 1] = Origin + new Vector3(Delta.X * i - Delta.X, Origin.Y, Delta.Y * j + Delta.Y);
                    VerticesPositions[i + 1, j + 1] = Origin + new Vector3(Delta.X * i + Delta.X, Origin.Y, Delta.Y * j + Delta.Y);
                }
            }
            //VerticesPositions[0, 0] = new Vector3(Origin.X, Origin.Y, Origin.Z);
            //VerticesPositions[1, 0] = new Vector3(Origin.X - Delta.X, Origin.Y, Origin.Z);
            //VerticesPositions[0, 1] = new Vector3(Origin.X, Origin.Y, Origin.Z + Delta.Y);
            //VerticesPositions[1, 1] = new Vector3(Origin.X - Delta.X, Origin.Y, Origin.Z + Delta.Y);
        }

        private void CreateTexturePositions()
        {
            TileTexturePositions[0, 0] = new Vector2(0, 1);
            TileTexturePositions[1, 0] = new Vector2(1, 1);
            TileTexturePositions[0, 1] = new Vector2(0, 0);
            TileTexturePositions[1, 1] = new Vector2(1, 0);
        }

        protected override void LoadContent()
        {
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            TileTexture = TextureManager.Find(NomTextureTuile);
            BasicEffect = new BasicEffect(GraphicsDevice);
            InitializeBasicEffectParameters();
            base.LoadContent();
        }

        protected void InitializeBasicEffectParameters()
        {
            //EffetDeBase.VertexColorEnabled = true;
            BasicEffect.TextureEnabled = true;
            BasicEffect.Texture = TileTexture;
            GestionAlpha = BlendState.AlphaBlend; // Attention à ceci...
        }

        protected override void InitialiserSommets() // Est appelée par base.Initialize()
        {
            int cpt = -1, maxJ = VerticesPositions.GetLength(1), maxI = VerticesPositions.GetLength(0);
            for (int j = 0; j < NumColumns * 2 ; j += 2)
            {
                for (int i = 0; i < NumRows * 2 ; i += 2)
                {
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j], TileTexturePositions[0, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j], TileTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j + 1 == maxJ ? j : j + 1], TileTexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j], TileTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j + 1 == maxJ ? j : j + 1], TileTexturePositions[1, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j + 1 == maxJ ? j : j + 1], TileTexturePositions[1, 0]);
                    //Sommets[++NoSommet] = new VertexPositionColor(PtsSommets[i, j], Color.LawnGreen);
                    //Sommets[++NoSommet] = new VertexPositionColor(PtsSommets[i, j + 1], Color.LawnGreen);
                    //Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j], TileTexturePositions[i, j]);
                    //Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j + 1], TileTexturePositions[i, j + 1]);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //BlendState oldBlendState = GraphicsDevice.BlendState; // ... et à cela!
            //GraphicsDevice.BlendState = GestionAlpha;
            //BasicEffect.World = GetMonde();
            //BasicEffect.View = CaméraJeu.Vue;
            //BasicEffect.Projection = CaméraJeu.Projection;
            //foreach (EffectPass passeEffet in BasicEffect.CurrentTechnique.Passes)
            //{
            //    passeEffet.Apply();
            //    DessinerTriangleStrip();
            //}
            //GraphicsDevice.BlendState = oldBlendState;
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

        //protected void DessinerTriangleStrip()
        //{
        //    //GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets, 0, NB_TRIANGLES);
        //    GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Vertices, 0, NB_TRIANGLES);
        //}

        public Vector3 GetPositionWithHeight(Vector3 position, int hauteur)
        {
            Vector3 positionAvecHauteur;
            if (EstEntre(position.Z, VerticesPositions[0, 0].Z, VerticesPositions[VerticesPositions.GetLength(0) - 1, VerticesPositions.GetLength(1) - 1].Z) &&
                EstEntre(position.X, VerticesPositions[0, 0].X, VerticesPositions[VerticesPositions.GetLength(0) - 1, VerticesPositions.GetLength(1) - 1].X))
            {
                positionAvecHauteur = new Vector3(position.X, VerticesPositions[0, 0].Y + hauteur, position.Z);
            }
            else
            {
                positionAvecHauteur = position;
            }
            return positionAvecHauteur;
        }
    }
}
