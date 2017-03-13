using AtelierXNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace HyperV
{
    public class Camera2 : CaméraJoueur
    {
        //Added from first Camera1
        float Height { get; set; }

        Maze Maze { get; set; }
        List<Character> Characters { get; set; }

        public Camera2(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ)
            : base(jeu, positionCaméra, cible, orientation, intervalleMAJ)
        { }

        protected override void ChargerContenu()
        {
            base.ChargerContenu();
            Maze = Game.Services.GetService(typeof(Maze)) as Maze;
            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
        }

        protected override void GérerDéplacement(float direction, float latéral)
        {
            base.GérerDéplacement(direction, latéral);

            if (Maze.CheckForCollisions(Position))
            {
                Position -= direction * VitesseTranslation * Direction;
                Position += latéral * VitesseTranslation * Latéral;
            }
        }
    }
}

