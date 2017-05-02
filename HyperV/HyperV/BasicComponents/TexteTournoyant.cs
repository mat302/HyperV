/*
TexteTournoyant.cs
------------------

Par Mathieu Godin

Rôle : Composant s'occupant d'afficher
       un message tournoyant personnalisé
       par une chaîne de charactères
       représentant ce message, sa position,
       sa couleur et un intervalle de mise
       à jour représentant aussi la vitesse
       auquel le message tourne 

Créé : 29 août 2016
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AtelierXNA
{
    /// <summary>
    /// Composant qui permet d'afficher un texte tournoyant personnalisé
    /// </summary>
    public class TexteTournoyant : Microsoft.Xna.Framework.DrawableGameComponent
    {
        /// <summary>
        /// Constructeur qui permet de sauvegarder les personnalisations passées en paramètres
        /// </summary>
        /// <param name="game">Jeu qui a appelé ce composant</param>
        /// <param name="message">Chaîne rerpésentant le message que l'on veut afficher</param>
        /// <param name="position">Position du message dans l'écran par rapport à son centre</param>
        /// <param name="couleur">Couleur voulue pour le message tournoyant en question</param>
        /// <param name="intervalleMAJ">Intervalle de mise à jour de l'angle et de l'échelle du message, une grande valeur fera tourner et agrandir le message plus rapidement</param>
        public TexteTournoyant(Game game, string message, Vector2 position, Color couleur, float intervalleMAJ) : base(game)
        {
            Message = message;
            Position = position;
            Couleur = couleur;
            IntervalleMAJ = intervalleMAJ;
        }

        /// <summary>
        /// Initialise les mécaniques de temps, d'angle et d'échelle du message tournoyant
        /// </summary>
        public override void Initialize()
        {
            TempsÉcouléDepuisMAJ = AUCUN_TEMPS_ÉCOULÉ;
            Échelle = ÉCHELLE_DÉPART;
            AngleRotation = ANGLE_DÉPART;
            base.Initialize();
        }

        /// <summary>
        /// Charge la police et la gestion des sprites et fait le calcul de l'origine du message tournoyant
        /// </summary>
        protected override void LoadContent()
        {
            ArialFont = Game.Content.Load<SpriteFont>("Fonts/Arial");
            InitialiserOrigine();
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
        }

        /// <summary>
        /// Initialise l'origine
        /// </summary>
        void InitialiserOrigine()
        {
            Vector2 dimension = ArialFont.MeasureString(Message);
            Origine = new Vector2(dimension.X / 2, dimension.Y / 2);
        }

        /// <summary>
        /// Met à jour l'angle et l'échelle du message tournoyant, se désactive après deux tours
        /// </summary>
        /// <param name="gameTime">Donne des informations sur le temps du jeu</param>
        public override void Update(GameTime gameTime)
        {
            TempsÉcouléDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            VérifierSiIncrémentationNécessaire();
        }

        /// <summary>
        /// Vérifie si l'intervalle de mise à jour a été écoulé pour incrémenter l'angle et l'échelle du texte tournoyant si c'est le cas
        /// </summary>
        void VérifierSiIncrémentationNécessaire()
        {
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                TempsÉcouléDepuisMAJ = AUCUN_TEMPS_ÉCOULÉ;
                AngleRotation += INCRÉMENT_ANGLE;
                Échelle += INCRÉMENT_ZOOM;
                VérifierSiDésactivationNécessaire();
            }
        }

        /// <summary>
        /// Vérifie si le texte tournoyant a fait deux tours afin de désactiver son tournage et son grossissement si c'est le cas
        /// </summary>
        void VérifierSiDésactivationNécessaire()
        {
            if (AngleRotation > DEUX_TOURS)
            {
                AngleRotation = DEUX_TOURS;
                this.Enabled = false;
            }
        }

        /// <summary>
        /// Dessine le message tournoyant à l'écran
        /// </summary>
        /// <param name="gameTime">Donne des informations sur le temps du jeu</param>
        public override void Draw(GameTime gameTime)
        {
            GestionSprites.DrawString(ArialFont, Message, Position, Couleur, MathHelper.ToRadians(AngleRotation), Origine, Échelle, SpriteEffects.None, AUCUNE_COUCHE_DE_PROFONDEUR);
        }

        string Message { get; set; }
        Vector2 Position { get; set; }
        Color Couleur { get; set; }
        float IntervalleMAJ { get; set; }
        SpriteFont ArialFont { get; set; }
        float Échelle { get; set; }
        float AngleRotation { get; set; }
        Vector2 Origine { get; set; }
        SpriteBatch GestionSprites { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }

        const float INCRÉMENT_ANGLE = 3.6F;
        const float INCRÉMENT_ZOOM = 0.01F;
        const float DEUX_TOURS = 720.0F;
        const float ANGLE_DÉPART = 0.0F;
        const float ÉCHELLE_DÉPART = 0.01F;
        const float AUCUN_TEMPS_ÉCOULÉ = 0.0F;
        const float AUCUNE_COUCHE_DE_PROFONDEUR = 0.0F;
    }
}
