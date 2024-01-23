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
    internal class ButtonCredits : Button
    {
        public ButtonCredits(Vector2 pos, String name, SpriteFont font, UIManager uiManager) : base(pos, name, font, uiManager)
        {

        }

        public override void CliclMe()
        {
            uiManager.menuPanel = UIManager.MenuPanel.Options;
        }

    }
}
