using AtelierXNA;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace HyperV
{
    public class Camera3 : Caméra
    {
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        const float ACCÉLÉRATION = 0.001f;
        const float VITESSE_INITIALE_ROTATION = 5f;
        const float VITESSE_INITIALE_ROTATION_SOURIS = 0.1f;
        const float VITESSE_INITIALE_TRANSLATION = 0.5f;
        const float DELTA_LACET = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_TANGAGE = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degré à la fois
        const float RAYON_COLLISION = 1f;
        const int HAUTEUR_PERSONNAGE = -6;

        bool EnColision { get; set; }
        bool placerJoueur { get; set; }
        public Vector3 Direction { get; private set; }
        Vector3 Latéral { get; set; }
        Grass Grass { get; set; }
        Walls Walls { get; set; }
        float VitesseTranslation { get; set; }
        float VitesseRotation { get; set; }
        Point AnciennePositionSouris { get; set; }
        Point NouvellePositionSouris { get; set; }
        public Vector2 DéplacementSouris { get; set; }
        Vector3 PositionSauvegardée { get; set; }

        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        InputManager GestionInput { get; set; }
        float Height { get; set; }
        List<Character> Characters { get; set; }
        bool DésactiverDéplacement { get; set; }

        public Camera3(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ) : base(jeu)
        {
            IntervalleMAJ = intervalleMAJ;
            CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
            CréerPointDeVue(positionCaméra, cible, orientation);
            Height = positionCaméra.Y;
            Position = positionCaméra;
        }

        public override void Initialize()
        {
            EnColision = false;
            DésactiverDéplacement = false;
            VitesseRotation = VITESSE_INITIALE_ROTATION;
            VitesseTranslation = VITESSE_INITIALE_TRANSLATION;
            TempsÉcouléDepuisMAJ = 0;
            base.Initialize();
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Grass = Game.Services.GetService(typeof(Grass)) as Grass;
            Walls = Game.Services.GetService(typeof(Walls)) as Walls;
            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
            NouvellePositionSouris = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            AnciennePositionSouris = new Point(NouvellePositionSouris.X, NouvellePositionSouris.Y);
            Mouse.SetPosition(NouvellePositionSouris.X, NouvellePositionSouris.Y);
        }

        protected override void CréerPointDeVue()
        {
            Vector3.Normalize(Direction);
            Vector3.Normalize(OrientationVerticale);
            Vector3.Normalize(Latéral);

            Vue = Matrix.CreateLookAt(Position, Position + Direction, OrientationVerticale);
        }

        protected override void CréerPointDeVue(Vector3 position, Vector3 cible, Vector3 orientation)
        {
            Position = position;
            Cible = cible;
            OrientationVerticale = orientation;

            Direction = cible - Position;
            //Direction = cible;

            Vector3.Normalize(Cible);

            CréerPointDeVue();
        }

        public override void Update(GameTime gameTime)
        {
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                List<CreateurModele> Models = new List<CreateurModele>();
                foreach (CreateurModele modele in Game.Components.Where(x => x is ICollisionable))
                {
                    Models.Add(modele);
                }
                //if (!EnColision)
                {
                    if (!DésactiverDéplacement)
                    {
                        if (placerJoueur)
                        {
                            placerJoueur = false;
                            Position = new Vector3(-27, 2, -28);
                        }
                        FonctionsClavier();
                    }
                    if (DésactiverDéplacement)
                    {
                        PositionSauvegardée = Position;
                        Position = new Vector3(-57, 15, -52);
                        placerJoueur = true;
                    }
                    FonctionsSouris();

                    GérerHauteur();
                    CréerPointDeVue();
                    
                    Position = new Vector3(Position.X, Height, Position.Z);
                }

                TempsÉcouléDepuisMAJ = 0;
            }
            base.Update(gameTime);
        }
        
        #region Souris
        private void FonctionsSouris()
        {
            AnciennePositionSouris = NouvellePositionSouris;
            NouvellePositionSouris = GestionInput.GetPositionSouris();
            DéplacementSouris = new Vector2(NouvellePositionSouris.X - AnciennePositionSouris.X,
                                            NouvellePositionSouris.Y - AnciennePositionSouris.Y);

            GérerRotationSouris();

            NouvellePositionSouris = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            Mouse.SetPosition(NouvellePositionSouris.X, NouvellePositionSouris.Y);

        }

        private void GérerRotationSouris()
        {
            GérerLacetSouris();
            if (!DésactiverDéplacement)
            {
                GérerTangageSouris();
            }
        }

        private void GérerLacetSouris()
        {
            Matrix matriceLacet = Matrix.Identity;

            matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, DELTA_LACET * VITESSE_INITIALE_ROTATION_SOURIS * -DéplacementSouris.X);

            Direction = Vector3.Transform(Direction, matriceLacet);
        }

        private void GérerTangageSouris()
        {
            Matrix matriceTangage = Matrix.Identity;

            matriceTangage = Matrix.CreateFromAxisAngle(Latéral, DELTA_TANGAGE * VITESSE_INITIALE_ROTATION_SOURIS * -DéplacementSouris.Y);

            Direction = Vector3.Transform(Direction, matriceTangage);
        }
        #endregion
        
        #region Clavier
        private void FonctionsClavier()
        {
            GérerDéplacement();
        }

        private void GérerDéplacement()
        {
            float déplacementDirection = (GérerTouche(Keys.W) - GérerTouche(Keys.S)) * VitesseTranslation;
            float déplacementLatéral = (GérerTouche(Keys.A) - GérerTouche(Keys.D)) * VitesseTranslation;

            Direction = Vector3.Normalize(Direction);
            Latéral = Vector3.Cross(Direction, OrientationVerticale);
            Position += déplacementDirection * Direction;
            Position -= déplacementLatéral * Latéral;
            //if (Walls.CheckForCollisions(Position) || CheckForCharacterCollision())
            //{
            //    Position -= déplacementDirection * Direction;
            //    Position += déplacementLatéral * Latéral;
            //}
        }

        const float MAX_DISTANCE = 4.5f;

        bool CheckForCharacterCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Characters.Count && !result; ++i)
            {
                result = Vector3.Distance(Characters[i].GetPosition(), Position) < MAX_DISTANCE;
            }

            return result;
        }

        #endregion

        private void GérerHauteur()
        {
            //Position = Grass.GetPositionWithHeight(Position, HAUTEUR_PERSONNAGE);
        }

        private int GérerTouche(Keys touche)
        {
            return GestionInput.EstEnfoncée(touche) ? 1 : 0;
        }

        public void DésactiverCaméra()
        {
            DésactiverDéplacement = !DésactiverDéplacement;
            Direction = new Vector3(1, 0, 0);
        }

        private List<Vector3> CreerBoite()
        {
            List<Vector3> Liste = new List<Vector3>();
            Liste.Add(new Vector3(Position.X, Position.Y +0.01f, Position.Z));
            Liste.Add(new Vector3(Position.X, Position.Y - 0.01f, Position.Z));
            return Liste;
        }
    }
}
