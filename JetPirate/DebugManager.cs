using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace JetPirate
{
    /// <summary>
    /// Debug manager to simplify draw essential data due developing
    /// </summary>
    static class DebugManager
    {


        static public SpriteFont debugFont;
        static public Texture2D debugTexture;
        static public SpriteBatch spriteBatch;
        static public bool isWorking;

        static public void DebugString(string message, Vector2 pos)
        {
            if (isWorking)
            {
                spriteBatch.DrawString(debugFont, message, pos, Color.White);
            }
        }

        static public void DebugRectangle(Rectangle rec)
        {
            if (isWorking)
            {

                spriteBatch.Draw(debugTexture,rec,Color.White);
            }
        }


    }
}
