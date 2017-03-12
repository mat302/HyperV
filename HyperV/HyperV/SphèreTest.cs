using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AtelierXNA;
using System.Collections.Generic;

namespace HyperV
{
    class SphèreTest : PrimitiveDeBaseAnimée//, ICollisionable
    {
        Texture2D Texture { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        BasicEffect EffetDeBase { get; set; }
        Vector2 Charpente { get; set; }
        float Rayon { get; set; }
        string NomTexture { get; set; }
        float Delta { get; set; }
        Vector3 Origine { get; set; }
        bool Colision { get; set; }

        CaméraJoueur CaméraJoueur { get; set; }

        Vector3[,] PtsSommets { get; set; }
        Vector2[,] PtsTexture { get; set; }
        VertexPositionTexture[] Sommets { get; set; }
        List<Vector3> Listepoints { get; set; }

        public float? EstEnCollision(Ray autreObjet)
        {
            return SphèreDeCollision.Intersects(autreObjet);
        }

        public BoundingSphere SphèreDeCollision { get { return new BoundingSphere(Position, Rayon); } }

        public bool EstRamassée { get; set; }

        public SphèreTest(Game jeu, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float rayon, Vector2 charpente, string nomTexture, float intervalleMAJ) 
            : base(jeu, échelleInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
        {
            Charpente = charpente;
            Rayon = rayon;
            NomTexture = nomTexture;
            NbTriangles = (int)(Charpente.X * Charpente.Y * 2);
            Origine = new Vector3(-(float)(Math.PI * Rayon), -Rayon, (float)(Math.PI * Rayon));
            EstRamassée = false;
        }


        public override void Initialize()
        {
            Delta = (Rayon * 2) / Charpente.X;
            PtsSommets = new Vector3[(int)(Charpente.Y + 1), (int)(Charpente.X + 1)]; //rangée,colonne
            PtsTexture = new Vector2[(int)(Charpente.Y + 1), (int)(Charpente.X + 1)];
            Sommets = new VertexPositionTexture[NbTriangles * 3];
            CréerTableauPointsTexture();
            CréerTableauPoints();
            base.Initialize();
            CaméraJoueur = CaméraJeu as CaméraJoueur;
        }

        protected override void LoadContent()
        {
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Texture = GestionnaireDeTextures.Find(NomTexture);
            EffetDeBase = new BasicEffect(GraphicsDevice);
            EffetDeBase.TextureEnabled = true;
            EffetDeBase.Texture = Texture;
            base.LoadContent();
        }

        protected override void EffectuerMiseÀJour()
        {
            base.EffectuerMiseÀJour();

            if (EstRamassée)
            {
                Position = CaméraJeu.Position + 4 * Vector3.Normalize(CaméraJoueur.Direction)
                            + 2.5f * Vector3.Normalize(CaméraJoueur.Latéral)
                            - 1.5f * Vector3.Normalize(Vector3.Cross(CaméraJoueur.Latéral, CaméraJoueur.Direction));

                CréerTableauPointsTexture();
                CréerTableauPoints();
                InitialiserSommets();

                Game.Window.Title = Position.ToString();
            }
        }

        private void CréerTableauPointsTexture()
        {
            for (int rangée = 0; rangée < Charpente.Y + 1; ++rangée)
            {
                for (int colonne = 0; colonne < Charpente.X + 1; ++colonne)
                {
                    PtsTexture[rangée, colonne] = new Vector2((1f / Charpente.X) * colonne, 1 - (1f / Charpente.Y) * rangée);
                }
            }
        }

        private void CréerTableauPoints()
        {
            List<float> listeHauteur = new List<float>();
            for (int i = 0; i <= Charpente.X / 2; ++i)
            {
                listeHauteur.Add((2 * i) / Charpente.X);
            }
            float DeltaHauteur = -10; //pour le 2
            for (int rangée = 0; rangée < Charpente.Y + 1; ++rangée)
            {
                float Phi = 0; //pour le 1
                DeltaHauteur += 90f / (Charpente.X / 2); //doit changer a chaque changement de rangée
                for (int colonne = 0; colonne < Charpente.X + 1; ++colonne)
                {
                    if (rangée <= 10)
                        PtsSommets[rangée, colonne] = new Vector3((float)Math.Sin(MathHelper.ToRadians(Phi)) * (float)Math.Sin(MathHelper.ToRadians(DeltaHauteur)), //1 : tourne le plan, 2 : réduit le x de 0 a 90 a 0
                                                               (-Rayon + Rayon * listeHauteur[rangée] * listeHauteur[rangée]),                                                                   //1 : réduit le y de 0 a 90 a 0
                                                               (float)Math.Cos(MathHelper.ToRadians(Phi)) * (float)Math.Sin(MathHelper.ToRadians(DeltaHauteur))); //1 : tourne le plan, 2 : réduit le z de 0 a 90 a 0
                    else
                    {
                        PtsSommets[rangée, colonne] = new Vector3((float)Math.Sin(MathHelper.ToRadians(Phi)) * (float)Math.Sin(MathHelper.ToRadians(DeltaHauteur)), //1 : tourne le plan, 2 : réduit le x de 0 a 90 a 0
                                                              (Rayon - Rayon * listeHauteur[20 - rangée] * listeHauteur[20 - rangée]),                                                                   //1 : réduit le y de 0 a 90 a 0
                                                              (float)Math.Cos(MathHelper.ToRadians(Phi)) * (float)Math.Sin(MathHelper.ToRadians(DeltaHauteur))); //1 : tourne le plan, 2 : réduit le z de 0 a 90 a 0
                    }
                    Phi += 360f / Charpente.X; //doit toujours changer
                }
            }
            Listepoints = new List<Vector3>();
            foreach (Vector3 v in PtsSommets)
            {
                Listepoints.Add(v);
            }
        }

        protected override void InitialiserSommets()
        {
            int NoSommet = -1;
            for (int ligne = 0; ligne < Charpente.Y; ++ligne)
            {
                for (int colonne = 0; colonne < Charpente.X; ++colonne)
                {
                    Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[ligne, colonne], PtsTexture[ligne, colonne]);
                    Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[ligne + 1, colonne], PtsTexture[ligne + 1, colonne]);
                    Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[ligne + 1, colonne + 1], PtsTexture[ligne + 1, colonne + 1]);
                    Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[ligne, colonne], PtsTexture[ligne, colonne]);
                    Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[ligne + 1, colonne + 1], PtsTexture[ligne + 1, colonne + 1]);
                    Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[ligne, colonne + 1], PtsTexture[ligne, colonne + 1]);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            EffetDeBase.World = GetMonde();
            EffetDeBase.View = CaméraJeu.Vue;
            EffetDeBase.Projection = CaméraJeu.Projection;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Sommets, 0, NbTriangles);
            }
        }


    }
}
