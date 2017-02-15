//
// Auteur : Vincent Echelard
// Date : CrÈation - Septembre 2014
//        Modification - Novembre 2016
//
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
   public class TexteCentrÈ : Microsoft.Xna.Framework.DrawableGameComponent
   {
      float PourcentageZoneAffichable;
      string Texte¿Afficher { get; set; }
      string NomFont { get; set; }
      Rectangle ZoneAffichage { get; set; }
      Vector2 PositionAffichage { get; set; }
      Color CouleurTexte { get; set; }
      Vector2 Origine { get; set; }
      float …chelle { get; set; }
      SpriteFont PoliceDeCaractËres { get; set; }
      SpriteBatch GestionSprites { get; set; }
      RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }

      public TexteCentrÈ(Game jeu, string texte¿Afficher, string nomFont, Rectangle zoneAffichage, 
                         Color couleurTexte, float marge)
         : base(jeu)
      {
         Texte¿Afficher = texte¿Afficher;
         NomFont = nomFont;
         CouleurTexte = couleurTexte;
         ZoneAffichage = zoneAffichage;
         PourcentageZoneAffichable = 1f - marge;
         PositionAffichage = new Vector2(zoneAffichage.X + zoneAffichage.Width / 2,
                                         zoneAffichage.Y + zoneAffichage.Height / 2);
      }

      protected override void LoadContent()
      {
         GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
         GestionnaireDeFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
         PoliceDeCaractËres = GestionnaireDeFonts.Find(NomFont);
         ModifierTexte(Texte¿Afficher);
      }

      public void ModifierTexte(string texte¿Afficher)
      {
         Vector2 dimensionTexte = PoliceDeCaractËres.MeasureString(Texte¿Afficher);
         float ÈchelleHorizontale = MathHelper.Max(MathHelper.Min(ZoneAffichage.Width * PourcentageZoneAffichable, dimensionTexte.X),ZoneAffichage.Width * PourcentageZoneAffichable) / dimensionTexte.X;
         float ÈchelleVerticale = MathHelper.Max(MathHelper.Min(ZoneAffichage.Height * PourcentageZoneAffichable, dimensionTexte.Y),ZoneAffichage.Height * PourcentageZoneAffichable) / dimensionTexte.Y;
         …chelle = MathHelper.Min(ÈchelleHorizontale, ÈchelleVerticale);
         Origine = dimensionTexte / 2;
      }

      public override void Draw(GameTime gameTime)
      {
         GestionSprites.Begin();
         GestionSprites.DrawString(PoliceDeCaractËres, Texte¿Afficher, PositionAffichage, CouleurTexte, 0, Origine, …chelle, SpriteEffects.None, 0);
         GestionSprites.End();
      }
   }
}