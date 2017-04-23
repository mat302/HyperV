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
    public class SphèreRythmée : SphèreTexturée, ICollisionable
    {
        public bool ÀDétruire { get; set; }

        #region
        float Rayon { get; set; }

        public new bool EstEnCollision(object autreObjet)
        {
            BoundingSphere obj2 = (autreObjet as ICollisionable).SphèreDeCollision;
            return obj2.Intersects(SphèreDeCollision);
        }

        public new BoundingSphere SphèreDeCollision
        {
            get
            {
                return new BoundingSphere(Position, Rayon);
            }
        }
        #endregion

        public Vector3 Extrémité1 { get; set; }
        Vector3 Extrémité2 { get; set; }
        Vector3 VecteurDéplacement { get; set; }
        int i { get; set; }
        LifeBar[] LifeBars { get; set; }

        NiveauRythmé Niveau { get; set; }

        public SphèreRythmée(Game jeu, float homothétieInitiale, Vector3 rotationInitiale,
                       Vector3 positionInitiale, float rayon, Vector2 charpente,
                       string nomTexture, float intervalleMAJ, Vector3 extrémité2)
            : base(jeu, homothétieInitiale, rotationInitiale,
                   positionInitiale, rayon, charpente,
                   nomTexture, intervalleMAJ)
        {
            Rayon = rayon;//****

            Extrémité1 = positionInitiale;
            Extrémité2 = extrémité2;
        }

        public override void Initialize()
        {
            base.Initialize();
            ÀDétruire = false;
            i = 0;
            VecteurDéplacement = Vector3.Normalize(Extrémité2 - Extrémité1);
            Niveau = Game.Services.GetService(typeof(NiveauRythmé)) as NiveauRythmé;
            LifeBars = Game.Services.GetService(typeof(LifeBar[])) as LifeBar[];

        }

        protected override void EffectuerMiseÀJour()
        {
            base.EffectuerMiseÀJour();

            Position += VecteurDéplacement;
            CalculerMatriceMonde();
            i++;

            if(i > (Extrémité1 - Extrémité2).Length() + 3 || ÀDétruire)
            {
                if (!ÀDétruire)
                {
                    Niveau.PositionCubeRouge = Extrémité2;
                    LifeBars[0].Attack(10);
                }
                Game.Components.Remove(this);

                //faire enlever de la vie
            }

        }


    }
}
