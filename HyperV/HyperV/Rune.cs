using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtelierXNA;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;

namespace HyperV
{
    public class Rune : PlanTexturé
    {
        float TempsActivationRune { get; set; }
        float TempsÉcouléMAJ { get; set; }
        bool EstSousJoueur { get; set; }
        public bool EstActivée { get; private set; }
        Camera2 Caméra { get; set; }
        CreateurModele CubeRuneActivée { get; set; }
        RessourcesManager<SoundEffect> SoundManager { get; set; }
        SoundEffect RuneActivée { get; set; }
        SoundEffect RuneDésactivée { get; set; }

        public Rune(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, Vector2 charpante, string nomTextureTuile, float intervalleMAJ)
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, charpante, nomTextureTuile, intervalleMAJ)
        { }

        public override void Initialize()
        {
            base.Initialize();
            TempsÉcouléMAJ = 4;
            EstActivée = false;
            RuneActivée = SoundManager.Find("Rune_Activée");
            RuneDésactivée = SoundManager.Find("Rune_Désactivée");
        }

        protected override void LoadContent()
        {
            Caméra = Game.Services.GetService(typeof(Caméra)) as Camera2;
            SoundManager = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            TempsÉcouléMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsActivationRune += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléMAJ >= 1 / 60f)
            {
                TesterPositionJoueur();
                if (EstSousJoueur)
                {
                    if (TempsActivationRune > 2)
                        ActiverRune();
                }
                TempsÉcouléMAJ = 0;
            }
            base.Update(gameTime);

        }

        private void TesterPositionJoueur()
        {
            const int GROSSEUR_RUNE = 2;
            if (Caméra.Position.X < PositionInitiale.X + GROSSEUR_RUNE && Caméra.Position.X > PositionInitiale.X - GROSSEUR_RUNE && Caméra.Position.Z > PositionInitiale.Z - GROSSEUR_RUNE && Caméra.Position.Z < PositionInitiale.Z + GROSSEUR_RUNE)
            {
                EstSousJoueur = true;
            }
            else
            {
                EstSousJoueur = false;
            }
        }

        private void ActiverRune()
        {
            if (EstActivée)
            {
                Game.Components.Remove(CubeRuneActivée);
                RuneDésactivée.Play();
                EstActivée = false;
            }
            else
            {
                CubeRuneActivée = new CreateurModele(Game, "axle", new Vector3(PositionInitiale.X, PositionInitiale.Y + 2, PositionInitiale.Z), 0.005f, 0);
                Game.Components.Add(new Afficheur3D(Game));
                Game.Components.Add(CubeRuneActivée);
                RuneActivée.Play();
                EstActivée = true;
            }
            TempsActivationRune = 0;
        }
    }
}
