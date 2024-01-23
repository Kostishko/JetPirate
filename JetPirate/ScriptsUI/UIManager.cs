using JetPirate.ScriptsUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
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

        //Menu's panels
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

        //back to main menu button
        private ButtonMainMenu backButton;


        public UIManager(Camera cam, JetShip jetShip, ContentManager content, Game1 gameMajor)
        {
            contentUI = content;
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
            pausePanel = contentUI.Load<Texture2D>("Sprites/");
            controlBackground = contentUI.Load<Texture2D>("Sprites/");
            loseBackground = contentUI.Load<Texture2D>("Sprites/");
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
            pauseButtons[2] = new Button(new Vector2(600, 490), "Exit", fontUI, this);
            pauseButtons[2] = (ButtonExit)pauseButtons[2];

            //back to main menu button
            backButton = new ButtonMainMenu(new Vector2(600, 450), "Back to main Menu", fontUI, this);

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
                    OptionUpdate(oldPadState, curPadState);
                    break;
                case MenuPanel.Lose:
                    LoseUpdate(oldPadState, curPadState);
                    break;
                case MenuPanel.Game: // all updates of Game layout are in Draw method
                    break;
                case MenuPanel.Win:
                    WinUpdate(oldPadState, curPadState);
                    break;
                case MenuPanel.Pause:
                    PauseUpdate(oldPadState, curPadState);
                    break;
                case MenuPanel.Control:
                    ControlUpdate(oldPadState, curPadState);
                    break;
            }

        }


        public void DrawMe(SpriteBatch sp)
        {
            switch (menuPanel)
            {
                case MenuPanel.MainMenu:
                    MainMenuDraw(sp);
                    break;
                case MenuPanel.Options:
                    OptionDraw(sp);
                    break;
                case MenuPanel.Lose:
                    LoseDraw(sp);
                    break;
                case MenuPanel.Game:
                    UIGameStateDraw(sp);
                    break;
                case MenuPanel.Win:
                    WinDraw(sp);
                    break;
                case MenuPanel.Pause:
                    PauseDraw(sp);
                    break;
                case MenuPanel.Control:
                    ControlDraw(sp);
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
        //Panels have a similar logic, but I decided to split them out to make the code more scalable for future, in case if 
        //panels will add more logic (for example change the volume in options or smth like that)


        /// <summary>
        /// Main Menu control
        /// </summary>
        /// <param name="oldPadState"></param>
        /// <param name="curPadState"></param>
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

        /// <summary>
        /// Main menu draw
        /// </summary>
        /// <param name="sp"></param>
        private void MainMenuDraw(SpriteBatch sp)
        {
            sp.Draw(mainMenuBackground, ancorUI + Vector2.Zero, Color.White);

            for (int i = 0; i < mainMenuButtons.Count; i++)
            {
                mainMenuButtons[i].DrawMe(sp);
            }
        }


        /// <summary>
        /// Option update (basicaly it is just back button, and there is no options)
        /// </summary>
        /// <param name="oldPadState"></param>
        /// <param name="curPadState"></param>
        public void OptionUpdate(GamePadState oldPadState, GamePadState curPadState)
        {
            if (oldPadState.Buttons.B == ButtonState.Pressed && curPadState.Buttons.B == ButtonState.Released)
            {
                backButton.CliclMe();
            }
        }

        /// <summary>
        /// Option panel draw
        /// </summary>
        /// <param name="sp"></param>
        public void OptionDraw(SpriteBatch sp)
        {
            sp.Draw(optionBackground, ancorUI + Vector2.Zero, Color.White);
            backButton.DrawMe(sp);
        }


        /// <summary>
        /// Lose panel update
        /// </summary>
        /// <param name="oldPadState"></param>
        /// <param name="curPadState"></param>
        public void LoseUpdate(GamePadState oldPadState, GamePadState curPadState)
        {
            if (oldPadState.Buttons.B == ButtonState.Pressed && curPadState.Buttons.B == ButtonState.Released)
            {
                backButton.CliclMe();
            }
        }

        /// <summary>
        /// Lose panel draw method
        /// </summary>
        /// <param name="sp"></param>
        public void LoseDraw(SpriteBatch sp)
        {
            sp.Draw(loseBackground, ancorUI + Vector2.Zero, Color.White);
            backButton.DrawMe(sp);
        }


        /// <summary>
        /// Win update (turn back to main menu)
        /// </summary>
        /// <param name="oldPadState"></param>
        /// <param name="curPadState"></param>
        public void WinUpdate(GamePadState oldPadState, GamePadState curPadState)
        {
            if (oldPadState.Buttons.B == ButtonState.Pressed && curPadState.Buttons.B == ButtonState.Released)
            {
                backButton.CliclMe();
            }
        }


        /// <summary>
        /// win state draw
        /// </summary>
        /// <param name="sp"></param>
        public void WinDraw(SpriteBatch sp)
        {
            sp.Draw(winBackground, ancorUI + Vector2.Zero, Color.White);
            backButton.DrawMe(sp);
        }

        /// <summary>
        /// Pause menu buttons update
        /// </summary>
        /// <param name="oldPadState"></param>
        /// <param name="curPadState"></param>
        public void PauseUpdate(GamePadState oldPadState, GamePadState curPadState)
        {
            for (int i = 0; i < pauseButtons.Count; i++)
            {
                if (i == currentButton)
                {
                    pauseButtons[i].UpdateMe(Button.UIButtonState.highlighted);
                }
                else
                {
                    pauseButtons[i].UpdateMe(Button.UIButtonState.calm);
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
                pauseButtons[currentButton].CliclMe();
            }
        }

        /// <summary>
        /// Pause UI draw
        /// </summary>
        /// <param name="sp"></param>
        public void PauseDraw(SpriteBatch sp)
        {
            UIGameStateDraw(sp);
            sp.Draw(pausePanel, ancorUI+Vector2.Zero, Color.White);
            for (int i = 0; i < mainMenuButtons.Count; i++)
            {
                pauseButtons[i].DrawMe(sp);
            }
        }


        /// <summary>
        /// Control update 
        /// </summary>
        /// <param name="oldPadState"></param>
        /// <param name="curPadState"></param>
        public void ControlUpdate(GamePadState oldPadState, GamePadState curPadState)
        {
            if (oldPadState.Buttons.B == ButtonState.Pressed && curPadState.Buttons.B == ButtonState.Released)
            {
                backButton.CliclMe();
            }
        }


        /// <summary>
        /// Control Draw - picture with explanations of the game
        /// </summary>
        /// <param name="sp"></param>
        public void ControlDraw(SpriteBatch sp)
        {
            sp.Draw(controlBackground, ancorUI + Vector2.Zero, Color.White);
            backButton.DrawMe(sp);
        }





        #endregion




    }
}
