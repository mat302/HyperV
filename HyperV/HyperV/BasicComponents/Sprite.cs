using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
   public class Sprite : Microsoft.Xna.Framework.DrawableGameComponent
   {
      string NomImage { get; set; }
      protected Vector2 Position { get; set; }        // En prévision d'une spécialisation vers un sprite dynamique
      protected Texture2D Image { get; private set; } // En prévision d'une spécialisation vers un sprite animé
      protected SpriteBatch GestionSprites { get; private set; } // En prévision d'une spécialisation vers un sprite animé
      RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }

      public Sprite(Game jeu, string nomImage, Vector2 position)
         : base(jeu)
      {
         NomImage = nomImage;
         Position = position;
      }

      protected override void LoadContent()
      {
         GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
         GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
         Image = GestionnaireDeTextures.Find(NomImage);
      }

      public override void Draw(GameTime gameTime)
      {
            GestionSprites.Begin();
         GestionSprites.Draw(Image, Position, Color.White);
            GestionSprites.End();
      }
   }
}