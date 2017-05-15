using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public class PlanTexturé : Plan
   {
        //Initialement gérées par le constructeur
        readonly string NomTexture;

        //Initialement gérées par LoadContent()
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        Texture2D Texture { get; set; }

        //Initialement gérées par des fonctions appellées par base.Initialize()
        Vector2[,] PtsTexture { get; set; }
        protected VertexPositionTexture[] Sommets { get; set; }
        BlendState GestionAlpha { get; set; }

        public PlanTexturé(Game jeu, float homothétieInitiale, Vector3 rotationInitiale,
                          Vector3 positionInitiale, Vector2 étendue, Vector2 charpente,
                          string nomTexture, float intervalleMAJ)
            :base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, charpente, intervalleMAJ)
        {
            NomTexture = nomTexture;
        }

        protected override void CréerTableauSommets()
        {
            PtsTexture = new Vector2[NbColonnes + 1, NbRangées + 1];
            Sommets = new VertexPositionTexture[NbSommets];
            CréerTableauPointsTexture();
        }

        private void CréerTableauPointsTexture()
        {
            for (int i = 0; i < PtsTexture.GetLength(0); ++i)
            {
                for(int j = 0; j < PtsTexture.GetLength(1); ++j)
                {
                    PtsTexture[i, j] = new Vector2(i / (float)NbColonnes, -j / (float)NbRangées);
                }
            }
        }

        protected override void InitialiserParamètresEffetDeBase()
        {
            EffetDeBase.TextureEnabled = true;
            EffetDeBase.Texture = Texture;
            GestionAlpha = BlendState.AlphaBlend;
        }

        protected override void LoadContent()
        {
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Texture = GestionnaireDeTextures.Find(NomTexture);
            base.LoadContent();
        }

        protected override void InitialiserSommets()
        {
            int NoSommet = -1;
            for (int j = 0; j < NbRangées; ++j)
            {
                for (int i = 0; i < NbColonnes + 1; ++i)
                {
                    Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j], PtsTexture[i, j]);
                    Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[i, j + 1]);
                }
            }
        }

        protected override void DessinerTriangleStrip(int noStrip)
        {
            int vertexOffset = (noStrip * NbSommets) / NbRangées;
            GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Sommets, vertexOffset, NbTrianglesParStrip);
        }

    }
}
