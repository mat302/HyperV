/*
Water.cs
--------

By Mathieu Godin

Role : Used to create a flat water surface.

Created : 3/28/17
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
    public class Water : PrimitiveDeBase
    {
        public Vector3 Position { get; private set; }
        float IntervalleMAJ { get; set; }
        protected InputManager GestionInput { get; private set; }
        float TempsÉcouléDepuisMAJ { get; set; }

        const int NB_TRIANGLES = 2;
        protected Vector3[,] PtsSommets { get; private set; }
        Vector3 Origine { get; set; }
        Vector2 Delta { get; set; }
        protected BasicEffect EffetDeBase { get; private set; }

        VertexPositionColor[] Sommets { get; set; }
        //VertexPositionTexture[] Sommets { get; set; }
        BlendState GestionAlpha { get; set; }

        //Vector2[,] PtsTexture { get; set; }
        Afficheur3D Display3D { get; set; }
        Color Color { get; set; }

        public Vector3 GetPositionAvecHauteur(Vector3 position, int hauteur)
        {
            Vector3 positionAvecHauteur;
            if (EstEntre(position.Z, PtsSommets[0, 0].Z, PtsSommets[PtsSommets.GetLength(0) - 1, PtsSommets.GetLength(1) - 1].Z) &&
                EstEntre(position.X, PtsSommets[0, 0].X, PtsSommets[PtsSommets.GetLength(0) - 1, PtsSommets.GetLength(1) - 1].X))
            {
                positionAvecHauteur = new Vector3(position.X, PtsSommets[0, 0].Y + hauteur, position.Z);
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

        public Water(Game jeu, float homothétieInitiale, Vector3 rotationInitiale,
                     Vector3 positionInitiale, Vector2 étendue,
                     float intervalleMAJ)
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale)
        {
            IntervalleMAJ = intervalleMAJ;
            Delta = new Vector2(étendue.X, étendue.Y);
            Origine = new Vector3(-Delta.X / 2, 0, -Delta.Y / 2); //pour centrer la primitive au point (0,0,0)
            Color = new Color(20, 50, 250, 50);
        }

        public override void Initialize()
        {
            NbSommets = NB_TRIANGLES + 2;
            PtsSommets = new Vector3[2, 2];
            CréerTableauPoints();
            CréerTableauSommets();
            Position = PositionInitiale;
            Display3D = Game.Services.GetService(typeof(Afficheur3D)) as Afficheur3D;
            base.Initialize();
        }

        private void CréerTableauPoints()
        {
            PtsSommets[0, 0] = new Vector3(Origine.X, Origine.Y, Origine.Z);
            PtsSommets[1, 0] = new Vector3(Origine.X - Delta.X, Origine.Y, Origine.Z);
            PtsSommets[0, 1] = new Vector3(Origine.X, Origine.Y, Origine.Z + Delta.Y);
            PtsSommets[1, 1] = new Vector3(Origine.X - Delta.X, Origine.Y, Origine.Z + Delta.Y);
        }

        protected void CréerTableauSommets()
        {
            //PtsTexture = new Vector2[2, 2];
            Sommets = new VertexPositionColor[NbSommets];
            //Sommets = new VertexPositionTexture[NbSommets];
            //CréerTableauPointsTexture();
        }

        //private void CréerTableauPointsTexture()
        //{
        //    PtsTexture[0, 0] = new Vector2(0, 1);
        //    PtsTexture[1, 0] = new Vector2(1, 1);
        //    PtsTexture[0, 1] = new Vector2(0, 0);
        //    PtsTexture[1, 1] = new Vector2(1, 0);
        //}

        protected override void LoadContent()
        {
            EffetDeBase = new BasicEffect(GraphicsDevice);
            InitialiserParamètresEffetDeBase();
            base.LoadContent();
        }

        protected void InitialiserParamètresEffetDeBase()
        {
            EffetDeBase.VertexColorEnabled = true;
            //EffetDeBase.TextureEnabled = true;
            //EffetDeBase.Texture = TextureTuile;
            //GestionAlpha = BlendState.AlphaBlend; // Attention à ceci...
        }

        protected override void InitialiserSommets() // Est appelée par base.Initialize()
        {
            int NoSommet = -1;
            for (int j = 0; j < 1; ++j)
            {
                for (int i = 0; i < 2; ++i)
                {
                    Sommets[++NoSommet] = new VertexPositionColor(PtsSommets[i, j], Color);
                    Sommets[++NoSommet] = new VertexPositionColor(PtsSommets[i, j + 1], Color);
                    //Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j], PtsTexture[i, j]);
                    //Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[i, j + 1]);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //BlendState oldBlendState = GraphicsDevice.BlendState; // ... et à cela!
            //GraphicsDevice.BlendState = GestionAlpha;
            //EffetDeBase.World = GetMonde();
            //EffetDeBase.View = CaméraJeu.Vue;
            //EffetDeBase.Projection = CaméraJeu.Projection;
            //foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            //{
            //    passeEffet.Apply();
            //    DessinerTriangleStrip();
            //}
            //GraphicsDevice.BlendState = oldBlendState;
            RasterizerState s;// = Display3D.JeuRasterizerState;
            s = new RasterizerState();
            s.CullMode = CullMode.None;
            s.FillMode = FillMode.Solid;
            Game.GraphicsDevice.RasterizerState = s;

            EffetDeBase.World = GetMonde();
            EffetDeBase.View = CaméraJeu.Vue;
            EffetDeBase.Projection = CaméraJeu.Projection;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                DessinerTriangleStrip();
            }

            Game.GraphicsDevice.RasterizerState = Display3D.JeuRasterizerState;
        }

        protected void DessinerTriangleStrip()
        {
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets, 0, NB_TRIANGLES);
            //GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Sommets, 0, NB_TRIANGLES);
        }

        public Vector3 GetPositionWithHeight(Vector3 position, int hauteur)
        {
            Vector3 positionAvecHauteur;
            if (EstEntre(position.Z, PtsSommets[0, 0].Z, PtsSommets[PtsSommets.GetLength(0) - 1, PtsSommets.GetLength(1) - 1].Z) &&
                EstEntre(position.X, PtsSommets[0, 0].X, PtsSommets[PtsSommets.GetLength(0) - 1, PtsSommets.GetLength(1) - 1].X))
            {
                positionAvecHauteur = new Vector3(position.X, PtsSommets[0, 0].Y + hauteur, position.Z);
            }
            else
            {
                positionAvecHauteur = position;
            }
            return positionAvecHauteur;
        }
    }
}
