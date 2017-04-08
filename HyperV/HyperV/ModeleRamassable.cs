//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using AtelierXNA;


//namespace HyperV
//{
//    //DOIT UPDATER TON DLL POUR BON FONCTIONNEMENT
//    public class ModeleRamassable : ObjetDeBase
//    {
//        public static bool Taken { get; set; }

//        static ModeleRamassable()
//        {
//            Taken = false;
//        }

//        public bool Ramasser { get; set; }

//        public bool EstRamassée { get; set; }
//        public bool Placed { get; set; }

//        private float Rayon { get; set; }

//        CaméraJoueur CaméraJoueur { get; set; }

//        //Matrix MondeInitial { get; set; }

//        public BoundingSphere SphèreDeCollision
//        {
//            get { return new BoundingSphere(Position, Rayon); }
//        }

//        public float? EstEnCollision(Ray autreObjet)
//        {
//            return SphèreDeCollision.Intersects(autreObjet);
//        }

//        public ModeleRamassable(Game jeu, string nomModèle, float échelleInitiale,
//                                Vector3 rotationInitiale, Vector3 positionInitiale)
//            : base(jeu, nomModèle, échelleInitiale, rotationInitiale, positionInitiale)
//        {

//        }

//        public override void Initialize()
//        {
//            base.Initialize();
//            EstRamassée = false;
//            Placed = false;
//            Rayon = 4;
//            //MondeInitial = base.GetMonde();
//        }

//        protected override void LoadContent()
//        {
//            base.LoadContent();
//            CaméraJoueur = Game.Services.GetService(typeof(Caméra)) as CaméraJoueur;
//        }

//        public override void Update(GameTime gameTime)
//        {
//            if (EstRamassée)
//            {
//                Position = CaméraJoueur.Position + 4 * Vector3.Normalize(CaméraJoueur.Direction)
//                            + 2.5f * Vector3.Normalize(CaméraJoueur.Latéral)
//                            - 1.5f * Vector3.Normalize(Vector3.Cross(CaméraJoueur.Latéral, CaméraJoueur.Direction));
//                CalculerMonde();
//                //Game.Window.Title = Position.ToString();
//            }
//            base.Update(gameTime);
//        }

//        private void CalculerMonde()
//        {
//            Monde = Matrix.Identity;
//            Monde *= Matrix.CreateScale(Échelle);
//            Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
//            Monde *= Matrix.CreateTranslation(Position);
//        }


//    }
//}





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
    public class ModeleRamassable : ObjetDeBase
    {
        public static bool Taken { get; set; }

        static ModeleRamassable()
        {
            Taken = false;
        }
        public bool Placed { get; set; }
        public bool Ramasser { get; set; }
        //UP WAS COMMENTED

        public bool EstRamassée { get; set; }

        private float Rayon { get; set; }

        protected CaméraJoueur CaméraJoueur { get; set; }

        public BoundingSphere SphèreDeCollision
        {
            get { return new BoundingSphere(Position, Rayon); }
        }

        public float? EstEnCollision(Ray autreObjet)
        {
            return SphèreDeCollision.Intersects(autreObjet);
        }

        public ModeleRamassable(Game jeu, string nomModèle, float échelleInitiale,
                                Vector3 rotationInitiale, Vector3 positionInitiale)
            : base(jeu, nomModèle, échelleInitiale, rotationInitiale, positionInitiale)
        {
            EstRamassée = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            //EstRamassée = false;
            Rayon = 10;
            //Placed = false;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            CaméraJoueur = Game.Services.GetService(typeof(Caméra)) as CaméraJoueur;
        }

        public override void Update(GameTime gameTime)
        {
            if (EstRamassée)
            {
                Position = CaméraJoueur.Position + 4 * Vector3.Normalize(CaméraJoueur.Direction)
                            + 2.5f * Vector3.Normalize(CaméraJoueur.Latéral)
                            - 1.5f * Vector3.Normalize(Vector3.Cross(CaméraJoueur.Latéral, CaméraJoueur.Direction));
                CalculerAngles();
                CalculerMonde();
            }
            base.Update(gameTime);
        }

        protected float angleX { set; get; }

        protected float angleY { set; get; }

        protected virtual void CalculerAngles()
        {
            Vector3 DirectionXYZ = Vector3.Normalize(new Vector3(CaméraJoueur.Direction.X, CaméraJoueur.Direction.Y, CaméraJoueur.Direction.Z));

            angleX = -(float)Math.PI / 2f +//********
                (float)Math.PI * 3 / 2f +
                (DirectionXYZ.Z >= 0 ? -1 : 1) *
                (float)Math.Acos(Vector2.Dot((new Vector2(DirectionXYZ.X, DirectionXYZ.Z)),
                                              new Vector2(1, 0)));
            angleY =
                MathHelper.Pi / 2f -
                (float)Math.Acos(Vector2.Dot(new Vector2(DirectionXYZ.X, DirectionXYZ.Y),
                                             new Vector2(0, 1)));
        }

        private void CalculerMonde()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(Échelle);
            Monde *= Matrix.CreateFromYawPitchRoll(angleX, angleY, Rotation.Z);
            Monde *= Matrix.CreateTranslation(Position);

            //Game.Window.Title = CaméraJoueur.Direction.ToString() + "      " + MathHelper.ToDegrees(angleX).ToString() + "       " + MathHelper.ToDegrees(angleY).ToString().ToString();
        }
    }
}
