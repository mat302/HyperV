/*
CutscenePlayer.cs
-----------------

By Mathieu Godin

Role : Creates a cutscene

Created : 2/25/17
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
    public class CutscenePlayer : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Video Video { get; set; }
        VideoPlayer Player { get; set; }
        Texture2D VideoTexture { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        RessourcesManager<Video> VideoManager { get; set; }
        Rectangle Screen { get; set; }
        string VideoName { get; set; }
        InputManager InputManager { get; set; }
        GamePadManager GestionGamePad { get; set; }
        SkipCutsceneLabel Label { get; set; }
        string FontName { get; set; }
        public bool CutsceneFinished { get; private set; }

        public CutscenePlayer(Game game, string videoName, bool cutsceneFinished, string fontName) : base(game)
        {
            VideoName = videoName;
            CutsceneFinished = cutsceneFinished;
            FontName = fontName;
        }

        public void ResetCutsceneFinished()
        {
            CutsceneFinished = false;
        }

        //public TexteCentré Loading { get; private set; }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            //Loading = new TexteCentré(Game, "Loading . . .", "Arial", new Rectangle(Game.Window.ClientBounds.Width / 2 - 200, Game.Window.ClientBounds.Height / 2 - 40, 400, 80), Color.White, 0);
            Player = new VideoPlayer();
            Video = VideoManager.Find(VideoName);
            Screen = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            Player.Play(Video);
        }

        protected override void LoadContent()
        {
            SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            VideoManager = Game.Services.GetService(typeof(RessourcesManager<Video>)) as RessourcesManager<Video>;
            InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionGamePad = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
            Label = new SkipCutsceneLabel(Game, FontName);
            Game.Components.Add(Label);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //if (Player.State == MediaState.Stopped)
            //{
            //    Player.IsLooped = true;
            //    Player.Play(Video);
            //}
            if ((InputManager.EstClavierActivé && InputManager.EstNouvelleTouche(/*Keys.Space*/Keys.R) )||GestionGamePad.EstNouveauBouton(Buttons.Start))
            {
                Player.Stop();
            }
            if (Player.State == MediaState.Stopped)
            {
                CutsceneFinished = true;
                Game.Components.Remove(this);
                Game.Components.Remove(Label);
                //Game.Components.Add(Loading);
                (Game as Atelier).AddLoading();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            VideoTexture = Player.GetTexture();
            if (VideoTexture != null)
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(VideoTexture, Screen, Color.White);
                SpriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
