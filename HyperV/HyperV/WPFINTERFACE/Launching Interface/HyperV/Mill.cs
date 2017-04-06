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
    class Mill : TuileTexturée
    {
        GearWheel[] GearWheels { get; set; }
        float Timer { get; set; }
        float Interval { get; set; }

        public Mill(Game game, float scale, Vector3 initialRotation, Vector3 initialPosition, Vector2 range, string textureName, float interval) : base(game, scale, initialRotation, initialPosition, range, textureName, interval)
        {
            // TODO: Construct any child components here
            Interval = interval;
            Timer = 0;
            GearWheels = new GearWheel[2];
            GearWheels[0] = new GearWheel(Game, "gearwheel1", 0.025f, new Vector3(0, MathHelper.ToRadians(90), 0), new Vector3(300, 10, 102));
            Game.Components.Add(GearWheels[0]);
            GearWheels[1] = new GearWheel(Game, "gearwheel1", 0.025f, new Vector3(0, MathHelper.ToRadians(90), 0), new Vector3(305, 15, 102));
            Game.Components.Add(GearWheels[1]);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Interval)
            {
                GearWheels[0].UpdateRotation(0.1f);
                GearWheels[1].UpdateRotation(-0.1f);
                Timer = 0;
            }
            base.Update(gameTime);
        }
    }
}
