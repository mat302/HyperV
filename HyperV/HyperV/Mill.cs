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
        const int NUM_GEARS = 2;

        Gear[,] Gears { get; set; }
        ObjetDeBase[] Axles { get; set; }
        float Timer { get; set; }
        float Interval { get; set; }
        Vector3[] Positions { get; set; }
        Vector3[] GearPositions { get; set; }
        float Radius { get; set; }
        Camera2 Camera { get; set; }
        PressSpaceLabel PressSpaceLabel { get; set; }
        ModeleRamassable[] Takables { get; set; }
        InputManager InputManager { get; set; }
        bool[] Placed { get; set; }
        bool[] AxleTaken { get; set; }
        int[] AxleObjects { get; set; }

        public Mill(Game game, float scale, Vector3 initialRotation, Vector3 initialPosition, Vector2 range, string textureName, float interval) : base(game, scale, initialRotation, initialPosition, range, textureName)
        {
            Interval = interval;
            Timer = 0;
            Radius = 1;
            PressSpaceLabel = new PressSpaceLabel(Game);
            Gears = new Gear[NUM_GEARS, NUM_GEARS];
            Positions = new Vector3[NUM_GEARS];
            Placed = new bool[NUM_GEARS];
            Takables = new ModeleRamassable[NUM_GEARS];
            Axles = new ObjetDeBase[NUM_GEARS];
            AxleTaken = new bool[NUM_GEARS];
            AxleObjects = new int[NUM_GEARS];
            GearPositions = new Vector3[NUM_GEARS];
            for (int i = 0; i < NUM_GEARS; ++i)
            {
                AxleTaken[i] = false;
            }
            for (int i = 0; i < NUM_GEARS; ++i)
            {
                Placed[i] = false;
            }
            for (int i = 0; i < NUM_GEARS; ++i)
            {
                AxleObjects[i] = NUM_GEARS;
            }
            Positions[0] = new Vector3(299, 9, 100);
            GearPositions[0] = new Vector3(299, 9, 101);
            Positions[1] = new Vector3(305.4f, 15.4f, 100);
            GearPositions[1] = new Vector3(305.4f, 15.4f, 101);
            Gears[0, 0] = new Gear(Game, "gear5", 0.025f, new Vector3(0, MathHelper.ToRadians(90), 0), GearPositions[0]);
            Gears[0, 1] = new Gear(Game, "gear5", 0.025f, new Vector3(0, MathHelper.ToRadians(90), 0), GearPositions[1]);
            Axles[0] = new ObjetDeBase(Game, "axle", 0.01f, new Vector3(MathHelper.ToRadians(90), 0, 0), Positions[0]);
            Game.Components.Add(Gears[0, 0]);
            Game.Components.Add(Gears[0, 1]);
            Game.Components.Add(Axles[0]);
            Gears[1, 0] = new Gear(Game, "gear2", 0.025f, new Vector3(0, MathHelper.ToRadians(90), 0), GearPositions[0]);
            Gears[1, 1] = new Gear(Game, "gear2", 0.025f, new Vector3(0, MathHelper.ToRadians(90), 0), GearPositions[1]);
            Axles[1] = new ObjetDeBase(Game, "axle", 0.01f, new Vector3(MathHelper.ToRadians(90), 0, 0), Positions[1]);
            Game.Components.Add(Gears[1, 0]);
            Game.Components.Add(Gears[1, 1]);
            Game.Components.Add(Axles[1]);
            Takables[0] = new ModeleRamassable(Game, "gear5", 0.01f, new Vector3(0, 0, MathHelper.ToRadians(90)), new Vector3(370, 10, 100));
            Game.Components.Add(Takables[0]);
            Takables[1] = new ModeleRamassable(Game, "gear2", 0.025f, new Vector3(0, 0, MathHelper.ToRadians(90)), new Vector3(420, 10, 100));
            Game.Components.Add(Takables[1]);
            Gears[0, 0].Visible = false;
            Gears[0, 1].Visible = false;
            Gears[1, 0].Visible = false;
            Gears[1, 1].Visible = false;
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
        }

        public void AddLabel()
        {
            Game.Components.Add(PressSpaceLabel);
            PressSpaceLabel.Visible = false;
        }

        bool Space { get; set; }
        float? GameCollision { get; set; }
        bool Collided { get; set; }
        int TakenObject { get; set; }
        bool Taken { get; set; }
        bool First;

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
                Gears[0, 0].UpdateRotation(0.005f);
                Gears[0, 1].UpdateRotation(0.005f);
                Gears[1, 0].UpdateRotation(-0.01f);
                Gears[1, 1].UpdateRotation(-0.01f);
                Collided = false;
                for (int i = 0; i < NUM_GEARS && !Collided; ++i)
                {
                    GameCollision = Collision(new Ray(Camera.Position, (Camera as Camera2).Direction), i);
                    Collided = GameCollision != null;
                    TakenObject = ReturnTakenObject();
                    //Game.Window.Title = TakenObject.ToString();
                    if (/*collision < Radius + 0.5f && */ Collided /*&& Takables[i].EstRamassée*/ /*&& TakenObject != NUM_GEARS && Takables[TakenObject].EstRamassée && (!AxleTaken[i] && !Placed[TakenObject] || AxleTaken[i] && Placed[Taken])*/)
                    {
                        if (!AxleTaken[i] && TakenObject != NUM_GEARS)
                        {
                            PressSpaceLabel.Visible = true;
                            if (Space)
                            {
                                Takables[TakenObject].Visible = false;
                                Takables[TakenObject].Enabled = false;
                                Gears[TakenObject, i].Visible = true;
                                Placed[TakenObject] = true;
                                AxleTaken[i] = true;
                                Space = false;
                                AxleObjects[i] = TakenObject;
                                ModeleRamassable.Taken = false;
                                Takables[TakenObject].Placed = true;
                            }
                        }
                        else if (AxleTaken[i] && TakenObject == NUM_GEARS)
                        {
                            PressSpaceLabel.Visible = true;
                            if (Space)
                            {
                                Takables[AxleObjects[i]].Visible = true;
                                Takables[AxleObjects[i]].Enabled = true;
                                Gears[AxleObjects[i], i].Visible = false;
                                Placed[AxleObjects[i]] = false;
                                AxleTaken[i] = false;
                                Space = false;
                                Takables[AxleObjects[i]].Placed = false;
                                ModeleRamassable.Taken = true;
                                AxleObjects[i] = NUM_GEARS;
                            }
                        }
                        else
                        {
                            PressSpaceLabel.Visible = false;
                        }
                        //if (Space)
                        //{
                        //    if (!Placed[TakenObject, i])
                        //    {
                                
                        //        Placed[TakenObject, i] = true;
                        //        AxleTaken[i] = true;
                        //        Space = false;
                        //    }
                        //    else
                        //    {
                        //        Gears[TakenObject, i].Visible = false;
                        //        Takables[TakenObject].Visible = true;
                        //        Takables[TakenObject].Enabled = true;
                        //        Placed[TakenObject, i] = false;
                        //        AxleTaken[i] = false;
                        //        Space = false;
                        //    }
                        //}
                    }
                    else
                    {
                        PressSpaceLabel.Visible = false;
                    }
                }
                Space = false;
                Timer = 0;
            }
            base.Update(gameTime);
        }

        int ReturnTakenObject()
        {
            int i;
            for (i = 0; i < NUM_GEARS && (!Takables[i].EstRamassée || Placed[i]); ++i) { }
            return i;
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
