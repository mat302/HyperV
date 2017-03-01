/*
CharacterScript.cs
------------------

By Mathieu Godin

Role : Shows the script of the chracter

Created : 2/28/17
*/
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
using System.IO;

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
        Rectangle ScriptRectanglePosition { get; set; }
        string ScriptRectangleName { get; set; }
        Vector2 TextPosition { get; set; }

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
            float height = (160f / 600f) * Game.Window.ClientBounds.Height;
            FaceImageRectangle = new Rectangle(10, (int)(Game.Window.ClientBounds.Height - height - 10) - 20, (int)((250f / 800f) * Game.Window.ClientBounds.Width) - 100, (int)height + 20);
            ScriptRectanglePosition = new Rectangle(FaceImageRectangle.X + FaceImageRectangle.Width + 10, FaceImageRectangle.Y + 10, (int)((510f / 800f) * Game.Window.ClientBounds.Width) + 140, (int)((143f / 600f) * Game.Window.ClientBounds.Height) + 25);
            TextPosition = new Vector2(ScriptRectanglePosition.X + 10, ScriptRectanglePosition.Y + 10);
        } // 800x600 510x143

        protected override void LoadContent()
        {
            SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            FaceImage = TextureManager.Find(FaceImageName);
            ScriptRectangle = TextureManager.Find(ScriptRectangleName);
            InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            FontManager = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            Font = FontManager.Find("Arial");
            ReadScript();
            base.LoadContent();
        }

        void ReadScript()
        {
            StreamReader reader = new StreamReader(TextFile);
            TextFile = "";
            while(!reader.EndOfStream)
            {
                TextFile += reader.ReadLine() + "\n";
            }
            reader.Close();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (InputManager.EstClavierActivé && InputManager.EstNouvelleTouche(Keys.Space))
            {
                Visible = !Visible;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(ScriptRectangle, ScriptRectanglePosition, Color.White);
            SpriteBatch.Draw(FaceImage, FaceImageRectangle, Color.White);
            SpriteBatch.DrawString(Font, TextFile, TextPosition, Color.Black, 0, Vector2.Zero, (float)(ScriptRectanglePosition.Width) / (Game.Window.ClientBounds.Width * 1.6f), SpriteEffects.None, 0);
            SpriteBatch.End();
        }
    }
}
