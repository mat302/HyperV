using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    public class CylindreTexturé : PrimitiveDeBaseAnimée//, ICollisionable
    {
        //Initialement gérées par le constructeur
        //readonly Vector2 Delta;
        readonly int NbColonnes;
        readonly int NbLignes;

        protected string NomTexture { get; set; }

        readonly Vector3 Origine;

        //Initialement gérées par des fonctions appellées par base.Initialize()
        Vector3[,] PtsSommets { get; set; }
        Vector2[,] PtsTexture { get; set; }
        VertexPositionTexture[] Sommets { get; set; }
        BasicEffect EffetDeBase { get; set; }

        int NbTrianglesParStrip { get; set; }

        //Initialement gérées par LoadContent()
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        Texture2D TextureCylindre { get; set; }

        //public bool EstEnCollision(object autreObjet)
        //{
        //    return SphèreDeCollision.Intersects((autreObjet as CubeTexturé).SphèreDeCollision);
        //}

        //public BoundingSphere SphèreDeCollision { get { return new BoundingSphere(Position, Rayon); } }

        Vector3 Extrémité1 { get; set; }
        Vector3 Extrémité2 { get; set; }



        public CylindreTexturé(Game jeu, float homothétieInitiale, Vector3 rotationInitiale,
                              Vector3 positionInitiale, Vector2 étendue, Vector2 charpente,
                              string nomTexture, float intervalleMAJ, Vector3 extrémité1, Vector3 extrémité2)
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
        {
            //Delta = étendue / charpente;
            NbColonnes = (int)charpente.X;
            NbLignes = (int)charpente.Y;
            NomTexture = nomTexture;

            //Origine = new Vector3(0,0,0);
            Origine = new Vector3(-étendue.X / 2, -étendue.Y / 2, 0);

            Extrémité1 = extrémité1;
            Extrémité2 = extrémité2;

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

        protected void InitialiserParamètresEffetDeBase()
        {
            TextureCylindre = GestionnaireDeTextures.Find(NomTexture);
            EffetDeBase = new BasicEffect(GraphicsDevice);
            EffetDeBase.TextureEnabled = true;
            EffetDeBase.Texture = TextureCylindre;
        }

        protected override void InitialiserSommets()
        {
            AffecterPtsSommets();
            AffecterPtsTexture();
            AffecterSommets();
        }

        void AffecterPtsSommets()
        {
            for (int i = 0; i < PtsSommets.GetLength(0); ++i)
            {
                for (int j = 0; j < PtsSommets.GetLength(1); ++j)
                {
                    // ptite fonction-----------------------------------------------------------------------------------
                    PtsSommets[i, j] = new Vector3(Origine.X - (i/NbColonnes * (Extrémité2 - Extrémité1).Length()* (Vector3.Normalize(Extrémité1 - Extrémité2).X)) + Extrémité1.X,
                                                   Origine.Y - (i / NbColonnes * (Extrémité2 - Extrémité1).Length() * (Vector3.Normalize(Extrémité1 - Extrémité2).Y)) + (float)Math.Cos(j * 2 * Math.PI / NbLignes) + Extrémité1.Y,
                                                   Origine.Z - (i / NbColonnes * (Extrémité2 - Extrémité1).Length() * (Vector3.Normalize(Extrémité1 - Extrémité2).Z))+(float)Math.Sin(j * 2 * Math.PI / NbLignes) + Extrémité1.Z);
                }
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

        protected void AffecterSommets()
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
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Sommets, vertexOffset, NbTrianglesParStrip);
        }

    }
}
