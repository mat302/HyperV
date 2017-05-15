using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtelierXNA;

namespace HyperV
{
   /// <summary>
   /// This is a game component that implements IUpdateable.
   /// </summary>
   public class PressSpaceLabel : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string Message { get; set; }
        Vector2 Position { get; set; }
        SpriteFont Font { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        RessourcesManager<SpriteFont> FontManager { get; set; }
        Vector2 Origin { get; set; }
        float Scale { get; set; }
      Language Language { get; set; }

        public PressSpaceLabel(Game game) : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            DéterminerMesage();
            Position = new Vector2(Game.Window.ClientBounds.Width - 400, Game.Window.ClientBounds.Height - 80);
            Scale = 0.5f;
            base.Initialize();
        }

      public void DéterminerMesage()
      {
         switch ((Game as Atelier).Language)
        {
            case Language.French:
               Message = "Appuyer sur R";
               break;
            case Language.English:
               Message = "Press R";
               break;
            case Language.Spanish:
               Message = "Seguir adelante R";
               break;
            case Language.Japanese:
               Message = "Rを押して";
               break;
         }
      }

        protected override void LoadContent()
        {
            FontManager = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            Font = FontManager.Find("Arial");
            Vector2 dimension = Font.MeasureString(Message);
            Origin = new Vector2(dimension.X / 2, dimension.Y / 2);
            SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

      }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Font, Message, Position, Color.Black, 0, Origin, Scale, SpriteEffects.None, 0);
            SpriteBatch.End();
        }
    }
}
