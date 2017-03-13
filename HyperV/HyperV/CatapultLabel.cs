
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
    public class CatapultLabel : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string Name { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        RessourcesManager<Texture2D> TextureManager { get; set; }
        RessourcesManager<SpriteFont> FontManager { get; set; }
        SpriteFont Font { get; set; }
        Texture2D Dock { get; set; }
        Texture2D Gauge { get; set; }
        string GaugeName { get; set; }
        string DockName { get; set; }
        string FontName { get; set; }
        float Timer { get; set; }
        float Interval { get; set; }
        Rectangle GaugeRectangle { get; set; }
        Rectangle DockRectangle { get; set; }
        Vector2 StringPosition { get; set; }

        public CatapultLabel(Game game, string name, string gaugeName, string dockName, string fontName, float interval) : base(game)
        {
            Name = name;
            DockName = dockName;
            GaugeName = gaugeName;
            FontName = fontName;
            Interval = interval;
        }

        public override void Initialize()
        {
            //GaugeRectangle = new Rectangle(Game.Window.ClientBounds.Width / 2 - 150, 90, (int)((float)Life / MaxLife * 300), 50);
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

        public override void Update(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Interval)
            {
                //GaugeRectangle = new Rectangle(GaugeRectangle.X, GaugeRectangle.Y, (int)((float)Life / MaxLife * 300), GaugeRectangle.Height);
                Timer = 0;
            }
            base.Update(gameTime);
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
