using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace JetPirate
{
    /// <summary>
    /// Camera of the game
    /// </summary>
    public class Camera
    {
        //public due debug
        public Vector2 position;
        
        public float zoom;

        public Vector2 screenSize;

        public Vector2 leftUpperBorder;
        public Vector2 rightBottomBorder;

        //Shaking effect
        public Vector2 savedPos;
        private float shakeTimer;
        private float shakeTime;
        public Random rng;
        private int shakePower;

        public Camera(Vector2 pos, Vector2 leftUpperBord, Vector2 rightBottomBord, Vector2 screenSize)
        {
            position = pos;
            leftUpperBorder = leftUpperBord;
            rightBottomBorder = rightBottomBord;
            this.screenSize = screenSize;
            zoom = 1;
            rng = new Random();
            shakeTime = 1.5f;
            shakeTimer = 0;



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

            Shaking(jet.GetPosition());
        }


        /// <summary>
        /// Shaking
        /// </summary>
        /// <param name="jetPos"></param>
        public void Shaking(Vector2 jetPos)
        {
            if (shakeTimer > 0)
            {
                var temp = rng.Next(-shakePower, shakePower);
                if (jetPos.X + temp + screenSize.X / (2 * zoom) < rightBottomBorder.X && jetPos.X + temp + screenSize.X / (2 * zoom) > leftUpperBorder.X)
                {
                    position.X += temp;
                }
                else
                {
                    position.X -= temp;
                }
                if (jetPos.Y + temp + screenSize.Y / (2 * zoom) < rightBottomBorder.Y && jetPos.Y + temp + screenSize.Y / (2 * zoom) > leftUpperBorder.Y)
                { 
                    position.Y += temp;
                }
                else
                {
                    position.Y -= temp;
                }
                shakeTimer -= 0.1f;
            }
        }

        /// <summary>
        /// Initialise the shaking with power (unfortunatelly timer of shaking is needed)
        /// </summary>
        /// <param name="power"></param>
        public void StartShaking(int power)
        {
            shakeTimer = shakeTime;
            shakePower = power;
        }

    }
}
