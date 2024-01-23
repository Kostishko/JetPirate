using JetPirate.ScriptsUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace JetPirate
{
    internal class UIManager
    {

        //ContentManager
        private ContentManager contentUI;

        //Font
        private SpriteFont fontUI;

        //UI pictures
        private Texture2D shipHealthPic;
        private Texture2D bulletCounterPic;
        private Texture2D enemyPic;

        // to ancor ui elements
        private Camera cam;
        private Vector2 ancorUI;

        // current player's health and gun 
        private JetShip shipUI;
        private Gun gunUI;

        //Enemy manager
        public Game1 gameMajor;

        //Menus
        public enum MenuPanel
        {
            MainMenu,
            Pause,
            Control,
            Options, 
            Lose,
            Win,
            Game
        }
        public MenuPanel menuPanel;
        
        //main panel
        private List<Button> mainMenuButtons;
        private int currentButton;
        private Texture2D mainMenuBackground;

        //Option panel (or creditors)
        private Texture2D optionBackground;

        //Control panel
        private Texture2D controlBackground;

        // Lose panel
        private Texture2D loseBackground;

        //Win panel
        private Texture2D winBackground;

        //pause panel
        private Texture2D pausePanel;
        private List<Button> pauseButtons;


        public UIManager(Camera cam, JetShip jetShip, ContentManager content, Game1 gameMajor) 
        {
            contentUI= content;
            fontUI = contentUI.Load<SpriteFont>("Fonts/MajorFont");
            this.cam = cam;
            shipUI = jetShip;
            gunUI = shipUI.GetGun();
            this.gameMajor = gameMajor;



            //UI pics
            shipHealthPic = contentUI.Load<Texture2D>("Sprites/Ship_03");
            bulletCounterPic = contentUI.Load<Texture2D>("Sprites/Bullet");
            enemyPic = contentUI.Load<Texture2D>("Sprites/Rocket");

            //backgrounds pics
            mainMenuBackground = contentUI.Load<Texture2D>("Sprites/");
            optionBackground = contentUI.Load<Texture2D>("Sprites/");
            pausePanel= contentUI.Load<Texture2D>("Sprites/");
            controlBackground = contentUI.Load<Texture2D>("Sprites/");
            loseBackground= contentUI.Load<Texture2D>("Sprites/");
            winBackground = contentUI.Load<Texture2D>("Sprites/");

            //Main menu button - that's also looks like a terrible dessicion, but I don't have time to do smth smart, so it's just from the top of my head?
            mainMenuButtons = new List<Button>();
            mainMenuButtons[0] = new Button(new Vector2(600, 450), "New Game", fontUI, this);
            mainMenuButtons[0] = (ButtonNewGame)mainMenuButtons[0];
            mainMenuButtons[1] = new Button(new Vector2(600, 470), "Control", fontUI, this);
            mainMenuButtons[1] = (ButtonControl)mainMenuButtons[1];
            mainMenuButtons[2] = new Button(new Vector2(600, 490), "Credits", fontUI, this);
            mainMenuButtons[2] = (ButtonCredits)mainMenuButtons[2];
            mainMenuButtons[3] = new Button(new Vector2(600, 510), "Exit", fontUI, this);
            mainMenuButtons[3] = (ButtonExit)mainMenuButtons[3];

            //pause Buttons
            pauseButtons = new List<Button>();
            pauseButtons[0] = new Button(new Vector2(600, 450), "Back to game", fontUI, this);
            pauseButtons[0] = (ButtonBackToGame)pauseButtons[0];
            pauseButtons[1] = new Button(new Vector2(600, 470), "Main Menu", fontUI, this);
            pauseButtons[1] = (ButtonMainMenu)pauseButtons[1];
            pauseButtons[2] = new Button(new Vector2(600,490), "Exit", fontUI,this);
            pauseButtons[2] = (ButtonExit)pauseButtons[2];

        }


        public void UpdateMe(GamePadState oldPadState, GamePadState curPadState)
        {
            //update an ancor
            ancorUI = -cam.position;

            switch (menuPanel)
            {
                case MenuPanel.MainMenu:
                    MainMenuUpdate(oldPadState, curPadState);
                    break;
                case MenuPanel.Options:
                    break;
                case MenuPanel.Lose:
                    break;
                case MenuPanel.Game:
                    break;
                case MenuPanel.Win: 
                    break;
                case MenuPanel.Pause: 
                    break;
                case MenuPanel.Control: 
                    break;
            }

        }

       
        public void DrawMe(SpriteBatch sp)
        {
            switch (menuPanel)
            {
                case MenuPanel.MainMenu:
                    break;
                case MenuPanel.Options:
                    break;
                case MenuPanel.Lose:
                    break;
                case MenuPanel.Game:
                    break;
                case MenuPanel.Win:
                    break;
                case MenuPanel.Pause:
                    break;
                case MenuPanel.Control:
                    break;
            }

        }


        #region UI in game state
        private void UIGameStateDraw(SpriteBatch sp)
        {
            //health points draw
            sp.Draw(shipHealthPic, ancorUI + new Vector2(25, 50), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            sp.DrawString(fontUI, " X " + shipUI.GetHealth(), ancorUI + new Vector2(100, 45), Color.White);

            // draw all magazine capacity
            for (int i = 0; i < gunUI.GetMagCapacity(); i++)
            {
                if (i < gunUI.GetMag())
                {
                    sp.Draw(bulletCounterPic, ancorUI + new Vector2(50, 120 + i * 25), Color.White);
                }
                else
                {
                    sp.Draw(bulletCounterPic, ancorUI + new Vector2(50, 120 + i * 25), Color.Gray);
                }
            }

            //reload indicator
            if (gunUI.GetReloadTimer() > 0 && gunUI.GetReloadTimer() != gunUI.GetReloadTime())
            {
                sp.DrawString(fontUI, "RELOAD", ancorUI + new Vector2(25, 145), Color.Red, 0.5f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            }

            sp.Draw(enemyPic, ancorUI + new Vector2(1100, 50), null, Color.White, (float)Math.PI / 2, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            sp.DrawString(fontUI, " X " + gameMajor.enemyManager.enemyCounter, ancorUI + new Vector2(1100, 60), Color.White);
        }
        #endregion




        #region Panels

        //Update Main menu
        private void MainMenuUpdate(GamePadState oldPadState, GamePadState curPadState)
        {
            for (int i = 0; i < mainMenuButtons.Count; i++)
            {
                if (i == currentButton)
                {
                    mainMenuButtons[i].UpdateMe(Button.UIButtonState.highlighted);
                }
                else
                {
                    mainMenuButtons[i].UpdateMe(Button.UIButtonState.calm);
                }
            }

            if (oldPadState.DPad.Down == ButtonState.Pressed && curPadState.DPad.Down == ButtonState.Released)
            {
                currentButton = Math.Clamp(currentButton++, 0, mainMenuButtons.Count);
            }
            if (oldPadState.DPad.Up == ButtonState.Pressed && curPadState.DPad.Up == ButtonState.Released)
            {
                currentButton = Math.Clamp(currentButton--, 0, mainMenuButtons.Count);
            }
            if (oldPadState.Buttons.X == ButtonState.Pressed && curPadState.Buttons.X == ButtonState.Released)
            {
                mainMenuButtons[currentButton].CliclMe();
            }
        }


        #endregion




    }
}
