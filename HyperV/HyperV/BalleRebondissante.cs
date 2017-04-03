using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtelierXNA;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;

using System.Diagnostics;


namespace HyperV
{
   class BalleRebondissante : SphèreTexturée
   {
      const int AUCUN_TEMPS_ÉCOULÉ = 0,
                ANGLE_DÉPLACEMENT_DÉPART_MINIMAL = 15,
                ANGLE_DÉPLACEMENT_DÉPART_MAXIMAL = 75,
                ANGLE_DROIT = 90,
                FACTEUR_MINIMAL_CERCLE_360_DEGRÉS = 0,
                FACTEUR_MAXIMAL_CERCLE_360_DEGRÉS_EXCLU = 4,
                ANGLE_PLAT = 180, ANGLE_PLEIN = 360,
                ABSCISSE_UNITAIRE = 1,
                ORDONNÉE_UNITAIRE = 1;

     const float NORME_VECTEUR_DÉPLACEMENT = 1F;

      float TempsÉcouléDepuisMAJDéplacement { get; set; }
      float IntervalleMAJDéplacement { get; set; }
      Vector3 VecteurDéplacementMAJ { get; set; }
      Random GénérateurAléatoire { get; set; }
      Rectangle RectangleBalle { get; set; }
      float AngleDéplacementTeta { get; set; }
      float AngleDéplacementPhi { get; set; }
      Vector2 DimensionsBalle{ get; set; }

      




      public BalleRebondissante(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,
                               float rayon, Vector2 charpente, string nomTexture, float intervalleMAJ)
         : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, rayon, charpente, nomTexture, intervalleMAJ)
      {
         GénérateurAléatoire = new Random();
         DimensionsBalle = charpente;
         IntervalleMAJDéplacement = intervalleMAJ;
         Position = positionInitiale;
      }

      public override void Initialize()
      {         
         base.Initialize();
         AngleDéplacementTeta = GénérateurAléatoire.Next(FACTEUR_MINIMAL_CERCLE_360_DEGRÉS, FACTEUR_MAXIMAL_CERCLE_360_DEGRÉS_EXCLU) * ANGLE_DROIT +
                                GénérateurAléatoire.Next(ANGLE_DÉPLACEMENT_DÉPART_MINIMAL, ANGLE_DÉPLACEMENT_DÉPART_MAXIMAL);
         AngleDéplacementPhi = GénérateurAléatoire.Next(FACTEUR_MINIMAL_CERCLE_360_DEGRÉS, FACTEUR_MAXIMAL_CERCLE_360_DEGRÉS_EXCLU) * ANGLE_DROIT +
                                GénérateurAléatoire.Next(ANGLE_DÉPLACEMENT_DÉPART_MINIMAL, ANGLE_DÉPLACEMENT_DÉPART_MAXIMAL);
         CalculerVecteurDéplacement();
         TempsÉcouléDepuisMAJDéplacement = AUCUN_TEMPS_ÉCOULÉ;             
      }

      public override void Update(GameTime gameTime)
      {
         TempsÉcouléDepuisMAJDéplacement += (float)gameTime.ElapsedGameTime.TotalSeconds;
         if (TempsÉcouléDepuisMAJDéplacement >= IntervalleMAJDéplacement)
         {
            CalculerMatriceMonde();
            Position += VecteurDéplacementMAJ;

            Bordures(-197, 78, Position.X);
            Bordures(-35, -5,  Position.Y);
            Bordures(-48, 228, Position.Z);
           
            TempsÉcouléDepuisMAJDéplacement = AUCUN_TEMPS_ÉCOULÉ;
            
         }
         base.Update(gameTime);
      }

      void CalculerVecteurDéplacement()
      {
         //float x = NORME_VECTEUR_DÉPLACEMENT * (float)Math.Cos(MathHelper.ToRadians(AngleDéplacementTeta) * (float)Math.Sin(MathHelper.ToRadians(AngleDéplacementPhi)));
         //float y = NORME_VECTEUR_DÉPLACEMENT * (float)Math.Sin(MathHelper.ToRadians(AngleDéplacementTeta) * (float)Math.Sin(MathHelper.ToRadians(AngleDéplacementPhi)));
         //float z = NORME_VECTEUR_DÉPLACEMENT * (float)Math.Cos(MathHelper.ToRadians(AngleDéplacementPhi));


         float x = NORME_VECTEUR_DÉPLACEMENT * (float)Math.Cos(MathHelper.ToRadians(AngleDéplacementTeta));
         float y = NORME_VECTEUR_DÉPLACEMENT * (float)Math.Sin(MathHelper.ToRadians(AngleDéplacementTeta));
         float z = NORME_VECTEUR_DÉPLACEMENT * (float)Math.Sin(MathHelper.ToRadians(AngleDéplacementTeta));


         VecteurDéplacementMAJ = new Vector3(x, y, z);
      }

      void Bordures(int borneMin,int borneMax,float positionActuelle)
      {
         if (positionActuelle >= borneMax || positionActuelle <= borneMin)
         {
            AngleDéplacementTeta = ANGLE_PLEIN - AngleDéplacementTeta;
            AngleDéplacementPhi = ANGLE_PLEIN - AngleDéplacementPhi;
            CalculerVecteurDéplacement();
         }
      }
   }
}
