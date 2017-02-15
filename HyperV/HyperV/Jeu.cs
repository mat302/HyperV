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
using System.IO;
using AtelierXNA;


namespace HyperV
{
    public class Jeu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Skybox Skybox { get; set; }
        const string CHEMIN_FICHIER = "../../";
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        Rectangle ZoneAffichage { get; set; }
        Caméra CaméraJeu { get; set; }
        Song ChansonJeu { get; set; }
        InputManager GestionInput { get; set; }
        List<string[]> ListeModeles { get; set; } //liste de tous les modeles a placer dans un niveau (qui sont dans le fichier texte)

        public Jeu(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            base.Initialize();
            ZoneAffichage = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra;
            RessourcesManager<Song> gestionnaireDeMusiques = Game.Services.GetService(typeof(RessourcesManager<Song>)) as RessourcesManager<Song>;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            RessourcesManager<SoundEffect> gestionnaireDeSons = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
            ListeModeles = new List<string[]>();            
        }
        
        public override void Update(GameTime gameTime)
        {

        }

        private void InitialiserCaméra()
        {
            Vector3 positionCaméra = Vector3.One;
        }

        private void LireFichierNiveau(string nomFichier)
        {
            StreamReader fichier = new StreamReader(CHEMIN_FICHIER + nomFichier);
            while (fichier.EndOfStream)
            {
                string ligneLu = fichier.ReadLine();
                ListeModeles.Add(ligneLu.Split(';'));  //1.nom modele, 2.nom texture modele, 3.position x, 4.position y, 5.position z, 6.homothesie

            }
        }        
    }
}
