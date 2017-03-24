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
    public class LifeBar : Microsoft.Xna.Framework.DrawableGameComponent
    {
        int MaxLife { get; set; }
        int Life { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        RessourcesManager<Texture2D> TextureManager { get; set; }
        Texture2D Dock { get; set; }
        Texture2D Gauge { get; set; }
        string GaugeName { get; set; }
        string SecondaryGaugeName { get; set; }
        string DockName { get; set; }
        float Timer { get; set; }
        float Interval { get; set; }
        Rectangle GaugeRectangle { get; set; }
        Rectangle DockRectangle { get; set; }
        Vector2 Position { get; set; }

        public LifeBar(Game game, int maxLife, string gaugeName, string dockName, Vector2 position, float interval) : base(game)
        {
            MaxLife = maxLife;
            DockName = dockName;
            GaugeName = gaugeName;
            SecondaryGaugeName = gaugeName;
            Interval = interval;
            Life = MaxLife;
            Position = position;
            Tired = false;
        }

        public LifeBar(Game game, int maxLife, string gaugeName, string secondaryGaugeName, string dockName, Vector2 position, float interval) : base(game)
        {
            MaxLife = maxLife;
            DockName = dockName;
            GaugeName = gaugeName;
            SecondaryGaugeName = secondaryGaugeName;
            Interval = interval;
            Life = MaxLife;
            Position = position;
            Tired = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            GaugeRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)((float)Life / MaxLife * 300), 50);
            DockRectangle = new Rectangle((int)Position.X, (int)Position.Y, 300, 50);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Dock = TextureManager.Find(DockName);
            Gauge = TextureManager.Find(GaugeName);
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

        public bool Tired { get; private set; }

        public void Attack(int attackPts)
        {
            int newLife = Life - attackPts;
            if (newLife >= 0 && newLife <= MaxLife)
            {
                Life = newLife;
                if (Life == 0)
                {
                    Gauge = TextureManager.Find(SecondaryGaugeName);
                    Tired = true;
                }
                else if (Life == MaxLife)
                {
                    Gauge = TextureManager.Find(GaugeName);
                    Tired = false;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Gauge, GaugeRectangle, Color.White);
            SpriteBatch.Draw(Dock, DockRectangle, Color.White);
            SpriteBatch.End();
        }
    }
}
