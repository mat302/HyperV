/*
SkipCutSceneLabel.cs
--------------------

By Mathieu Godin

Role : Creates a floating label telling
       the player he can skip the playing
       cutscene by pressing space or start

Created : 2/26/17
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
    public class SkipCutsceneLabel : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string Message { get; set; }
        Vector2 Position { get; set; }
        float Interval { get; set; }
        SpriteFont Font { get; set; }
        double Scale { get; set; }
        Vector2 Origin { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        float Timer { get; set; }
        RessourcesManager<SpriteFont> FontManager { get; set; }
        string FontName { get; set; }

        const float ZOOM_INCREMENT = 0.0001F;

        public SkipCutsceneLabel(Game game, string fontName) : base(game)
        {
            FontName = fontName;
        }
        
        public override void Initialize()
        {
            Timer = 0;
            Scale = 0;
            Message = "Press R/Start to skip";//"Press Space/Start to skip";
            Position = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height - 50);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            FontManager = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            Font = FontManager.Find(FontName);
            
            Vector2 dimension = Font.MeasureString(Message);
            Origin = new Vector2(dimension.X / 2, dimension.Y / 2);
            SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
        }
        
        public override void Update(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Interval)
            {
                Timer = 0;
                Scale += ZOOM_INCREMENT;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Font, Message, Position, Color.Black, 0, Origin, ((float)Math.Abs(Math.Sin(Scale)) / 4 + 1) * (Game.Window.ClientBounds.Width / 5000f), SpriteEffects.None, 0);
            SpriteBatch.End();
        }
    }
}