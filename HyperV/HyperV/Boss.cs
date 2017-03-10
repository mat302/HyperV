/*
Character.cs
------------

By Mathieu Godin

Role : Used to create a non-playable
       character rendered with a .fbx 
       3d model that can talk to the
       player

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


namespace HyperV
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Boss : ObjetDeBase
    {
        float Interval { get; set; }
        float Radius { get; set; }
        CharacterScript CharacterScript { get; set; }

        public Boss(Game game, string modelName, float startScale, Vector3 startRotation, Vector3 startPosition) : base(game, modelName, startScale, startRotation, startPosition)
        {
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
            
            base.Update(gameTime);
        }

        public float? Collision(Ray ray)
        {
            return BoundingSphere.Intersects(ray);
        }

        public BoundingSphere BoundingSphere { get { return new BoundingSphere(Position, Radius); } }
    }
}

