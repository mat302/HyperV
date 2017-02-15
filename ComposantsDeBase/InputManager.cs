using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace AtelierXNA
{
   public class InputManager : Microsoft.Xna.Framework.GameComponent
   {
      Keys[] AnciennesTouches { get; set; }
      Keys[] NouvellesTouches { get; set; }
      KeyboardState ÉtatClavier { get; set; }
      MouseState AncienÉtatSouris { get; set; }
      MouseState NouvelÉtatSouris { get; set; }

      public InputManager(Game game)
         : base(game)
      { }

      public override void Initialize()
      {
         NouvellesTouches = new Keys[0];
         AnciennesTouches = NouvellesTouches;
         NouvelÉtatSouris = Mouse.GetState();
         AncienÉtatSouris = NouvelÉtatSouris;
         base.Initialize();
      }

      public override void Update(GameTime gameTime)
      {
         AnciennesTouches = NouvellesTouches;
         ÉtatClavier = Keyboard.GetState();
         NouvellesTouches = ÉtatClavier.GetPressedKeys();
         if (EstSourisActive)
         {
            ActualiserÉtatSouris();
         }
      }

      public bool EstClavierActivé
      {
         get { return NouvellesTouches.Length > 0; }
      }

      public bool EstEnfoncée(Keys touche)
      {
         return ÉtatClavier.IsKeyDown(touche);
      }

      public bool EstNouvelleTouche(Keys touche)
      {
         int nbTouches = AnciennesTouches.Length;
         bool estNouvelleTouche = ÉtatClavier.IsKeyDown(touche);
         int i = 0;
         while (i < nbTouches && estNouvelleTouche)
         {
            estNouvelleTouche = AnciennesTouches[i] != touche;
            ++i;
         }
         return estNouvelleTouche;
      }

      void ActualiserÉtatSouris()
      {
         AncienÉtatSouris = NouvelÉtatSouris;
         NouvelÉtatSouris = Mouse.GetState();
      }

      public bool EstSourisActive
      {
         get { return Game.IsMouseVisible; }
      }

      public bool EstAncienClicDroit()
      {
         return NouvelÉtatSouris.RightButton == ButtonState.Pressed && 
                AncienÉtatSouris.RightButton == ButtonState.Pressed;
      }

      public bool EstAncienClicGauche()
      {
         return NouvelÉtatSouris.LeftButton == ButtonState.Pressed && AncienÉtatSouris.LeftButton == ButtonState.Pressed;
      }

      public bool EstNouveauClicDroit()
      {
         return NouvelÉtatSouris.RightButton == ButtonState.Pressed && AncienÉtatSouris.RightButton == ButtonState.Released;
      }

      public bool EstNouveauClicGauche()
      {
         return NouvelÉtatSouris.LeftButton == ButtonState.Pressed && 
                AncienÉtatSouris.LeftButton == ButtonState.Released;
      }

      public Point GetPositionSouris()
      {
         return new Point(NouvelÉtatSouris.X, NouvelÉtatSouris.Y);
      }
   }
}