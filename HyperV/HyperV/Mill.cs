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
    class Mill : TuileTexturée
    {
        const int NUM_GEAR_WHEELS = 2;

        Gear[] Gears { get; set; }
        ObjetDeBase[] Axles { get; set; }
        float Timer { get; set; }
        float Interval { get; set; }
        Vector3[] Positions { get; set; }
        float Radius { get; set; }
        Camera2 Camera { get; set; }
        PressSpaceLabel PressSpaceLabel { get; set; }
        ModeleRamassable[] Takables { get; set; }
        InputManager InputManager { get; set; }
        bool[] Placed { get; set; }

        public Mill(Game game, float scale, Vector3 initialRotation, Vector3 initialPosition, Vector2 range, string textureName, float interval) : base(game, scale, initialRotation, initialPosition, range, textureName, interval)
        {
            // TODO: Construct any child components here
            Interval = interval;
            Timer = 0;
            Radius = 3;
            PressSpaceLabel = new PressSpaceLabel(Game);
            Gears = new Gear[NUM_GEAR_WHEELS];
            Positions = new Vector3[NUM_GEAR_WHEELS];
            Placed = new bool[NUM_GEAR_WHEELS];
            Takables = new ModeleRamassable[NUM_GEAR_WHEELS];
            Axles = new ObjetDeBase[NUM_GEAR_WHEELS];
            for (int i = 0; i < NUM_GEAR_WHEELS; ++i)
            {
                Placed[i] = false;
            }
            Positions[0] = new Vector3(299, 9, 100);
            Positions[1] = new Vector3(305.4f, 15.4f, 100);
            Gears[0] = new Gear(Game, "gear5", 0.025f, new Vector3(0, MathHelper.ToRadians(90), 0), Positions[0]);
            Axles[0] = new ObjetDeBase(Game, "axle", 0.01f, new Vector3(MathHelper.ToRadians(90), 0, 0), Positions[0]);
            Game.Components.Add(Gears[0]);
            Game.Components.Add(Axles[0]);
            Gears[1] = new Gear(Game, "gear2", 0.025f, new Vector3(0, MathHelper.ToRadians(90), 0), Positions[1]);
            Axles[1] = new ObjetDeBase(Game, "axle", 0.01f, new Vector3(MathHelper.ToRadians(90), 0, 0), Positions[1]);
            Game.Components.Add(Gears[1]);
            Game.Components.Add(Axles[1]);
            Takables[0] = new ModeleRamassable(Game, "gear5", 0.01f, new Vector3(0, 0, MathHelper.ToRadians(90)), new Vector3(370, 10, 100));
            Game.Components.Add(Takables[0]);
            Takables[1] = new ModeleRamassable(Game, "gear2", 0.025f, new Vector3(0, 0, MathHelper.ToRadians(90)), new Vector3(420, 10, 100));
            Game.Components.Add(Takables[1]);
            Gears[1].Visible = false;
            Gears[0].Visible = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //Game.Components.Add(new TuileTexturée(Game, 1, Vector3.Zero, new Vector3(Positions[1].X, Positions[1].Y, Positions[1].Z - 5), new Vector2(5, 5), "point", 1 / 60f));
            base.Initialize();
            Camera = Game.Services.GetService(typeof(Caméra)) as Camera2;
            InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Game.Components.Add(PressSpaceLabel);
            PressSpaceLabel.Visible = false;
        }

        bool Space { get; set; }
        bool Taken { get; set; }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            Space = InputManager.EstNouvelleTouche(Keys.Space) ? true : Space;
            Taken = InputManager.EstNouvelleTouche(Keys.E) ? true : Taken;
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Interval)
            {
                Gears[0].UpdateRotation(0.005f);
                Gears[1].UpdateRotation(-0.01f);
                for (int i = 0; i < NUM_GEAR_WHEELS; ++i)
                {
                    float? collision = Collision(new Ray(Camera.Position, (Camera as Camera2).Direction), i);
                    if (collision < 30 && collision != null && Takables[0].EstRamassée)
                    {
                        PressSpaceLabel.Visible = true;
                        if (Space && Takables[i].EstRamassée)
                        {
                            if (!Placed[i])
                            {
                                Takables[i].Visible = false;
                                Takables[i].Enabled = false;
                                Taken = false;
                                Gears[i].Visible = true;
                                Placed[i] = true;
                                Space = false;
                            }
                            else if (!Taken)
                            {
                                Gears[i].Visible = false;
                                Takables[i].Visible = true;
                                Takables[i].Enabled = true;
                                Taken = true;
                                Placed[i] = false;
                                Space = false;
                            }
                        }
                    }
                    else
                    {
                        PressSpaceLabel.Visible = false;
                    }
                }
                
                Timer = 0;
            }
            base.Update(gameTime);
        }

        public float? Collision(Ray ray, int i)
        {
            return BoundingSphere(i).Intersects(ray);
        }

        public BoundingSphere BoundingSphere(int i)
        {
            return new BoundingSphere(Positions[i], Radius);
        }
    }
}
