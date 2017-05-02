/*
ObjetDeDémo.cs
--------------

Par Mathieu Godin

Rôle : Composant qui permet de tester un modèle 3D
       en lui effectuant des changements d'échelle
       et des rotations sur les différents axes

Créé : 2 novembre 2016
*/
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtelierXNA
{
    /// <summary>
    /// Composant qui permet de tester un modèle 3D en lui effectuant des changements d'échelle et des rotations sur les différents axes
    /// </summary>
    public class ObjetDeDémo : ObjetDeBase
    {
        const float AUCUN_TEMPS_ÉCOULÉ = 0.0F, CHANGEMENT_ÉCHELLE = 0.001F, ÉCHELLE_MAXIMALE = 1.0F, ÉCHELLE_MINIMALE = 0.005F, INCRÉMENT_ANGLE = (float)Math.PI / 120, AUCUNE_ROTATION = 0.0F;

        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        InputManager GestionIntrants { get; set; }
        bool RotationYActivée { get; set; }
        bool RotationXActivée { get; set; }
        bool RotationZActivée { get; set; }
        Vector3 IncrémentRotationY { get; set; }
        Vector3 IncrémentRotationX { get; set; }
        Vector3 IncrémentRotationZ { get; set; }

        /// <summary>
        /// Retourne le vecteur des angles initiaux
        /// </summary>
        Vector3 AnglesRotationNormaux
        {
            get
            {
                return new Vector3(AUCUNE_ROTATION, MathHelper.PiOver2, AUCUNE_ROTATION);
            }
        }

        /// <summary>
        /// Constructeur de la classe ObjetDeDémo
        /// </summary>
        /// <param name="jeu">Jeu de type Game</param>
        /// <param name="nomModèle">Chaîne de caractères représentant le nom du fichier du modèle</param>
        /// <param name="échelleInitiale">Échelle initiale du modèle à afficher</param>
        /// <param name="rotationInitiale">Rotation Initiale du modèle à afficher</param>
        /// <param name="positionInitiale">Position Initiale du modèle à afficher</param>
        /// <param name="intervalleMAJ">Intervalle de mise à jour du modèle à afficher</param>
        public ObjetDeDémo(Game jeu, String nomModèle, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ) : base(jeu, nomModèle, échelleInitiale, rotationInitiale, positionInitiale)
        {
            IntervalleMAJ = intervalleMAJ;
        }

        /// <summary>
        /// Initialise les propriétés nécessaires au fonctionnement de ObjetDeDémo
        /// </summary>
        public override void Initialize()
        {
            TempsÉcouléDepuisMAJ = AUCUN_TEMPS_ÉCOULÉ;
            RotationYActivée = false;
            RotationXActivée = false;
            RotationZActivée = false;
            IncrémentRotationY = new Vector3(AUCUNE_ROTATION, INCRÉMENT_ANGLE, AUCUNE_ROTATION);
            IncrémentRotationX = new Vector3(INCRÉMENT_ANGLE, AUCUNE_ROTATION, AUCUNE_ROTATION);
            IncrémentRotationZ = new Vector3(AUCUNE_ROTATION, AUCUNE_ROTATION, INCRÉMENT_ANGLE);
            base.Initialize();
        }

        /// <summary>
        /// Charge le contenu
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            GestionIntrants = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        /// <summary>
        /// Met à jour le composant
        /// </summary>
        /// <param name="gameTime">Contient les informations de temps de jeu</param>
        public override void Update(GameTime gameTime)
        {
            TempsÉcouléDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                TempsÉcouléDepuisMAJ = AUCUN_TEMPS_ÉCOULÉ;
                VérifierSiClavierEstUtilisé();
                MettreÀJourAngles();
                MettreÀJourMatriceMonde();
            }
            VérifierNouvellesTouches();
        }

        /// <summary>
        /// Met à jour la matrice monde
        /// </summary>
        void MettreÀJourMatriceMonde()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(Échelle);
            Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            Monde *= Matrix.CreateTranslation(Position);
        }

        /// <summary>
        /// Appelle les différentes méthodes de vérification de nouvelles touches
        /// </summary>
        void VérifierNouvellesTouches()
        {
            VérifierRotations();
            VérifierRétablirAngles();
        }

        /// <summary>
        /// Appelle les différentes méthodes de vérification de rotations activées
        /// </summary>
        void MettreÀJourAngles()
        {
            VérifierRotationYActivée();
            VérifierRotationXActivée();
            VérifierRotationZActivée();
        }

        /// <summary>
        /// Vérifie si la rotation en Y est activée et l'incrémente par Pi sur 120 si c'est le cas
        /// </summary>
        void VérifierRotationYActivée()
        {
            if (RotationYActivée)
            {
                Rotation += IncrémentRotationY;
            }
        }

        /// <summary>
        /// Vérifie si la rotation en X est activée et l'incrémente par Pi sur 120 si c'est le cas
        /// </summary>
        void VérifierRotationXActivée()
        {
            if (RotationXActivée)
            {
                Rotation += IncrémentRotationX;
            }
        }

        /// <summary>
        /// Vérifie si la rotation en Z est activée et l'incrémente par Pi sur 120 si c'est le cas
        /// </summary>
        void VérifierRotationZActivée()
        {
            if (RotationZActivée)
            {
                Rotation += IncrémentRotationZ;
            }
        }

        /// <summary>
        /// Vérifie si il y a au moins une touche d'enfoncée afin de ne pas tout vérifier pour rien
        /// </summary>
        void VérifierSiClavierEstUtilisé()
        {
            if (GestionIntrants.EstClavierActivé)
            {
                VérifierAgrandissement();
                VérifierRéduction();
            }
        }

        /// <summary>
        /// Appelle les méthodes de vérifications de nouvelles touches propres aux rotations
        /// </summary>
        void VérifierRotations()
        {
            VérifierRotationY();
            VérifierRotationX();
            VérifierRotationZ();
        }

        /// <summary>
        /// Vérifie si la touche 1 du clavier numérique ou alphabétique est une nouvelle touche et active la rotation en Y si c'est le cas
        /// </summary>
        void VérifierRotationY()
        {
            if (GestionIntrants.EstNouvelleTouche(Keys.NumPad1) || GestionIntrants.EstNouvelleTouche(Keys.D1))
            {
                RotationYActivée = !RotationYActivée;
            }
        }

        /// <summary>
        /// Vérifie si la touche 2 du clavier numérique ou alphabétique est une nouvelle touche et active la rotation en X si c'est le cas
        /// </summary>
        void VérifierRotationX()
        {
            if (GestionIntrants.EstNouvelleTouche(Keys.NumPad2) || GestionIntrants.EstNouvelleTouche(Keys.D2))
            {
                RotationXActivée = !RotationXActivée;
            }
        }

        /// <summary>
        /// Vérifie si la touche 3 du clavier numérique ou alphabétique est une nouvelle touche et active la rotation en Z si c'est le cas
        /// </summary>
        void VérifierRotationZ()
        {
            if (GestionIntrants.EstNouvelleTouche(Keys.NumPad3) || GestionIntrants.EstNouvelleTouche(Keys.D3))
            {
                RotationZActivée = !RotationZActivée;
            }
        }

        /// <summary>
        /// Vérifie si la touche Espace du clavier numérique ou alphabétique est une nouvelle touche et remet les angles de rotations initiaux si c'est le cas
        /// </summary>
        void VérifierRétablirAngles()
        {
            if (GestionIntrants.EstNouvelleTouche(Keys.Space))
            {
                Rotation = AnglesRotationNormaux;
            }
        }

        /// <summary>
        /// Vérifie si la touche Plus est enfoncée et l'échelle n'est pas encore maximale et incrémente l'échelle si c'est le cas
        /// </summary>
        void VérifierAgrandissement()
        {
            if (GestionIntrants.EstEnfoncée(Keys.OemPlus) && Échelle < ÉCHELLE_MAXIMALE)
            {
                Échelle += CHANGEMENT_ÉCHELLE;
            }
        }

        /// <summary>
        /// Vérifie si la touche Moins est enfoncée et l'échelle n'est pas encore minimale et décrémente l'échelle si c'est le cas
        /// </summary>
        void VérifierRéduction()
        {
            if (GestionIntrants.EstEnfoncée(Keys.OemMinus) && Échelle > ÉCHELLE_MINIMALE)
            {
                Échelle -= CHANGEMENT_ÉCHELLE;
            }
        }
    }
}
