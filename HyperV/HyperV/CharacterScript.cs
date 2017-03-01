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
    public class CharacterScript : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D FaceImage { get; set; }
        Texture2D ScriptRectangle { get; set; }
        string FaceImageName { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        string TextFile { get; set; }
        InputManager InputManager { get; set; }
        RessourcesManager<Texture2D> TextureManager { get; set; }
        RessourcesManager<SpriteFont> FontManager { get; set; }
        SpriteFont Font { get; set; }
        Rectangle FaceImageRectangle { get; set; }
        Vector2 ScriptRectanglePosition { get; set; }
        string ScriptRectangleName { get; set; }

        public CharacterScript(Game game, string faceImageName, string textFile, string scriptRectangleName) : base(game)
        {
            FaceImageName = faceImageName;
            TextFile = textFile;
            ScriptRectangleName = scriptRectangleName;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            int height = 160;
            FaceImageRectangle = new Rectangle(10, Game.Window.ClientBounds.Height - height - 10, 250, height);
            ScriptRectanglePosition = new Vector2(FaceImageRectangle.X + FaceImageRectangle.Width + 10, FaceImageRectangle.Y + 10);
        }

        protected override void LoadContent()
        {
            SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            FaceImage = TextureManager.Find(FaceImageName);
            ScriptRectangle = TextureManager.Find(ScriptRectangleName);
            InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            FontManager = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            Font = FontManager.Find("Arial");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(ScriptRectangle, ScriptRectanglePosition, Color.White);
            SpriteBatch.Draw(FaceImage, FaceImageRectangle, Color.White);
            SpriteBatch.End();
        }
    }
}
