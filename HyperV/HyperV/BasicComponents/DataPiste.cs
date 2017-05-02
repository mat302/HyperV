using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using System.Globalization;


namespace AtelierXNA
{
    public class DataPiste
    {
        const string CHEMIN = "../../../../AS3D_2016Content/", CHAÎNE_TAB = "\t", CHAÎNE_TAB_ENTRE_ZÉROS = "0\t0", CHAÎNE_MOINS_POINT = "-.", CHAÎNE_MOINS_UN_POINT = "-1.", CHAÎNE_ZÉRO_MOINS_UN_POINT = "0-1.", CHAÎNE_ZÉRO_MOINS = "0-", CHAÎNE_MOINS = "-", CHAÎNE_E_MOINS = "e-", CHAÎNE_ZÉRO = "0", CHAÎNE_POINT_TAB = ".\t", CHAÎNE_POINT_ZÉRO_TAB = ".0\t", CHAÎNE_TAB_POINT = "\t.", CHAÎNE_TAB_ZÉRO_POINT = "\t0.", CHAÎNE_MOINS_ZÉRO_POINT = "-0.", CHAÎNE_VIDE = "";
        const int POSITION_SPHÈRE = 0, NOMBRE_DE_COMPOSNATES_VECTEUR_QUATRE = 4, PREMIÈRE_COMPOSANTE = 0, DEUXIÈME_COMPOSANTE = 1, TROISIÈME_COMPOSANTE = 2, QUATRIÈME_COMPOSANTE = 3, BOND_E_MOINS = 2, PREMIÈRE_LETTRE = 0, BASE_E_MAPLE = 10, PAS_TROUVÉ = -1, E_MAPLE_NUL = 0, NB_POINTS_DE_PATROUILLE = 4, NB_POINTS_CENTRAUX = 20, FACTEUR_DÉRIVÉE_PUISSANCE_CUBE = 3, FACTEUR_DÉRIVÉE_PUISSANCE_CARRÉ = 2, DISTANCE_ENTRE_BORDURE_ET_CENTRE_DE_PISTE = 3, DIMENSION_GRILLE_MAPLE = 8, DIMENSION_GRILLE_TERRAIN = 256, PREMIER_INDICE = 0;
        const char CARACTÈRE_TAB = '\t', CARACTÈRE_MOINS = '-', CARACTÈRE_POINT = '.', CARACTÈRE_VIRGULE = ',', CARACTÈRE_SAUT_DE_LIGNE = '\n';
        const bool BORDURE_INTÉRIEURE = true;

        const float UN_QUART = 1 / 4f;
        const float UN_VINGTIÈME = 1 / 20f;
        const int FACTEUR_HOMOTHÉTIE = 256 / 8;
        const int LARGEUR_DEMIE_PISTE = 8;

        List<Vector2> BordureExtérieure { get; set; }
        List<Vector2> BordureIntérieure { get; set; }
        List<Vector2> PointsCube { get; set; }
        List<Vector2> PointsDePatrouille { get; set; }
        List<Vector4> DonnéesBrutesX { get; set; }
        List<Vector4> DonnéesBrutesY { get; set; }
        List<Vector2> PointsCentraux { get; set; }


        List<Vector4> ListeSplineX { get; set; }
        List<Vector4> ListeSplineY { get; set; }

        public Vector2 PositionAvatar
        {
            get
            {
                return new Vector2(PointsCube[POSITION_SPHÈRE].X, PointsCube[POSITION_SPHÈRE].Y);
            }
        }

