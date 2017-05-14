using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public class SphèreTexturée : PrimitiveDeBaseAnimée //Plan
   {
      //Initialement gérées par le constructeur
      readonly float Rayon;
      readonly int NbColonnes;
      readonly int NbLignes;
      readonly string NomTexture;

      readonly Vector3 Origine;

      //Initialement gérées par des fonctions appellées par base.Initialize()
      Vector3[,] PtsSommets { get; set; }
      Vector2[,] PtsTexture { get; set; }
      VertexPositionTexture[] Sommets { get; set; }
      BasicEffect EffetDeBase { get; set; }
      //BlendState GestionAlpha { get; set; }

      int NbTrianglesParStrip { get; set; }

      //Initialement gérées par LoadContent()
      RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
      Texture2D TextureSphère { get; set; }

      public SphèreTexturée(Game jeu, float homothétieInitiale, Vector3 rotationInitiale,
                            Vector3 positionInitiale, float rayon, Vector2 charpente,
                            string nomTexture, float intervalleMAJ)
          : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
      {
         Rayon = rayon;
         NbColonnes = (int)charpente.X;
         NbLignes = (int)charpente.Y;
         NomTexture = nomTexture;

         Origine = new Vector3(0, 0, 0);
      }

      public override void Initialize()
      {
         NbTrianglesParStrip = NbColonnes * 2;
         NbSommets = (NbTrianglesParStrip + 2) * NbLignes;

         AllouerTableaux();
         base.Initialize();
         InitialiserParamètresEffetDeBase();
      }

      void AllouerTableaux()
      {
         PtsSommets = new Vector3[NbColonnes + 1, NbLignes + 1];
         PtsTexture = new Vector2[NbColonnes + 1, NbLignes + 1];
         Sommets = new VertexPositionTexture[NbSommets];
      }

      void InitialiserParamètresEffetDeBase()
      {
         EffetDeBase = new BasicEffect(GraphicsDevice);
         EffetDeBase.TextureEnabled = true;
         EffetDeBase.Texture = TextureSphère;
      }

      protected override void InitialiserSommets()
      {
         AffecterPtsSommets();
         AffecterPtsTexture();
         AffecterSommets();
      }

      void AffecterPtsSommets()
      {
         float angle = (float)(2 * Math.PI) / NbColonnes;
         float phi = 0;
         float theta = 0;

         for (int j = 0; j < PtsSommets.GetLength(0); ++j)
         {
            for (int i = 0; i < PtsSommets.GetLength(1); ++i)
            {
               PtsSommets[i, j] = new Vector3(Origine.X + Rayon * (float)(Math.Sin(phi) * Math.Cos(theta)),
                                              Origine.Z + Rayon * (float)(Math.Cos(phi)),
                                              Origine.Y + Rayon * (float)(Math.Sin(phi) * Math.Sin(theta)));
               theta += angle;
            }
            phi += (float)Math.PI / NbLignes;
         }
      }

      void AffecterPtsTexture()
      {
         for (int i = 0; i < PtsTexture.GetLength(0); ++i)
         {
            for (int j = 0; j < PtsTexture.GetLength(1); ++j)
            {
               PtsTexture[i, j] = new Vector2(i / (float)NbColonnes, -j / (float)NbLignes);
            }
         }
      }

      void AffecterSommets()
      {
         int NoSommet = -1;
         for (int j = 0; j < NbLignes; ++j)
         {
            for (int i = 0; i < NbColonnes + 1; ++i)
            {
               Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j], PtsTexture[i, j]);
               Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[i, j + 1]);
            }
         }
      }

      protected override void LoadContent()
      {
         GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
         TextureSphère = GestionnaireDeTextures.Find(NomTexture);
         base.LoadContent();
      }

      public override void Draw(GameTime gameTime)
      {
         EffetDeBase.World = GetMonde();
         EffetDeBase.View = CaméraJeu.Vue;
         EffetDeBase.Projection = CaméraJeu.Projection;
         foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
         {
            passeEffet.Apply();
            for (int i = 0; i < NbLignes; ++i)
            {
               DessinerTriangleStrip(i);
            }
         }
      }

      void DessinerTriangleStrip(int noStrip)
      {
         int vertexOffset = (noStrip * NbSommets) / NbLignes;
         GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Sommets, vertexOffset, NbTrianglesParStrip);
      }

   }
}
