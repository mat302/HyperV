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
using System.IO;


namespace HyperV
{
    //Classe qui lirait le fichier texte qui lui dit quoi load comme models et leur position ...
    public class Niveau : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        RessourcesManager<Model> ModelManager { get; set;}
        RessourcesManager<Texture2D> TextureManager { get; set; }

        string NomModele3D { get; set; }
        Model Modele3D { get; set; } //le modele 3d quon veut placer

        string NomTexture2D { get; set; } //la texture qui va avec le modele
        Texture2D Texture2D { get; set; }

        Vector3 Position { get; set; } //la position du modele dans le monde
        float RotationModele { get; set; }
        CaméraSubjective Camera { get; set; }
        float AspectRatio { get; set; }

        public Niveau(Game game): base(game) { }

        public Niveau(Game game, string modele3D, string texture2D, Vector3 position)
            : base(game)
        {
            NomModele3D = modele3D;
            NomTexture2D = texture2D;
            Position = position;
        }

        protected override void LoadContent()
        {
            Camera = Game.Services.GetService(typeof(CaméraSubjective)) as CaméraSubjective;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ModelManager = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;


            Modele3D = ModelManager.Find(NomModele3D);
            Texture2D = TextureManager.Find(NomTexture2D);
        }

        public override void Initialize()
        {
            base.Initialize();
            RotationModele = 0.0f;
            AspectRatio = 1.0f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[Modele3D.Bones.Count];
            Modele3D.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in Modele3D.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(RotationModele)
                        * Matrix.CreateTranslation(Position);
                    effect.View = Matrix.CreateLookAt(Camera.Position,
                        Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), AspectRatio,
                        1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
