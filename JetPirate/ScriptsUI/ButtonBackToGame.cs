using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace JetPirate.ScriptsUI
{
    internal class ButtonBackToGame : Button
    {

        public ButtonBackToGame(Vector2 pos, Vector2 shift, String name, SpriteFont font, UIManager uiManager) : base (pos, shift, name, font, uiManager) 
        { 
        
        }

        public override void CliclMe()
        {
            uiManager.gameMajor.currentGameState = Game1.GameState.game;
            uiManager.menuPanel = UIManager.MenuPanel.Game;
        }

    }
}
