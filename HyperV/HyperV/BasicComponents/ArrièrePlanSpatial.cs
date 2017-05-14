using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
   public class ArriËrePlanSpatial : ArriËrePlanDÈroulant
   {
      const float D…PLACEMENT_VERTICAL = 0.2f;

      public ArriËrePlanSpatial(Game jeu, string nomImage, float intervalMAJ)
         : base(jeu, nomImage, intervalMAJ)
      { }

      protected override void LoadContent()
      {
         base.LoadContent();
         TailleImage = new Vector2(0, ImageDeFond.Height * …chelle);
      }

      protected override void EffectuerMise¿Jour()
      {
         Position…cran = new Vector2(0, (Position…cran.Y + D…PLACEMENT_VERTICAL) % TailleImage.Y);
      }

      protected override void Afficher(GameTime gameTime)
      {
         GestionSprites.Draw(ImageDeFond, Position…cran, null, Color.White, 0, Vector2.Zero, …chelle, SpriteEffects.None, ARRI»RE_PLAN);
         GestionSprites.Draw(ImageDeFond, Position…cran - TailleImage, null, Color.White, 0, Vector2.Zero, …chelle, SpriteEffects.None, ARRI»RE_PLAN);
      }
   }
}