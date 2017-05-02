using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
    public class Drapeau : PlanTexturé
    {
        //Initialement gérées par le constructeur
        readonly float MaxVariation;
        readonly float IntervalleVariation;

        //Initialement gérées par Initialize()
        float tempsÉcouléDepuisMAJ { get; set; }
        float tempsÉcoulé { get; set; }
        float tempsTotal { get; set; }
        RasterizerState JeuRasterizerState { get; set; }

        public Drapeau(Game jeu, float homothétieInitiale, Vector3 rotationInitiale,
                       Vector3 positionInitiale, Vector2 étendue, Vector2 charpente,
                       string nomTexture, float maxVariation, float intervalleVariation,
                       float intervalleMAJ)
            :base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, 
                  charpente, nomTexture, intervalleMAJ)
        {
            MaxVariation = maxVariation;
            IntervalleVariation = intervalleVariation;
        }

        public override void Initialize()
        {
            tempsÉcouléDepuisMAJ = 0;
            tempsÉcoulé = 0;
            tempsTotal = 0;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            tempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (tempsÉcouléDepuisMAJ >= IntervalleVariation)
            {
                CréerTableauPoints();
                InitialiserSommets();
                tempsÉcouléDepuisMAJ = 0;
            }
            base.Update(gameTime);
        }

        private void CréerTableauPoints()
        {
            for (int i = 0; i < PtsSommets.GetLength(0); ++i)
            {
                for (int j = 0; j < PtsSommets.GetLength(1); ++j)
                {
                    PtsSommets[i, j] = new Vector3(PtsSommets[i, j].X, PtsSommets[i, j].Y, GetVariation(i,j));
                }
            }
        }

        float GetVariation(int i, int j)
        {
            tempsTotal += tempsÉcoulé;
            tempsTotal = tempsTotal > 10*Math.PI ? 0 : tempsTotal;
            return (MaxVariation *(float)Math.Sin(PtsSommets[i, j].X + 2*tempsTotal));
        }

        public override void Draw(GameTime gameTime)
        {
            RasterizerState oldRasterizerState = GraphicsDevice.RasterizerState;
            RasterizerState jeuRasterizerState = new RasterizerState();
            jeuRasterizerState.CullMode = CullMode.None;

            jeuRasterizerState.FillMode = oldRasterizerState.FillMode;
            GraphicsDevice.RasterizerState = jeuRasterizerState;
            base.Draw(gameTime);
            GraphicsDevice.RasterizerState = oldRasterizerState;
        }
    }
}
