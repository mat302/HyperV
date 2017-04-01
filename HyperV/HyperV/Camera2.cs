//using AtelierXNA;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using System.Collections.Generic;
//using System.Linq;

//namespace HyperV
//{
//    public class Camera2 : Caméra
//    {
//        //const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
//        //const float ACCÉLÉRATION = 0.001f;
//        //const float VITESSE_INITIALE_ROTATION = 5f;
//        //const float VITESSE_INITIALE_ROTATION_SOURIS = 0.1f;
//        //const float VITESSE_INITIALE_TRANSLATION = 0.5f;
//        //const float DELTA_LACET = MathHelper.Pi / 180; // 1 degré à la fois
//        //const float DELTA_TANGAGE = MathHelper.Pi / 180; // 1 degré à la fois
//        //const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degré à la fois
//        //const float RAYON_COLLISION = 1f;
//        //const int HAUTEUR_PERSONNAGE = -6;

//        //Vector3 Direction { get; set; }
//        //Vector3 Latéral { get; set; }
//        //Maze Maze { get; set; }
//        //Walls Walls { get; set; }
//        //float VitesseTranslation { get; set; }
//        //float VitesseRotation { get; set; }
//        //Point AnciennePositionSouris { get; set; }
//        //Point NouvellePositionSouris { get; set; }
//        //Vector2 DéplacementSouris { get; set; }

//        //float IntervalleMAJ { get; set; }
//        //float TempsÉcouléDepuisMAJ { get; set; }
//        //InputManager GestionInput { get; set; }
//        //float Height { get; set; }
//        //List<Character> Characters { get; set; }

//        //public Camera2(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ) : base(jeu)
//        //{
//        //    IntervalleMAJ = intervalleMAJ;
//        //    CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
//        //    CréerPointDeVue(positionCaméra, cible, orientation);
//        //    Height = positionCaméra.Y;
//        //}

//        //public override void Initialize()
//        //{
//        //    VitesseRotation = VITESSE_INITIALE_ROTATION;
//        //    VitesseTranslation = VITESSE_INITIALE_TRANSLATION;
//        //    TempsÉcouléDepuisMAJ = 0;
//        //    base.Initialize();
//        //    GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
//        //    Maze = Game.Services.GetService(typeof(Maze)) as Maze;
//        //    Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
//        //    NouvellePositionSouris = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
//        //    AnciennePositionSouris = new Point(NouvellePositionSouris.X, NouvellePositionSouris.Y);
//        //    Mouse.SetPosition(NouvellePositionSouris.X, NouvellePositionSouris.Y);
//        //}

//        //protected override void CréerPointDeVue()
//        //{
//        //    Vector3.Normalize(Direction);
//        //    Vector3.Normalize(OrientationVerticale);
//        //    Vector3.Normalize(Latéral);

//        //    Vue = Matrix.CreateLookAt(Position, Position + Direction, OrientationVerticale);
//        //}

//        //protected override void CréerPointDeVue(Vector3 position, Vector3 cible, Vector3 orientation)
//        //{
//        //    Position = position;
//        //    Cible = cible;
//        //    OrientationVerticale = orientation;

//        //    Direction = cible - Position;
//        //    //Direction = cible;

//        //    Vector3.Normalize(Cible);

//        //    CréerPointDeVue();
//        //}

//        //public override void Update(GameTime gameTime)
//        //{
//        //    float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
//        //    TempsÉcouléDepuisMAJ += TempsÉcoulé;
//        //    if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
//        //    {
//        //        FonctionsSouris();
//        //        FonctionsClavier();

//        //        GérerHauteur();
//        //        CréerPointDeVue();




//        //        Game.Window.Title = Position.ToString();
//        //        Position = new Vector3(Position.X, Height, Position.Z);
//        //        TempsÉcouléDepuisMAJ = 0;
//        //    }
//        //    base.Update(gameTime);
//        //}

//        ////Souris
//        //#region
//        //private void FonctionsSouris()
//        //{
//        //    AnciennePositionSouris = NouvellePositionSouris;
//        //    NouvellePositionSouris = GestionInput.GetPositionSouris();
//        //    DéplacementSouris = new Vector2(NouvellePositionSouris.X - AnciennePositionSouris.X,
//        //                                    NouvellePositionSouris.Y - AnciennePositionSouris.Y);

//        //    GérerRotationSouris();

