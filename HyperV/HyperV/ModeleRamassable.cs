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
    //DOIT UPDATER TON DLL POUR BON FONCTIONNEMENT
    public class ModeleRamassable : ObjetDeBase
    {
        public static bool Taken { get; set; }

        static ModeleRamassable()
        {
            Taken = false;
        }

        public bool Ramasser { get; set; }

        public bool EstRamassée { get; set; }
        public bool Placed { get; set; }

        private float Rayon { get; set; }

        CaméraJoueur CaméraJoueur { get; set; }

        //Matrix MondeInitial { get; set; }

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

        }

        public override void Initialize()
        {
            base.Initialize();
            EstRamassée = false;
            Placed = false;
            Rayon = 4;
            //MondeInitial = base.GetMonde();
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
                CalculerMonde();
                //Game.Window.Title = Position.ToString();
            }
            base.Update(gameTime);
        }

        private void CalculerMonde()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(Échelle);
            Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            Monde *= Matrix.CreateTranslation(Position);
        }


    }
}
