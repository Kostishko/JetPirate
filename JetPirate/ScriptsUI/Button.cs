using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace JetPirate
{
    abstract class Button
    {
        public enum UIButtonState
        {
            highlighted,            
            calm
        }
        public UIButtonState state;

        protected Vector2 position;
        protected Vector2 shift;
        protected String name;
        protected SpriteFont uiFont;

        protected UIManager uiManager;

        public Button(Vector2 position, Vector2 shift, String name, SpriteFont font, UIManager uiManager)
        {
            this.position = position;
            this.name = name;
            this.uiFont = font;
            this.uiManager = uiManager;
            this.shift = shift;
        }

        public void UpdateMe(UIButtonState state, Vector2 ancor)
        {
            this.state = state;
            position = ancor+shift;
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