//        //    NouvellePositionSouris = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
//        //    Mouse.SetPosition(NouvellePositionSouris.X, NouvellePositionSouris.Y);

//        //}

//        //private void GérerRotationSouris()
//        //{
//        //    GérerLacetSouris();
//        //    GérerTangageSouris();
//        //}

//        //private void GérerLacetSouris()
//        //{
//        //    Matrix matriceLacet = Matrix.Identity;

//        //    matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, DELTA_LACET * VITESSE_INITIALE_ROTATION_SOURIS * -DéplacementSouris.X);

//        //    Direction = Vector3.Transform(Direction, matriceLacet);
//        //}

//        //private void GérerTangageSouris()
//        //{
//        //    Matrix matriceTangage = Matrix.Identity;

//        //    matriceTangage = Matrix.CreateFromAxisAngle(Latéral, DELTA_TANGAGE * VITESSE_INITIALE_ROTATION_SOURIS * -DéplacementSouris.Y);

//        //    Direction = Vector3.Transform(Direction, matriceTangage);
//        //}
//        //#endregion

//        ////Clavier
//        //#region
//        //private void FonctionsClavier()
//        //{
//        //    GérerDéplacement();
//        //    GérerRotationClavier();
//        //}

//        //private void GérerDéplacement()
//        //{
//        //    float déplacementDirection = (GérerTouche(Keys.W) - GérerTouche(Keys.S)) * VitesseTranslation;
//        //    float déplacementLatéral = (GérerTouche(Keys.A) - GérerTouche(Keys.D)) * VitesseTranslation;

//        //    Direction = Vector3.Normalize(Direction);
//        //    Latéral = Vector3.Cross(Direction, OrientationVerticale);
//        //    Position += déplacementDirection * Direction;
//        //    Position -= déplacementLatéral * Latéral;
//        //    if (Maze.CheckForCollisions(Position))
//        //    {
//        //        Position -= déplacementDirection * Direction;
//        //        Position += déplacementLatéral * Latéral;
//        //    }
//        //}

//        //const float MAX_DISTANCE = 4.5f;

//        //bool CheckForCharacterCollision()
//        //{
//        //    bool result = false;
//        //    int i;

//        //    for (i = 0; i < Characters.Count && !result; ++i)
//        //    {
//        //        result = Vector3.Distance(Characters[i].GetPosition(), Position) < MAX_DISTANCE;
//        //    }

//        //    return result;
//        //}

//        //private void GérerRotationClavier()
//        //{
//        //    GérerLacetClavier();
//        //    GérerTangageClavier();
//        //}

//        //private void GérerLacetClavier()
//        //{
//        //    Matrix matriceLacet = Matrix.Identity;

//        //    if (GestionInput.EstEnfoncée(Keys.Left))
//        //    {
//        //        matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, DELTA_LACET * VITESSE_INITIALE_ROTATION);
//        //    }
//        //    if (GestionInput.EstEnfoncée(Keys.Right))
//        //    {
//        //        matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, -DELTA_LACET * VITESSE_INITIALE_ROTATION);
//        //    }

//        //    Direction = Vector3.Transform(Direction, matriceLacet);
//        //}

//        //private void GérerTangageClavier()
//        //{
//        //    Matrix matriceTangage = Matrix.Identity;

//        //    if (GestionInput.EstEnfoncée(Keys.Down))
//        //    {
//        //        matriceTangage = Matrix.CreateFromAxisAngle(Latéral, -DELTA_TANGAGE * VITESSE_INITIALE_ROTATION);
//        //    }
//        //    if (GestionInput.EstEnfoncée(Keys.Up))
//        //    {
//        //        matriceTangage = Matrix.CreateFromAxisAngle(Latéral, DELTA_TANGAGE * VITESSE_INITIALE_ROTATION);
//        //    }

//        //    Direction = Vector3.Transform(Direction, matriceTangage);
//        //}
//        //#endregion

//        //private void GérerHauteur()
//        //{
//        //    Position = Maze.GetPositionWithHeight(Position, HAUTEUR_PERSONNAGE);
//        //}

//        //private int GérerTouche(Keys touche)
//        //{
//        //    return GestionInput.EstEnfoncée(touche) ? 1 : 0;
//        //}

