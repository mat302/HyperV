using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public class CaméraFixe : Caméra
   {
      public CaméraFixe(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation)
         : base(jeu)
      {
         CréerPointDeVue(positionCaméra, cible, orientation); // Création de la matrice de vue
         CréerVolumeDeVisualisation(Caméra.OUVERTURE_OBJECTIF, Caméra.DISTANCE_PLAN_RAPPROCHÉ, Caméra.DISTANCE_PLAN_ÉLOIGNÉ); // Création de la matrice de projection (volume de visualisation)
      }
   }
}
