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
        bool TroisièmeBouton { get; set; }
        bool QuatrièmeBouton { get; set; }
        public bool EstComplété { get; set; }
        int NumeroSave { get; set; }
        float alpha { get; set; }
        float TempsÉcouléMAJ { get; set; }
        int[] OrdreBoutons { get; set; }
        List<CreateurModele> ListeBoutons { get; set; }
        string PositionBoutons { get; set; }
        InputManager GestionInputs { get; set; }
        GamePadManager GestionManette { get; set; }
        Camera2 Caméra { get; set; }
        RessourcesManager<SoundEffect> SoundManager { get; set; }
        SoundEffect ClocheRéussi { get; set; }
        SoundEffect ClocheManquée { get; set; }
        SoundEffect PuzzleComplété { get; set; }

        public PuzzleBouton(Game game, int[] ordreBoutons, string positionBoutons, int numeroSave)
            : base(game)
        {
            OrdreBoutons = ordreBoutons;
            PositionBoutons = positionBoutons;
            NumeroSave = numeroSave;
        }

        protected override void LoadContent()
        {
            GestionInputs = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionManette = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
            Caméra = Game.Services.GetService(typeof(Caméra)) as Camera2;
            SoundManager = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
        }

        public override void Initialize()
        {
            base.Initialize();
            ListeBoutons = new List<CreateurModele>();
            StreamReader fichier = new StreamReader(PositionBoutons);
            StreamReader save = new StreamReader("../../../WPFINTERFACE/Launching Interface/Saves/PuzzlesSave" + NumeroSave + ".txt");

            string ligneSave = save.ReadLine();
            fichier.ReadLine();
            while (!fichier.EndOfStream)
            {
                string ligneLu = fichier.ReadLine();
                string[] ligneSplit = ligneLu.Split(';');
                CreateurModele x = new CreateurModele(Game, ligneSplit[0], new Vector3(float.Parse(ligneSplit[1]), float.Parse(ligneSplit[2]), float.Parse(ligneSplit[3])), int.Parse(ligneSplit[4]), int.Parse(ligneSplit[5]), "Rock");
                Game.Components.Add(new Afficheur3D(Game));
                Game.Components.Add(x);
                ListeBoutons.Add(x);
            }
            ClocheRéussi = SoundManager.Find("Cloche_Réussi");
            ClocheManquée = SoundManager.Find("Cloche_Manqué");
            PuzzleComplété = SoundManager.Find("PuzzleBoutonComplété");
            alpha = 0;
            PremierBouton = false;
            DeuxiemeBouton = false;
            TroisièmeBouton = false;
            QuatrièmeBouton = false;
            EstComplété = false;
            if (ligneSave == "True")
            {
                EstComplété = true;
            }
            fichier.Close();
            save.Close();
        }

        float? TrouverDistance(Ray autreObjet, BoundingSphere SphèreDeCollision)
        {
            return SphèreDeCollision.Intersects(autreObjet);
        }

        public override void Update(GameTime gameTime)
        {
            //Cheat pour mettre dans le titre l'ordre des boutons quil faut peser !
            if (GestionInputs.EstEnfoncée(Microsoft.Xna.Framework.Input.Keys.B))
            {
                Game.Window.Title = OrdreBoutons[0].ToString() + OrdreBoutons[1].ToString() + OrdreBoutons[2].ToString() + OrdreBoutons[3].ToString();
            }


            if (GestionInputs.EstNouveauClicGauche() || GestionInputs.EstNouvelleTouche(Microsoft.Xna.Framework.Input.Keys.R) || GestionManette.EstNouveauBouton(Microsoft.Xna.Framework.Input.Buttons.A))
            {
                for (int i = 0; i < ListeBoutons.Capacity; ++i)
                {
                    if (EstABonneDistance(ListeBoutons[i]))
                    {
                        ListeBoutons[i].DéplacementBouton = true;
                        VérifierOrdre(i);
                    }
                }
            }
            foreach (CreateurModele bouton in ListeBoutons)
            {
                if (bouton.DéplacementBouton)
                {
                    DéplacerBouton(bouton, gameTime);
                }
            }
            if (QuatrièmeBouton)
            {
                EstComplété = true;
                Save();
            }
        }

        bool EstABonneDistance(CreateurModele modele)
        {
            float? minDistance = float.MaxValue;
            BoundingSphere sphère = new BoundingSphere(modele.GetPosition(), 2.14f);
            float? distance = TrouverDistance(new Ray(Caméra.Position, Caméra.Direction), sphère);
            if (minDistance > distance)
            {
                minDistance = distance;
            }
            return minDistance < DISTANCE_MINIMALE;
        }

        void VérifierOrdre(int boutonActivé)
        {
            bool continuer = true;
            if (PremierBouton && DeuxiemeBouton && TroisièmeBouton && !QuatrièmeBouton)
            {
                QuatrièmeBouton = TesterQuatrièmeBouton(boutonActivé);
                continuer = false;
            }

            if (PremierBouton && DeuxiemeBouton && !TroisièmeBouton && continuer)
            {
                TroisièmeBouton = TesterTroisièmeBouton(boutonActivé);
                continuer = false;
            }
            if (PremierBouton && !DeuxiemeBouton && continuer)
            {
                DeuxiemeBouton = TesterDeuxièmeBouton(boutonActivé);
                continuer = false;
            }
            if (!PremierBouton && continuer)
            {
                PremierBouton = TesterPremierBouton(boutonActivé);
            }
        }

        bool TesterPremierBouton(int boutonActivé)
        {
            bool estOk = false;
            if (boutonActivé == OrdreBoutons[0]) //si bon bouton et il na pas ete encore peser correctement
            {
                ClocheRéussi.Play();
                estOk = true;
            }
            else
            {
                ClocheManquée.Play();
            }
            return estOk;
        }

        bool TesterDeuxièmeBouton(int boutonActivé)
        {
            bool estOk = false;
            if (boutonActivé == OrdreBoutons[1]) //si bon bouton et il na pas ete encore peser correctement
            {
                ClocheRéussi.Play();
                estOk = true;
            }
            else
            {
                ClocheManquée.Play();
                PremierBouton = false;
            }
            return estOk;
        }

        bool TesterTroisièmeBouton(int boutonActivé)
        {
            bool estOk = false;
            if (boutonActivé == OrdreBoutons[2]) //si bon bouton et il na pas ete encore peser correctement
            {
                ClocheRéussi.Play();
                estOk = true;
            }
            else
            {
                ClocheManquée.Play();
                PremierBouton = false;
                DeuxiemeBouton = false;
            }
            return estOk;
        }

        bool TesterQuatrièmeBouton(int boutonActivé)
        {
            bool estOk = false;
            if (boutonActivé == OrdreBoutons[3]) //si bon bouton et il na pas ete encore peser correctement
            {
                PuzzleComplété.Play();
                estOk = true;
            }
            else
            {
                ClocheManquée.Play();
                PremierBouton = false;
                DeuxiemeBouton = false;
                TroisièmeBouton = false;
            }
            return estOk;
        }

        void DéplacerBouton(CreateurModele bouton, GameTime gameTime)
        {
            TempsÉcouléMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléMAJ >= 1 / 60f)
            {
                if (bouton.DéplacementBouton)
                {
                    bouton.DéplacerModele(0.03f * new Vector3(0, 0, -(float)(Math.Cos(MathHelper.ToRadians(alpha)))));
                    alpha += 10;
                    if (alpha > 180)
                    {
                        bouton.DéplacementBouton = false;
                        alpha = 0;
                    }
                }
                TempsÉcouléMAJ = 0;
            }
        }

        void Save()
        {
            StreamWriter writer = new StreamWriter("../../../WPFINTERFACE/Launching Interface/Saves/PuzzlesSave" + NumeroSave.ToString() + ".txt");
            writer.WriteLine(true);
            writer.Close();
        }
    }
}
