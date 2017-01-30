using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public abstract class Caméra : Microsoft.Xna.Framework.GameComponent
   {
      protected const float OUVERTURE_OBJECTIF = MathHelper.PiOver4; //45 degrés
      protected const float DISTANCE_PLAN_RAPPROCHÉ = 0.001f;
      protected const float DISTANCE_PLAN_ÉLOIGNÉ = 500;

      public Matrix Vue { get; protected set; }
      public Matrix Projection { get; protected set; }
      public BoundingFrustum Frustum { get; protected set; }

      // Propriétés relatives au "Point de vue"
		public Vector3 Position { get; protected set; }
		public Vector3 Cible { get; protected set; }
		public Vector3 OrientationVerticale { get; protected set; }
 
      // Propriétés relatives au "Volume de visualisation"
      protected float AngleOuvertureObjectif { get; set; }
      protected float AspectRatio { get; set; }
      protected float DistancePlanRapproché { get; set; }
      protected float DistancePlanÉloigné { get; set; }

      public Caméra(Game jeu)
         : base(jeu)
      { }

      protected virtual void CréerPointDeVue()
      {
         //Création de la matrice de vue (point de vue)
         Vue = Matrix.CreateLookAt(Position, Cible, OrientationVerticale);
      }

      protected virtual void CréerPointDeVue(Vector3 position, Vector3 cible)
      {
         //Initialisation des propriétés de la matrice de vue (point de vue)
         Position = position;
         Cible = cible;
         OrientationVerticale = Vector3.Up;
         //Création de la matrice de vue (point de vue)
         CréerPointDeVue();
      }

      protected virtual void CréerPointDeVue(Vector3 position, Vector3 cible, Vector3 orientationVerticale)
      {
         //Initialisation des propriétés de la matrice de vue (point de vue)
         Position = position;
         Cible = cible;
         OrientationVerticale = orientationVerticale;
         //Création de la matrice de vue (point de vue)
         CréerPointDeVue();
      }

      private void CréerVolumeDeVisualisation()
      {
         //Création de la matrice de projection (volume de visualisation)
         Projection = Matrix.CreatePerspectiveFieldOfView(AngleOuvertureObjectif, AspectRatio, 
                                                          DistancePlanRapproché, DistancePlanÉloigné);
      }

      protected virtual void CréerVolumeDeVisualisation(float angleOuvertureObjectif, float distancePlanRapproché, float distancePlanÉloigné)
      {
         //Initialisation des propriétés de la matrice de projection (volume de visualisation)
         AngleOuvertureObjectif = angleOuvertureObjectif;
         AspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
         DistancePlanRapproché = distancePlanRapproché;
         DistancePlanÉloigné = distancePlanÉloigné;
         //Création de la matrice de projection (volume de visualisation)
         CréerVolumeDeVisualisation();
      }

      protected virtual void CréerVolumeDeVisualisation(float angleOuvertureObjectif, float aspectRatio, 
                                                        float distancePlanRapproché, float distancePlanÉloigné)
      {
         //Initialisation des propriétés de la matrice de projection (volume de visualisation)
         AngleOuvertureObjectif = angleOuvertureObjectif;
         AspectRatio = aspectRatio;
         DistancePlanRapproché = distancePlanRapproché;
         DistancePlanÉloigné = distancePlanÉloigné;
         //Création de la matrice de projection (volume de visualisation)
         CréerVolumeDeVisualisation();
      }

      protected void GénérerFrustum()
      {
         Matrix vueProjection = Vue * Projection;
         Frustum = new BoundingFrustum(vueProjection);
      }

      public virtual void Déplacer(Vector3 position, Vector3 cible, Vector3 orientationVerticale)
      {
         CréerPointDeVue(position, cible, orientationVerticale);
      }
   }
}
