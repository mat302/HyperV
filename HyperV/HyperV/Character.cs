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
    public class Character : ObjetDeBase
    {
        string TextFile { get; set; }
        string FaceImageName { get; set; }
        string ScriptRectangleName { get; set; }
        float Interval { get; set; }
        float Radius { get; set; }

        public Character(Game game, string modelName, float startScale, Vector3 startRotation, Vector3 startPosition, string textFile, string faceImageName, string scriptRectangleName) : base(game, modelName, startScale, startRotation, startPosition)
        {
            TextFile = textFile;
            FaceImageName = faceImageName;
            ScriptRectangleName = scriptRectangleName;
            Radius = 5;
        }

        public void AddLabel()
        {
            Game.Components.Add(new CharacterScript(Game, this, FaceImageName, TextFile, ScriptRectangleName));
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
