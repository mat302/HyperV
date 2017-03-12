/*
BossLabel.cs
------------

By Mathieu Godin

Role : Shows the life points of the boss
       and its name

Created : 3/12/17
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
        RessourcesManager<SpriteFont> FontManager { get; set; }
        SpriteFont Font { get; set; }
        Texture2D Dock { get; set; }
        Texture2D Gauge { get; set; }
        Boss Boss { get; set; }
        string GaugeName { get; set; }
        string DockName { get; set; }
        string FontName { get; set; }
        float Timer { get; set; }
        float Interval { get; set; }
        Rectangle GaugeRectangle { get; set; }
        Rectangle DockRectangle { get; set; }
        Vector2 StringPosition { get; set; }

        public BossLabel(Game game, Boss boss, string name, int maxLife, string gaugeName, string dockName, string fontName, float interval) : base(game)
        {
            Boss = boss;
            Name = name;
            MaxLife = maxLife;
            DockName = dockName;
            GaugeName = gaugeName;
            FontName = fontName;
            Interval = interval;
            Life = MaxLife;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            GaugeRectangle = new Rectangle(Game.Window.ClientBounds.Width / 2 - 150, 90, (int)((float)Life / MaxLife * 300), 50);
            DockRectangle = new Rectangle(Game.Window.ClientBounds.Width / 2 - 150, 90, 300, 50);
            StringPosition = new Vector2(Game.Window.ClientBounds.Width / 2 - 170, 15);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Dock = TextureManager.Find(DockName);
            Gauge = TextureManager.Find(GaugeName);
            FontManager = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            Font = FontManager.Find(FontName);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Interval)
            {
                GaugeRectangle = new Rectangle(GaugeRectangle.X, GaugeRectangle.Y, (int)((float)Life / MaxLife * 300), GaugeRectangle.Height);
                Timer = 0;
            }
            base.Update(gameTime);
        }

        public void Attack(int attackPts)
        {
            Life -= attackPts;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Gauge, GaugeRectangle, Color.White);
            SpriteBatch.Draw(Dock, DockRectangle, Color.White);
            SpriteBatch.DrawString(Font, Name, StringPosition, Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            SpriteBatch.End();
        }
    }
}
