using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
   public class ArrièrePlan : Microsoft.Xna.Framework.DrawableGameComponent
   {
      protected const float ARRIÈRE_PLAN = 1f;
      protected SpriteBatch GestionSprites { get; private set; }
      RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
      string NomImage { get; set; }
      Rectangle ZoneAffichage { get; set; }
      protected Texture2D ImageDeFond { get; private set; }

      public ArrièrePlan(Game jeu, string nomImage)
         : base(jeu)
      {
         NomImage = nomImage;
      }

      public override void Initialize()
      {
         ZoneAffichage = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
         base.Initialize();
      }

      protected override void LoadContent()
      {
         GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
         GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
         ImageDeFond = GestionnaireDeTextures.Find(NomImage);
      }

      public override void Draw(GameTime gameTime)
      {
         GestionSprites.Begin();
         Afficher(gameTime);
         GestionSprites.End();
      }

      protected virtual void Afficher(GameTime gameTime)
      {
         GestionSprites.Draw(ImageDeFond, ZoneAffichage, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, ARRIÈRE_PLAN);
      }
   }
}