//        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
//        const float ACCÉLÉRATION = 0.001f;
//        const float VITESSE_INITIALE_ROTATION = 5f;
//        const float VITESSE_INITIALE_ROTATION_SOURIS = 0.1f;
//        const float VITESSE_INITIALE_TRANSLATION = 0.5f;
//        const float DELTA_LACET = MathHelper.Pi / 180; // 1 degré à la fois
//        const float DELTA_TANGAGE = MathHelper.Pi / 180; // 1 degré à la fois
//        const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degré à la fois
//        const float RAYON_COLLISION = 1f;
//        const int HAUTEUR_PERSONNAGE = 10;
//        const int FACTEUR_COURSE = 4;
//        const int DISTANCE_MINIMALE_POUR_RAMASSAGE = 45;

//        public Vector3 Direction { get; private set; }//
//        public Vector3 Latéral { get; private set; }//
//        Gazon Gazon { get; set; }
//        float VitesseTranslation { get; set; }
//        float VitesseRotation { get; set; }
//        Point AnciennePositionSouris { get; set; }
//        Point NouvellePositionSouris { get; set; }
//        Vector2 DéplacementSouris { get; set; }

//        float IntervalleMAJ { get; set; }
//        float TempsÉcouléDepuisMAJ { get; set; }
//        InputManager GestionInput { get; set; }
//        GamePadManager GestionGamePad { get; set; }


//        bool Sauter { get; set; }
//        bool Courrir { get; set; }
//        bool Ramasser { get; set; }

//        Ray Viseur { get; set; }

//        //Added from first Camera1
//        float Height { get; set; }
//        Maze Maze { get; set; }
//        List<Character> Characters { get; set; }

//        public Camera2(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ) : base(jeu)
//        {
//            IntervalleMAJ = intervalleMAJ;
//            CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
//            CréerPointDeVue(positionCaméra, cible, orientation);
//            //Added from first Camera1
//            Height = positionCaméra.Y;
//        }

//        public override void Initialize()
//        {
//            VitesseRotation = VITESSE_INITIALE_ROTATION;
//            VitesseTranslation = VITESSE_INITIALE_TRANSLATION;
//            TempsÉcouléDepuisMAJ = 0;

//            Courrir = false;
//            Sauter = false;
//            Ramasser = false;
//            Viseur = new Ray();

//            NouvellePositionSouris = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
//            AnciennePositionSouris = new Point(NouvellePositionSouris.X, NouvellePositionSouris.Y);
//            Mouse.SetPosition(NouvellePositionSouris.X, NouvellePositionSouris.Y);
//            //NEW*****************************************
//            InGame = true;

//            base.Initialize();
//            ChargerContenu();

//            InitialiserObjetsComplexesSaut();
//            Hauteur = Height;//HAUTEUR_PERSONNAGE;
//        }

//        Boss Boss { get; set; }

//        private void ChargerContenu()
//        {
//            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
//            GestionGamePad = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
//            //Gazon = Game.Services.GetService(typeof(Gazon)) as Gazon;
//            Maze = Game.Services.GetService(typeof(Maze)) as Maze;
//            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
//            Boss = Game.Services.GetService(typeof(Boss)) as Boss;
//        }


//        protected override void CréerPointDeVue()
//        {
//            Vector3.Normalize(Direction);
//            Vector3.Normalize(OrientationVerticale);
//            Vector3.Normalize(Latéral);

//            Vue = Matrix.CreateLookAt(Position, Position + Direction, OrientationVerticale);
//        }

//        protected override void CréerPointDeVue(Vector3 position, Vector3 cible, Vector3 orientation)
//        {
//            Position = position;
//            Cible = cible;
//            OrientationVerticale = orientation;

//            Direction = cible - Position;

//            Vector3.Normalize(Cible);

//            CréerPointDeVue();
//        }

//        public override void Update(GameTime gameTime)
//        {
//            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
//            TempsÉcouléDepuisMAJ += TempsÉcoulé;
//            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
//            {
//                FonctionsSouris();
//                FonctionsClavier();
//                FonctionsGamePad();

//                GérerHauteur();
//                CréerPointDeVue();

//                AffecterCommandes();

//                GérerRamassage();
//                GérerCourse();
//                GérerSaut();

//                //Game.Window.Title = Position.ToString();
//                //Position = new Vector3(Position.X, Height, Position.Z);
//                TempsÉcouléDepuisMAJ = 0;
//            }
//            base.Update(gameTime);

