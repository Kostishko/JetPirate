using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace JetPirate
{
    internal class ShipGun : Module
    {

        public Gun gun;

        public ShipGun( Object2D par, Vector2 shift, Texture2D tex, Texture2D bulletTex) : base( par, shift) 
        {
            gun = new Gun(parent.GetPosition(),parent.GetRotation(), tex, bulletTex);
        }

        public void UpdateMe(GamePadState oldGP, GamePadState currGP)
        {
            base.UpdateMe();
            gun.UpdateMe(currGP, oldGP, position);
        }
        

    }
}
