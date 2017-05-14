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
using System.IO;

namespace HyperV
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class UnlockableWall : PrimitiveDeBase
    {
        Vector3 Position { get; set; }
        float IntervalleMAJ { get; set; }
        protected InputManager GestionInput { get; private set; }
        float TempsÉcouléDepuisMAJ { get; set; }

        const int NB_TRIANGLES = 2;
        protected Vector3[,] PtsSommets { get; private set; }
        Vector3 Origine { get; set; }
        Vector2 Delta { get; set; }
        protected BasicEffect EffetDeBase { get; private set; }

        //VertexPositionColor[] Sommets { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures;
        Texture2D TextureTuile;
        VertexPositionTexture[] Sommets { get; set; }
        BlendState GestionAlpha { get; set; }

        Vector2[,] PtsTexture { get; set; }
        string NomTextureTuile { get; set; }
        Vector3 CentrePosition { get; set; }

        Vector3 PlanePoint { get; set; }
        Vector2 FirstVertex { get; set; }
        Vector2 SecondVertex { get; set; }
        Vector3 PlaneEquation { get; set; }
        float Magnitude { get; set; }
        int NombreRunesOuvrirMur { get; set; }
        int NombreNiveauxComplétés { get; set; }
        int NumeroSave { get; set; }

        List<Rune> ListeRunes { get; set; }
        PuzzleBouton PuzzleBoutons { get; set; }


        private bool EstEntre(float valeur, float borneA, float borneB)
        {
            return (valeur >= borneA && valeur <= borneB || valeur <= borneA && valeur >= borneB);
        }

        public UnlockableWall(Game game, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomTextureTuile, float intervalleMAJ, int nombreRunesOuvrirMur, int nombreNiveauxComplétés, List<Rune> listeRunes, int numeroSave) : base(game, homothétieInitiale, rotationInitiale, positionInitiale)
        {
            NomTextureTuile = nomTextureTuile;
            IntervalleMAJ = intervalleMAJ;
            Delta = new Vector2(étendue.X, étendue.Y);
            Origine = new Vector3(0, -Delta.Y / 2, -Delta.X / 2); //pour centrer la primitive au point (0,0,0)
            NombreRunesOuvrirMur = nombreRunesOuvrirMur;
            NombreNiveauxComplétés = nombreNiveauxComplétés;
            ListeRunes = listeRunes;
            NumeroSave = numeroSave;
        }

        public override void Initialize()
        {
            NbSommets = NB_TRIANGLES + 2;
            PtsSommets = new Vector3[2, 2];
            CréerTableauPoints();
            CréerTableauSommets();
            Position = PositionInitiale;
            CentrePosition = Position + Origine;

            if (RotationInitiale.Y == -1.570796f)
            {
                FirstVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(PtsSommets[0, 0].Z, PtsSommets[0, 0].X);
                SecondVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(-PtsSommets[1, 1].Z, PtsSommets[1, 1].X);
            }
            else if (RotationInitiale.Y == 3.141593f)
            {
                FirstVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(PtsSommets[0, 0].X, PtsSommets[0, 0].Z);
                SecondVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(-PtsSommets[1, 1].X, -PtsSommets[1, 1].Z);
            }
            else if (RotationInitiale.Y == 1.570796f)
            {
                FirstVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(PtsSommets[0, 0].Z, PtsSommets[0, 0].X);
                SecondVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(PtsSommets[1, 1].Z, -PtsSommets[1, 1].X);
            }
            else if (RotationInitiale.Y == 0)
            {
                FirstVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(PtsSommets[0, 0].X, PtsSommets[0, 0].Z);
                SecondVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(PtsSommets[1, 1].X, PtsSommets[1, 1].Z);
            }
            PlanePoint = new Vector3(SecondVertex.X, 0, SecondVertex.Y);
            Vector2 u2 = SecondVertex - FirstVertex;
            Vector3 u = new Vector3(u2.X, 0, u2.Y);
            Vector3 v = new Vector3(0, Delta.Y * 2, 0);
            PlaneEquation = Vector3.Cross(u, v);
            Magnitude = PlaneEquation.Length();
            base.Initialize();
        }
        
        private void CréerTableauPoints()
        {
            PtsSommets[0, 0] = new Vector3(Origine.X, Origine.Y, Origine.Z);
            PtsSommets[1, 0] = new Vector3(Origine.X, Origine.Y, Origine.Z - Delta.X);
            PtsSommets[0, 1] = new Vector3(Origine.X, Origine.Y + Delta.Y, Origine.Z);
            PtsSommets[1, 1] = new Vector3(Origine.X, Origine.Y + Delta.Y, Origine.Z - Delta.X);
        }

        protected void CréerTableauSommets()
        {
            PtsTexture = new Vector2[2, 2];
            Sommets = new VertexPositionTexture[NbSommets];
            CréerTableauPointsTexture();
        }

        private void CréerTableauPointsTexture()
        {
            PtsTexture[0, 0] = new Vector2(0, 1);
            PtsTexture[1, 0] = new Vector2(1, 1);
            PtsTexture[0, 1] = new Vector2(0, 0);
            PtsTexture[1, 1] = new Vector2(1, 0);
        }

        protected override void LoadContent()
        {
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            PuzzleBoutons = Game.Services.GetService(typeof(PuzzleBouton)) as PuzzleBouton;
            TextureTuile = GestionnaireDeTextures.Find(NomTextureTuile);
            EffetDeBase = new BasicEffect(GraphicsDevice);
            InitialiserParamètresEffetDeBase();
            base.LoadContent();
        }

        protected void InitialiserParamètresEffetDeBase()
        {
            //EffetDeBase.VertexColorEnabled = true;
            EffetDeBase.TextureEnabled = true;
            EffetDeBase.Texture = TextureTuile;
            GestionAlpha = BlendState.AlphaBlend; // Attention à ceci...
        }

        protected override void InitialiserSommets() // Est appelée par base.Initialize()
        {
            int NoSommet = -1;
            for (int j = 0; j < 1; ++j)
            {
                for (int i = 0; i < 2; ++i)
                {
                    Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j], PtsTexture[i, j]);
                    Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[i, j + 1]);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            BlendState oldBlendState = GraphicsDevice.BlendState; // ... et à cela!
            GraphicsDevice.BlendState = GestionAlpha;
            EffetDeBase.World = GetMonde();
            EffetDeBase.View = CaméraJeu.Vue;
            EffetDeBase.Projection = CaméraJeu.Projection;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                DessinerTriangleStrip();
            }
            GraphicsDevice.BlendState = oldBlendState;
        }

        protected void DessinerTriangleStrip()
        {
            GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Sommets, 0, NB_TRIANGLES);
        }

        public Vector3 GetPositionWithHeight(Vector3 position, int hauteur)
        {
            Vector3 positionAvecHauteur;
            if (EstEntre(position.Z, PtsSommets[0, 0].Z, PtsSommets[PtsSommets.GetLength(0) - 1, PtsSommets.GetLength(1) - 1].Z) &&
                EstEntre(position.X, PtsSommets[0, 0].X, PtsSommets[PtsSommets.GetLength(0) - 1, PtsSommets.GetLength(1) - 1].X))
            {
                positionAvecHauteur = new Vector3(position.X, PtsSommets[0, 0].Y + hauteur, position.Z);
            }
            else
            {
                positionAvecHauteur = position;
            }
            return positionAvecHauteur;
        }

        const int MAX_DISTANCE = 1;

        public bool CheckForCollisions(Vector3 Position)
        {
            Vector3 AP;
            bool result = false;
            float wallDistance;
            if (Visible)
            {
                AP = Position - PlanePoint;
                wallDistance = Vector2.Distance(FirstVertex, SecondVertex);
                result = Math.Abs(Vector3.Dot(AP, PlaneEquation)) / Magnitude < MAX_DISTANCE && (Position - new Vector3(FirstVertex.X, Position.Y, FirstVertex.Y)).Length() < wallDistance && (Position - new Vector3(SecondVertex.X, Position.Y, SecondVertex.Y)).Length() < wallDistance;
            }
            return result;
        }

        public bool PuzzleRunesComplete()
        {
            StreamReader save = new StreamReader("../../../WPFINTERFACE/Launching Interface/Saves/PuzzlesSave" + NumeroSave + ".txt");
            save.ReadLine();
            string ligneSave = save.ReadLine();
            return (ListeRunes[0].EstActivée && !ListeRunes[1].EstActivée && ListeRunes[2].EstActivée && !ListeRunes[3].EstActivée && !ListeRunes[4].EstActivée && ListeRunes[5].EstActivée) || (ligneSave == "True");
        }

        public override void Update(GameTime gameTime)
        {
            if (NombreRunesOuvrirMur <= NombreNiveauxComplétés)
            {
                if (NombreRunesOuvrirMur == 1 && PuzzleRunesComplete())
                {
                    Visible = false;
                    Game.Components.Remove(this);
                }
                if (NombreRunesOuvrirMur == 2 || NombreRunesOuvrirMur == 5)
                {
                    Visible = false;
                    Game.Components.Remove(this);
                }
                if (NombreRunesOuvrirMur == 3 && PuzzleBoutons.EstComplété)
                {
                    Visible = false;
                    Game.Components.Remove(this);
                }
            }
        }
    }
}
