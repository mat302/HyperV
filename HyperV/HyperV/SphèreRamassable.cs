using AtelierXNA;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HyperV
{
    class SphèreRamassable : PrimitiveDeBaseAnimée//, ICollisionable
   {
        //Initialement gérées par le constructeur
        readonly float Rayon;
        readonly int NbColonnes;
        readonly int NbLignes;
        readonly string NomTexture;

        readonly Vector3 Origine;
        Camera1 CaméraJoueur { get; set; }

        //Initialement gérées par des fonctions appellées par base.Initialize()
        Vector3[,] PtsSommets { get; set; }
        Vector2[,] PtsTexture { get; set; }
        VertexPositionTexture[] Sommets { get; set; }
        BasicEffect EffetDeBase { get; set; }

        int NbTrianglesParStrip { get; set; }

        //Initialement gérées par LoadContent()
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        Texture2D TextureSphère { get; set; }

        public float? EstEnCollision(Ray autreObjet)
        {
            return SphèreDeCollision.Intersects(autreObjet);
        }

        public BoundingSphere SphèreDeCollision { get { return new BoundingSphere(Position, Rayon); } }

        public bool EstRamassée { get; set; }

        public SphèreRamassable(Game jeu, float homothétieInitiale, Vector3 rotationInitiale,
                              Vector3 positionInitiale, float rayon, Vector2 charpente,
                              string nomTexture, float intervalleMAJ)
            :base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
        {
            Rayon = rayon;
            NbColonnes = (int)charpente.X;
            NbLignes = (int)charpente.Y;
            NomTexture = nomTexture;

            Origine = new Vector3(0,0,0);

            Position = Origine;//*******************

            EstRamassée = false;
        }

        public override void Initialize()
        {
            NbTrianglesParStrip = NbColonnes * 2;
            NbSommets = (NbTrianglesParStrip + 2) * NbLignes;

            AllouerTableaux();
            base.Initialize();
            InitialiserParamètresEffetDeBase();
            CaméraJoueur = CaméraJeu as Camera1;
        }

        protected override void EffectuerMiseÀJour()
        {
            base.EffectuerMiseÀJour();

            //if (EstRamassée)
            //{
            //    Position = CaméraJeu.Position + 4 * Vector3.Normalize(CaméraJoueur.Direction)
            //                + 2.5f * Vector3.Normalize(CaméraJoueur.Latéral)
            //                - 1.5f * Vector3.Normalize(Vector3.Cross(CaméraJoueur.Latéral, CaméraJoueur.Direction));
            //    InitialiserSommets();

            //    Game.Window.Title = Position.ToString();
            //}
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
                    PtsSommets[i, j] = new Vector3(Position.X + Rayon * (float)(Math.Sin(phi)*Math.Cos(theta)),
                                                   Position.Y + Rayon * (float)(Math.Cos(phi)),
                                                   Position.Z + Rayon * (float)(Math.Sin(phi) * Math.Sin(theta)));
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
