/*
PlanColoré.cs
-------------

Par Mathieu Godin

Rôle : Composant qui permet d'afficher
       un plan composé de bandes colorées
       de triangles à l'écran

Créé : 21 novembre 2016
*/
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public class PlanColoré : Plan
   {
        const int AUCUN_DÉCALAGE_DE_SOMMET = 0, AVANT_PREMIER_SOMMET = -1;

        VertexPositionColor[] Sommets { get; set; }
        Color Couleur { get; set; }

        /// <summary>
        /// Constructeur de la classe PlanColoré
        /// </summary>
        /// <param name="jeu">Contient la classe Atelier</param>
        /// <param name="homothétieInitiale">Agrandissement ou rappetissement initial</param>
        /// <param name="rotationInitiale">Tangage, roulis et lacet initiaux</param>
        /// <param name="positionInitiale">Position initiale</param>
        /// <param name="étendue">Largeur et hauteur du plan</param>
        /// <param name="charpente">Nombre de rectangles en abscisse et en ordonnée</param>
        /// <param name="intervalleMAJ">Intervalle de mise à jour auquel on met à jour</param>
        public PlanColoré(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, Vector2 charpente, Color couleur, float intervalleMAJ) : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, charpente, intervalleMAJ)
        {
            Couleur = couleur;
        }

        /// <summary>
        /// Instancie le tableau Sommets
        /// </summary>
        protected override void CréerTableauSommets()
        {
            Sommets = new VertexPositionColor[NbSommets];
        }

        /// <summary>
        /// Intialise les paramètres pour le shader
        /// </summary>
        protected override void InitialiserParamètresEffetDeBase()
        {
            EffetDeBase.VertexColorEnabled = true;
        }

        /// <summary>
        /// Affecte les cases de Sommets
        /// </summary>
        protected override void InitialiserSommets()
        {
            int NoSommet = -1;
            for (int i = 0 ; i < NbRangées ; ++i)
            {
                for (int j = 0 ; j < NbColonnes + SOMMET_SUPPLÉMENTAIRE_POUR_LIGNE ; ++j)
                {
                    Sommets[++NoSommet] = new VertexPositionColor(PtsSommets[j, i], Couleur);
                    Sommets[++NoSommet] = new VertexPositionColor(PtsSommets[j, i + SOMMET_SUPPLÉMENTAIRE_POUR_LIGNE], Couleur);
                }
            }
        }

        /// <summary>
        /// Dessine une bande de triangles à l'écran
        /// </summary>
        /// <param name="noStrip">Numéro de la bande de triangle qu,on est rendu à afficher</param>
        protected override void DessinerTriangleStrip(int noStrip)
        {
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets, noStrip * NbSommets / NbRangées, NbTrianglesParStrip);
        }
    }
}
