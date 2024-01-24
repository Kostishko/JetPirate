using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetPirate.ScriptsUI
{
    internal class ButtonExit : Button
    {
        public ButtonExit(Vector2 pos, Vector2 shift, String name, SpriteFont font, UIManager uiManager) : base(pos, shift, name, font, uiManager)
        {

        }

        public override void CliclMe()
        {          
                uiManager.gameMajor.Exit();
        }
    }
}
