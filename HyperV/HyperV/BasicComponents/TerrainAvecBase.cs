using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
    public class TerrainAvecBase : PrimitiveDeBaseAnimée
    {
        const int NB_TRIANGLES_PAR_TUILE = 2;
        const int NB_SOMMETS_PAR_TRIANGLE = 3;
        const int NB_TUILES_PAR_COTÉ = 256;     //pour une 256x256 de dimension   

        string NomCarteTerrain { get; set; }
        string NomTextureBase { get; set; }
        string[] NomTextureTerrain { get; set; }
        
        Color[] DataCarteTexture { get; set; }
        VertexPositionTexture[] SommetsBase { get; set; }

        Texture2D Terrain { get; set; }
        Texture2D TextureBase { get; set; }
        Texture2D TextureTerrainSable { get; set; }
        Texture2D TextureTerrainHerbe { get; set; }
        
        public int NbRangées { get; set; }
        public int NbColonnes { get; set; }

        public Vector3 Origine { get; private set; }
        Vector3 Delta { get; set; }

        Texture2D TextureCombinee { get; set; }
        BasicEffect EffetDeBase { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }

        VertexPositionTexture[] Sommets { get; set; }
        Vector3 Étendue { get; set; }
        Vector3[,] PtsSommets { get; set; }
        Vector2[,] PtsTexture { get; set; }
        Vector3[,] PtsSommetsBase { get; set; }

        Vector3[] PtsSommetsAvant { get; set; }
        VertexPositionTexture[] SommetsAvant { get; set; }

        Vector3[] PtsSommetsDroite { get; set; }
        VertexPositionTexture[] SommetsDroite { get; set; }

        Vector3[] PtsSommetsGauche { get; set; }
        VertexPositionTexture[] SommetsArrière { get; set; }

        Vector3[] PtsSommetsArrière { get; set; }
        VertexPositionTexture[] SommetsGauche { get; set; }

        public TerrainAvecBase(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector3 étendue, string nomCarteTerrain, string[] nomTextureTerrain, string nomTextureBase, float intervalleMAJ)
           : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
        {
            Étendue = étendue;
            NomCarteTerrain = nomCarteTerrain;
            NomTextureTerrain = nomTextureTerrain;
            NomTextureBase = nomTextureBase;
        }

        public Vector3 GetPointSpatial(int x, int y)
        {
            return new Vector3(PtsSommets[x, y].X, PtsSommets[x, y].Y, PtsSommets[x, y].Z);
        }

        public Vector3 GetNormale(int x, int y)
        {
            Vector3 A = GetPointSpatial(x, y);
            Vector3 B = GetPointSpatial(x + 1, y); //le point a droite de A
            Vector3 C = GetPointSpatial(x, y + 1); //le point en haut de A
            Vector3 AB = B - A;
            Vector3 AC = C - A;
            return Vector3.Cross(AB, AC);
        }

        public override void Initialize()
        {
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;

            //pour la carte
            Terrain = GestionnaireDeTextures.Find(NomCarteTerrain);
            DataCarteTexture = new Color[Terrain.Width * Terrain.Height];
            Terrain.GetData<Color>(DataCarteTexture);

            //pour le terrain
            TextureBase = GestionnaireDeTextures.Find(NomTextureBase);
            TextureTerrainSable = GestionnaireDeTextures.Find(NomTextureTerrain[0]);
            TextureTerrainHerbe = GestionnaireDeTextures.Find(NomTextureTerrain[1]);

            NbColonnes = Terrain.Width - 1;
            NbRangées = Terrain.Height - 1;
            NbSommets = (NbColonnes) * (NbRangées) * NB_SOMMETS_PAR_TRIANGLE * NB_TRIANGLES_PAR_TUILE;
            Origine = new Vector3(-Étendue.X / 2, 0, -Étendue.Z / 2);
            Delta = new Vector3(Étendue.X / NbColonnes, Étendue.Y / 255f, Étendue.Z / NbRangées);
            
            CreerTableau();
            CreerTableauPtsSommets();
            CreerTableauPtsTexture();
            CombinerTextures();
            CreerTableauxBase();
            CreerTableauPtsSommetsBase();
            InitialiserSommetsBase();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            EffetDeBase = new BasicEffect(GraphicsDevice);
            EffetDeBase.TextureEnabled = true;
        }

        protected override void InitialiserSommets()
        {
            int cpt = -1;
            for (int j = 0; j < NbRangées; j++)
            {
                for (int i = 0; i < NbColonnes; i++)
                {
                    Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i + 1, j], PtsTexture[i + 1, j]);
                    Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[i, j + 1]);
                    Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i, j], PtsTexture[i, j]);
                    Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[i, j + 1]);
                    Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i + 1, j], PtsTexture[i + 1, j]);
                    Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i + 1, j + 1], PtsTexture[i + 1, j + 1]);
                }
            }
        }

        void InitialiserSommetsBase()
        {
            Vector2 TextureBasGauche = new Vector2(0, 1);
            Vector2 TextureHautGauche = Vector2.Zero;
            Vector2 TextureBasDroite = new Vector2(1, 1);
            Vector2 TextureHautDroite = new Vector2(1, 0);

            for (int i = 0; i < 2 * NbColonnes; i++)
            {
                SommetsAvant[i] = new VertexPositionTexture(PtsSommetsAvant[i++], TextureBasGauche);
                SommetsAvant[i] = new VertexPositionTexture(PtsSommetsAvant[i++], TextureHautGauche);
                SommetsAvant[i] = new VertexPositionTexture(PtsSommetsAvant[i++], TextureBasDroite);
                SommetsAvant[i] = new VertexPositionTexture(PtsSommetsAvant[i], TextureHautGauche);
            }
            SommetsAvant[512] = new VertexPositionTexture(PtsSommetsAvant[512], TextureBasGauche);
            SommetsAvant[513] = new VertexPositionTexture(PtsSommetsAvant[513], TextureHautGauche);

            for (int i = 0; i < 2 * NbRangées; i++)
            {
                SommetsDroite[i] = new VertexPositionTexture(PtsSommetsDroite[i++], TextureBasGauche);
                SommetsDroite[i] = new VertexPositionTexture(PtsSommetsDroite[i++], TextureHautGauche);
                SommetsDroite[i] = new VertexPositionTexture(PtsSommetsDroite[i++], TextureBasDroite);
                SommetsDroite[i] = new VertexPositionTexture(PtsSommetsDroite[i], TextureHautDroite);
            }
            SommetsDroite[512] = new VertexPositionTexture(PtsSommetsDroite[512], TextureBasGauche);
            SommetsDroite[513] = new VertexPositionTexture(PtsSommetsDroite[513], TextureHautGauche);

            for (int i = 0; i < 2 * NbRangées; i++)
            {
                SommetsArrière[i] = new VertexPositionTexture(PtsSommetsArrière[i++], TextureBasGauche);
                SommetsArrière[i] = new VertexPositionTexture(PtsSommetsArrière[i++], TextureHautGauche);
                SommetsArrière[i] = new VertexPositionTexture(PtsSommetsArrière[i++], TextureBasDroite);
                SommetsArrière[i] = new VertexPositionTexture(PtsSommetsArrière[i], TextureHautDroite);
            }
            SommetsArrière[512] = new VertexPositionTexture(PtsSommetsArrière[512], TextureBasGauche);
            SommetsArrière[513] = new VertexPositionTexture(PtsSommetsArrière[513], TextureHautGauche);

            for (int i = 0; i < 2 * NbColonnes; i++)
            {
                SommetsGauche[i] = new VertexPositionTexture(PtsSommetsGauche[i++], TextureBasGauche);
                SommetsGauche[i] = new VertexPositionTexture(PtsSommetsGauche[i++], TextureHautGauche);
                SommetsGauche[i] = new VertexPositionTexture(PtsSommetsGauche[i++], TextureBasDroite);
                SommetsGauche[i] = new VertexPositionTexture(PtsSommetsGauche[i], TextureHautDroite);
            }
            SommetsGauche[512] = new VertexPositionTexture(PtsSommetsGauche[512], TextureBasGauche);
            SommetsGauche[513] = new VertexPositionTexture(PtsSommetsGauche[513], TextureHautGauche);

        }

        void CreerTableau()
        {
            Sommets = new VertexPositionTexture[NbSommets];
            PtsSommets = new Vector3[Terrain.Width, Terrain.Height];
            PtsTexture = new Vector2[Terrain.Width, Terrain.Height];
        }

        void CreerTableauPtsSommets()
        {
            for (int i = 0; i < PtsSommets.GetLength(0); ++i)
            {
                for (int j = 0; j < PtsSommets.GetLength(1); ++j)
                {
                    PtsSommets[i, j] = new Vector3(Origine.X + (i * Delta.X), Origine.Y + (DataCarteTexture[j * PtsSommets.GetLength(0) + i].B * Delta.Y), Origine.Z + (j * Delta.Z));
                }
            }
        }

        void CreerTableauPtsTexture()
        {
            for (int i = 0; i < PtsTexture.GetLength(0); ++i)
            {
                for (int j = 0; j < PtsTexture.GetLength(1); ++j)
                {
                    PtsTexture[i, j] = new Vector2(i / (float)NbColonnes, -j / (float)NbRangées);
                }
            }
        }

        void CreerTableauxBase()
        {
            PtsSommetsAvant = new Vector3[2 * (NbColonnes + 1)];
            SommetsAvant = new VertexPositionTexture[2 * (NbColonnes + 1)];

            PtsSommetsDroite = new Vector3[2 * (NbRangées + 1)];
            SommetsDroite = new VertexPositionTexture[2 * (NbRangées + 1)];

            PtsSommetsGauche = new Vector3[2 * (NbRangées + 1)];
            SommetsGauche = new VertexPositionTexture[2 * (NbRangées + 1)];

            PtsSommetsArrière = new Vector3[2 * (NbColonnes + 1)];
            SommetsArrière = new VertexPositionTexture[2 * (NbColonnes + 1)];
        }

        void CreerTableauPtsSommetsBase()
        {
            int cpt = 0;
            for (int i = 0; i < 2 * (NbColonnes + 1) - 1; i++)
            {
                PtsSommetsArrière[i] = new Vector3(Origine.X + cpt * Delta.X, PtsSommets[i / 2, 0].Y, Origine.Z);
                PtsSommetsArrière[++i] = new Vector3(Origine.X + cpt * Delta.X, 0, Origine.Z);
                cpt++;
            }

            cpt = 256;
            for (int i = 0; i < 2 * (NbColonnes + 1) - 1; i++)
            {
                PtsSommetsGauche[i] = new Vector3(Origine.X, PtsSommets[0, ((-1 * i) + (2 * (NbColonnes + 1) - 1)) / 2].Y, Origine.Z + cpt * Delta.Z);
                PtsSommetsGauche[++i] = new Vector3(Origine.X, 0, Origine.Z + cpt * Delta.Z);
                cpt--;
            }

            cpt = 256;
            for (int i = 0; i < 2 * (NbColonnes + 1) - 1; i++)
            {
                PtsSommetsAvant[i] = new Vector3(Origine.X + cpt * Delta.X, PtsSommets[((-1 * i) + (2 * (NbColonnes + 1) - 1)) / 2, PtsSommets.GetLength(1) - 1].Y, Origine.Z + Terrain.Height - 1);
                PtsSommetsAvant[++i] = new Vector3(Origine.X + cpt * Delta.X, 0, Origine.Z + Terrain.Height - 1);
                cpt--;
            }

            cpt = 0;
            for (int i = 0; i < 2 * (NbRangées + 1) - 1; ++i)
            {
                PtsSommetsDroite[i] = new Vector3(Origine.X + Terrain.Width - 1, PtsSommets[PtsSommets.GetLength(0) - 1, i / 2].Y, Origine.Z + cpt * Delta.Z);
                PtsSommetsDroite[++i] = new Vector3(Origine.X + Terrain.Width - 1, 0, Origine.Z + cpt * Delta.Z);
                cpt++;
            }
        }
        
        void CombinerTextures()
        {
            TextureCombinee = new Texture2D(TextureTerrainSable.GraphicsDevice, PtsSommets.GetLength(0), PtsSommets.GetLength(1));
            int nbTexels = PtsSommets.GetLength(0) * PtsSommets.GetLength(0);
            Color[] texels = new Color[nbTexels];
            TextureTerrainSable.GetData(texels);

            Color[] texelsHerbe = new Color[TextureTerrainHerbe.Width * TextureTerrainHerbe.Height];
            TextureTerrainHerbe.GetData(texelsHerbe);

            Color[] texelsSable = new Color[TextureTerrainSable.Width * TextureTerrainSable.Height];
            TextureTerrainSable.GetData(texelsSable);
            
            for (int noTexel = 0; noTexel < nbTexels; ++noTexel)
            {
                float j = DataCarteTexture[noTexel].B / 255f;

                texels[noTexel].R = (byte)((j * (byte)texelsHerbe[noTexel].R) + (byte)((1 - j) * (byte)texelsSable[noTexel].R));
                texels[noTexel].G = (byte)((j * (byte)texelsHerbe[noTexel].G) + (byte)((1 - j) * (byte)texelsSable[noTexel].G));
                texels[noTexel].B = (byte)((j * (byte)texelsHerbe[noTexel].B) + (byte)((1 - j) * (byte)texelsSable[noTexel].B));
            }
            TextureCombinee.SetData<Color>(texels);
        }

        public override void Draw(GameTime gameTime)
        {
            EffetDeBase.World = GetMonde();
            EffetDeBase.View = CaméraJeu.Vue;
            EffetDeBase.Projection = CaméraJeu.Projection;

            EffetDeBase.Texture = TextureCombinee;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Sommets, 0, Sommets.Length / NB_SOMMETS_PAR_TRIANGLE);                
            }

            EffetDeBase.Texture = TextureBase;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, SommetsAvant, 0, NB_TUILES_PAR_COTÉ * NB_TRIANGLES_PAR_TUILE);
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, SommetsDroite, 0, NB_TUILES_PAR_COTÉ * NB_TRIANGLES_PAR_TUILE);
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, SommetsArrière, 0, NB_TUILES_PAR_COTÉ * NB_TRIANGLES_PAR_TUILE);
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, SommetsGauche, 0, NB_TUILES_PAR_COTÉ * NB_TRIANGLES_PAR_TUILE);
            }
        }
    }
}