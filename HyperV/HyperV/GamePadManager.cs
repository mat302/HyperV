using AtelierXNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace HyperV
{
    public class GamePadManager : Microsoft.Xna.Framework.GameComponent
    {
        GamePadCapabilities CapacitÈs { get; set; }
        GamePadState Ancien…tatGamePad { get; set; }
        GamePadState Nouvel…tatGamePad { get; set; }

        public GamePadManager(Game game)
         : base(game)
        { }

        public override void Initialize()
        {
            CapacitÈs = GamePad.GetCapabilities(PlayerIndex.One);
            Nouvel…tatGamePad = GamePad.GetState(PlayerIndex.One);
            Ancien…tatGamePad = Nouvel…tatGamePad;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Ancien…tatGamePad = Nouvel…tatGamePad;
            Nouvel…tatGamePad = GamePad.GetState(PlayerIndex.One);
        }

        public bool EstGamepadActivÈ//update capacitÈs?
        {
            get { return CapacitÈs.IsConnected; }
        }

        public Vector2 PositionsG‚chettes
        {
            get { return new Vector2(Nouvel…tatGamePad.Triggers.Left, Nouvel…tatGamePad.Triggers.Right); }
        }

        public Vector2 PositionThumbStickGauche
        {
            get { return new Vector2(Nouvel…tatGamePad.ThumbSticks.Left.X, Nouvel…tatGamePad.ThumbSticks.Left.Y); }
        }

        public Vector2 PositionThumbStickDroit
        {
            get { return new Vector2(Nouvel…tatGamePad.ThumbSticks.Right.X, Nouvel…tatGamePad.ThumbSticks.Right.Y); }
        }

        public bool EstEnfoncÈ(Buttons bouton)
        {
            return Nouvel…tatGamePad.IsButtonDown(bouton);
        }

        public bool EstNouveauBouton(Buttons bouton)
        {
            return Ancien…tatGamePad.PacketNumber != Nouvel…tatGamePad.PacketNumber &&
                   Nouvel…tatGamePad.IsButtonDown(bouton);
        }
    }
}