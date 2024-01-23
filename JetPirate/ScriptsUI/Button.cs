using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace JetPirate
{
    internal class Button
    {
        public enum UIButtonState
        {
            highlighted,            
            calm
        }
        public UIButtonState state;

        protected Vector2 position;
        protected String name;
        protected SpriteFont uiFont;

        protected UIManager uiManager;

        public Button(Vector2 position, String name, SpriteFont font, UIManager uiManager)
        {
            this.position = position;
            this.name = name;
            this.uiFont = font;
            this.uiManager = uiManager;
        }

        public void UpdateMe(UIButtonState state)
        {
            this.state = state;
        }

        public virtual void CliclMe()
        {

        }

        public void DrawMe(SpriteBatch sp)
        {
            switch (state)
            {
                case UIButtonState.highlighted:
                    sp.DrawString(uiFont, name, position, Color.Yellow);
                    break;
                case UIButtonState.calm:
                    sp.DrawString(uiFont, name, position, Color.White);
                    break;
            }
        }

    }
}
