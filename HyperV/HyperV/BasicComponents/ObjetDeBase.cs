using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AtelierXNA
{
   public class ObjetDeBase : Microsoft.Xna.Framework.DrawableGameComponent
   {
      string NomModèle { get; set; }
      RessourcesManager<Model> GestionnaireDeModèles { get; set; }
      protected Caméra CaméraJeu { get; set; }
      protected float Échelle { get; set; }
      protected Vector3 Rotation { get; set; }
      protected Vector3 Position { get; set; }

      protected Model Modèle { get; private set; }
      protected Matrix[] TransformationsModèle { get; private set; }
      protected Matrix Monde { get; set; }

      public ObjetDeBase(Game jeu, string nomModèle, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
         : base(jeu)
      {
         NomModèle = nomModèle;
         Position = positionInitiale;
         Échelle = échelleInitiale;
         Rotation = rotationInitiale;
      }

      public override void Initialize()
      {
         base.Initialize();
         CalculerMonde();
      }

      private void CalculerMonde()
      {
         Monde = Matrix.Identity;
         Monde *= Matrix.CreateScale(Échelle);
         Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
         Monde *= Matrix.CreateTranslation(Position);
      }

      protected override void LoadContent()
      {
         CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra;
         GestionnaireDeModèles = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
         Modèle = GestionnaireDeModèles.Find(NomModèle);
         TransformationsModèle = new Matrix[Modèle.Bones.Count];
         Modèle.CopyAbsoluteBoneTransformsTo(TransformationsModèle);
      }

      public override void Draw(GameTime gameTime)
      {
         foreach (ModelMesh maille in Modèle.Meshes)
         {
            Matrix mondeLocal = TransformationsModèle[maille.ParentBone.Index] * GetMonde();
            foreach (ModelMeshPart portionDeMaillage in maille.MeshParts)
            {
               BasicEffect effet = (BasicEffect)portionDeMaillage.Effect;
               effet.EnableDefaultLighting();
               effet.Projection = CaméraJeu.Projection;
               effet.View = CaméraJeu.Vue;
               effet.World = mondeLocal;
            }
            maille.Draw();
         }
      }

      public virtual Matrix GetMonde()
      {
         return Monde;
      }
   }
}
