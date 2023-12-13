using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace JetPirate
{
    internal class Camera
    {
        //public due debug
        public Vector2 position;
        public Vector2 savedPos;
        public float zoom;

        public Vector2 screenSize;

        public Vector2 leftUpperBorder;
        public Vector2 rightBottomBorder;

        public Random rng;

        public Camera(Vector2 pos, Vector2 leftUpperBord, Vector2 rightBottomBord, Vector2 screenSize)
        {
            position = pos;
            leftUpperBorder = leftUpperBord;
            rightBottomBorder = rightBottomBord;
            this.screenSize = screenSize;
            zoom = 1;
            rng = new Random();
        }

        public Matrix GetCam()
        {
            Matrix temp;
            temp = Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0));
            temp *= Matrix.CreateScale(zoom);
            return temp;
        }

        public void UpdateMe(JetShip jet)
        {
            if (jet.GetPosition().X + screenSize.X / (2 * zoom)<rightBottomBorder.X && jet.GetPosition().X+screenSize.X/(2*zoom)>leftUpperBorder.X)
            {
                position.X = (-jet.GetPosition().X + screenSize.X / (2 * zoom));
            }

            if (jet.GetPosition().Y + screenSize.Y / (2 * zoom) < rightBottomBorder.Y && jet.GetPosition().Y + screenSize.Y/ (2 * zoom) > leftUpperBorder.Y)
            {
                position.Y = (-jet.GetPosition().Y + screenSize.Y / (2 * zoom));
            }
        }

        public void Shaking(int power)
        {
            savedPos = position;
            position.X += rng.Next(-power,power);
            position.Y += rng.Next(-power,power) ;
        }

        public void StopShaking()
        {
            position = savedPos;
        }

    }
}