        public DataPiste(string nomFichierSplineX, string nomFichierSplineY)
        {
            char virguleÀRemplacer = CARACTÈRE_VIRGULE, pointÀMettre = CARACTÈRE_POINT;
            string[] chaînesX, chaînesY;

            chaînesX = CréerChaînesCaractèresRemplacés(nomFichierSplineX, virguleÀRemplacer, pointÀMettre);
            chaînesY = CréerChaînesCaractèresRemplacés(nomFichierSplineY, virguleÀRemplacer, pointÀMettre);            
            DonnéesBrutesX = EnregistrerDonnéesBrutes(DonnéesBrutesX, chaînesX);
            DonnéesBrutesY = EnregistrerDonnéesBrutes(DonnéesBrutesY, chaînesY);
            EnregistrerPointsCube();

            PointsDePatrouille = EnregistrerPoints(PointsDePatrouille, NB_POINTS_DE_PATROUILLE);
            PointsCentraux = EnregistrerPoints(PointsCentraux, NB_POINTS_CENTRAUX);
            BordureExtérieure = EnregistrerBordure(BordureExtérieure, !BORDURE_INTÉRIEURE, NB_POINTS_CENTRAUX);
            BordureIntérieure = EnregistrerBordure(BordureIntérieure, BORDURE_INTÉRIEURE, NB_POINTS_CENTRAUX);

            BordureExtérieure.Add(new Vector2(BordureExtérieure[PREMIER_INDICE].X, BordureExtérieure[PREMIER_INDICE].Y));
            BordureIntérieure.Add(new Vector2(BordureIntérieure[PREMIER_INDICE].X, BordureIntérieure[PREMIER_INDICE].Y));
        }

        public List<Vector2> GetBordureExtérieure()
        {
            return FaireCopieEnProfondeurDeListe(BordureExtérieure);
        }

        public List<Vector2> GetBordureIntérieure()
        {
            return FaireCopieEnProfondeurDeListe(BordureIntérieure);
        }

        public List<Vector2> GetPointsCube()
        {
            return FaireCopieEnProfondeurDeListe(PointsCube);
        }

        public List<Vector2> GetPointsDePatrouille()
        {
            return FaireCopieEnProfondeurDeListe(PointsDePatrouille);
        }

        List<Vector2> FaireCopieEnProfondeurDeListe(List<Vector2> ancienneListe)
        {
            List<Vector2> nouvelleListe = new List<Vector2>();
            foreach (Vector2 v in ancienneListe)
            {
                nouvelleListe.Add(new Vector2(v.X, v.Y));
            }

            return nouvelleListe;
        }

        List<Vector2> AjusterPourNouvelleGrille(List<Vector2> liste, int ancienneDimensionGrille, int nouvelleDimensionGrille)
        {
            int facteurAjustement = nouvelleDimensionGrille / ancienneDimensionGrille;

            for (int i = 0; i < liste.Count; ++i)
            {
                liste[i] *= facteurAjustement;
            }

            return liste;
        }

        List<Vector2> EnregistrerBordure(List<Vector2> liste, bool intérieur, int nbPoints)
        {
            liste = new List<Vector2>();

            for (int i = 0; i < PointsCentraux.Count; ++i)
            {
                Vector2 vd = PointsCentraux[i] - PointsCentraux[i - 1 == -1 ? PointsCentraux.Count - 1 : i - 1];
                Vector2 vo = (intérieur ? new Vector2(vd.Y, -vd.X) : new Vector2(-vd.Y, vd.X) * DISTANCE_ENTRE_BORDURE_ET_CENTRE_DE_PISTE);
                liste.Add(PointsCentraux[i] + vo);
            }
            return liste;
        }

        List<Vector2> EnregistrerPoints(List<Vector2> liste, int nbPoints)
        {
            liste = new List<Vector2>();
            for (int i = 0; i < DonnéesBrutesX.Count; ++i)
            {
                liste = EnregistrerPlusieursPoints(liste, nbPoints, i);
            }

            return liste;
        }

        List<Vector2> EnregistrerPlusieursPoints(List<Vector2> liste, int nbPoints, int i)
        {
            float facteur;

            for (int j = 0; j < nbPoints; ++j)
            {
                facteur = i + j / (float)nbPoints;
                liste.Add(new Vector2(DonnéesBrutesX[i].X + DonnéesBrutesX[i].Y * facteur * facteur * facteur + DonnéesBrutesX[i].Z * facteur * facteur + DonnéesBrutesX[i].W * facteur, DonnéesBrutesY[i].X + DonnéesBrutesY[i].Y * facteur * facteur * facteur + DonnéesBrutesY[i].Z * facteur * facteur + DonnéesBrutesY[i].W * facteur));
            }

            return liste;
        }

