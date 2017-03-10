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
    public class BossLabel : Microsoft.Xna.Framework.DrawableGameComponent
    {
        int MaxLife { get; set; }
        int Life { get; set; }
        string Name { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        RessourcesManager<Texture2D> TextureManager { get; set; }
        InputManager InputManager { get; set; }
        RessourcesManager<SpriteFont> FontManager { get; set; }
        SpriteFont Font { get; set; }
        Texture2D Dock { get; set; }
        Texture2D Gauge { get; set; }

        public BossLabel(Game game, string name, int maxLife) : base(game)
        {
            Name = name;
            MaxLife = maxLife;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Dock = TextureManager.Find("Dock");
            Gauge = TextureManager.Find("Gauge");
            InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            FontManager = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            Font = FontManager.Find("Arial");
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
    }
}
