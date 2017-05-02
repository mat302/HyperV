/*
CaméraSubjective.cs
-------------------

Par Marc-Olivier Fillion et Mathieu Godin

Rôle : Caméra pouvant se déplacer et tourner selon 
       tous les degrés de liberté

Créé : 7 décembre 2016
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtelierXNA
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
      float VitesseTranslation { get; set; }
      float VitesseRotation { get; set; }

      float IntervalleMAJ { get; set; }
      float TempsÉcouléDepuisMAJ { get; set; }
      InputManager GestionInput { get; set; }

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

      public CaméraSubjective(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ) : base(jeu)
      {
         IntervalleMAJ = intervalleMAJ;
         CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
         CréerPointDeVue(positionCaméra, cible, orientation);
         EstEnZoom = false;
      }

      public override void Initialize()
      {
         VitesseRotation = VITESSE_INITIALE_ROTATION;
         VitesseTranslation = VITESSE_INITIALE_TRANSLATION;
         TempsÉcouléDepuisMAJ = 0;
         base.Initialize();
         GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
      }

      protected override void CréerPointDeVue()
      {
            // Méthode appelée s'il est nécessaire de recalculer la matrice de vue.
            // Calcul et normalisation de certains vecteurs
            // (à compléter)
            Latéral = Vector3.Cross(Direction, OrientationVerticale);
            Latéral = Vector3.Normalize(Latéral);
            OrientationVerticale = Vector3.Cross(Latéral, Direction);
            OrientationVerticale = Vector3.Normalize(OrientationVerticale);
            Vue = Matrix.CreateLookAt(Position, Position + Direction, OrientationVerticale);
         GénérerFrustum();
      }

      protected override void CréerPointDeVue(Vector3 position, Vector3 cible, Vector3 orientation)
      {
            // À la construction, initialisation des propriétés Position, Cible et OrientationVerticale,
            // ainsi que le calcul des vecteur Direction, Latéral et le recalcul du vecteur OrientationVerticale
            // permettant de calculer la matrice de vue de la caméra subjective
            // (à compléter)
            Position = position;
            OrientationVerticale = orientation;
            Direction = cible - position;
            Direction = Vector3.Normalize(Direction);
         //Création de la matrice de vue (point de vue)
         CréerPointDeVue();
      }

      public override void Update(GameTime gameTime)
      {
         float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
         TempsÉcouléDepuisMAJ += TempsÉcoulé;
         GestionClavier();
         if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
         {
            if (GestionInput.EstEnfoncée(Keys.LeftShift) || GestionInput.EstEnfoncée(Keys.RightShift))
            {
               GérerAccélération();
               GérerDéplacement();
               GérerRotation();
               CréerPointDeVue();
            }
            TempsÉcouléDepuisMAJ = 0;
         }
         base.Update(gameTime);
      }

      private int GérerTouche(Keys touche)
      {
         return GestionInput.EstEnfoncée(touche) ? 1 : 0;
      }

      private void GérerAccélération()
      {
         int valAccélération = (GérerTouche(Keys.Subtract) + GérerTouche(Keys.OemMinus)) - (GérerTouche(Keys.Add)+GérerTouche(Keys.OemPlus));
         if (valAccélération != 0)
         {
            IntervalleMAJ += ACCÉLÉRATION * valAccélération;
            IntervalleMAJ = MathHelper.Max(INTERVALLE_MAJ_STANDARD, IntervalleMAJ);
         }
      }

      private void GérerDéplacement()
      {
         Vector3 nouvellePosition = Position;
         float déplacementDirection = (GérerTouche(Keys.W) - GérerTouche(Keys.S)) * VitesseTranslation;
         float déplacementLatéral = (GérerTouche(Keys.A) - GérerTouche(Keys.D)) * VitesseTranslation;

            // Calcul du déplacement avant arrière
            // Calcul du déplacement latéral
            // À compléter
            Position += Direction * déplacementDirection;
            Position -= Latéral * déplacementLatéral;
      }

      private void GérerRotation()
      {
         GérerLacet();
         GérerTangage();
         GérerRoulis();
      }

      private void GérerLacet()
      {
            // Gestion du lacet
            // À compléter
            if (GestionInput.EstEnfoncée(Keys.Right))
            {
                Direction = Vector3.Transform(Direction, Matrix.CreateFromAxisAngle(OrientationVerticale, -DELTA_LACET * VitesseRotation));
                Direction = Vector3.Normalize(Direction);
            }
            else if (GestionInput.EstEnfoncée(Keys.Left))
            {
                Direction = Vector3.Transform(Direction, Matrix.CreateFromAxisAngle(OrientationVerticale, DELTA_LACET * VitesseRotation));
                Direction = Vector3.Normalize(Direction);
            }
        }

      private void GérerTangage()
      {
            // Gestion du tangage
            // À compléter
            if (GestionInput.EstEnfoncée(Keys.Up))
            {
                Direction = Vector3.Transform(Direction, Matrix.CreateFromAxisAngle(Latéral, -DELTA_TANGAGE * VitesseRotation));
                OrientationVerticale = Vector3.Transform(OrientationVerticale, Matrix.CreateFromAxisAngle(Latéral, -DELTA_TANGAGE * VitesseRotation));
                Direction = Vector3.Normalize(Direction);
                OrientationVerticale = Vector3.Normalize(OrientationVerticale);
            }
            else if (GestionInput.EstEnfoncée(Keys.Down))
            {
                Direction = Vector3.Transform(Direction, Matrix.CreateFromAxisAngle(Latéral, DELTA_TANGAGE * VitesseRotation));
                OrientationVerticale = Vector3.Transform(OrientationVerticale, Matrix.CreateFromAxisAngle(Latéral, DELTA_TANGAGE * VitesseRotation));
                Direction = Vector3.Normalize(Direction);
                OrientationVerticale = Vector3.Normalize(OrientationVerticale);
            }
        }

      private void GérerRoulis()
      {
            // Gestion du roulis
            // À compléter
            if (GestionInput.EstEnfoncée(Keys.PageUp))
            {
                OrientationVerticale = Vector3.Transform(OrientationVerticale, Matrix.CreateFromAxisAngle(Direction, -DELTA_ROULIS * VitesseRotation));
                OrientationVerticale = Vector3.Normalize(OrientationVerticale);
            }
            else if (GestionInput.EstEnfoncée(Keys.PageDown))
            {
                OrientationVerticale = Vector3.Transform(OrientationVerticale, Matrix.CreateFromAxisAngle(Direction, DELTA_ROULIS * VitesseRotation));
                OrientationVerticale = Vector3.Normalize(OrientationVerticale);
            }
        }

      private void GestionClavier()
      {
         if (GestionInput.EstNouvelleTouche(Keys.Z))
         {
            EstEnZoom = !EstEnZoom;
         }
      }
   }
}
