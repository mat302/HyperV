using AtelierXNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace HyperV
{
    public class CaméraJoueur : Caméra
    {
        const int VALEUR_SECONDE = 60;
        const int FACTEUR_COURSE_MAXIMAL = 4;
        const int DISTANCE_MINIMALE_POUR_RAMASSAGE = 45;
        const int HAUTEUR_SAUT = 10;
        const int SAUT = 25;
        const int VALEURE_VECTORIELLE_DÉPLACEMENT_GAMEPAD = 35;
        const float VITESSE_LORSQUE_FATIGUÉ = 0.1f;
        const float VITESSE_INITIALE_ROTATION = 5f;
        const float VITESSE_INITIALE_ROTATION_SOURIS = 0.1f;
        protected const float VITESSE_INITIALE_TRANSLATION = 0.5f;
        const float DELTA_LACET = MathHelper.Pi / 180; 
        const float DELTA_TANGAGE = MathHelper.Pi / 180; 


        //CONSTRUCTEUR
        readonly float IntervalleMAJ;
        protected float HauteurDeBase { get; set; }
        Vector2 Origine { get; set; }
        //CréerPointDeVue
        public Vector3 Direction { get; private set; }
        public Vector3 Latéral { get; private set; }


        //INITIALIZE
        //Souris
        Point AnciennePositionSouris { get; set; }
        Point NouvellePositionSouris { get; set; }
        public Vector2 DéplacementSourisOuStickGamePad { get; private set; }//Cette protection pour la catapulte
        //Déplacement
        protected float VitesseTranslation { get; set; }
        //Actions joueur
        protected bool Sauter { get; private set; }
        bool Courrir { get; set; }
        bool Ramasser { get; set; }
        //Activé
        protected bool DésactiverCertainesCommandes { get; set; } //Utile pour le niveau catapulte
        public bool EstCaméraSourisActivée { get; set; }
        public bool EstCaméraClavierActivée { get; set; }
        public bool EstDéplacementEtCommandesClavierActivé { get; set; }
        public bool EstMort { get; private set; }
        //Autres
        public Ray Viseur { get; private set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        //*Saut*
        bool ContinuerSaut { get; set; }
        float t { get; set; }
        protected float Hauteur { get; set; }
        Vector3 PositionPtsDeControle { get; set; }
        Vector3 PositionPtsDeControlePlusUn { get; set; }
        Vector3[] PtsDeControle { get; set; }


        //ChargerContenu
        InputManager GestionInput { get; set; }
        GamePadManager GestionGamePad { get; set; }
        LifeBar[] BarresDeVie { get; set; }


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

        public override void Initialize()
        {
            //Souris
            NouvellePositionSouris = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            AnciennePositionSouris = new Point(NouvellePositionSouris.X, NouvellePositionSouris.Y);
            Mouse.SetPosition(NouvellePositionSouris.X, NouvellePositionSouris.Y);
            DéplacementSourisOuStickGamePad = Vector2.Zero;

            //Déplacement
            VitesseTranslation = VITESSE_INITIALE_TRANSLATION;

            //Actions joueur
            Courrir = false;
            Sauter = false;
            Ramasser = false;

            //Activé
            DésactiverCertainesCommandes = false;
            EstDéplacementEtCommandesClavierActivé = true;
            EstCaméraClavierActivée = true;
            EstCaméraSourisActivée = true;
            EstMort = false;

            //Autres
            Viseur = new Ray();
            
            TempsÉcouléDepuisMAJ = 0;

            //*Saut*
            ContinuerSaut = false;
            t = 0;
            Hauteur = HauteurDeBase;
            InitialiserObjetsComplexesSaut();

            base.Initialize();
            ChargerContenu();
        }

        protected virtual void ChargerContenu()
        {
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionGamePad = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
            BarresDeVie = Game.Services.GetService(typeof(LifeBar[])) as LifeBar[];
        }

        protected override void CréerPointDeVue()
        {
            Direction = Vector3.Normalize(Direction);
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

        public void ÉtablirDistenceDeRendu(float distanceDeRendu)
        {
            DistancePlanÉloigné = distanceDeRendu;
            CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DistancePlanÉloigné);
        }

        public void ÉtablirDirection(Vector3 direction)
        {
            Direction = direction;
        }

        public void Attaquer(int val)
        {
            BarresDeVie[0].Attack(val);
        }

        protected virtual void GérerBarresDeVie()
        {
            if (!BarresDeVie[1].Water)
            {
                if (Courrir && !BarresDeVie[1].Tired && (GestionInput.EstEnfoncée(Keys.W) ||
                    GestionInput.EstEnfoncée(Keys.A) || GestionInput.EstEnfoncée(Keys.S) ||
                    GestionInput.EstEnfoncée(Keys.D) || GestionGamePad.PositionThumbStickGauche.X != 0 ||
                    GestionGamePad.PositionThumbStickGauche.Y != 0))
                {
                    BarresDeVie[1].Attack();
                }
                else
                {
                    BarresDeVie[1].AttackNegative();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            AffecterCommandes();
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
            if (!DésactiverCertainesCommandes)
            {
                FonctionsClavier();
            }
            FonctionsGamePad();

            GérerHauteur();
            CréerPointDeVue();


            GérerRamassage();
            GérerCourse();
            GérerSaut();

            GérerBarresDeVie();
        }


        //Souris
        #region
        private void FonctionsSouris()
        {
            if (EstCaméraSourisActivée)
            {
                AnciennePositionSouris = NouvellePositionSouris;
                NouvellePositionSouris = GestionInput.GetPositionSouris();
                DéplacementSourisOuStickGamePad = new Vector2(NouvellePositionSouris.X - AnciennePositionSouris.X, NouvellePositionSouris.Y - AnciennePositionSouris.Y);

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
            if (!DésactiverCertainesCommandes)
            {
                GérerTangageSouris();
            }
        }

        private void GérerLacetSouris()
        {
            Matrix matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, DELTA_LACET * VITESSE_INITIALE_ROTATION_SOURIS * -DéplacementSourisOuStickGamePad.X);

            Direction = Vector3.Transform(Direction, matriceLacet);
        }

        private void GérerTangageSouris()
        {
            Matrix matriceTangage = Matrix.CreateFromAxisAngle(Latéral, DELTA_TANGAGE * VITESSE_INITIALE_ROTATION_SOURIS * -DéplacementSourisOuStickGamePad.Y);

            Direction = Vector3.Transform(Direction, matriceTangage);
        }
        #endregion

        //Clavier
        #region
        private void FonctionsClavier()
        {
            if (EstDéplacementEtCommandesClavierActivé)
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

                DéplacementSourisOuStickGamePad = new Vector2(VALEURE_VECTORIELLE_DÉPLACEMENT_GAMEPAD,
                                                            -VALEURE_VECTORIELLE_DÉPLACEMENT_GAMEPAD) * GestionGamePad.PositionThumbStickDroit;
                GérerRotationSouris();//Fonctionne avec variable précédente, donc rotation Gamepad aussi 
            }
        }
        #endregion

        private void AffecterCommandes()
        {
            Courrir = (GestionInput.EstEnfoncée(Keys.RightShift) && EstDéplacementEtCommandesClavierActivé) ||
                      (GestionInput.EstEnfoncée(Keys.LeftShift) && EstDéplacementEtCommandesClavierActivé) ||
                      GestionGamePad.PositionsGâchettes.X > 0;

            Sauter = (GestionInput.EstEnfoncée(Keys.Space) && EstDéplacementEtCommandesClavierActivé) ||
                     GestionGamePad.EstEnfoncé(Buttons.A);

            Ramasser = GestionInput.EstNouveauClicGauche() ||
                       GestionInput.EstAncienClicGauche() ||
                       GestionInput.EstNouvelleTouche(Keys.E) && EstDéplacementEtCommandesClavierActivé ||
                       GestionGamePad.EstNouveauBouton(Buttons.RightStick) || Ramasser;
        }


        protected virtual void GérerHauteur()
        {
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

                if (sphereRamassable.Ramasser && !sphereRamassable.Placé)
                {
                    if (!ModeleRamassable.DéjàPris)
                    {
                        sphereRamassable.EstRamassée = true;
                        ModeleRamassable.DéjàPris = true;
                        break;
                    }
                    else 
                    {
                        sphereRamassable.EstRamassée = false;
                        ModeleRamassable.DéjàPris = false;
                        break;
                    }
                }
            }
            Ramasser = false;
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
                if (t > VALEUR_SECONDE)
                {
                    InitialiserObjetsComplexesSaut();
                    ContinuerSaut = false;
                    t = 0;
                }
                Hauteur = CalculerBesier(t * IntervalleMAJ, PtsDeControle).Y;
                ++t;
            }
        }

        void InitialiserObjetsComplexesSaut()
        {
            Position = new Vector3(Position.X, HauteurDeBase, Position.Z);
            PositionPtsDeControle = new Vector3(Position.X, Position.Y, Position.Z);
            PositionPtsDeControlePlusUn = Position + Vector3.Normalize(new Vector3(Direction.X, 0, Direction.Z)) * SAUT;
            PtsDeControle = CalculerPointsControle();
        }

        private Vector3[] CalculerPointsControle()
        {
            Vector3[] pts = new Vector3[4];
            pts[0] = PositionPtsDeControle;
            pts[3] = PositionPtsDeControlePlusUn;
            pts[1] = new Vector3(pts[0].X, pts[0].Y + HAUTEUR_SAUT, pts[0].Z);
            pts[2] = new Vector3(pts[3].X, pts[3].Y + HAUTEUR_SAUT, pts[3].Z);
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

        

        private void GérerCourse()
        {
            VitesseTranslation = BarresDeVie[1].Tired ? VITESSE_LORSQUE_FATIGUÉ : Courrir ? (GestionGamePad.PositionsGâchettes.X > 0 ? GestionGamePad.PositionsGâchettes.X : 1) * FACTEUR_COURSE_MAXIMAL * VITESSE_INITIALE_TRANSLATION : VITESSE_INITIALE_TRANSLATION;
        }
    }
}
