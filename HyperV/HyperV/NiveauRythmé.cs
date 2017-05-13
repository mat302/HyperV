using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using AtelierXNA;

namespace HyperV
{
    //void RythmLevel()
    //{
    //    NiveauRythmé circuit = new NiveauRythmé(this, "Fil Electrique", "../../../Data3.txt",
    //                                            3, "Blanc", "Rouge",
    //                                            "Vert", "BleuBlancRouge", "Arial50",
    //                                            Color.Black, 15, 1,
    //                                            FpsInterval);
    //    Components.Add(circuit);
    //    Services.AddService(typeof(NiveauRythmé), circuit);
    //}


    //Faire enlever de la vie si pese touche pas colision
    public class NiveauRythmé : Microsoft.Xna.Framework.GameComponent
    {
        //CONSTRUCTEUR
        //Cylindre
        readonly string TextureCylindre,
                        NomFichierLecturePositionsCylindre;
        //Cube
        readonly float LongueurArêteCube;
        readonly string TextureCubeBase,
                        TextureCubeÉchec,
                        TextureCubeRéussite;
        //SphèreRythmée
        readonly string TextureSphèreRythmée;
        //Pointage
        readonly string NomPolicePointage;
        readonly Color CouleurPointage;
        readonly int NbreBalleÀRéussir,
                     Difficultée;

        readonly float IntervalleMAJ;


        //Initialize
        bool BoutonUn { get; set; }
        bool BoutonDeux { get; set; }
        bool BoutonTrois { get; set; }
        bool NiveauEstTerminé { get; set; }

        float TempsÉcouléDepuisMAJ { get; set; }
        List<Vector3> Positions { get; set; }
        int i { get; set; }
        int j { get; set; }
        int nombreRéussi { get; set; }
        public Vector3? PositionCubeRouge { get; set; }
        AfficheurPointage Pointage { get; set; }
        int BorneMaximale_i { get; set; }
        int BorneMaximale_j { get; set; }

        //ChargerContenu
        Random GénérateurAléatoire { get; set; }
        InputManager GestionInput { get; set; }
        GamePadManager GestionGamePad { get; set; }
        List<UnlockableWall> MurÀEnlever { get; set; }
        List<Portal> ListePortails { get; set; }


        public NiveauRythmé(Game jeu, string textureCylindre, string nomFichierLecturePositionsCylindre,
                            float longueurArêteCube, string textureCubeBase, string textureCubeÉchec,
                            string textureCubeRéussite, string textureSphèreRythmée, string nomPolicePointage,
                            Color couleurPointage, int nbreBalleÀRéussir, int difficultée,
                            float intervalleMAJ)
            : base(jeu)
        {
            TextureCylindre = textureCylindre;
            NomFichierLecturePositionsCylindre = nomFichierLecturePositionsCylindre;

            LongueurArêteCube = longueurArêteCube;
            TextureCubeBase = textureCubeBase;
            TextureCubeÉchec = textureCubeÉchec;
            TextureCubeRéussite = textureCubeRéussite;

            TextureSphèreRythmée = textureSphèreRythmée;

            NomPolicePointage = nomPolicePointage;
            CouleurPointage = couleurPointage;
            NbreBalleÀRéussir = nbreBalleÀRéussir;
            Difficultée = difficultée;

            IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            base.Initialize();

            BoutonUn = false;
            BoutonDeux = false;
            BoutonTrois = false;
            NiveauEstTerminé = false;
            PositionCubeRouge = null;
            nombreRéussi = 0;
            i = 0;
            j = 0;
            BorneMaximale_i = 120;
            BorneMaximale_j = 20;  // CONSTANTES ________________________________________________________
            TempsÉcouléDepuisMAJ = 0;

            Positions = new List<Vector3>();
            InitialiserPositions();
            Pointage = new AfficheurPointage(Game, NomPolicePointage, CouleurPointage, IntervalleMAJ);
            ChargerContenu();
            InitialisationComposants();

        }

        void InitialiserPositions()
        {
            string ligneLue;
            int indicateurDebut;
            float composanteX, composanteY, composanteZ;

            StreamReader lecteurFichier = new StreamReader(NomFichierLecturePositionsCylindre);

            while (!lecteurFichier.EndOfStream)
            {
                ligneLue = lecteurFichier.ReadLine();
                // Faire fonction une ligne
                indicateurDebut = ligneLue.IndexOf("X:") + 2;
                composanteX = float.Parse(ligneLue.Substring(indicateurDebut, ligneLue.IndexOf(" Y") - indicateurDebut));

                indicateurDebut = ligneLue.IndexOf("Y:") + 2;
                composanteY = float.Parse(ligneLue.Substring(indicateurDebut, ligneLue.IndexOf(" Z") - indicateurDebut));

                indicateurDebut = ligneLue.IndexOf("Z:") + 2;
                composanteZ = float.Parse(ligneLue.Substring(indicateurDebut, ligneLue.IndexOf("}") - indicateurDebut));

                Positions.Add(new Vector3(composanteX, composanteY, composanteZ));
            }
            lecteurFichier.Close();
        }

        protected virtual void ChargerContenu()
        {
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionGamePad = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
            GénérateurAléatoire = Game.Services.GetService(typeof(Random)) as Random;
            MurÀEnlever = Game.Services.GetService(typeof(List<UnlockableWall>)) as List<UnlockableWall>;
            ListePortails = Game.Services.GetService(typeof(List<Portal>)) as List<Portal>;
        }

