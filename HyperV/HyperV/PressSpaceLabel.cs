using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
            Message = "Press Space/A";
            Position = new Vector2(Game.Window.ClientBounds.Width - 150, Game.Window.ClientBounds.Height - 50);
            Scale = 0.25f;
            base.Initialize();
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
