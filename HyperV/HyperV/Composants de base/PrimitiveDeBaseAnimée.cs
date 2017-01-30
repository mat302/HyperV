using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace HyperV
{
   public abstract class PrimitiveDeBaseAnimÈe : PrimitiveDeBase
   {
      protected float HomothÈtie { get; set; }
      protected Vector3 Position { get; set; }
      float IntervalleMAJ { get; set; }
      float Temps…coulÈDepuisMAJ { get; set; }
      protected InputManager GestionInput { get; private set; }
      float IncrÈmentAngleRotation { get; set; }
      protected bool Lacet { get; set; }
      protected bool Tangage { get; set; }
      protected bool Roulis { get; set; }
      protected bool Monde¿Recalculer { get; set; }

      float angleLacet;
      protected float AngleLacet
      {
         get
         {
            if (Lacet)
            {
               angleLacet += IncrÈmentAngleRotation;
               angleLacet = MathHelper.WrapAngle(angleLacet);
            }
            return angleLacet;
         }
         set { angleLacet = value; }
      }

      float angleTangage;
      protected float AngleTangage
      {
         get
         {
            if (Tangage)
            {
               angleTangage += IncrÈmentAngleRotation;
               angleTangage = MathHelper.WrapAngle(angleTangage);
            }
            return angleTangage;
         }
         set { angleTangage = value; }
      }

      float angleRoulis;
      protected float AngleRoulis
      {
         get
         {
            if (Roulis)
            {
               angleRoulis += IncrÈmentAngleRotation;
               angleRoulis = MathHelper.WrapAngle(angleRoulis);
            }
            return angleRoulis;
         }
         set { angleRoulis = value; }
      }


      protected PrimitiveDeBaseAnimÈe(Game jeu, float homothÈtieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ)
         : base(jeu, homothÈtieInitiale, rotationInitiale, positionInitiale)
      {
         IntervalleMAJ = intervalleMAJ;
      }

      public override void Initialize()
      {
         HomothÈtie = HomothÈtieInitiale;
         InitialiserRotations();
         Position = PositionInitiale;
         GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
         IncrÈmentAngleRotation = MathHelper.Pi * IntervalleMAJ / 2;
         Temps…coulÈDepuisMAJ = 0;
         base.Initialize();
      }

      protected override void CalculerMatriceMonde()
      {
         Monde = Matrix.Identity *
                 Matrix.CreateScale(HomothÈtie) *
                 Matrix.CreateFromYawPitchRoll(AngleLacet, AngleTangage, AngleRoulis) *
                 Matrix.CreateTranslation(Position);
      }

      public override void Update(GameTime gameTime)
      {
         GÈrerClavier();
         float Temps…coulÈ = (float)gameTime.ElapsedGameTime.TotalSeconds;
         Temps…coulÈDepuisMAJ += Temps…coulÈ;
         if (Temps…coulÈDepuisMAJ >= IntervalleMAJ)
         {
            EffectuerMise¿Jour();
            Temps…coulÈDepuisMAJ = 0;
         }
      }

      protected virtual void EffectuerMise¿Jour()
      {
         if (Monde¿Recalculer)
         {
            CalculerMatriceMonde();
            Monde¿Recalculer = false;
         }
      }

      private void InitialiserRotations()
      {
         AngleLacet = RotationInitiale.Y;
         AngleTangage = RotationInitiale.X;
         AngleRoulis = RotationInitiale.Z;
      }

      protected virtual void GÈrerClavier()
      {
         if (GestionInput.EstEnfoncÈe(Keys.LeftControl) || GestionInput.EstEnfoncÈe(Keys.RightControl))
         {
            if (GestionInput.EstNouvelleTouche(Keys.Space))
            {
               InitialiserRotations();
               Monde¿Recalculer = true;
            }
            if (GestionInput.EstNouvelleTouche(Keys.D1) || GestionInput.EstNouvelleTouche(Keys.NumPad1))
            {
               Lacet = !Lacet;
            }
            if (GestionInput.EstNouvelleTouche(Keys.D2) || GestionInput.EstNouvelleTouche(Keys.NumPad2))
            {
               Tangage = !Tangage;
            }
            if (GestionInput.EstNouvelleTouche(Keys.D3) || GestionInput.EstNouvelleTouche(Keys.NumPad3))
            {
               Roulis = !Roulis;
            }
         }
         Monde¿Recalculer = Monde¿Recalculer || Lacet || Tangage || Roulis;
      }
   }
}