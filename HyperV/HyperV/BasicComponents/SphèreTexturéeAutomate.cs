//
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtelierXNA
{
    public class SphèreTexturéeAutomate : SphèreTexturée
    {
        DataPiste DataPiste { get; set; }
        TerrainAvecBase Terrain { get; set; }

        List<Vector2> ListePointsPatrouilles { get; set; }
        Vector3[] PtsDeControle { get; set; }

        float t { get; set; }
        bool Pause { get; set; }

        Vector3 Direction { get; set; }
        Vector3 Position0 { get; set; }
        Vector3 PositionPlus1 { get; set; }
        Vector3 Déplacement { get; set; }

        public SphèreTexturéeAutomate(Game jeu, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float rayon, Vector2 charpente, string nomTexture, float intervalleMAJ) : base(jeu, échelleInitiale, rotationInitiale, positionInitiale, rayon, charpente, nomTexture, intervalleMAJ)
        { }

        public override void Initialize()
        {
            base.Initialize();
            Pause = false;
            t = 0;
            PtsDeControle = new Vector3[4];
            ListePointsPatrouilles = DataPiste.GetPointsDePatrouille();

            Position0 = Terrain.GetPointSpatial((int)ListePointsPatrouilles[0].X, Terrain.NbRangées - (int)ListePointsPatrouilles[0].Y);
            PositionPlus1 = Terrain.GetPointSpatial((int)ListePointsPatrouilles[1].X, Terrain.NbRangées - (int)ListePointsPatrouilles[1].Y);
            Position = new Vector3(Position0.X, Position0.Y, Position0.Z);
            cpt = 1;
            AngleAxe = 0;

            Direction = PositionPlus1 - Position0;
            PtsDeControle = CalculerPointsControle();
        }

        protected override void LoadContent()
        {
            DataPiste = Game.Services.GetService(typeof(DataPiste)) as DataPiste;
            Terrain = Game.Services.GetService(typeof(TerrainAvecBase)) as TerrainAvecBase;
            base.LoadContent();
        }

        protected override void EffectuerMiseÀJour()
        {
            if (Pause)
            {
                SuivrePointsPatrouilles();
                GérerRotationSphère();
                GérerCaméra();
            }
            base.EffectuerMiseÀJour();
        }

        int cpt { get; set; }

        void SuivrePointsPatrouilles() //passe ici tout les 1/60 de seconde
        {                       
            if (t > 60)//on veux que ce soit true apres 1 seconde, donc 60 cycles
            {
                Position0 = Terrain.GetPointSpatial((int)ListePointsPatrouilles[cpt].X, Terrain.NbRangées - (int)ListePointsPatrouilles[cpt].Y); //point ou se trouve la balle
                PositionPlus1 = Terrain.GetPointSpatial((int)ListePointsPatrouilles[cpt + 1].X, Terrain.NbRangées - (int)ListePointsPatrouilles[cpt + 1].Y); //destination
                Direction = PositionPlus1 - Position0; //vecteur reliant les deux points
                Position = Position0;
                ++cpt;
                if(cpt > ListePointsPatrouilles.Count - 2)
                {
                    cpt = 0;
                }
                PtsDeControle = CalculerPointsControle(); //besoin de nouveau pts de controle pour les nouveaux points
                t = 0; //remet t a 0 pour un nouveau cycle de points
            }
            //Position += Direction * 1f / 60f;          //si courbe marche pas
            Position = CalculerCourbeDeBezier(t*(1f/60f), PtsDeControle);
            ++t;
        }

        private Vector3[] CalculerPointsControle()
        {
            Vector3[] pts = new Vector3[4];
            pts[0] = Position0;
            pts[3] = PositionPlus1;
            pts[1] = new Vector3(pts[0].X, pts[0].Y + 10, pts[0].Z);
            pts[2] = new Vector3(pts[3].X, pts[3].Y + 10, pts[3].Z);
            return pts;
        }

        private Vector3 CalculerCourbeDeBezier(float t, Vector3[] PtsDeControle)
        {
            float x = (1 - t);
            return PtsDeControle[0] * (float)Math.Pow(x, 3) + 3 * PtsDeControle[1] * t * (float)Math.Pow(x, 2) + 3 * PtsDeControle[2] * t * t * x + PtsDeControle[3] * t * t * t;
        }

        void GérerCaméra()
        {
            Vector3 déplacement = Vector3.Normalize(Direction) * -25 + Vector3.Up * 10;

            Vector3 positionCaméra = Position + déplacement;

            CaméraJeu.Déplacer(positionCaméra, Déplacement + Position, Vector3.Up);
        }

        protected override void GérerClavier()
        {
            base.GérerClavier();
            if (GestionInput.EstNouvelleTouche(Keys.Space))
            {
                Pause = !Pause;
            }
        }

        int AngleAxe { get; set; }

        void GérerRotationSphère()
        {
            CalculerMatriceMonde(Vector3.Cross(Vector3.Normalize(Direction), Vector3.Up), 6 * (--AngleAxe));
        }

        protected void CalculerMatriceMonde(Vector3 axe, float angle)
        {
            Monde = Matrix.Identity *
                    Matrix.CreateScale(Homothétie) *
                    Matrix.CreateFromAxisAngle(axe, MathHelper.ToRadians(angle)) *
                    Matrix.CreateTranslation(Position);
        }
    }
}
