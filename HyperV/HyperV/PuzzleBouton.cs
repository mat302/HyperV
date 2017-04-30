using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using AtelierXNA;
using System.IO;
using System.Linq;
using System;

namespace HyperV
{
    public class PuzzleBouton : Microsoft.Xna.Framework.DrawableGameComponent
    {
        float DISTANCE_MINIMALE = 10;

        bool PremierBouton { get; set; }
        bool DeuxiemeBouton { get; set; }
        bool Troisi�meBouton { get; set; }
        bool Quatri�meBouton { get; set; }
        public bool EstCompl�t� { get; set; }

        float alpha { get; set; }
        float Temps�coul�MAJ { get; set; }
        int[] OrdreBoutons { get; set; }
        List<CreateurModele> ListeBoutons { get; set; }
        string PositionBoutons { get; set; }
        InputManager GestionInputs { get; set; }
        Camera2 Cam�ra { get; set; }
        RessourcesManager<SoundEffect> SoundManager { get; set; }
        SoundEffect ClocheR�ussi {get;set;}
        SoundEffect ClocheManqu�e { get; set; }
        SoundEffect PuzzleCompl�t� { get; set; }    

        public PuzzleBouton(Game game, int[] ordreBoutons, string positionBoutons)
            : base(game)
        {
            OrdreBoutons = ordreBoutons;
            PositionBoutons = positionBoutons;
        }
        protected override void LoadContent()
        {
            GestionInputs = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Cam�ra = Game.Services.GetService(typeof(Cam�ra)) as Camera2;
            SoundManager = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
        }

        public override void Initialize()
        {
            base.Initialize();
            ListeBoutons = new List<CreateurModele>();
            StreamReader fichier = new StreamReader(PositionBoutons);
            fichier.ReadLine();
            while (!fichier.EndOfStream)
            {
                string ligneLu = fichier.ReadLine();
                string[] ligneSplit = ligneLu.Split(';');
                CreateurModele x = new CreateurModele(Game, ligneSplit[0], new Vector3(float.Parse(ligneSplit[1]), float.Parse(ligneSplit[2]), float.Parse(ligneSplit[3])), int.Parse(ligneSplit[4]), int.Parse(ligneSplit[5]),"Rock");
                Game.Components.Add(new Afficheur3D(Game));
                Game.Components.Add(x);
                ListeBoutons.Add(x);
            }
            ClocheR�ussi = SoundManager.Find("Cloche_R�ussi");
            ClocheManqu�e = SoundManager.Find("Cloche_Manqu�");
            PuzzleCompl�t� = SoundManager.Find("PuzzleBoutonCompl�t�");
            alpha = 0;
            PremierBouton = false;
            DeuxiemeBouton = false;
            Troisi�meBouton = false;
            Quatri�meBouton = false;
            EstCompl�t� = false;
        }

        float? TrouverDistance(Ray autreObjet, BoundingSphere Sph�reDeCollision)
        {
            return Sph�reDeCollision.Intersects(autreObjet);
        }

        public override void Update(GameTime gameTime)
        {
            if (GestionInputs.EstNouveauClicGauche())
            {
                for (int i = 0; i < ListeBoutons.Capacity; ++i)
                {
                    if (EstABonneDistance(ListeBoutons[i]))
                    {
                        ListeBoutons[i].D�placementBouton = true;
                        V�rifierOrdre(i);
                    }
                }
            }
            foreach (CreateurModele bouton in ListeBoutons)
            {
                if (bouton.D�placementBouton)
                {
                    D�placerBouton(bouton, gameTime);
                }
            }
            if (Quatri�meBouton)
            {
                EstCompl�t� = true;
            }
        }

        bool EstABonneDistance(CreateurModele modele)
        {
            float? minDistance = float.MaxValue;
            BoundingSphere sph�re = new BoundingSphere(modele.GetPosition(), 2.14f);
            float? distance = TrouverDistance(new Ray(Cam�ra.Position, Cam�ra.Direction), sph�re);
            if (minDistance > distance)
            {
                minDistance = distance;
            }
            return minDistance < DISTANCE_MINIMALE;
        }

        void V�rifierOrdre(int boutonActiv�)
        {
            bool continuer = true;
            if (PremierBouton && DeuxiemeBouton && Troisi�meBouton && !Quatri�meBouton)
            {
                Quatri�meBouton = TesterQuatri�meBouton(boutonActiv�);
                continuer = false;
            }

            if (PremierBouton && DeuxiemeBouton && !Troisi�meBouton && continuer)
            {
                Troisi�meBouton = TesterTroisi�meBouton(boutonActiv�);
                continuer = false;
            }
            if (PremierBouton && !DeuxiemeBouton && continuer)
            {
                DeuxiemeBouton = TesterDeuxi�meBouton(boutonActiv�);
                continuer = false;
            }
            if (!PremierBouton && continuer)
            {
                PremierBouton = TesterPremierBouton(boutonActiv�);
            }
        }

        bool TesterPremierBouton(int boutonActiv�)
        {            
            bool estOk = false;
            if (boutonActiv� == OrdreBoutons[0]) //si bon bouton et il na pas ete encore peser correctement
            {                
                ClocheR�ussi.Play();
                estOk = true;
            }
            else
            {
                ClocheManqu�e.Play();
            }            
            return estOk;
        }

        bool TesterDeuxi�meBouton(int boutonActiv�)
        {
            bool estOk = false;
            if (boutonActiv� == OrdreBoutons[1]) //si bon bouton et il na pas ete encore peser correctement
            {
                ClocheR�ussi.Play();
                estOk = true;
            }
            else
            {
                ClocheManqu�e.Play();
                PremierBouton = false;
            }
            return estOk;
        }

        bool TesterTroisi�meBouton(int boutonActiv�)
        {
            bool estOk = false;
            if (boutonActiv� == OrdreBoutons[2]) //si bon bouton et il na pas ete encore peser correctement
            {
                ClocheR�ussi.Play();
                estOk = true;
            }
            else
            {
                ClocheManqu�e.Play();
                PremierBouton = false;
                DeuxiemeBouton = false;
            }
            return estOk;
        }

        bool TesterQuatri�meBouton(int boutonActiv�)
        {
            bool estOk = false;
            if (boutonActiv� == OrdreBoutons[3]) //si bon bouton et il na pas ete encore peser correctement
            {
                PuzzleCompl�t�.Play();
                estOk = true;
            }
            else
            {
                ClocheManqu�e.Play();
                PremierBouton = false;
                DeuxiemeBouton = false;
                Troisi�meBouton = false;
            }
            return estOk;
        }

        void D�placerBouton(CreateurModele bouton, GameTime gameTime)
        {
            Temps�coul�MAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Temps�coul�MAJ >= 1 / 60f)
            {
                if (bouton.D�placementBouton)
                {
                    bouton.D�placerModele(0.03f * new Vector3(-(float)(Math.Cos(MathHelper.ToRadians(alpha))), 0, 0));
                    alpha += 10;
                    if (alpha > 180)
                    {
                        bouton.D�placementBouton = false;
                        alpha = 0;
                    }
                }
                Temps�coul�MAJ = 0;
            }
        }        
    }
}