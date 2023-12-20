using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace JetPirate
{
    internal class ShipGun : Module
    {

        public Gun gun;

        public ShipGun(Vector2 pos, float rot, Object2D par, Vector2 shift, Texture2D tex) : base(pos, rot, par, shift) 
        {
            gun = new Gun(pos,rot, tex);
        }

        public void UpdateMe(GamePadState oldGP, GamePadState currGP)
        {
            base.UpdateMe();
            gun.UpdateMe(currGP, oldGP, position);
        }
        

    }
}
