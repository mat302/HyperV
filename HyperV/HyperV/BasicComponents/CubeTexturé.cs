using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
    {
        public class CubeTexturé : PrimitiveDeBaseAnimée, ICollisionable, IDestructible
        {
            //Constantes
            const int NB_SOMMETS = 8;
            const int NB_TRIANGLES = 6;

            const int NB_COLONNES = 3;
            const int NB_LIGNES = 1;

            const int NB_STRIP_AFFICHER = 2;

            //Gérées initialement par le constructeur
            public string NomTextureCube { get; set; }
            readonly Vector3 Delta;
            readonly Vector3 Origine;

            //Gérées initialement par Initialize()
            VertexPositionTexture[] Sommets { get; set; }
            protected Vector3[] PtsSommets { get; set; }
            Vector2[,] PtsTexture { get; set; }

            //Gérée initialement par LoadContent()
            BasicEffect EffetDeBase { get; set; }
            RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
            Texture2D Texture { get; set; }


            public bool ÀDétruire { get; set; }

            public bool EstEnCollision(object autreObjet)
            {
                BoundingSphere obj2 = (BoundingSphere)autreObjet;
                return obj2.Intersects(SphèreDeCollision);
            }

            public BoundingSphere SphèreDeCollision
            {
                get
                {
                    return new BoundingSphere(Position, 2 * Delta.X);
                }
            }


            public CubeTexturé(Game game, float homothétieInitiale, Vector3 rotationInitiale,
                            Vector3 positionInitiale, string nomTextureCube, Vector3 dimension,
                            float intervalleMAJ)
                : base(game, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
            {
                NomTextureCube = nomTextureCube;
                Delta = dimension;
                Origine = new Vector3(-Delta.X / 2, -Delta.Y / 2, Delta.Z / 2);
            }

            public override void Initialize()
            {
                ÀDétruire = false;
                AllouerTableaux();

                base.Initialize();
                InitialiserParamètresEffetDeBase();
            }

            void AllouerTableaux()
            {
                PtsSommets = new Vector3[NB_SOMMETS];
                PtsTexture = new Vector2[NB_COLONNES + 1, NB_LIGNES + 1];
                Sommets = new VertexPositionTexture[NB_SOMMETS * 2];
            }

            public void InitialiserParamètresEffetDeBase()
            {
                Texture = GestionnaireDeTextures.Find(NomTextureCube);
                EffetDeBase = new BasicEffect(GraphicsDevice);
                EffetDeBase.TextureEnabled = true;
                EffetDeBase.Texture = Texture;
            }

            protected override void InitialiserSommets()
            {
                AffecterPtsSommets();
                AffecterPtsTexture();
                AffecterSommets();
            }

            void AffecterPtsSommets()
            {
                //Sommets de la base du bas à partir de l'origine (sens horraire lorsqu'on observe du haut)
                PtsSommets[0] = new Vector3(-Delta.X / 2, -Delta.Y / 2, Delta.Z / 2);
                PtsSommets[1] = new Vector3(PtsSommets[0].X, PtsSommets[0].Y, PtsSommets[0].Z - Delta.Z);
                PtsSommets[2] = new Vector3(PtsSommets[0].X + Delta.X, PtsSommets[0].Y, PtsSommets[0].Z - Delta.Z);
                PtsSommets[3] = new Vector3(PtsSommets[0].X + Delta.X, PtsSommets[0].Y, PtsSommets[0].Z);

                //Sommets de la base du haut à partir de l'origine (sens horraire lorsqu'on observe du haut)
                PtsSommets[4] = new Vector3(PtsSommets[0].X, PtsSommets[0].Y + Delta.Y, PtsSommets[0].Z);
                PtsSommets[5] = new Vector3(PtsSommets[0].X, PtsSommets[0].Y + Delta.Y, PtsSommets[0].Z - Delta.Z);
                PtsSommets[6] = new Vector3(PtsSommets[0].X + Delta.X, PtsSommets[0].Y + Delta.Y, PtsSommets[0].Z - Delta.Z);
                PtsSommets[7] = new Vector3(PtsSommets[0].X + Delta.X, PtsSommets[0].Y + Delta.Y, PtsSommets[0].Z);
            }

            void AffecterPtsTexture()
            {
                for (int i = 0; i < PtsTexture.GetLength(0); ++i)
                {
                    for (int j = 0; j < PtsTexture.GetLength(1); ++j)
                    {
                        PtsTexture[i, j] = new Vector2(i / (float)NB_COLONNES, j / (float)NB_LIGNES);
                    }
                }
            }

            protected void AffecterSommets()
            {
                Sommets[0] = new VertexPositionTexture(PtsSommets[0], PtsTexture[0, 1]);
                Sommets[1] = new VertexPositionTexture(PtsSommets[4], PtsTexture[0, 0]);
                Sommets[2] = new VertexPositionTexture(PtsSommets[3], PtsTexture[1, 1]);
                Sommets[3] = new VertexPositionTexture(PtsSommets[7], PtsTexture[1, 0]);
                Sommets[4] = new VertexPositionTexture(PtsSommets[2], PtsTexture[2, 1]);
                Sommets[5] = new VertexPositionTexture(PtsSommets[6], PtsTexture[2, 0]);
                Sommets[6] = new VertexPositionTexture(PtsSommets[1], PtsTexture[3, 1]);
                Sommets[7] = new VertexPositionTexture(PtsSommets[5], PtsTexture[3, 0]);


                Sommets[8] = new VertexPositionTexture(PtsSommets[3], PtsTexture[0, 1]);
                Sommets[9] = new VertexPositionTexture(PtsSommets[2], PtsTexture[0, 0]);
                Sommets[10] = new VertexPositionTexture(PtsSommets[0], PtsTexture[1, 1]);
                Sommets[11] = new VertexPositionTexture(PtsSommets[1], PtsTexture[1, 0]);
                Sommets[12] = new VertexPositionTexture(PtsSommets[4], PtsTexture[2, 1]);
                Sommets[13] = new VertexPositionTexture(PtsSommets[5], PtsTexture[2, 0]);
                Sommets[14] = new VertexPositionTexture(PtsSommets[7], PtsTexture[3, 1]);
                Sommets[15] = new VertexPositionTexture(PtsSommets[6], PtsTexture[3, 0]);
            }

            protected override void LoadContent()
            {
                GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
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
                    for (int i = 0; i < NB_STRIP_AFFICHER; ++i)
                    {
                        DessinerTriangleStrip(i);
                    }
                }
            }

            void DessinerTriangleStrip(int noStrip)
            {
                int vertexOffset = noStrip * NB_SOMMETS;
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Sommets, vertexOffset, NB_TRIANGLES);
            }
        }
    }