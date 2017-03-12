/*
Boss.cs
-------

By Mathieu Godin

Role : Used to create a non-playable
       boss rendered with a .fbx 
       3d model that can have a fight
       with the player

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
    public class Boss : ObjetDeBase
    {
        float Interval { get; set; }
        float Radius { get; set; }
        BossLabel BossLabel { get; set; }
        string GaugeName { get; set; }
        string DockName { get; set; }
        string FontName { get; set; }
        string Name { get; set; }
        int MaxLife { get; set; }
        float Timer { get; set; }
        float LabelInterval { get; set; }
        InputManager InputManager { get; set; }

        public Boss(Game game, string name, int maxLife, string modelName, string gaugeName, string dockName, string fontName, float interval, float labelInterval, float startScale, Vector3 startRotation, Vector3 startPosition) : base(game, modelName, startScale, startRotation, startPosition)
        {
            Radius = 6;
            DockName = dockName;
            GaugeName = gaugeName;
            FontName = fontName;
            Name = name;
            Interval = interval;
            LabelInterval = labelInterval;
            MaxLife = maxLife;
        }

        protected override void LoadContent()
        {
            InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            base.LoadContent();
        }

        public Vector3 GetPosition()
        {
            return new Vector3(Position.X, Position.Y, Position.Z);
        }

        public void AddLabel()
        {
            BossLabel = new BossLabel(Game, this, Name, MaxLife, GaugeName, DockName, FontName, LabelInterval);
            Game.Components.Add(BossLabel);
        }

        public void RemoveLabel()
        {
            Game.Components.Remove(BossLabel);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (Timer >= Interval)
            //{

            //    Timer = 0;
            //}
            if (InputManager.EstNouvelleTouche(Keys.E))
            {
                BossLabel.Attack(1);
            }
            base.Update(gameTime);
        }

        public float? Collision(Ray ray)
        {
            return BoundingSphere.Intersects(ray);
        }

        public BoundingSphere BoundingSphere { get { return new BoundingSphere(Position, Radius); } }
    }
}