//        }

//        //Souris
//        #region

//        //NEW*****************************
//        bool InGame { get; set; }

//        public void SetInGame(bool inGame)
//        {
//            InGame = inGame;
//        }

//        private void FonctionsSouris()
//        {
//            AnciennePositionSouris = NouvellePositionSouris;
//            NouvellePositionSouris = GestionInput.GetPositionSouris();
//            DéplacementSouris = new Vector2(NouvellePositionSouris.X - AnciennePositionSouris.X,
//                                            NouvellePositionSouris.Y - AnciennePositionSouris.Y);

//            GérerRotationSouris();

//            //NEW**********************
//            if (InGame)
//            {
//                NouvellePositionSouris = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
//                Mouse.SetPosition(NouvellePositionSouris.X, NouvellePositionSouris.Y);
//            }
//        }

//        private void GérerRotationSouris()
//        {
//            GérerLacetSouris();
//            GérerTangageSouris();
//        }

//        private void GérerLacetSouris()
//        {
//            Matrix matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, DELTA_LACET * VITESSE_INITIALE_ROTATION_SOURIS * -DéplacementSouris.X);

//            Direction = Vector3.Transform(Direction, matriceLacet);
//        }

//        private void GérerTangageSouris()
//        {
//            Matrix matriceTangage = Matrix.CreateFromAxisAngle(Latéral, DELTA_TANGAGE * VITESSE_INITIALE_ROTATION_SOURIS * -DéplacementSouris.Y);

//            Direction = Vector3.Transform(Direction, matriceTangage);
//        }
//        #endregion

//        //Clavier
//        #region
//        private void FonctionsClavier()
//        {
//            GérerDéplacement((GérerTouche(Keys.W) - GérerTouche(Keys.S)),
//                             (GérerTouche(Keys.A) - GérerTouche(Keys.D)));
//            GérerRotationClavier();
//        }

//        private void GérerDéplacement(float direction, float latéral)
//        {
//            float déplacementDirection = direction * VitesseTranslation;
//            float déplacementLatéral = latéral * VitesseTranslation;

//            Direction = Vector3.Normalize(Direction);
//            Position += déplacementDirection * Direction;

//            Latéral = Vector3.Cross(Direction, OrientationVerticale);
//            Position -= déplacementLatéral * Latéral;

//            //Added from first Camera2
//            if (Maze.CheckForCollisions(Position) || CheckForBossCollision())
//            {
//                Position -= déplacementDirection * Direction;
//                Position += déplacementLatéral * Latéral;
//            }
//        }

//        //Added from first Camera
//        const float MAX_DISTANCE = 4.5f, MAX_DISTANCE_BOSS = 80f;

//        bool CheckForBossCollision()
//        {
//            return Vector3.Distance(Boss.GetPosition(), Position) < MAX_DISTANCE_BOSS;
//        }

//        bool CheckForCharacterCollision()
//        {
//            bool result = false;
//            int i;

//            for (i = 0; i < Characters.Count && !result; ++i)
//            {
//                result = Vector3.Distance(Characters[i].GetPosition(), Position) < MAX_DISTANCE;
//            }

//            return result;
//        }

//        private void GérerRotationClavier()
//        {
//            GérerLacetClavier();
//            GérerTangageClavier();
//        }

//        private void GérerLacetClavier()
//        {
//            Matrix matriceLacet = Matrix.Identity;

//            if (GestionInput.EstEnfoncée(Keys.Left))
//            {
//                matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, DELTA_LACET * VITESSE_INITIALE_ROTATION);
//            }
//            if (GestionInput.EstEnfoncée(Keys.Right))
//            {
//                matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, -DELTA_LACET * VITESSE_INITIALE_ROTATION);
//            }

//            Direction = Vector3.Transform(Direction, matriceLacet);
//        }

//        private void GérerTangageClavier()
//        {
//            Matrix matriceTangage = Matrix.Identity;

//            if (GestionInput.EstEnfoncée(Keys.Down))
//            {
//                matriceTangage = Matrix.CreateFromAxisAngle(Latéral, -DELTA_TANGAGE * VITESSE_INITIALE_ROTATION);
//            }
//            if (GestionInput.EstEnfoncée(Keys.Up))
//            {
//                matriceTangage = Matrix.CreateFromAxisAngle(Latéral, DELTA_TANGAGE * VITESSE_INITIALE_ROTATION);
//            }

