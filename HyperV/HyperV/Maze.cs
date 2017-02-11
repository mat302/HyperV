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
    public class Maze //: PrimitiveDeBase
    {
        Vector3 Position { get; set; }
        float IntervalleMAJ { get; set; }
        protected InputManager GestionInput { get; private set; }
        float TempsÉcouléDepuisMAJ { get; set; }

        const int NB_TRIANGLES = 2;
        protected Vector3[,] PtsSommets { get; private set; }
        Vector3 Origine { get; set; }
        Vector2 Delta { get; set; }
        protected BasicEffect EffetDeBase { get; private set; }

        //VertexPositionColor[] Sommets { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures;
        Texture2D TextureTuile;
        VertexPositionTexture[] Sommets { get; set; }
        BlendState GestionAlpha { get; set; }

        Vector2[,] PtsTexture { get; set; }
        string NomTextureTuile { get; set; }
        string MazeImageName { get; set; }

        public Maze(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomTextureTuile, float intervalleMAJ, string mazeImageName) //: base(jeu, homothétieInitiale, rotationInitiale, positionInitiale)
        {
            IntervalleMAJ = intervalleMAJ;
            NomTextureTuile = nomTextureTuile;
            MazeImageName = mazeImageName;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        //public override void Initialize()
        //{
        //    GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
        //    NbSommets = ;
        //    PtsSommets = new Vector3[2, 2];
        //    CréerTableauPoints();
        //    CréerTableauSommets();
        //    Position = PositionInitiale;
        //    GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
        //    TempsÉcouléDepuisMAJ = 0;
        //    base.Initialize();
        //}

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        //public override void Update(GameTime gameTime)
        //{
        //    // TODO: Add your update code here

        //    base.Update(gameTime);
        //}
    }
}
