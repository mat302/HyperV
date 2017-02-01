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
        Model Modele3D { get; set; } //le modele 3d quon veut placer
        Texture3D Texture3D { get; set; } //la texture qui va avec le modele
        Vector3 Position { get; set; } //la position du modele dans le monde
        float Homothesie { get; set; } //la grosseur du modele

        public Niveau(Game game): base(game) { }

        public Niveau(Game game, Model modele3D, Texture3D texture3D, Vector3 position, float homothesie)
            : base(game)
        {
            Modele3D = modele3D;
            Texture3D = texture3D;
            Position = position;
            Homothesie = homothesie;
        }

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
