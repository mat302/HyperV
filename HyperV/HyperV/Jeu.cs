using AtelierXNA;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.IO;


namespace HyperV
{
    public class Jeu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Skybox Skybox { get; set; }
        const string CHEMIN_FICHIER = "../../../";
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        Rectangle ZoneAffichage { get; set; }
        Caméra CaméraJeu { get; set; }
        Song ChansonJeu { get; set; }
        InputManager GestionInput { get; set; }

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
            LireFichierNiveau("Hub.txt");         
        }
        
        public override void Update(GameTime gameTime)
        {

        }

        private void LireFichierNiveau(string nomFichier)
        {
            StreamReader fichier = new StreamReader(CHEMIN_FICHIER + nomFichier);
            while (!fichier.EndOfStream)
            {
                List<string> ListeModeles = new List<string>();
                string[] ligneLu = fichier.ReadLine().Split(';');
                foreach(string s in ligneLu)
                {
                    ListeModeles.Add(s);  //0.nom modele, 1.position x, 2.position y, 3.position z, 4.homothesie, 5.rotation
                }
                CreateurModele modele = new CreateurModele(Game, ListeModeles[0], new Vector3(float.Parse(ListeModeles[1]), float.Parse(ListeModeles[2]), float.Parse(ListeModeles[3])),
                                                            float.Parse(ListeModeles[4]), float.Parse(ListeModeles[5]));
                Game.Components.Add(modele);
            }
        }        
    }
}
