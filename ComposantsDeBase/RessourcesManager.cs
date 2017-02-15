using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public class RessourcesManager<T>
   {
      Game Jeu { get; set; }
      string RépertoireDesRessources { get; set; }
      List<RessourceDeBase<T>> ListeRessources { get; set; }

      public RessourcesManager(Game jeu, string répertoireDesRessources)
      {
         Jeu = jeu;
         RépertoireDesRessources = répertoireDesRessources;
         ListeRessources = new List<RessourceDeBase<T>>();
      }

      public void Add(string nom, T ressourceExistante)
      {
         RessourceDeBase<T> ressourceÀAjouter = new RessourceDeBase<T>(nom, ressourceExistante);
         if (!ListeRessources.Contains(ressourceÀAjouter))
         {
            ListeRessources.Add(ressourceÀAjouter);
         }
      }

      void Add(RessourceDeBase<T> ressourceÀAjouter)
      {
         ressourceÀAjouter.Load();
         ListeRessources.Add(ressourceÀAjouter);
      }

      public T Find(string nomRessource)
      {
         const int RESSOURCE_PAS_TROUVÉE = -1;
         RessourceDeBase<T> ressourceÀRechercher = new RessourceDeBase<T>(Jeu.Content, RépertoireDesRessources, nomRessource);
         int indexRessource = ListeRessources.IndexOf(ressourceÀRechercher);
         if (indexRessource == RESSOURCE_PAS_TROUVÉE)
         {
            Add(ressourceÀRechercher);
            indexRessource = ListeRessources.Count - 1;
         }
         return ListeRessources[indexRessource].Ressource;
      }
   }
}
