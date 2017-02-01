using Microsoft.Xna.Framework;

namespace HyperV
{
    public class CaméraSubjective : Caméra
    {
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        const float ACCÉLÉRATION = 0.001f;
        const float VITESSE_INITIALE_ROTATION = 5f;
        const float VITESSE_INITIALE_TRANSLATION = 0.5f;
        const float DELTA_LACET = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_TANGAGE = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degré à la fois
        const float RAYON_COLLISION = 1f;

        Vector3 Direction { get; set; }
        Vector3 Latéral { get; set; }
        float VitesseRotation { get; set; }

        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        
        bool estEnZoom;
        bool EstEnZoom
        {
            get { return estEnZoom; }
            set
            {
                float ratioAffichage = Game.GraphicsDevice.Viewport.AspectRatio;
                estEnZoom = value;
                if (estEnZoom)
                {
                    CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF / 2, ratioAffichage, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
                }
                else
                {
                    CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, ratioAffichage, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
                }
            }
        }

        public CaméraSubjective(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ)
           : base(jeu)
        {
            IntervalleMAJ = intervalleMAJ;
            CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
            CréerPointDeVue(positionCaméra, cible, orientation);
            EstEnZoom = false;
        }

        public override void Initialize()
        {
            VitesseRotation = VITESSE_INITIALE_ROTATION;
            TempsÉcouléDepuisMAJ = 0;
            base.Initialize();
        }

        protected override void CréerPointDeVue()
        {
            Vector3.Normalize(Direction);
            Vector3.Normalize(OrientationVerticale);
            Vector3.Normalize(Latéral);


            Vue = Matrix.CreateLookAt(Position, Position + Direction, OrientationVerticale);

            Game.Window.Title = Position.ToString();
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
            base.Update(gameTime);
        }

        public void DeplacerCamera(float deplacementDirection, float deplacementLatéral, float vitesse, bool zoom)
        {
            GérerDéplacement(deplacementDirection, deplacementLatéral, vitesse);
            //GérerRotation();
            GérerZoom(zoom);
        }

        private void GérerDéplacement(float deplacementDirection, float deplacementLatéral, float vitesse)
        {
            float déplacementDirection = deplacementDirection * vitesse;
            float déplacementLatéral = deplacementLatéral * vitesse;

            Direction = Vector3.Normalize(Direction);
            Position += déplacementDirection * Direction;

            Latéral = Vector3.Cross(Direction, OrientationVerticale);
            Position -= déplacementLatéral * Latéral;
        }

        //private void GérerRotation()
        //{
        //    GérerLacet();
        //    GérerTangage();
        //    GérerRoulis();
        //}

        //private void GérerLacet()
        //{
        //    Matrix matriceLacet = Matrix.Identity;

        //    if (GestionInput.EstEnfoncée(Keys.Left))
        //    {
        //        matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, DELTA_LACET*VITESSE_INITIALE_ROTATION);
        //    }
        //    if(GestionInput.EstEnfoncée(Keys.Right))
        //    {
        //        matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, -DELTA_LACET* VITESSE_INITIALE_ROTATION);
        //    }

        //    Direction = Vector3.Transform(Direction, matriceLacet);
        //}

        //private void GérerTangage()
        //{
        //    Matrix matriceTangage = Matrix.Identity;

        //    if (GestionInput.EstEnfoncée(Keys.Up))
        //    {
        //        matriceTangage = Matrix.CreateFromAxisAngle(Latéral, DELTA_TANGAGE* VITESSE_INITIALE_ROTATION);
        //    }
        //    if(GestionInput.EstEnfoncée(Keys.Down))
        //    {
        //        matriceTangage = Matrix.CreateFromAxisAngle(Latéral, -DELTA_TANGAGE* VITESSE_INITIALE_ROTATION);
        //    }

        //    Direction = Vector3.Transform(Direction, matriceTangage);
        //    OrientationVerticale = Vector3.Transform(OrientationVerticale, matriceTangage);
        //}

        //private void GérerRoulis()
        //{
        //    Matrix matriceRoulis = Matrix.Identity;

        //    if (GestionInput.EstEnfoncée(Keys.PageUp))
        //    {
        //        matriceRoulis = Matrix.CreateFromAxisAngle(Direction, DELTA_ROULIS * VITESSE_INITIALE_ROTATION);
        //    }
        //    if (GestionInput.EstEnfoncée(Keys.PageDown))
        //    {
        //        matriceRoulis = Matrix.CreateFromAxisAngle(Direction, -DELTA_ROULIS * VITESSE_INITIALE_ROTATION);
        //    }

        //    OrientationVerticale = Vector3.Transform(OrientationVerticale, matriceRoulis);
        //}

        private void GérerZoom(bool zoom)
        {
            if (zoom)
            {
                EstEnZoom = !EstEnZoom;
            }
        }
    }
}
