/*
Plan.cs
-------

Par Mathieu Godin

Rôle : Composant qui permet d'afficher
       un plan composé de bandes de
       triangles à l'écran

Créé : 21 novembre 2016
*/
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
    public abstract class Plan : PrimitiveDeBaseAnimée
   {
      protected const int NB_TRIANGLES = 2, DIVISEUR_DEMI_GRANDEUR = 2, COTE_NULLE = 0, NB_TRIANGLES_PAR_CARRÉ = 2, PREMIERS_SOMMETS_DU_STRIP = 2, SOMMET_SUPPLÉMENTAIRE_POUR_LIGNE = 1;
      protected Vector3 Origine { get; private set; }  //Le coin inférieur gauche du plan en tenant compte que la primitive est centrée au point (0,0,0)
      Vector2 Delta { get; set; } // un vecteur contenant l'espacement entre deux colonnes (en X) et entre deux rangées (en Y)
      protected Vector3[,] PtsSommets { get; private set; } //un tableau contenant les positions des différents sommets du plan
      protected int NbColonnes { get; private set; } // Devinez...
      protected int NbRangées { get; private set; } // idem 
      protected int NbTrianglesParStrip { get; private set; } //...
      protected BasicEffect EffetDeBase { get; private set; } // 

        /// <summary>
        /// Constructeur de la classe Plan
        /// </summary>
        /// <param name="jeu">Contient la classe Atelier</param>
        /// <param name="homothétieInitiale">Agrandissement ou rappetissement initial</param>
        /// <param name="rotationInitiale">Tangage, roulis et lacet initiaux</param>
        /// <param name="positionInitiale">Position initiale</param>
        /// <param name="étendue">Largeur et hauteur du plan</param>
        /// <param name="charpente">Nombre de rectangles en abscisse et en ordonnée</param>
        /// <param name="intervalleMAJ">Intervalle de mise à jour auquel on met à jour</param>
        public Plan(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, Vector2 charpente, float intervalleMAJ) : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
      {
         NbColonnes = (int)charpente.X;
         NbRangées = (int)charpente.Y;
         Delta = étendue / charpente;
         Origine = new Vector3(-étendue.X / DIVISEUR_DEMI_GRANDEUR, -étendue.Y / DIVISEUR_DEMI_GRANDEUR, COTE_NULLE);
      }

        /// <summary>
        /// Initialise le plan
        /// </summary>
      public override void Initialize()
      {
         NbTrianglesParStrip = NbColonnes * NB_TRIANGLES_PAR_CARRÉ;
         NbSommets = (NbTrianglesParStrip + PREMIERS_SOMMETS_DU_STRIP) * NbRangées;
         PtsSommets = new Vector3[NbColonnes + SOMMET_SUPPLÉMENTAIRE_POUR_LIGNE, NbRangées + SOMMET_SUPPLÉMENTAIRE_POUR_LIGNE];
         CréerTableauSommets();
         CréerTableauPoints();
         base.Initialize();
      }

      protected abstract void CréerTableauSommets();

        /// <summary>
        /// Charge le contenu
        /// </summary>
      protected override void LoadContent()
      {
         EffetDeBase = new BasicEffect(GraphicsDevice);
         InitialiserParamètresEffetDeBase();
         base.LoadContent();
      }

      protected abstract void InitialiserParamètresEffetDeBase();

        /// <summary>
        /// Met les valeurs dans le tableau PtsSommets
        /// </summary>
      private void CréerTableauPoints()
      {
            for (int i = 0; i < PtsSommets.GetLength(0); ++i)
            {
                for (int j = 0; j < PtsSommets.GetLength(1); ++j)
                {
                    PtsSommets[i, j] = Origine + new Vector3(Delta, COTE_NULLE) * new Vector3(i, j, COTE_NULLE);
                }
            }
      }

        /// <summary>
        /// Dessine à l'écran le plan
        /// </summary>
        /// <param name="gameTime">Contient les informations de temps de jeu</param>
      public override void Draw(GameTime gameTime)
      {
         EffetDeBase.World = GetMonde();
         EffetDeBase.View = CaméraJeu.Vue;
         EffetDeBase.Projection = CaméraJeu.Projection;
         foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
         {
            passeEffet.Apply();
            for (int i = 0 ; i < NbRangées ; ++i)
            {
                    DessinerTriangleStrip(i);
            }
            // Ici, il devrait y avoir une boucle qui provoque le dessin de chaque TriangleStrip du plan
            // Le dessin d'un TriangleStrip en particulier devrait se faire par le biais d'un appel à la méthode DessinerTriangleStrip()
         }
         //base.Draw(gameTime);
      }

      protected abstract void DessinerTriangleStrip(int noStrip);
   }
}
