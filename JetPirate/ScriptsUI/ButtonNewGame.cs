using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JetPirate.Game1;

namespace JetPirate.ScriptsUI
{
    internal class ButtonNewGame : Button
    {

        public ButtonNewGame(Vector2 position, Vector2 shift, String name, SpriteFont font, UIManager uiManager) : base(position, shift, name, font, uiManager) { }


        public override void CliclMe()
        {
            
                uiManager.gameMajor.jetShip.Restore();
                uiManager.gameMajor.enemyManager.ResetMe();
                uiManager.gameMajor.currentGameState = GameState.game;
                uiManager.menuPanel = UIManager.MenuPanel.Game;
        }
    }
}
