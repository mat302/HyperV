using AtelierXNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace HyperV
{
    public class Camera2 : Cam�raJoueur
    {
        //Added from first Camera1
        float Height { get; set; }

        Maze Maze { get; set; }
        List<Character> Characters { get; set; }

        public Camera2(Game jeu, Vector3 positionCam�ra, Vector3 cible, Vector3 orientation, float intervalleMAJ)
            : base(jeu, positionCam�ra, cible, orientation, intervalleMAJ)
        { }

        protected override void ChargerContenu()
        {
            base.ChargerContenu();
            Maze = Game.Services.GetService(typeof(Maze)) as Maze;
            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
        }

        protected override void G�rerD�placement(float direction, float lat�ral)
        {
            base.G�rerD�placement(direction, lat�ral);

            if (Maze.CheckForCollisions(Position))
            {
                Position -= direction * VitesseTranslation * Direction;
                Position += lat�ral * VitesseTranslation * Lat�ral;
            }
        }
    }
}

