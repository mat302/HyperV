using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using AtelierXNA;

namespace HyperV
{
    public class NiveauRythmé : Microsoft.Xna.Framework.GameComponent
    {
        const int NB_À_RÉUSSIR = 5;

        //Constructeur
        readonly string NomFichierLecture;
        readonly string NomTexture;
        readonly float IntervalleMAJ;

        //Initialize
        bool BoutonUn { get; set; }
        bool BoutonDeux { get; set; }
        bool BoutonTrois { get; set; }
        bool NiveauEstTerminé { get; set; }

        float TempsÉcouléDepuisMAJ { get; set; }
        List<Vector3> Positions { get; set; }
        int cpt { get; set; }
        int nombreRéussi { get; set; }
        public Vector3? PositionCubeRouge { get; set; }
        AfficheurPointage Pointage { get; set; }

        //ChargerContenu
        Random GénérateurAléatoire { get; set; }
        InputManager GestionInput { get; set; }
        GamePadManager GestionGamePad { get; set; }
        List<UnlockableWall> MurÀEnlever { get; set; }
        List<Portal> ListePortails { get; set; }

        public NiveauRythmé(Game jeu, string nomFichierLecture, string nomTexture, float intervalleMAJ)
            : base(jeu)
        {
            NomFichierLecture = nomFichierLecture;
            NomTexture = nomTexture;
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
            cpt = 0;
            TempsÉcouléDepuisMAJ = 0;

            Positions = new List<Vector3>();
            InitialiserPositions();
            Pointage = new AfficheurPointage(Game, "Arial50", Color.Black, IntervalleMAJ);
            ChargerContenu();
            InitialisationComposants();
            
        }

        void InitialiserPositions()
        {
            string ligneLue;
            int indicateurDebut;
            float composanteX, composanteY, composanteZ;

            StreamReader lecteurFichier = new StreamReader(NomFichierLecture);

            while (!lecteurFichier.EndOfStream)
            {
                ligneLue = lecteurFichier.ReadLine();

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

            for(int i = 0; i < Positions.Count; i += 2)
            {
                Game.Components.Add(new CylindreTexturé(Game, 1, new Vector3(0, 0, 0),
                                    Vector3.Zero, new Vector2(1, 1), new Vector2(20, 20),
                                    "Fil Electrique", IntervalleMAJ, Positions[i],
                                    Positions[i+1]));

                Game.Components.Add(new CubeTexturé(Game, 1, Vector3.Zero, Positions[i+1],
                                    "Blanc", new Vector3(3, 3, 3), IntervalleMAJ));

                Game.Components.Add(new TuileTexturée(Game, 1, new Vector3(0, -MathHelper.PiOver2, 0),
                                    Positions[i+1] - 1.65f * Vector3.UnitX, new Vector2(3, 3), (i/2+1).ToString()));
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
            BoutonUn = GestionInput.EstNouvelleTouche(Keys.NumPad1)|| 
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
            cpt++;

            foreach (CubeTexturé cube in Game.Components.Where(composant => composant is CubeTexturé))
            {
                if (SontVecteursÉgaux(PositionCubeRouge, cube.Position))
                {
                    cube.NomTextureCube = "Rouge";
                    cube.InitialiserParamètresEffetDeBase();
                    PositionCubeRouge = null;
                }

                foreach (SphèreRythmée sp in Game.Components.Where(composant => composant is SphèreRythmée))
                {
                    if (sp.EstEnCollision(cube))
                    {
                        if (SontVecteursÉgaux(sp.Extrémité1, Positions[0]) && BoutonUn ||
                        SontVecteursÉgaux(sp.Extrémité1, Positions[2]) && BoutonDeux ||
                        SontVecteursÉgaux(sp.Extrémité1, Positions[4]) && BoutonTrois)
                        {
                            sp.ÀDétruire = true;
                            cube.NomTextureCube = "Vert";
                            cube.InitialiserParamètresEffetDeBase();
                            ++nombreRéussi;
                        }
                    }
                }
            }

            Pointage.Chaîne = nombreRéussi.ToString() + "/" + NB_À_RÉUSSIR.ToString();

            if(nombreRéussi >= NB_À_RÉUSSIR)
            {
                NiveauEstTerminé = true;
                cpt = 121;
                Game.Components.Remove(MurÀEnlever[0]);
                ListePortails.Add(new Portal(Game, 1, new Vector3(0, 1.570796f, 0),
                                  new Vector3(170, -60, -10), new Vector2(40, 40), "Transparent",
                                  1, IntervalleMAJ));
                Game.Components.Add(ListePortails.Last());
            }

            if (cpt > 120 )
            {
                if (!NiveauEstTerminé)
                {
                    //int nbreBalles = GénérateurAléatoire.Next(1, 4);
                    //for(int i = 0; i < nbreBalles; i++)
                    //{
                        int choixPente = GénérateurAléatoire.Next(0, 3) * 2;
                        Game.Components.Add(new Afficheur3D(Game));
                        Game.Components.Add(new SphèreRythmée(Game, 1, Vector3.Zero,
                                            Positions[choixPente], 1, new Vector2(20, 20),
                                            "BleuBlancRouge", IntervalleMAJ, Positions[choixPente + 1]));
                    //}
                }

                cpt = 0;

                foreach (CubeTexturé cube in Game.Components.Where(composant => composant is CubeTexturé))
                {
                    cube.NomTextureCube = "Blanc";
                    cube.InitialiserParamètresEffetDeBase();
                }
            }

            BoutonUn = false;
            BoutonDeux = false;
            BoutonTrois = false;
        }

        bool SontVecteursÉgaux(Vector3? a, Vector3 b)
        {
            bool égaux;

            if(a == null)
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
