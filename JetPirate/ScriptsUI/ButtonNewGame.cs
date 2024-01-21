using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetPirate.ScriptsUI
{
    internal class ButtonNewGame : Button
    {

        public ButtonNewGame(Vector2 position, String name, SpriteFont font) : base(position, name, font) { }


        public override void CliclMe()
        {
            base.CliclMe();

        }
    }
}