//            Direction = Vector3.Transform(Direction, matriceTangage);
//        }
//        #endregion

//        //GamePad
//        #region
//        private void FonctionsGamePad()
//        {
//            if (GestionGamePad.EstGamepadActivé)
//            {
//                GérerDéplacement(GestionGamePad.PositionThumbStickGauche.Y,
//                                 -GestionGamePad.PositionThumbStickGauche.X);

//                DéplacementSouris = new Vector2(35, -35) * GestionGamePad.PositionThumbStickDroit;
//                GérerRotationSouris();
//            }
//        }
//        #endregion

//        private void AffecterCommandes()
//        {
//            Courrir = GestionInput.EstEnfoncée(Keys.RightShift) ||
//                      GestionInput.EstEnfoncée(Keys.LeftShift) ||
//                      GestionGamePad.EstEnfoncé(Buttons.LeftStick);

//            Sauter = GestionInput.EstEnfoncée(Keys.R/*Keys.Space*/) ||
//                     GestionGamePad.EstEnfoncé(Buttons.A);

//            Ramasser = GestionInput.EstNouveauClicGauche() ||
//                       GestionInput.EstAncienClicGauche() ||
//                       GestionGamePad.EstNouveauBouton(Buttons.RightStick);
//        }

//        private void GérerHauteur()
//        {
//            //Position = Gazon.GetPositionAvecHauteur(Position, (int)Hauteur);
//            Position = new Vector3(Position.X, Hauteur, Position.Z);
//        }

//        private int GérerTouche(Keys touche)
//        {
//            return GestionInput.EstEnfoncée(touche) ? 1 : 0;
//        }

//        private void GérerRamassage()
//        {
//            Viseur = new Ray(Position, Direction);

//            foreach (SphèreRamassable sphereRamassable in Game.Components.Where(composant => composant is SphèreRamassable))
//            {
//                Ramasser = sphereRamassable.EstEnCollision(Viseur) <= DISTANCE_MINIMALE_POUR_RAMASSAGE &&
//                           sphereRamassable.EstEnCollision(Viseur) != null &&
//                           Ramasser;

//                //Game.Window.Title = sphereRamassable.EstEnCollision(Viseur).ToString();
//                if (Ramasser)
//                {
//                    sphereRamassable.EstRamassée = true;
//                }
//            }
//        }

//        //Saut
//        #region
//        private void GérerSaut()
//        {
//            if (Sauter)
//            {
//                ContinuerSaut = true;
//            }

//            if (ContinuerSaut)
//            {
//                if (t > 60)
//                {
//                    InitialiserObjetsComplexesSaut();
//                    ContinuerSaut = false;
//                    t = 0;
//                }
//                Hauteur = CalculerBesier(t * (1f / 60f), PtsDeControle).Y;
//                ++t;
//            }
//        }

//        bool ContinuerSaut { get; set; }
//        float t { get; set; }
//        float Hauteur { get; set; }

//        Vector3 PositionPtsDeControle { get; set; }
//        Vector3 PositionPtsDeControlePlusUn { get; set; }
//        Vector3[] PtsDeControle { get; set; }

//        void InitialiserObjetsComplexesSaut()
//        {
//            Position = new Vector3(Position.X, Height/*HAUTEUR_PERSONNAGE*/, Position.Z);
//            PositionPtsDeControle = new Vector3(Position.X, Position.Y, Position.Z);
//            PositionPtsDeControlePlusUn = Position + Vector3.Normalize(new Vector3(Direction.X, 0, Direction.Z)) * 25;
//            //Position = new Vector3(PositionPtsDeControle.X, PositionPtsDeControle.Y, PositionPtsDeControle.Z);//******
//            //Direction = PositionPtsDeControlePlusUn - PositionPtsDeControle;//******
//            PtsDeControle = CalculerPointsControle();
//        }

//        private Vector3[] CalculerPointsControle()
//        {
//            Vector3[] pts = new Vector3[4];
//            pts[0] = PositionPtsDeControle;
//            pts[3] = PositionPtsDeControlePlusUn;
//            pts[1] = new Vector3(pts[0].X, pts[0].Y + 20, pts[0].Z);
//            pts[2] = new Vector3(pts[3].X, pts[3].Y + 20, pts[3].Z);
//            return pts;
//        }

