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


namespace HyperV
{
    //Classe qui lirait le fichier texte qui lui dit quoi load comme models et leur position ...
    public class Niveau : Microsoft.Xna.Framework.GameComponent
    {
        public Niveau(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            Game.Components.Add(new CubeTexturé(Game, 1, Vector3.Zero, Vector3.Zero, "CielBleu", new Vector3(10, 10, 10), 60));
            base.Initialize();
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
