using AtelierXNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace HyperV
{
    public class CaméraJoueur : Caméra
    {
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        const float ACCÉLÉRATION = 0.001f;
        const float VITESSE_INITIALE_ROTATION = 5f;
        const float VITESSE_INITIALE_ROTATION_SOURIS = 0.1f;
        protected const float VITESSE_INITIALE_TRANSLATION = 0.5f;
        const float DELTA_LACET = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_TANGAGE = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degré à la fois
        const float RAYON_COLLISION = 1f;
        const int HAUTEUR_PERSONNAGE = 10;
        const int FACTEUR_COURSE_MAXIMAL = 4;
        const int DISTANCE_MINIMALE_POUR_RAMASSAGE = 45;

        //Constructeur
        readonly float IntervalleMAJ;
        protected float HauteurDeBase { get; set; }
        Vector2 Origine { get; set; }

        //Initialize
        protected float VitesseTranslation { get; set; }
        //readonly float VitesseRotation;


        public Vector3 Direction { get; private set; }//
        public Vector3 Latéral { get; private set; }//

        Point AnciennePositionSouris { get; set; }
        Point NouvellePositionSouris { get; set; }
        public Vector2 DéplacementSouris { get; private set; }   //**************************

        protected bool DésactiverDéplacement { get; set; }

        protected bool Sauter { get; private set; }
        bool Courrir { get; set; }
        bool Ramasser { get; set; }

        public bool EstCaméraSourisActivée { get; set; }
        bool EstDéplacementEtAutresClavierActivé { get; set; }
        bool EstCaméraClavierActivée { get; set; }

        public Ray Viseur { get; private set; }

        float TempsÉcouléDepuisMAJ { get; set; }

        LifeBar[] BarresDeVie { get; set; }
        InputManager GestionInput { get; set; }
        GamePadManager GestionGamePad { get; set; }

        public CaméraJoueur(Game jeu, Vector3 positionCaméra, Vector3 cible,
                            Vector3 orientation, float intervalleMAJ, float distanceDeRendu)
            : base(jeu)
        {
            CréerPointDeVue(positionCaméra, cible, orientation);
            IntervalleMAJ = intervalleMAJ;
            DistancePlanÉloigné = distanceDeRendu;
            CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DistancePlanÉloigné);

            HauteurDeBase = Position.Y;
            Origine = new Vector2(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height) / 2;
        }

        public void SetRenderDistance(float distanceDeRendu)
        {
            DistancePlanÉloigné = distanceDeRendu;
            CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DistancePlanÉloigné);
        }

        public void InitializeDirection(Vector3 direction)
        {
            Direction = direction;
        }

        public override void Initialize()
        {
            //VitesseRotation = VITESSE_INITIALE_ROTATION;
            VitesseTranslation = VITESSE_INITIALE_TRANSLATION;
            TempsÉcouléDepuisMAJ = 0;

            EstDéplacementEtAutresClavierActivé = true;
            EstCaméraClavierActivée = true;

            Courrir = false;
            Sauter = false;
            Ramasser = false;
            ContinuerSaut= false;
            EstCaméraSourisActivée = true;


            Viseur = new Ray();

            NouvellePositionSouris = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            AnciennePositionSouris = new Point(NouvellePositionSouris.X, NouvellePositionSouris.Y);
            Mouse.SetPosition(NouvellePositionSouris.X, NouvellePositionSouris.Y);

            base.Initialize();
            ChargerContenu();

            InitialiserObjetsComplexesSaut();
            Hauteur = HauteurDeBase;//HAUTEUR_PERSONNAGE;
        }

        protected virtual void ChargerContenu()
        {
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionGamePad = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
            BarresDeVie = Game.Services.GetService(typeof(LifeBar[])) as LifeBar[];
        }

        public bool Dead { get; private set; }

        public void Attack(int val)
        {
            BarresDeVie[0].Attack(val);
        }

        protected override void CréerPointDeVue()
        {
            Direction = Vector3.Normalize(Direction); // NEW FROM 4/7/2017 2:30 AM was only Vector3.Normalize(Direction); before ******************************************************************************************************************************************************************
            Vector3.Normalize(OrientationVerticale);
            Vector3.Normalize(Latéral);
            //Position -= new Vector3(Origin.X, 0, Origin.Y);

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
            AffectCommandsForGrab();
            GérerRamassage();
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                EffectuerMAJ();
                TempsÉcouléDepuisMAJ = 0;
            }
            base.Update(gameTime);
        }

        protected virtual void EffectuerMAJ()
        {
            FonctionsSouris();
            if (!DésactiverDéplacement)
            {
                FonctionsClavier();
            }
            FonctionsGamePad();

            GérerHauteur();
            CréerPointDeVue();

            AffecterCommandes(); // Grab moved to AffectCommandsForGrab()

            //GérerRamassage();
            GérerCourse();
            GérerSaut();

            ManageLifeBars();
        }

        protected virtual void ManageLifeBars()
        {
            if (!BarresDeVie[1].Water)
            {
                if (Courrir && !BarresDeVie[1].Tired && (GestionInput.EstEnfoncée(Keys.W) || GestionInput.EstEnfoncée(Keys.A) || GestionInput.EstEnfoncée(Keys.S) || GestionInput.EstEnfoncée(Keys.D) || GestionGamePad.PositionThumbStickGauche.X!=0 || GestionGamePad.PositionThumbStickGauche.Y != 0))
                {
                    BarresDeVie[1].Attack();
                }
                else
                {
                    BarresDeVie[1].AttackNegative();
                }
            }
        }


        //Souris
        #region
        private void FonctionsSouris()
        {
            if (EstCaméraSourisActivée)
            {
                AnciennePositionSouris = NouvellePositionSouris;
                NouvellePositionSouris = GestionInput.GetPositionSouris();
                DéplacementSouris = new Vector2(NouvellePositionSouris.X - AnciennePositionSouris.X, NouvellePositionSouris.Y - AnciennePositionSouris.Y);

                GérerRotationSouris();

                NouvellePositionSouris = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
                Mouse.SetPosition(NouvellePositionSouris.X, NouvellePositionSouris.Y);
            }
            else
            {
                Game.IsMouseVisible = true;
            }
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
            Matrix matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, DELTA_LACET * VITESSE_INITIALE_ROTATION_SOURIS * -DéplacementSouris.X);

            Direction = Vector3.Transform(Direction, matriceLacet);
        }

        private void GérerTangageSouris()
        {
            Matrix matriceTangage = Matrix.CreateFromAxisAngle(Latéral, DELTA_TANGAGE * VITESSE_INITIALE_ROTATION_SOURIS * -DéplacementSouris.Y);

            Direction = Vector3.Transform(Direction, matriceTangage);
        }
        #endregion

        //Clavier
        #region
        private void FonctionsClavier()
        {
            if (EstDéplacementEtAutresClavierActivé)
            {
                GérerDéplacement((GérerTouche(Keys.W) - GérerTouche(Keys.S)),
                             (GérerTouche(Keys.A) - GérerTouche(Keys.D)));
            }
            if (EstCaméraClavierActivée)
            {
                GérerRotationClavier();
            }
        }

 
        protected virtual void GérerDéplacement(float direction, float latéral)
        {
            float déplacementDirection = direction * VitesseTranslation;
            float déplacementLatéral = latéral * VitesseTranslation;

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

        //GamePad
        #region
        private void FonctionsGamePad()
        {
            if (GestionGamePad.EstGamepadActivé)
            {
                GérerDéplacement(GestionGamePad.PositionThumbStickGauche.Y,
                                 -GestionGamePad.PositionThumbStickGauche.X);

                DéplacementSouris = new Vector2(35, -35) * GestionGamePad.PositionThumbStickDroit;
                GérerRotationSouris();
            }
        }
        #endregion

        private void AffecterCommandes()
        {
            Courrir = (GestionInput.EstEnfoncée(Keys.RightShift) && EstDéplacementEtAutresClavierActivé) ||
                      (GestionInput.EstEnfoncée(Keys.LeftShift) && EstDéplacementEtAutresClavierActivé) ||
                      GestionGamePad.PositionsGâchettes.X > 0;

            Sauter = (GestionInput.EstEnfoncée(/*Keys.R*/Keys.Space) && EstDéplacementEtAutresClavierActivé) ||
                     GestionGamePad.EstEnfoncé(Buttons.A);

            //Ramasser = GestionInput.EstNouveauClicGauche() ||
            //           GestionInput.EstAncienClicGauche() ||
            //           GestionInput.EstNouvelleTouche(Keys.E) && EstDéplacementEtAutresClavierActivé ||
            //           GestionGamePad.EstNouveauBouton(Buttons.RightStick);
        }

        private void AffectCommandsForGrab()
        {
            Ramasser = GestionInput.EstNouveauClicGauche() ||
                       GestionInput.EstAncienClicGauche() ||
                       GestionInput.EstNouvelleTouche(Keys.E) && EstDéplacementEtAutresClavierActivé ||
                       GestionGamePad.EstNouveauBouton(Buttons.X);
        }

        protected virtual void GérerHauteur()
        {
            //Position = Gazon.GetPositionAvecHauteur(Position, (int)Hauteur);
            if (!ContinuerSaut)
            {
                Hauteur = HauteurDeBase;
            }
            Position = new Vector3(Position.X, Hauteur, Position.Z);
        }

        private int GérerTouche(Keys touche)
        {
            return GestionInput.EstEnfoncée(touche) ? 1 : 0;
        }

        private void GérerRamassage()
        {
            Viseur = new Ray(Position, Direction);

            foreach (ModeleRamassable sphereRamassable in Game.Components.Where(composant => composant is ModeleRamassable))
            {
                sphereRamassable.Ramasser = sphereRamassable.EstEnCollision(Viseur) <= DISTANCE_MINIMALE_POUR_RAMASSAGE &&
                           sphereRamassable.EstEnCollision(Viseur) != null && Ramasser;

                if (sphereRamassable.Ramasser && !sphereRamassable.Placed)
                {
                    if (!ModeleRamassable.Taken)
                    {
                        sphereRamassable.EstRamassée = true;
                        ModeleRamassable.Taken = true;
                        break;
                    }
                    else if (sphereRamassable.EstRamassée)
                    {
                        sphereRamassable.EstRamassée = false;
                        ModeleRamassable.Taken = false;
                        break;
                    }
                }
            }

            //NEW
            foreach (Arc sphereRamassable in Game.Components.Where(composant => composant is Arc))
            {
                sphereRamassable.Ramasser = sphereRamassable.EstEnCollision(Viseur) <= DISTANCE_MINIMALE_POUR_RAMASSAGE &&
                           sphereRamassable.EstEnCollision(Viseur) != null && Ramasser;
                
                if (sphereRamassable.Ramasser && !sphereRamassable.Placed)
                {
                    if (/*!ModeleRamassable.Taken*/true)
                    {
                        sphereRamassable.EstRamassée = true;
                        ModeleRamassable.Taken = true;
                        break;
                    }
                    else if (sphereRamassable.EstRamassée)
                    {
                        sphereRamassable.EstRamassée = false;
                        ModeleRamassable.Taken = false;
                        break;
                    }
                }
            }
            //NEW
        }


        //Saut
        #region
        protected virtual void GérerSaut()
        {
            if (Sauter)
            {
                InitialiserObjetsComplexesSaut();
                ContinuerSaut = true;
            }

            if (ContinuerSaut)
            {
                if (t > 60)
                {
                    InitialiserObjetsComplexesSaut();
                    ContinuerSaut = false;
                    t = 0;
                }
                Hauteur = CalculerBesier(t * (1f / 60f), PtsDeControle).Y;
                ++t;
            }
        }

        bool ContinuerSaut { get; set; }
        float t { get; set; }
        protected float Hauteur { get; set; }

        Vector3 PositionPtsDeControle { get; set; }
        Vector3 PositionPtsDeControlePlusUn { get; set; }
        Vector3[] PtsDeControle { get; set; }

        void InitialiserObjetsComplexesSaut()
        {
            Position = new Vector3(Position.X, HauteurDeBase/*HAUTEUR_PERSONNAGE*/, Position.Z);
            PositionPtsDeControle = new Vector3(Position.X, Position.Y, Position.Z);
            PositionPtsDeControlePlusUn = Position + Vector3.Normalize(new Vector3(Direction.X, 0, Direction.Z)) * 25;
            //Position = new Vector3(PositionPtsDeControle.X, PositionPtsDeControle.Y, PositionPtsDeControle.Z);//******
            //Direction = PositionPtsDeControlePlusUn - PositionPtsDeControle;//******
            PtsDeControle = CalculerPointsControle();
        }

        private Vector3[] CalculerPointsControle()
        {
            Vector3[] pts = new Vector3[4];
            pts[0] = PositionPtsDeControle;
            pts[3] = PositionPtsDeControlePlusUn;
            pts[1] = new Vector3(pts[0].X, pts[0].Y + 20, pts[0].Z);
            pts[2] = new Vector3(pts[3].X, pts[3].Y + 20, pts[3].Z);
            return pts;
        }

        private Vector3 CalculerBesier(float t, Vector3[] PtsDeControle)
        {
            float x = (1 - t);
            return PtsDeControle[0] * (x * x * x) +
                   3 * PtsDeControle[1] * t * (x * x) +
                   3 * PtsDeControle[2] * t * t * x +
                   PtsDeControle[3] * t * t * t;

        }
        #endregion

        const float TIRED_SPEED = 0.1f;

        private void GérerCourse()
        {
            VitesseTranslation = BarresDeVie[1].Tired ? TIRED_SPEED : Courrir ? (GestionGamePad.PositionsGâchettes.X > 0 ? GestionGamePad.PositionsGâchettes.X : 1) * FACTEUR_COURSE_MAXIMAL * VITESSE_INITIALE_TRANSLATION : VITESSE_INITIALE_TRANSLATION;
        }
    }
}
