using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtelierXNA
{
   public class SpriteMobile : Sprite
   {
      const int NB_PIXELS_DE_DÉPLACEMENT = 2;
      const float FACTEUR_ACCÉLÉRATION = 1f / 600f;
      const float INTERVALLE_MIN = 0.01f;
      float IntervalleMax { get; set; }
      float IntervalleMAJ { get; set; }
      float TempsÉcouléDepuisMAJ { get; set; }
      InputManager GestionInput { get; set; }
      protected int MargeGauche { get; set; }
      protected int MargeDroite { get; set; }
      protected int MargeHaut { get; set; }
      protected int MargeBas { get; set; }


      public SpriteMobile(Game game, string nomSprite, Vector2 positionDépart, float intervalleMAJ)
         : base(game, nomSprite, ValiderPosition(positionDépart, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height))
      {
         IntervalleMAJ = intervalleMAJ;
      }

      static Vector2 ValiderPosition(Vector2 position, int largeurÉcran, int hauteurÉcran)
      {
         float posX = MathHelper.Max(MathHelper.Min(position.X, largeurÉcran), 0);
         float posY = MathHelper.Max(MathHelper.Min(position.Y, hauteurÉcran), 0);
         return new Vector2(posX, posY);
      }

      public override void Initialize()
      {
         TempsÉcouléDepuisMAJ = 0;
         MargeGauche = 0;
         MargeHaut = 0;
         IntervalleMax = IntervalleMAJ;
         base.Initialize();
      }

      protected override void LoadContent()
      {
         base.LoadContent();
         GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
         CalculerMarges();
         AjusterPosition(0, 0);
      }

      protected virtual void CalculerMarges()
      {
         MargeDroite = Game.Window.ClientBounds.Width - Image.Width;
         MargeBas = Game.Window.ClientBounds.Height - Image.Height;
      }

      public override void Update(GameTime gameTime)
      {
         float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
         TempsÉcouléDepuisMAJ += TempsÉcoulé;
         if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
         {
            EffectuerMiseÀJour();
            TempsÉcouléDepuisMAJ = 0;
         }
      }

      protected virtual void EffectuerMiseÀJour()
      {
         GérerClavier();
      }

      void GérerClavier()
      {
         if (GestionInput.EstClavierActivé)
         {

            int déplacementHorizontal = GérerTouche(Keys.D) - GérerTouche(Keys.A);
            int déplacementVertical = GérerTouche(Keys.S) - GérerTouche(Keys.W);
            GérerAccélération();
            if (déplacementHorizontal != 0 || déplacementVertical != 0)
            {
               AjusterPosition(déplacementHorizontal, déplacementVertical);
            }
         }
      }

      int GérerTouche(Keys touche)
      {
         return GestionInput.EstEnfoncée(touche) ? NB_PIXELS_DE_DÉPLACEMENT : 0;
      }

      void GérerAccélération()
      {
         int modificateurAccélération = GérerTouche(Keys.PageDown) - GérerTouche(Keys.PageUp);
         if (modificateurAccélération != 0)
         {
            IntervalleMAJ += modificateurAccélération * FACTEUR_ACCÉLÉRATION;
            IntervalleMAJ = MathHelper.Max(MathHelper.Min(IntervalleMAJ, IntervalleMax), INTERVALLE_MIN);

         }
      }

      void AjusterPosition(int déplacementHorizontal, int déplacementVertical)
      {
         float posX = CalculerPosition(déplacementHorizontal, Position.X, MargeGauche, MargeDroite);
         float posY = CalculerPosition(déplacementVertical, Position.Y, MargeHaut, MargeBas);
         Position = new Vector2(posX, posY);
      }

      float CalculerPosition(int déplacement, float posActuelle, int BorneMin, int BorneMax)
      {
         float position = posActuelle + déplacement;
         return MathHelper.Min(MathHelper.Max(BorneMin, position), BorneMax);
      }
   }
}