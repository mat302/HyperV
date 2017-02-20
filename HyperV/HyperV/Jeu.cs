using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.IO;
using AtelierXNA;


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
        List<string> ParametresModele { get; set; } //liste des propriétés du modèle a télécharger (venant du fichier texte)        

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
            ParametresModele = new List<string>();
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
                string[] ligneLu = fichier.ReadLine().Split(';');
                foreach(string s in ligneLu)
                {
                    ParametresModele.Add(s);  //1.nom modele, 2.position x, 3.position y, 4.position z, 5.homothesie, 6.rotation
                }
                Niveau modele = new Niveau(Game, ParametresModele[0], new Vector3(float.Parse(ParametresModele[1]), float.Parse(ParametresModele[2]), float.Parse(ParametresModele[3])));
                Game.Components.Add(modele);
            }
        }
    }
}
