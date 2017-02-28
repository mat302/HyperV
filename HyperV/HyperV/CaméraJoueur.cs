using AtelierXNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace HyperV
{
    public class CaméraJoueur : AtelierXNA.Caméra
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
        const int HAUTEUR_PERSONNAGE = 10;

        public Vector3 Direction { get; private set; }//
        public Vector3 Latéral { get; private set; }//
        Gazon Gazon { get; set; }
        float VitesseTranslation { get; set; }
        float VitesseRotation { get; set; }
        Point AnciennePositionSouris { get; set; }
        Point NouvellePositionSouris { get; set; }
        Vector2 DéplacementSouris { get; set; }

        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        InputManager GestionInput { get; set; }

        public CaméraJoueur(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ)
           : base(jeu)
        {
            IntervalleMAJ = intervalleMAJ;
            CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
            CréerPointDeVue(positionCaméra, cible, orientation);
        }

        public override void Initialize()
        {
            VitesseRotation = VITESSE_INITIALE_ROTATION;
            VitesseTranslation = VITESSE_INITIALE_TRANSLATION;
            TempsÉcouléDepuisMAJ = 0;
            base.Initialize();
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Gazon = Game.Services.GetService(typeof(Gazon)) as Gazon;
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

            Vector3.Normalize(Cible);

            CréerPointDeVue();
        }

        public override void Update(GameTime gameTime)
        {
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                FonctionsSouris();
                FonctionsClavier();

                GérerHauteur();
                CréerPointDeVue();


                GérerRamassage();

                //Game.Window.Title = Position.ToString();

                TempsÉcouléDepuisMAJ = 0;
            }
            base.Update(gameTime);
        }

        //Souris
        #region
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
            GérerTangageSouris();
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

        //Clavier
        #region
        private void FonctionsClavier()
        {
            GérerDéplacement();
            GérerRotationClavier();
        }

        private void GérerDéplacement()
        {
            float déplacementDirection = (GérerTouche(Keys.W) - GérerTouche(Keys.S)) * VitesseTranslation;
            float déplacementLatéral = (GérerTouche(Keys.A) - GérerTouche(Keys.D)) * VitesseTranslation;

            Direction = Vector3.Normalize(Direction);
            Position += déplacementDirection * Direction;

            Latéral = Vector3.Cross(Direction, OrientationVerticale);
            Position -= déplacementLatéral * Latéral;
        }

        private void GérerRotationClavier()
        {
            GérerLacetClavier();
            GérerTangageClavier();
        }

        private void GérerLacetClavier()
        {
            Matrix matriceLacet = Matrix.Identity;

            if (GestionInput.EstEnfoncée(Keys.Left))
            {
                matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, DELTA_LACET * VITESSE_INITIALE_ROTATION);
            }
            if (GestionInput.EstEnfoncée(Keys.Right))
            {
                matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, -DELTA_LACET * VITESSE_INITIALE_ROTATION);
            }

            Direction = Vector3.Transform(Direction, matriceLacet);
        }

        private void GérerTangageClavier()
        {
            Matrix matriceTangage = Matrix.Identity;

            if (GestionInput.EstEnfoncée(Keys.Down))
            {
                matriceTangage = Matrix.CreateFromAxisAngle(Latéral, -DELTA_TANGAGE * VITESSE_INITIALE_ROTATION);
            }
            if (GestionInput.EstEnfoncée(Keys.Up))
            {
                matriceTangage = Matrix.CreateFromAxisAngle(Latéral, DELTA_TANGAGE * VITESSE_INITIALE_ROTATION);
            }

            Direction = Vector3.Transform(Direction, matriceTangage);
        }
        #endregion

        private void GérerHauteur()
        {
            Position = Gazon.GetPositionAvecHauteur(Position, HAUTEUR_PERSONNAGE);
        }

        private int GérerTouche(Keys touche)
        {
            return GestionInput.EstEnfoncée(touche) ? 1 : 0;
        }

        private void GérerRamassage()
        {
            Ray viseur = new Ray(Position, Direction);

            foreach (SphèreRamassable sphereRamassable in Game.Components.Where(composant => composant is SphèreRamassable))
            {
                Game.Window.Title = sphereRamassable.EstEnCollision(viseur).ToString();
                if (sphereRamassable.EstEnCollision(viseur) <= 45 && 
                    GestionInput.EstNouveauClicGauche()|| GestionInput.EstAncienClicGauche())
                {
                    sphereRamassable.EstRamassée = true;
                }
            }
        }
    }
}
