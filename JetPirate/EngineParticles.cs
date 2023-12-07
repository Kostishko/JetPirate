using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Windows.Forms.Design;
using static System.Net.Mime.MediaTypeNames;

namespace JetPirate
{
    internal class EngineParticles : Module
    {

        public ParticleSystem engineParticles;

        public EngineParticles(Vector2 pos, float rot, Object2D par, Vector2 shift, Texture2D tex ) : base(pos, rot, par, shift)
        {
                engineParticles = new ParticleSystem(pos, rot, tex,2f, 15f, 0.2f);
        }

        public void UpdateMe(bool play)
        {
            base.UpdateMe();
            engineParticles.UpdateMe(position, -parent.GetRotation() );
            if(play)
            {
                engineParticles.Play();
            }
            else
            {
                engineParticles.Stop();
            }
        }



    }
}
