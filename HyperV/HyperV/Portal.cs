/*
Portal.cs
---------

By Mathieu Godin

Role : Enables teleportation to another
       level if the player press Space
       on the portal. The portal is a
       simple Texture2D panel for now.

Created : 3/7/17
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
    public class Portal : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string TextFile { get; set; }
        string FaceImageName { get; set; }
        string ScriptRectangleName { get; set; }
        float Interval { get; set; }
        float Radius { get; set; }
        Vector3 Position { get; set; }

        public Portal(Game game, string modelName, float startScale, Vector3 startRotation, Vector3 startPosition, string textFile, string faceImageName, string scriptRectangleName) : base(game)
        {
            TextFile = textFile;
            FaceImageName = faceImageName;
            ScriptRectangleName = scriptRectangleName;
            Radius = 6;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(Position.X, Position.Y, Position.Z);
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

        public float? Collision(Ray ray)
        {
            return BoundingSphere.Intersects(ray);
        }

        public BoundingSphere BoundingSphere { get { return new BoundingSphere(Position, Radius); } }
    }
}
