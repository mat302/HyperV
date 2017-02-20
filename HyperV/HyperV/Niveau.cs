using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtelierXNA;


namespace HyperV
{
    //Classe qui lirait le fichier texte qui lui dit quoi load comme models et leur position ...
    public class Niveau : Microsoft.Xna.Framework.DrawableGameComponent
    {
        RessourcesManager<Model> ModelManager { get; set; }
        RessourcesManager<Texture2D> TextureManager { get; set; }

        string NomModele3D { get; set; }
        Model Modele3D { get; set; } //le modele 3d quon veut placer

        string NomTexture2D { get; set; } //la texture qui va avec le modele
        Texture2D Texture2D { get; set; }

        Vector3 Position { get; set; } //la position du modele dans le monde
        float RotationModele { get; set; }
        CaméraJoueur Camera { get; set; }
        float AspectRatio { get; set; }
        float Grosseur { get; set; }

        public Niveau(Game game) : base(game) { }

        public Niveau(Game game, string modele3D, Vector3 position)
            : base(game)
        {
            NomModele3D = modele3D;
            Position = position;
        }

        protected override void LoadContent()
        {
            Camera = Game.Services.GetService(typeof(CaméraJoueur)) as CaméraJoueur;
            ModelManager = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;


            Modele3D = ModelManager.Find(NomModele3D);
            Texture2D = TextureManager.Find("Texture_Rockwall");
        }

        public override void Initialize()
        {
            base.Initialize();
            RotationModele = 0.0f;
            AspectRatio = 1.0f;
            Grosseur = 1f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        
        public override void Draw(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[Modele3D.Bones.Count];
            Modele3D.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in Modele3D.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateScale(new Vector3(0.05f, 0.05f, 0.05f)) * Matrix.CreateRotationY(RotationModele) * Matrix.CreateTranslation(Position);
                    effect.View = Matrix.CreateLookAt(Camera.Position, Camera.Cible, Vector3.Up);
                    effect.Projection = Camera.Projection;
                    effect.Texture = Texture2D;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
