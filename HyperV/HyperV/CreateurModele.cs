using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtelierXNA;
using System.Collections.Generic;
using System;

namespace HyperV
{
    public class CreateurModele : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public bool DéplacementBouton = false;
        RessourcesManager<Model> ModelManager { get; set; }
        RessourcesManager<Texture2D> TextureManager { get; set; }

        List<BoundingSphere> SphèreColision { get; set; }
        public List<BoundingSphere> GetSphèreColision()
        {
            return SphèreColision;
        }

        string NomModele3D { get; set; }
        protected Model Modele3D { get; set; } //le modele 3d quon veut placer

        string NomTexture2D { get; set; } //la texture qui va avec le modele
        Texture2D Texture2D { get; set; }
        public bool EstTour = false;

        protected Vector3 Position { get; set; } //la position du modele dans le monde
        public Vector3 GetPosition()
        {
            return Position;
        }
        public void DéplacerModele(Vector3 déplacement)
        {
            Position += déplacement;
        }

        protected float Rotation { get; set; }
        Caméra Camera { get; set; }
        float Homothésie { get; set; }

        public CreateurModele(Game game) : base(game) { }

        public CreateurModele(Game game, string modele3D, Vector3 position, float homothésie, float rotation)
            : base(game)
        {
            NomModele3D = modele3D;
            Position = position;
            Homothésie = homothésie;
            Rotation = rotation;
        }

        public CreateurModele(Game game, string modele3D, Vector3 position, float homothésie, float rotation, string nomTexture2D)
            : base(game)
        {
            NomModele3D = modele3D;
            Position = position;
            Homothésie = homothésie;
            Rotation = rotation;
            NomTexture2D = nomTexture2D;
        }

        public override void Initialize()
        {
            base.Initialize();
            SphèreColision = new List<BoundingSphere>();
        }

        protected override void LoadContent()
        {
            Camera = Game.Services.GetService(typeof(Caméra)) as Caméra;
            ModelManager = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;

            Modele3D = ModelManager.Find(NomModele3D);
            if (NomTexture2D != null)
            {
                Texture2D = TextureManager.Find(NomTexture2D);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[Modele3D.Bones.Count];
            Modele3D.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in Modele3D.Meshes)
            {
                SphèreColision.Add(mesh.BoundingSphere);
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (Texture2D != null)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = Texture2D;
                    }
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateScale(Homothésie) * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(Position);
                    effect.View = Camera.Vue;
                    effect.Projection = Camera.Projection;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
