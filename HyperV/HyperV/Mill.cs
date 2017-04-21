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
        int NumGears { get; set; }
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
        string MillImageName { get; set; }
        RessourcesManager<Texture2D> TextureManager { get; set; }
        Texture2D MillImage { get; set; }
        int NumTexels { get; set; }
        Color[] TextureData { get; set; }
        List<Vector3> GearInfo { get; set; }

        public Mill(Game game, float scale, Vector3 initialRotation, Vector3 initialPosition, string textureName, Vector2 range, float interval, string millImageName) : base(game, scale, initialRotation, initialPosition, range, textureName)
        {
            MillImageName = millImageName;
            Interval = interval;
            Timer = 0;
            Radius = 1;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            PressSpaceLabel = new PressSpaceLabel(Game);
            InitializeMillData();
            for (int i = 0; i < NumGears; ++i)
            {
                Positions[i] = new Vector3(PositionInitiale.X + GearInfo[i].X - range.X / 2, PositionInitiale.Y - GearInfo[i].Y + range.Y / 2, PositionInitiale.Z);
                GearPositions[i] = new Vector3(PositionInitiale.X + GearInfo[i].X - range.X / 2, PositionInitiale.Y - GearInfo[i].Y + range.Y / 2, PositionInitiale.Z + 1);
                Axles[i] = new ObjetDeBase(Game, "axle", 0.01f, new Vector3(MathHelper.ToRadians(90), 0, 0), Positions[i]);
                Game.Components.Add(Axles[i]);
                Takables[i] = new ModeleRamassable(Game, "gear" + GearInfo[i].Z.ToString(), 0.01f, new Vector3(0, 0, MathHelper.ToRadians(90)), new Vector3(350 + GearInfo[i].X / 10, 10, 100 + GearInfo[i].Y / 10));
                Game.Components.Add(Takables[i]);
            }
            for (int i = 0; i < NumGears; ++i)
            {
                for (int j = 0; j < NumGears; ++j)
                {
                    Gears[i, j] = new Gear(Game, "gear" + GearInfo[i].Z.ToString(), 0.025f, new Vector3(0, MathHelper.ToRadians(90), 0), GearPositions[j]);
                    Game.Components.Add(Gears[i, j]);
                    Gears[i, j].Visible = false;
                }
            }
        }

        void InitializeMillData()
        {
            MillImage = TextureManager.Find(MillImageName);
            NumTexels = MillImage.Width * MillImage.Height;
            TextureData = new Color[NumTexels];
            MillImage.GetData<Color>(TextureData);
            NumGears = 0;
            GearInfo = new List<Vector3>();
            for (int i = 0; i < NumTexels; ++i)
            {
                if (TextureData[i].R != 0 || TextureData[i].G != 0 || TextureData[i].B != 0)
                {
                    if (TextureData[i].R == 237 && TextureData[i].G == 28 && TextureData[i].B == 36)
                    {
                        ++NumGears;
                        GearInfo.Add(new Vector3(i - i / MillImage.Height * MillImage.Height, i / MillImage.Height, 2));
                    }
                    else if (TextureData[i].R == 34 && TextureData[i].G == 177 && TextureData[i].B == 76)
                    {
                        ++NumGears;
                        GearInfo.Add(new Vector3(i - i / MillImage.Height * MillImage.Height, i / MillImage.Height, 5));
                    }
                }
            }
            Gears = new Gear[NumGears, NumGears];
            Positions = new Vector3[NumGears];
            Placed = new bool[NumGears];
            Takables = new ModeleRamassable[NumGears];
            Axles = new ObjetDeBase[NumGears];
            AxleTaken = new bool[NumGears];
            AxleObjects = new int[NumGears];
            GearPositions = new Vector3[NumGears];
            for (int i = 0; i < NumGears; ++i)
            {
                AxleTaken[i] = false;
            }
            for (int i = 0; i < NumGears; ++i)
            {
                Placed[i] = false;
            }
            for (int i = 0; i < NumGears; ++i)
            {
                AxleObjects[i] = NumGears;
            }
        }

        public void RemoveComponents()
        {
            Game.Components.Remove(Gears[0, 0]);
            Game.Components.Remove(Gears[0, 1]);
            Game.Components.Remove(Axles[0]);
            Game.Components.Remove(Gears[1, 0]);
            Game.Components.Remove(Gears[1, 1]);
            Game.Components.Remove(Axles[1]);
            Game.Components.Remove(Takables[0]);
            Game.Components.Remove(Takables[1]);
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

        public void RemoveLabel()
        {
            Game.Components.Remove(PressSpaceLabel);
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
                for (int i = 0; i < NumGears; ++i)
                {
                    for (int j = 0; j < NumGears; ++j)
                    {
                        if (GearInfo[i].Z == 5)
                        {
                            Gears[i, j].UpdateRotation(0.005f);
                        }
                        else if (GearInfo[i].Z == 2)
                        {
                            Gears[i, j].UpdateRotation(-0.01f);
                        }
                    }
                }
                //Gears[0, 0].UpdateRotation(0.005f);
                //Gears[0, 1].UpdateRotation(0.005f);
                //Gears[1, 0].UpdateRotation(-0.01f);
                //Gears[1, 1].UpdateRotation(-0.01f);
                Collided = false;
                for (int i = 0; i < NumGears && !Collided; ++i)
                {
                    GameCollision = Collision(new Ray(Camera.Position, (Camera as Camera2).Direction), i);
                    Collided = GameCollision != null;
                    TakenObject = ReturnTakenObject();
                    if (/*collision < Radius + 0.5f && */ Collided /*&& Takables[i].EstRamassée*/ /*&& TakenObject != NumGears && Takables[TakenObject].EstRamassée && (!AxleTaken[i] && !Placed[TakenObject] || AxleTaken[i] && Placed[Taken])*/)
                    {
                        if (!AxleTaken[i] && TakenObject != NumGears)
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
                        else if (AxleTaken[i] && TakenObject == NumGears)
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
                                AxleObjects[i] = NumGears;
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
            for (i = 0; i < NumGears && (!Takables[i].EstRamassée || Placed[i]); ++i) { }
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
