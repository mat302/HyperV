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
using AtelierXNA;

namespace HyperV
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Food : ObjetDeBase
    {
        CaméraJoueur Camera { get; set; }
        float Timer { get; set; }
        float Interval { get; set; }
        LifeBar[] LifeBars { get; set; }
        InputManager InputManager { get; set; }
        GamePadManager GamePadManager { get; set; }
        int Heal { get; set; }
        PressSpaceLabel PressSpaceLabel { get; set; }

        public Food(Game game, string name, float scale, Vector3 rotation, Vector3 position, int heal, float interval) : base(game, name, scale, rotation, position)
        {
            Interval = interval;
            Heal = heal;
            PressSpaceLabel = new PressSpaceLabel(Game);
            Input = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            Camera = Game.Services.GetService(typeof(Caméra)) as CaméraJoueur;
            LifeBars = Game.Services.GetService(typeof(LifeBar[])) as LifeBar[];
            InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GamePadManager = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
            base.Initialize();
        }

        public void AddLabel()
        {
            Game.Components.Add(PressSpaceLabel);
            PressSpaceLabel.Visible = false;
        }

        public void RemoveLabel()
        {
            Game.Components.Remove(PressSpaceLabel);
        }

        bool Input { get; set; }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Input = InputManager.EstNouvelleTouche(Keys.Space) || GamePadManager.EstNouveauBouton(Buttons.A) ? true : Input;
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Interval)
            {
                if (Vector3.Distance(Camera.Position, Position) < 5)
                {
                    PressSpaceLabel.Visible = true;
                    if (Input)
                    {
                        LifeBars[0].Heal(100);
                        RemoveLabel();
                        Game.Components.Remove(this);
                    }
                }
                else
                {
                    PressSpaceLabel.Visible = false;
                }
                Timer = 0;
            }
        }
    }
}
