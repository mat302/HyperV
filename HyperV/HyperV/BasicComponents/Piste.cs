using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
    public class Piste : PrimitiveDeBaseAnimée
   {
      Vector3[] PtsSommets { get; set; }
      BasicEffect EffetDeBase { get; set; }
      DataPiste DonnéesPiste { get; set; }
      TerrainAvecBase Terrain { get; set; }
      List<Vector2> BordureIntérieure { get; set; }
      List<Vector2> BordureExtérieure { get; set; }
      VertexPositionColor[] Sommets { get; set; }

      public Piste(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ) : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ) { }

      protected override void InitialiserSommets()
      {
         for (int i = 0; i < NbSommets; ++i)
         {
            Sommets[i] = new VertexPositionColor(PtsSommets[i], Color.Black);
         }
      }

      public override void Initialize()
      {
         base.Initialize();
         BordureIntérieure = DonnéesPiste.GetBordureIntérieure();
         BordureExtérieure = DonnéesPiste.GetBordureExtérieure();
         NbTriangles = (BordureIntérieure.Count - 1) * 2;
         NbSommets = BordureIntérieure.Count + BordureExtérieure.Count + 2;
         PtsSommets = new Vector3[NbSommets];
         Sommets = new VertexPositionColor[NbSommets];
         CréerTableauPoints();
         InitialiserSommets();
      }

        protected override void LoadContent()
        {
            DonnéesPiste = Game.Services.GetService(typeof(DataPiste)) as DataPiste;
            Terrain = Game.Services.GetService(typeof(TerrainAvecBase)) as TerrainAvecBase;
            EffetDeBase = new BasicEffect(GraphicsDevice);
            InitialiserParamètresEffetDeBase();
            base.LoadContent();
        }

        void InitialiserParamètresEffetDeBase()
        {
            EffetDeBase.VertexColorEnabled = true;
        }


        void CréerTableauPoints()
        {
            for (int i = 0; i < BordureIntérieure.Count; ++i)
            {
                PtsSommets[2 * i] = Terrain.GetPointSpatial((int)BordureIntérieure[i].X, Terrain.NbRangées - (int)BordureIntérieure[i].Y);
                PtsSommets[2 * i + 1] = Terrain.GetPointSpatial((int)BordureExtérieure[i].X, Terrain.NbRangées - (int)BordureExtérieure[i].Y);

                if (i == BordureIntérieure.Count - 1)
                {
                    PtsSommets[2 * i - 1] = Terrain.GetPointSpatial((int)BordureExtérieure[i].X, Terrain.NbRangées - (int)BordureExtérieure[i].Y);
                    PtsSommets[2 * i] = Terrain.GetPointSpatial((int)BordureIntérieure[i].X, Terrain.NbRangées - (int)BordureIntérieure[i].Y);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            EffetDeBase.World = GetMonde();
            EffetDeBase.View = CaméraJeu.Vue;
            EffetDeBase.Projection = CaméraJeu.Projection;
            GérerVisibilitéPiste();
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets, 0, NbTriangles);
            }
        }

      void GérerVisibilitéPiste()
      {
         DepthStencilState vieux = GraphicsDevice.DepthStencilState;
         DepthStencilState jeuDepthBufferState = new DepthStencilState();
         jeuDepthBufferState.DepthBufferFunction = vieux.DepthBufferFunction;
         jeuDepthBufferState.DepthBufferEnable = false;
         GraphicsDevice.DepthStencilState = jeuDepthBufferState;
      }
   }
}
