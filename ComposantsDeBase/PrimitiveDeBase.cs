using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace AtelierXNA
{
   public abstract class PrimitiveDeBase : Microsoft.Xna.Framework.DrawableGameComponent
   {
      protected float HomothétieInitiale { get; private set; }
      protected Vector3 RotationInitiale { get; private set; }
      protected Vector3 PositionInitiale { get; private set; }
      protected int NbSommets { get; set; }
      protected int NbTriangles { get; set; }
      protected Matrix Monde { get; set; }
      protected Caméra CaméraJeu { get; private set; }

      protected PrimitiveDeBase(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
         : base(jeu)
      {
         HomothétieInitiale = homothétieInitiale;
         RotationInitiale = rotationInitiale;
         PositionInitiale = positionInitiale;
      }

      public override void Initialize()
      {
         InitialiserSommets();
         CalculerMatriceMonde();
         base.Initialize();
      }

      protected override void LoadContent()
      {
         CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra;
         base.LoadContent();
      }

      protected virtual void CalculerMatriceMonde()
      {
         Monde = Matrix.Identity *
                 Matrix.CreateScale(HomothétieInitiale) *
                 Matrix.CreateFromYawPitchRoll(RotationInitiale.Y, RotationInitiale.X, RotationInitiale.Z) *
                 Matrix.CreateTranslation(PositionInitiale);
      }

      protected abstract void InitialiserSommets();

      public virtual Matrix GetMonde()
      {
         return Monde;
      }
   }
}