        void InitialisationComposants()
        {

            Game.Components.Add(Pointage);
            Game.Components.Add(new Afficheur3D(Game));

            for (int i = 0; i < Positions.Count; i += 2)
            {
                // constantes ---------------------------------------------------------------------

                Game.Components.Add(new CylindreTexturé(Game, 1, Vector3.Zero,
                                    Vector3.Zero, new Vector2(1, 1), new Vector2(20, 20),
                                    TextureCylindre, IntervalleMAJ, Positions[i],
                                    Positions[i + 1]));

                Game.Components.Add(new CubeTexturé(Game, 1, Vector3.Zero, Positions[i + 1],
                                    TextureCubeBase, new Vector3(LongueurArêteCube, LongueurArêteCube, LongueurArêteCube), IntervalleMAJ));

                Game.Components.Add(new TuileTexturée(Game, 1, new Vector3(0, -MathHelper.PiOver2, 0),
                                    Positions[i + 1] - 1.65f * Vector3.UnitX, new Vector2(LongueurArêteCube, LongueurArêteCube), (i / 2 + 1).ToString()));
            }
        }

        public override void Update(GameTime gameTime)
        {
            RegarderTouches();

            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                if (!NiveauEstTerminé)
                {
                    EffectuerMAJ();
                }
                TempsÉcouléDepuisMAJ = 0;
            }
            base.Update(gameTime);
        }

        void RegarderTouches()
        {
            BoutonUn = GestionInput.EstNouvelleTouche(Keys.NumPad1) ||
                       GestionInput.EstNouvelleTouche(Keys.D1) ||
                      GestionGamePad.EstEnfoncé(Buttons.DPadLeft) || BoutonUn;
            BoutonDeux = GestionInput.EstNouvelleTouche(Keys.NumPad2) ||
                        GestionInput.EstNouvelleTouche(Keys.D2) ||
                      GestionGamePad.EstEnfoncé(Buttons.DPadDown) || BoutonDeux;
            BoutonTrois = GestionInput.EstNouvelleTouche(Keys.NumPad3) ||
                        GestionInput.EstNouvelleTouche(Keys.D3) ||
                      GestionGamePad.EstEnfoncé(Buttons.DPadRight) || BoutonTrois;
        }

        void EffectuerMAJ()
        {
            i++;
            j++;

            foreach (CubeTexturé cube in Game.Components.Where(composant => composant is CubeTexturé))
            {
                RemettreCubesTextureInitiale(cube);
                GérerÉchec(cube);

                foreach (SphèreRythmée sp in Game.Components.Where(composant => composant is SphèreRythmée))
                {
                    if (sp.EstEnCollision(cube))
                    {
                        GérerRéussite(sp, cube);
                    }
                }
            }

            GérerPointage();
            AjouterSphères();

            BoutonUn = false;
            BoutonDeux = false;
            BoutonTrois = false;
        }

        void GérerPointage()
        {
            Pointage.Chaîne = nombreRéussi.ToString() + "/" + NbreBalleÀRéussir.ToString();

            if (nombreRéussi >= NbreBalleÀRéussir)
            {

                // constantes  ----------------------------------

                NiveauEstTerminé = true;
                i = 1000;
                Game.Components.Remove(MurÀEnlever[0]);
                ListePortails.Add(new Portal(Game, 1, new Vector3(0, MathHelper.PiOver2, 0),
                                  new Vector3(170, -60, -10), new Vector2(40, 40), "Transparent",
                                  1, IntervalleMAJ));
                Game.Components.Add(ListePortails.Last());
            }
        }

        void AjouterSphères()
        {
            if (i > BorneMaximale_i)
            {
                // constantes ---------------------------------------------


                if (!NiveauEstTerminé)
                {
                    BorneMaximale_i = GénérateurAléatoire.Next(30 / Difficultée, 90 / Difficultée);

                    int choixPente = GénérateurAléatoire.Next(0, 3) * 2;
                    Game.Components.Add(new Afficheur3D(Game));
                    Game.Components.Add(new SphèreRythmée(Game, 1, Vector3.Zero,
                                        Positions[choixPente], 1, new Vector2(20, 20),
                                        TextureSphèreRythmée, IntervalleMAJ, Positions[choixPente + 1]));

                }
                i = 0;
            }
        }

        void RemettreCubesTextureInitiale(CubeTexturé cube)
        {
            if (j > BorneMaximale_j / Difficultée || NiveauEstTerminé)
            {
                cube.NomTextureCube = TextureCubeBase;
                cube.InitialiserParamètresEffetDeBase();

                //j = 0;
            }
        }

        void GérerÉchec(CubeTexturé cube)
        {
            if (SontVecteursÉgaux(PositionCubeRouge, cube.Position))
            {
                cube.NomTextureCube = TextureCubeÉchec;
                cube.InitialiserParamètresEffetDeBase();
                PositionCubeRouge = null;
                j = 0;
            }
        }

        void GérerRéussite(SphèreRythmée sp, CubeTexturé cube)
        {
            if (SontVecteursÉgaux(sp.Extrémité1, Positions[0]) && BoutonUn ||
                                    SontVecteursÉgaux(sp.Extrémité1, Positions[2]) && BoutonDeux ||
                                    SontVecteursÉgaux(sp.Extrémité1, Positions[4]) && BoutonTrois)
            {
                sp.ÀDétruire = true;
                cube.NomTextureCube = TextureCubeRéussite;
                cube.InitialiserParamètresEffetDeBase();
                ++nombreRéussi;
                j = 0;
            }
        }

        bool SontVecteursÉgaux(Vector3? a, Vector3 b)
        {
            bool égaux;

            if (a == null)
            {
                égaux = false;
            }
            else
            {
                Vector3 c = (Vector3)a - b;
                égaux = (c.X < 1 && c.X > -1) && (c.Y < 1 && c.Y > -1) && (c.Z < 1 && c.Z > -1);
            }

            return égaux;
        }
    }
}
