using AtelierXNA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HyperV
{
    public class Skybox : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Model skyBox;
        string SkyboxTexture { get; set; }
        CaméraSubjective Camera { get; set; }

        RessourcesManager<Model> ModelManager { get; set; }
        RessourcesManager<TextureCube> TextureManager { get; set; }
        RessourcesManager<Effect> EffectManager { get; set; }

        private TextureCube skyBoxTexture;
        
        // The effect file that the skybox will use to render
        private Effect skyBoxEffect;

        private float size = 50f;
        
        public Skybox(Game game, string skyboxTexture) : base(game)
        {
            SkyboxTexture = skyboxTexture;
        }

        public override void Initialize()
        {
            skyBox = ModelManager.Find("Skyboxes/cube");
            skyBoxTexture = TextureManager.Find(SkyboxTexture);
            skyBoxEffect = EffectManager.Find("Skyboxes/Skybox");
        }

        protected override void LoadContent()
        {
            ModelManager = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<TextureCube>)) as RessourcesManager<TextureCube>;
            EffectManager = Game.Services.GetService(typeof(RessourcesManager<Effect>)) as RessourcesManager<Effect>;
            Camera = Game.Services.GetService(typeof(CaméraSubjective)) as CaméraSubjective;
        }
        
        public void Draw()
        {
            foreach (EffectPass pass in skyBoxEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                
                foreach (ModelMesh mesh in skyBox.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = skyBoxEffect;
                        part.Effect.Parameters["World"].SetValue(Matrix.CreateScale(size) * Matrix.CreateTranslation(Camera.Position));
                        part.Effect.Parameters["View"].SetValue(Camera.Vue);
                        part.Effect.Parameters["Projection"].SetValue(Camera.Projection);
                        part.Effect.Parameters["SkyBoxTexture"].SetValue(skyBoxTexture);
                        part.Effect.Parameters["CameraPosition"].SetValue(Camera.Position);
                    }
                    mesh.Draw();
                }
            }
        }
    }
}
