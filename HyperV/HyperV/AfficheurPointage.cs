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
    public class AfficheurPointage : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //Constantes
        const int MARGE_BAS = 200;
        //const int MARGE_DROITE = 700;
        const float AUCUNE_ROTATION = 0f;
        const float AUCUNE_HOMOTHÉTIE = 1f;
        const float AVANT_PLAN = 0f;

        //Constructeur
        readonly string NomFont;
        readonly Color CouleurFPS;
        readonly float IntervalleMAJ;

        //Initialize
        float TempsÉcouléDepuisMAJ { get; set; }
        public string Chaîne { get; set; }
        Vector2 PositionGaucheCentre { get; set; }
        Vector2 PositionChaîne { get; set; }

        //LoadContent
        SpriteBatch GestionSprites { get; set; }
        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        SpriteFont PoliceDeCaractères { get; set; }
        

        public AfficheurPointage(Game game, string nomFont, Color couleurFPS, float intervalleMAJ)
           : base(game)
        {
            NomFont = nomFont;
            CouleurFPS = couleurFPS;
            IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            DrawOrder = 1000;
            TempsÉcouléDepuisMAJ = 0;
            Chaîne = "";
            PositionGaucheCentre = new Vector2(30,
                                            Game.Window.ClientBounds.Height - MARGE_BAS);
            PositionChaîne = PositionGaucheCentre;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionSprites = new SpriteBatch(Game.GraphicsDevice);
            GestionnaireDeFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            PoliceDeCaractères = GestionnaireDeFonts.Find(NomFont);
        }

        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                EffectuerMAJ();
                TempsÉcouléDepuisMAJ = 0;
            }
        }

        void EffectuerMAJ()
        {
            //Vector2 dimension = PoliceDeCaractères.MeasureString(ChaîneFPS);
            //PositionChaîne = PositionGaucheCentre - dimension;
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            GestionSprites.DrawString(PoliceDeCaractères, Chaîne, PositionChaîne, CouleurFPS, AUCUNE_ROTATION,
                                      Vector2.Zero, AUCUNE_HOMOTHÉTIE, SpriteEffects.None, AVANT_PLAN);
            GestionSprites.End();
        }
    }
}

