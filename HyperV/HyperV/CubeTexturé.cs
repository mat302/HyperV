using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace HyperV
{
    public class CubeTexturé : PrimitiveDeBaseAnimée
    {
        const int NB_SOMMETS = 8;
        const int NB_TRIANGLES = 6;

        protected VertexPositionTexture[] Sommets { get; set; }
        VertexPositionTexture[] Sommets2 { get; set; }
        
        Texture2D Texture { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        BlendState GestionAlpha { get; set; }
        BasicEffect EffetDeBase { get; set; }
        protected Vector3 Origine { get; set; }
        Vector3 Delta { get; set; }
        Vector3 DeltaModifié { get; set; }
        float DeltaTexture { get; set; }
        string NomTextureCube { get; set; }
        bool àDétruire;

        public bool ÀDétruire
        {
            get
            {
                return àDétruire;
            }
            set
            {
                àDétruire = value;
            }
        }

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

        protected List<Vector3> ListePoints { get; set; }

    public CubeTexturé(Game game, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, string nomTextureCube, 
            Vector3 dimension, float intervalleMAJ) : base(game, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
        {
            NomTextureCube = nomTextureCube;
            Delta = new Vector3(dimension.X, dimension.Y, dimension.Z);
            DeltaModifié = new Vector3(Delta.X / 2, Delta.Y / 2, Delta.Z / 2);
            DeltaTexture = 1f / 3;
            Origine = new Vector3(0, 0, 0);
        }

        public override void Initialize()
        {
            Sommets = new VertexPositionTexture[NB_SOMMETS];
            Sommets2 = new VertexPositionTexture[NB_SOMMETS];
            ListePoints = new List<Vector3>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Texture = GestionnaireDeTextures.Find(NomTextureCube);
            EffetDeBase = new BasicEffect(GraphicsDevice);
            EffetDeBase.TextureEnabled = true;
            EffetDeBase.Texture = Texture;
            GestionAlpha = BlendState.AlphaBlend;
            base.LoadContent();
        }

        protected override void InitialiserSommets() //vestige pas tres beau du labo sur le cube...
        {
            Vector3 positionSommet = new Vector3(Origine.X - DeltaModifié.X, Origine.Y - DeltaModifié.Y, Origine.Z + DeltaModifié.Z);
            ListePoints.Add(positionSommet);
            Sommets[0] = new VertexPositionTexture(positionSommet, new Vector2(0,1));
            positionSommet = new Vector3(Origine.X - DeltaModifié.X, Origine.Y + DeltaModifié.Y, Origine.Z + DeltaModifié.Z);
            ListePoints.Add(positionSommet);
            Sommets[1] = new VertexPositionTexture(positionSommet, new Vector2(0, 0));
            positionSommet = new Vector3(Origine.X + DeltaModifié.X, Origine.Y - DeltaModifié.Y, Origine.Z + DeltaModifié.Z);
            ListePoints.Add(positionSommet);
            Sommets[2] = new VertexPositionTexture(positionSommet, new Vector2(DeltaTexture, 1));
            positionSommet = new Vector3(Origine.X + DeltaModifié.X, Origine.Y + DeltaModifié.Y, Origine.Z + DeltaModifié.Z);
            ListePoints.Add(positionSommet);
            Sommets[3] = new VertexPositionTexture(positionSommet, new Vector2(DeltaTexture, 0));
            positionSommet = new Vector3(Origine.X + DeltaModifié.X, Origine.Y - DeltaModifié.Y, Origine.Z - DeltaModifié.Z);
            ListePoints.Add(positionSommet);
            Sommets[4] = new VertexPositionTexture(positionSommet, new Vector2(DeltaTexture*2, 1));
            positionSommet = new Vector3(Origine.X + DeltaModifié.X, Origine.Y + DeltaModifié.Y, Origine.Z - DeltaModifié.Z);
            ListePoints.Add(positionSommet);
            Sommets[5] = new VertexPositionTexture(positionSommet, new Vector2(DeltaTexture*2, 0));
            positionSommet = new Vector3(Origine.X - DeltaModifié.X, Origine.Y - DeltaModifié.Y, Origine.Z - DeltaModifié.Z);
            ListePoints.Add(positionSommet);
            Sommets[6] = new VertexPositionTexture(positionSommet, new Vector2(1, 1));
            positionSommet = new Vector3(Origine.X - DeltaModifié.X, Origine.Y + DeltaModifié.Y, Origine.Z - DeltaModifié.Z);
            ListePoints.Add(positionSommet);
            Sommets[7] = new VertexPositionTexture(positionSommet, new Vector2(1, 0));            

            positionSommet = new Vector3(Origine.X + DeltaModifié.X, Origine.Y + DeltaModifié.Y, Origine.Z - DeltaModifié.Z);
            Sommets2[0] = new VertexPositionTexture(positionSommet, new Vector2(0, 1));
            positionSommet = new Vector3(Origine.X + DeltaModifié.X, Origine.Y + DeltaModifié.Y, Origine.Z + DeltaModifié.Z);
            Sommets2[1] = new VertexPositionTexture(positionSommet, new Vector2(0, 0));
            positionSommet = new Vector3(Origine.X - DeltaModifié.X, Origine.Y + DeltaModifié.Y, Origine.Z - DeltaModifié.Z);
            Sommets2[2] = new VertexPositionTexture(positionSommet, new Vector2(DeltaTexture, 1));
            positionSommet = new Vector3(Origine.X - DeltaModifié.X, Origine.Y + DeltaModifié.Y, Origine.Z + DeltaModifié.Z);
            Sommets2[3] = new VertexPositionTexture(positionSommet, new Vector2(DeltaTexture, 0));
            positionSommet = new Vector3(Origine.X - DeltaModifié.X, Origine.Y - DeltaModifié.Y, Origine.Z - DeltaModifié.Z);
            Sommets2[4] = new VertexPositionTexture(positionSommet, new Vector2(DeltaTexture*2, 1));
            positionSommet = new Vector3(Origine.X - DeltaModifié.X, Origine.Y - DeltaModifié.Y, Origine.Z + DeltaModifié.Z);
            Sommets2[5] = new VertexPositionTexture(positionSommet, new Vector2(DeltaTexture*2, 0));
            positionSommet = new Vector3(Origine.X + DeltaModifié.X, Origine.Y - DeltaModifié.Y, Origine.Z - DeltaModifié.Z);
            Sommets2[6] = new VertexPositionTexture(positionSommet, new Vector2(1, 1));
            positionSommet = new Vector3(Origine.X + DeltaModifié.X, Origine.Y - DeltaModifié.Y, Origine.Z + DeltaModifié.Z);
            Sommets2[7] = new VertexPositionTexture(positionSommet, new Vector2(1, 0));
        }

        public override void Draw(GameTime gameTime)
        {
            EffetDeBase.World = GetMonde();
            EffetDeBase.View = CaméraJeu.Vue;
            EffetDeBase.Projection = CaméraJeu.Projection;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Sommets, 0, NB_TRIANGLES);
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Sommets2, 0, NB_TRIANGLES);
            }
        }
    }
}