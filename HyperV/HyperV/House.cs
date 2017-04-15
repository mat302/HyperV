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
    public class House : ObjetDeBase
    {
        string TextFile { get; set; }
        string FaceImageName { get; set; }
        string ScriptRectangleName { get; set; }
        float Interval { get; set; }
        Vector3 Radius { get; set; }
        CharacterScript CharacterScript { get; set; }
        string FontName { get; set; }
        float LabelInterval { get; set; }

        public House(Game game, string modelName, float startScale, Vector3 startRotation, Vector3 startPosition, Vector3 diff) : base(game, modelName, startScale, startRotation, startPosition)
        {
            Radius = startPosition + diff;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(Position.X, Position.Y, Position.Z);
        }

        public bool Collision(BoundingSphere sphere)
        {
            return BoundingBox.Intersects(sphere);
        }

        public BoundingBox BoundingBox { get { return new BoundingBox(Position, Radius); } }
    }
}