//        private Vector3 CalculerBesier(float t, Vector3[] PtsDeControle)
//        {
//            float x = (1 - t);
//            return PtsDeControle[0] * (x * x * x) +
//                   3 * PtsDeControle[1] * t * (x * x) +
//                   3 * PtsDeControle[2] * t * t * x +
//                   PtsDeControle[3] * t * t * t;

//        }
//        #endregion

//        private void GérerCourse()
//        {
//            VitesseTranslation = Courrir ? FACTEUR_COURSE * VITESSE_INITIALE_TRANSLATION : VITESSE_INITIALE_TRANSLATION;
//        }
//    }
//}



using AtelierXNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace HyperV
{
    public class Camera2 : CaméraJoueur
    {
        //Added from first Camera1
        //float Height { get; set; }

        Maze Maze { get; set; }
        List<Character> Characters { get; set; }
        Boss Boss { get; set; }
        HeightMap HeightMap { get; set; }
        Water Water { get; set; }

        public Camera2(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ, float renderDistance)
            : base(jeu, positionCaméra, cible, orientation, intervalleMAJ, renderDistance)
        { }

        protected override void ChargerContenu()
        {
            base.ChargerContenu();
            Maze = Game.Services.GetService(typeof(Maze)) as Maze;
            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
            Boss = Game.Services.GetService(typeof(Boss)) as Boss;
            HeightMap = Game.Services.GetService(typeof(HeightMap)) as HeightMap;
            GérerHauteur();
            Water = Game.Services.GetService(typeof(Water)) as Water;
        }

        //NO WATER
        //protected override void GérerHauteur()
        //{
        //    //Hauteur = HeightMap.GetHeight(Position);
        //    //NO WATER
        //    //    if (!LifeBars[1].Water)
        //    //    {
        //    //        Height = HeightMap.GetHeight(Position); //HERE
        //    //        base.GérerHauteur();
        //    //    }
        //}

        protected override void GérerDéplacement(float direction, float latéral)
        {
            base.GérerDéplacement(direction, latéral);

            if (Maze.CheckForCollisions(Position) /*|| CheckForBossCollision()*/)
            {
                Position -= direction * VitesseTranslation * Direction;
                Position += latéral * VitesseTranslation * Latéral;
            }
            // NO WATER
            //if (LifeBars[1].Water)
            //{
            //    Position -= direction * VitesseTranslation * Direction;
            //    Position += latéral * VitesseTranslation * Latéral;
            //    Position += direction * VITESSE_INITIALE_TRANSLATION * Direction;
            //    Position -= latéral * VITESSE_INITIALE_TRANSLATION * Latéral;
            //}
            //if (!LifeBars[1].Water && Position.Y <= Water.AdjustedHeight)
            //{
            //    LifeBars[1].TurnWaterOn();
            //}
            //else if (LifeBars[1].Water && Position.Y > Water.AdjustedHeight)
            //{
            //    LifeBars[1].TurnWaterOff();
            //}
            //if (LifeBars[1].Drowned)
            //{
            //    LifeBars[0].Attack(1);
            //}
        }
        //NO WATER
        //protected override void GérerSaut()
        //{
        //    if (LifeBars[1].Water)
        //    {
        //        if (Sauter)
        //        {
        //            Height += 0.4f;
        //            if (Height > Water.AdjustedHeight)
        //            {
        //                Height = Water.AdjustedHeight;
        //                LifeBars[1].Restore();
        //            }
        //            Position = new Vector3(Position.X, Height/*HAUTEUR_PERSONNAGE*/, Position.Z);
        //            //++Hauteur;
        //        }
        //        else
        //        {
        //            Height -= 0.4f;
        //            if (Height < HeightMap.GetHeight(Position))
        //            {
        //                Height = HeightMap.GetHeight(Position);
        //            }
        //            Position = new Vector3(Position.X, Height/*HAUTEUR_PERSONNAGE*/, Position.Z);
        //            //--Hauteur;
        //        }
        //    }
        //    else
        //    {
        //        base.GérerSaut();
        //    }
        //}

        const float MAX_DISTANCE = 4.5f, MAX_DISTANCE_BOSS = 80f;

        bool CheckForBossCollision()
        {
            return Vector3.Distance(Boss.GetPosition(), Position) < MAX_DISTANCE_BOSS;
        }
    }
}

