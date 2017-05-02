/*
SpriteMobileAnimé.cs
--------------------

Par Mathieu Godin

Rôle : Composant qui hérite de SpriteMobile et qui a 
       la particularité d'animer son sprite en défilant 
       différent sprites présent sur la même image

Créé : 1er octobre 2016
*/
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
    /// <summary>
    /// Composant qui peut afficher un sprite animé par un défilement de sprites présents sur la même image chargée
    /// </summary>
    public class SpriteMobileAnimé : SpriteMobile
   {
      /// <summary>
      /// Énumération de nombres allant de 0 à 3 représentants les facteurs propres à retrouver le sprite représentant la bonne direction dans laquelle on se déplace
      /// </summary>
      enum FacteurSélectionDirectionSprite
      {
           GAUCHE, BAS, DROITE, HAUT
      }

      const float AUCUN_DÉPLACEMENT = 0.0F;
      const int ORIGINE = 0;
      Vector2 NbImages {get; set;}
      Vector2 Delta { get; set; }
      Rectangle RectangleSource { get; set; }
      Vector2 AnciennePosition { get; set; }
      Vector2 Déplacement { get; set; }
      Vector2 DéplacementNul { get; set; }

      /// <summary>
      /// Constructeur de la classe SpriteMobileAnimé
      /// </summary>
      /// <param name="game">Jeu dde type Game</param>
      /// <param name="nomSprite">Nom du sprite tel qu'inscrit dans son dossier respectif</param>
      /// <param name="nbImages">Vector2 indiquant le nombre d'images en hauteur et en largeur</param>
      /// <param name="positionDépart">Position de départ du sprite</param>
      /// <param name="intervalleMAJ">Intervalle de mise à jour du sprite</param>
      public SpriteMobileAnimé(Game game, string nomSprite, Vector2 nbImages, Vector2 positionDépart, float intervalleMAJ) : base(game, nomSprite, positionDépart, intervalleMAJ)
      {
            NbImages = new Vector2(nbImages.X, nbImages.Y);
      }

      /// <summary>
      /// Méthode de chargement de contenu nécessaire au SpriteMobileAnimé
      /// </summary>
      protected override void LoadContent()
      {
         base.LoadContent();
         RectangleSource = new Rectangle(ORIGINE, (int)FacteurSélectionDirectionSprite.DROITE * (int)Delta.Y, (int)Delta.X, (int)Delta.Y);
         DéplacementNul = new Vector2(AUCUN_DÉPLACEMENT, AUCUN_DÉPLACEMENT);
      }

      protected override void CalculerMarges()
      {
         Delta = new Vector2(Image.Width, Image.Height) / NbImages;
         MargeDroite = Game.Window.ClientBounds.Width - (int)Delta.X;
         MargeBas = Game.Window.ClientBounds.Height - (int)Delta.Y;
      }

      /// <summary>
      /// Méthode qui met à jour le SpriteMobileAnimé selon le temps écoulé et le déplacement causé par les touches de direction WASD enfoncées
      /// </summary>
      protected override void EffectuerMiseÀJour()
      {
         AnciennePosition = new Vector2(Position.X, Position.Y);
         base.EffectuerMiseÀJour();
         Déplacement = Position - AnciennePosition;
         if (Déplacement != DéplacementNul)
         {
            RectangleSource = new Rectangle((RectangleSource.X + (int)Delta.X) % Image.Width, Déplacement.Y > AUCUN_DÉPLACEMENT ? (int)FacteurSélectionDirectionSprite.BAS * (int)Delta.Y : (Déplacement.Y < AUCUN_DÉPLACEMENT ? (int)FacteurSélectionDirectionSprite.HAUT * (int)Delta.Y : (Déplacement.X > AUCUN_DÉPLACEMENT ? (int)FacteurSélectionDirectionSprite.DROITE * (int)Delta.Y : (int)FacteurSélectionDirectionSprite.GAUCHE * (int)Delta.Y)), (int)Delta.X, (int)Delta.Y);
         }
      }

      /// <summary>
      /// Méthode qui dessine le SpriteMobileAnimé à l'écran
      /// </summary>
      /// <param name="gameTime">Objet contenant l'information de temps de jeu de type GameTime</param>
      public override void Draw(GameTime gameTime)
      {
         GestionSprites.Draw(Image, Position, RectangleSource, Color.White);
      }
   }
}
