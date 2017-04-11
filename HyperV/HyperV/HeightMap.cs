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
    public class HeightMap : PrimitiveDeBase
    {
        const int NB_TRIANGLES_PAR_TUILE = 2;
        const int NB_SOMMETS_PAR_TRIANGLE = 3;
        const float MAX_COULEUR = 255f;

        const int AUCUN_DÉCALAGE_DE_SOMMET = 0;
        const int AVANT_PREMIER_SOMMET = -1;
        const int NB_TRIANGLES = 2;
        const int COTE_NULLE = 0;
        const int PREMIERS_SOMMETS_DU_STRIP = 2;

        const int SOMMET_SUPPLÉMENTAIRE_POUR_LIGNE = 1;
        const int COMPENSATION_NULLE = 0;
        const int DIVISEUR_DEMI_GRANDEUR = 2;
        const int NB_PTS_TEXTURE_POSSIBLE = 20;
        const int NB_SOMMETS_PAR_TUILE = 4;
        const int ORDONNÉE_NULLE = 0;
        const int POSITION_TEXEL_DE_MOINS_PAR_RAPPORT_À_DIMENSION = 1;
        const int VALEUR_J_MAX_HAUT_DE_TUILE = 1;
        const int VALEUR_HAUT_DE_TUILE = 0;
        const int VALEUR_BAS_DE_TUILE = 1;

        Vector3 Étendue { get; set; }
        string NomCarteTerrain { get; set; }
        string NomTextureTerrain { get; set; }
        int NbNiveauTexture { get; set; }

        BasicEffect EffetDeBase { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        Texture2D CarteTerrain { get; set; }
        Texture2D TextureTerrain { get; set; }
        Vector3 Origine { get; set; }

        // à compléter en ajoutant les propriétés qui vous seront nécessaires pour l'implémentation du composant
        int NbRangées { get; set; }
        int NbColonnes { get; set; }
        Color[] DataTexture { get; set; }
        int LargeurTuile { get; set; }
        Vector3[,] PtsSommets { get; set; }
        Vector2[] PtsTexture { get; set; }
        VertexPositionTexture[] Sommets { get; set; }
        Vector2 Delta { get; set; }
        int NbTexels { get; set; }
        float[,] Heights { get; set; }

        public HeightMap(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector3 étendue, string nomCarteTerrain, string nomTextureTerrain) : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale)
      {
            Étendue = étendue;
            NomCarteTerrain = nomCarteTerrain;
            NomTextureTerrain = nomTextureTerrain;
        }

        public override void Initialize()
        {
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            InitialiserDonnéesCarte();
            Heights = new float[CarteTerrain.Width, CarteTerrain.Height];
            InitialiserDonnéesTexture();
            Origine = new Vector3(/*-Étendue.X / DIVISEUR_DEMI_GRANDEUR, 0, -Étendue.Z / DIVISEUR_DEMI_GRANDEUR*/0, 0, 0); //pour centrer la primitive au point (0,0,0)##########################Moins à Z ajouté
            CréerTableauPoints(); // ############### INVERSÉ
            AllouerTableaux(); // ################## INVERSÉ
            base.Initialize();
        }

        //
        // à partir de la texture servant de carte de hauteur (HeightMap), on initialise les données
        // relatives à la structure de la carte
        //
        void InitialiserDonnéesCarte()
        {
            // à compléter
            CarteTerrain = GestionnaireDeTextures.Find(NomCarteTerrain);
            NbRangées = CarteTerrain.Width - SOMMET_SUPPLÉMENTAIRE_POUR_LIGNE;
            NbColonnes = CarteTerrain.Height - SOMMET_SUPPLÉMENTAIRE_POUR_LIGNE;
            NbTriangles = NbRangées * NbColonnes * NB_TRIANGLES_PAR_TUILE;
            NbSommets = NbTriangles * NB_SOMMETS_PAR_TRIANGLE;
            NbTexels = CarteTerrain.Width * CarteTerrain.Height;
            DataTexture = new Color[NbTexels];
            CarteTerrain.GetData<Color>(DataTexture);
        }

        //
        // à partir de la texture contenant les textures carte de hauteur (HeightMap), on initialise les données
        // relatives à l'application des textures de la carte
        //
        void InitialiserDonnéesTexture()
        {
            // à compléter
            TextureTerrain = GestionnaireDeTextures.Find(NomTextureTerrain);
            LargeurTuile = (int)(TextureTerrain.Height / (float)NbNiveauTexture);
        }

        //
        // Allocation des deux tableaux
        //    1) celui contenant les points de sommet (les points uniques), 
        //    2) celui contenant les sommets servant à dessiner les triangles
        void AllouerTableaux()
        {
            // à compléter
            Sommets = new VertexPositionTexture[NbSommets];
            //PtsTexture = new Vector2[CarteTerrain.Width, CarteTerrain.Height];
            PtsTexture = new Vector2[NB_PTS_TEXTURE_POSSIBLE];
            //PtsSommets = new Vector3[CarteTerrain.Width, CarteTerrain.Height];
            //Delta = new Vector2(Étendue.X / NbRangées, Étendue.Z / NbColonnes);
            AffecterPointsTexture();
            InitialiserSommets();
        }

        void AffecterPointsTexture()
        {
            //for (int i = 0; i < PtsTexture.GetLength(0); ++i)
            //{
            //    for (int j = 0; j < PtsTexture.GetLength(1); ++j)
            //    {
            //        PtsTexture[i, j] = new Vector2(0, PtsSommets[i, j].Y / Étendue.Y);
            //    }
            //}
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    PtsTexture[NB_SOMMETS_PAR_TUILE * i + j] = new Vector2(j % NB_TRIANGLES_PAR_TUILE, (i + (j > VALEUR_J_MAX_HAUT_DE_TUILE ? VALEUR_BAS_DE_TUILE : VALEUR_HAUT_DE_TUILE) * (1 - 1 / (float)TextureTerrain.Height)) / NbNiveauTexture);
                    //PtsTexture[NB_SOMMETS_PAR_TUILE * i + j] = new Vector2(0.5f, 0.9f);
                }
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            EffetDeBase = new BasicEffect(GraphicsDevice);
            InitialiserParamètresEffetDeBase();
        }

        void InitialiserParamètresEffetDeBase()
        {
            EffetDeBase.TextureEnabled = true;
            EffetDeBase.Texture = TextureTerrain;
        }

        //
        // Création du tableau des points de sommets (on crée les points)
        // Ce processus implique la transformation des points 2D de la texture en coordonnées 3D du terrain
        //
        private void CréerTableauPoints()
        {
            // à compléter
            PtsSommets = new Vector3[CarteTerrain.Width, CarteTerrain.Height];
            Delta = new Vector2(Étendue.X / NbRangées, Étendue.Z / NbColonnes);
            for (int i = 0; i < PtsSommets.GetLength(0); ++i)
            {
                for (int j = 0; j < PtsSommets.GetLength(1); ++j)
                {
                    PtsSommets[i, j] = Origine + new Vector3(Delta.X * i, DataTexture[i * PtsSommets.GetLength(1) + j].B / MAX_COULEUR * Étendue.Y, Delta.Y * j);
                }
            }
        }

        //
        // Création des sommets.
        // N'oubliez pas qu'il s'agit d'un TriangleList...
        //
        protected override void InitialiserSommets()
        {
            // à compléter
            int cpt = -1;
            for (int j = 0; j < NbRangées; ++j)
            {
                for (int i = 0; i < NbColonnes; ++i)
                {
                    //int val1 = (int)(PtsSommets[i, j].Y + PtsSommets[i + 1, j].Y + PtsSommets[i, j + 1].Y) / 3;
                    //int woho = (int)(val1 / MAX_COULEUR);

                    //Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i, j], PtsTexture[i, j]);
                    //Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i + 1, j], PtsTexture[i + 1, j]);
                    //Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[i, j + 1]);

                    ////AssociéTruc(ref Sommets[cpt - 2], ref Sommets[cpt - 1], ref Sommets[cpt]);

                    //int val2 = (int)(((PtsSommets[i + 1, j].Y + PtsSommets[i + 1, j + 1].Y + PtsSommets[i, j + 1].Y) / 3 - Origine.Y) / Delta.Y);
                    //woho = (int)(val2 / MAX_COULEUR);

                    //Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i + 1, j], PtsTexture[i + 1, j]);
                    //Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i + 1, j + 1], PtsTexture[i + 1, j + 1]);
                    //Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[i, j + 1]);
                    AffecterTuile(ref cpt, i, j);
                }
            }
        }

        void AffecterTuile(ref int cpt, int i, int j)
        {
            int noCase = (int)((PtsSommets[i, j].Y + PtsSommets[i + 1, j].Y + PtsSommets[i, j + 1].Y + PtsSommets[i + 1, j + 1].Y) / 4.0f / Étendue.Y * 19) / 4 * 4;

            //for (int k = 0; k < 4; ++k)
            //{
            //    Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i, j], PtsTexture[k + noCase * ]);
            //} 
            Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i, j], PtsTexture[noCase]);
            Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i + 1, j], PtsTexture[noCase + 1]);
            Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[noCase + 2]);
            Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i + 1, j], PtsTexture[noCase + 1]);
            Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i + 1, j + 1], PtsTexture[noCase + 3]);
            Sommets[++cpt] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[noCase + 2]);
        }

        //
        // Deviner ce que fait cette méthode...
        //
        public override void Draw(GameTime gameTime)
        {
            // à compléter
            EffetDeBase.World = GetMonde();
            EffetDeBase.View = CaméraJeu.Vue;
            EffetDeBase.Projection = CaméraJeu.Projection;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Sommets, COMPENSATION_NULLE, NbTriangles);
            }
        }

        public float GetHeight(Vector3 positionReceived)
        {
            Vector3 position = positionReceived - PositionInitiale;
            int i = (int)(position.X / Delta.X), j = (int)(position.Z / Delta.Y);
            Vector3 n;
            float height;
            if (i >= 0 && j >= 0 && i < Heights.GetLength(0) && j < Heights.GetLength(1))
            {
                n = Vector3.Cross(PtsSommets[i + 1, j] - PtsSommets[i, j], PtsSommets[i, j + 1] - PtsSommets[i, j]);
                height = (n.X * PtsSommets[i, j].X + n.Y * PtsSommets[i, j].Y + n.Z * PtsSommets[i, j].Z - n.X * position.X - n.Z * position.Z) / n.Y + 5;
                //height = Heights[i, j];
            }
            else
            {
                height = 5;//position.Y;
            }
            return height;
        }
    }
}