        void EnregistrerPointsCube()
        {
            PointsCube = new List<Vector2>();
            for (int i = 0; i < DonnéesBrutesX.Count; ++i)
            {
                PointsCube.Add(new Vector2(DonnéesBrutesX[i].X + DonnéesBrutesX[i].Y * i * i * i + DonnéesBrutesX[i].Z * i * i + DonnéesBrutesX[i].W * i, DonnéesBrutesY[i].X + DonnéesBrutesY[i].Y * i * i * i + DonnéesBrutesY[i].Z * i * i + DonnéesBrutesY[i].W * i));
            }
        }

        string[] CréerChaînesCaractèresRemplacés(string nomFichier, char vieuxCaractère, char nouveauCaractère)
        {
            StreamReader lecteur = new StreamReader(CHEMIN + nomFichier);
            string chaîne = CHAÎNE_VIDE;
            string[] chaînesNombres;
            char[] séparateur = { CARACTÈRE_TAB, CARACTÈRE_SAUT_DE_LIGNE };

            while (!lecteur.EndOfStream)
            {
                chaîne += lecteur.ReadLine();
                chaîne += CARACTÈRE_SAUT_DE_LIGNE;
            }
            lecteur.Close();
            chaîne = chaîne.Replace(vieuxCaractère, nouveauCaractère);
            chaîne = PlacerZérosAvantOuAprès(chaîne, nouveauCaractère);
            chaînesNombres = chaîne.Split(séparateur);

            return chaînesNombres;
        }

        string PlacerZérosAvantOuAprès(string chaîne, char caractère)
        {
            chaîne = chaîne.Replace(CHAÎNE_POINT_TAB, CHAÎNE_POINT_ZÉRO_TAB);
            chaîne = chaîne.Replace(CHAÎNE_TAB_POINT, CHAÎNE_TAB_ZÉRO_POINT);
            chaîne = chaîne.Replace(CHAÎNE_MOINS_POINT, CHAÎNE_MOINS_ZÉRO_POINT);

            return chaîne;
        }

        List<Vector4> EnregistrerDonnéesBrutes(List<Vector4> donnéesBrutes, string[] chaînes)
        {
            donnéesBrutes = new List<Vector4>();
            for (int i = 0; i < chaînes.Length - 1; i = i + 4)
            {
                donnéesBrutes.Add(32 * new Vector4(float.Parse(chaînes[i], CultureInfo.InvariantCulture), float.Parse(chaînes[i+1], CultureInfo.InvariantCulture), float.Parse(chaînes[i+2], CultureInfo.InvariantCulture), float.Parse(chaînes[i+3], CultureInfo.InvariantCulture)));
            }

            return donnéesBrutes;
        }

        void ConvertirChaînes(int i, string[] chaînesComposantes, float[] valeursComposantes, string[] chaînes)
        {
            for (int j = 0; j < NOMBRE_DE_COMPOSNATES_VECTEUR_QUATRE; ++j)
            {
                chaînesComposantes[j] = chaînes[i * NOMBRE_DE_COMPOSNATES_VECTEUR_QUATRE + j];
                valeursComposantes[j] = ConversionChaîneMapleEnFloat(chaînesComposantes[j]);
            }
        }

        float ConversionChaîneMapleEnFloat(string chaîneComposante)
        {
            int indexE = chaîneComposante.IndexOf(CHAÎNE_E_MOINS);
            bool eTrouvé = indexE != PAS_TROUVÉ;
            int valE = eTrouvé ? int.Parse(chaîneComposante.Remove(PREMIÈRE_LETTRE, indexE + BOND_E_MOINS)) : E_MAPLE_NUL;
            string chaîneSansE = eTrouvé ? chaîneComposante.Remove(indexE) : chaîneComposante;

            return float.Parse(chaîneSansE) / (float)Math.Pow(BASE_E_MAPLE, valE);
        }
    }
}