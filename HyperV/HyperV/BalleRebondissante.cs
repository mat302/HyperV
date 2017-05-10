using System;
using AtelierXNA;
using Microsoft.Xna.Framework;

namespace HyperV
{
    class BalleRebondissante : SphèreTexturée
    {
        const int ANGLE_DÉPLACEMENT_DÉPART_MINIMAL = 15,      //Constantes angulaires
                  ANGLE_DÉPLACEMENT_DÉPART_MAXIMAL = 75,
                  ANGLE_DROIT = 90,
                  FACTEUR_MINIMAL_CERCLE_360_DEGRÉS = 0,
                  FACTEUR_MAXIMAL_CERCLE_360_DEGRÉS_EXCLU = 4,
                  ANGLE_PLAT = 180, ANGLE_PLEIN = 360,
                  ABSCISSE_UNITAIRE = 1,
                  ORDONNÉE_UNITAIRE = 1;

        const int DISTANCE_COLLISION = 9,                     //Autres constantes
                  AUCUN_TEMPS_ÉCOULÉ = 0,
                  LONGUEUR_VISEUR = 25,
                  VALEUR_ATTAQUE = 10,
                  VITESSE_MIN_X_DIX = 6,
                  VITESSE_MAX_X_DIX = 10;

        const float ÉCHELLE_RAYON = 1.4f,
                    FACTEUR_VITESSE_INTERVALLE = 0.6f;

        float TempsÉcouléDepuisMAJDéplacement { get; set; }
        float IntervalleMAJDéplacement { get; set; }
        Vector3 VecteurDéplacementMAJ { get; set; }
        Random GénérateurAléatoire { get; set; }
        float AngleDéplacementTeta { get; set; }
        float AngleDéplacementPhi { get; set; }
        public bool estÉliminé { get; private set; }

        Epee Épée { get; set; }
        float Rayon { get; set; }
        CaméraJoueur CameraPrison { get; set; }
        float Vitesse { get; set; }

        int[] Marges { get; set; }

        public BoundingSphere SphèreDeCollisionBalle
        {
            get { return new BoundingSphere(Position, Rayon * ÉCHELLE_RAYON); }
        }

        public BalleRebondissante(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,
                                 float rayon, Vector2 charpente, string nomTexture, float intervalleMAJ)
           : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, rayon, charpente, nomTexture, intervalleMAJ)
        {
            IntervalleMAJDéplacement = intervalleMAJ * FACTEUR_VITESSE_INTERVALLE;
            Position = positionInitiale;
            Rayon = rayon;
            ++Count;
        }

        public override void Initialize()
        {
            base.Initialize();
            Marges = new int[] { -200, 80, -40, 0, -50, 230 };    // MargesX(2),MargesY(2),MargesZ(2)
            CalculerVecteurDéplacement();
            TempsÉcouléDepuisMAJDéplacement = AUCUN_TEMPS_ÉCOULÉ;

        }
        protected override void LoadContent()
        {
            base.LoadContent();
            GénérateurAléatoire = Game.Services.GetService(typeof(Random)) as Random;
            CameraPrison = Game.Services.GetService(typeof(Caméra)) as CaméraJoueur;
            Épée = Game.Services.GetService(typeof(Epee)) as Epee;

            AngleDéplacementTeta = CalculerAngleDéplacementAléatoire();
            AngleDéplacementPhi = CalculerAngleDéplacementAléatoire();
            Vitesse = (GénérateurAléatoire.Next(VITESSE_MIN_X_DIX, VITESSE_MAX_X_DIX)) / 10f;
        }

        public override void Update(GameTime gameTime)
        {
            TempsÉcouléDepuisMAJDéplacement += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisMAJDéplacement >= IntervalleMAJDéplacement)
            {
                CalculerMatriceMonde();
                Position += VecteurDéplacementMAJ;
                GérerCollisionsBalle();
                TempsÉcouléDepuisMAJDéplacement = AUCUN_TEMPS_ÉCOULÉ;

            }
            base.Update(gameTime);
        }

        void CalculerVecteurDéplacement()
        {
            float x = Vitesse * (float)Math.Cos(MathHelper.ToRadians(AngleDéplacementTeta) * (float)Math.Sin(MathHelper.ToRadians(AngleDéplacementPhi)));
            float y = Vitesse * (float)Math.Sin(MathHelper.ToRadians(AngleDéplacementTeta) * (float)Math.Sin(MathHelper.ToRadians(AngleDéplacementPhi)));
            float z = Vitesse * (float)Math.Cos(MathHelper.ToRadians(AngleDéplacementPhi));
            VecteurDéplacementMAJ = new Vector3(x, y, z);
        }

        void Bordures(int borneMin, int borneMax, float positionActuelle, string indicateur)
        {
            borneMin += (int)Math.Ceiling(Rayon);
            borneMax -= (int)Math.Ceiling(Rayon);

            if (positionActuelle <= borneMin || positionActuelle >= borneMax)
            {
                if (indicateur == "x")
                {
                    AngleDéplacementTeta = ANGLE_PLAT + AngleDéplacementTeta;
                    AngleDéplacementPhi = ANGLE_PLAT + AngleDéplacementPhi;
                }
                else if (indicateur == "z")
                {
                    AngleDéplacementPhi = ANGLE_PLAT + AngleDéplacementPhi;

                }
                else if (indicateur == "y")
                {
                    AngleDéplacementTeta = -AngleDéplacementTeta;

                }
                CalculerVecteurDéplacement();
            }
        }

        public static int Count { get; private set; }

        static BalleRebondissante()
        {
            Count = 0;
        }
      // rien
        void GérerCollisionsBalle()
        {
            if (EstEnCollisionBalle(CameraPrison.Viseur) < LONGUEUR_VISEUR && Épée.ContinuerCoupDEpee)
            {
                Game.Components.Remove(this);
                estÉliminé = true;
                --Count;
            }
            if (CollisionBalleCaméra(DISTANCE_COLLISION))
            {
                CameraPrison.Attack(VALEUR_ATTAQUE);
            }

            Bordures(Marges[0], Marges[1], Position.X, "x");
            Bordures(Marges[2], Marges[3], Position.Y, "y");
            Bordures(Marges[4], Marges[5], Position.Z, "z");
        }
        public float? EstEnCollisionBalle(Ray autreObjet)
        {
            return SphèreDeCollisionBalle.Intersects(autreObjet);
        }

        bool CollisionBalleCaméra(float distance)
        {
            return Vector3.Distance(CameraPrison.Position, Position) < distance;
        }

        float CalculerAngleDéplacementAléatoire()
        {
            return GénérateurAléatoire.Next(FACTEUR_MINIMAL_CERCLE_360_DEGRÉS, FACTEUR_MAXIMAL_CERCLE_360_DEGRÉS_EXCLU) * ANGLE_DROIT +
                                   GénérateurAléatoire.Next(ANGLE_DÉPLACEMENT_DÉPART_MINIMAL, ANGLE_DÉPLACEMENT_DÉPART_MAXIMAL);
        }
    }
}
