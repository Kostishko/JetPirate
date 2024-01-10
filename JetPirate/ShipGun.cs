using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;


namespace JetPirate
{
    internal class ShipGun : Module
    {

        private Gun gun;

        public ShipGun( Object2D par, Vector2 shift, ContentManager content) : base( par, shift) 
        {
            gun = new Gun(parent.GetPosition(),parent.GetRotation(), content.Load<Texture2D>("Sprites/Gun"), content.Load<Texture2D>("Sprites/Bullet"));
        }

        public void UpdateMe(GamePadState oldGP, GamePadState currGP)
        {
            base.UpdateMe();
            gun.UpdateMe(currGP, oldGP, position);
        }

        public Gun GetGun()
        { 
            return gun; 
        }
        


    }
}
