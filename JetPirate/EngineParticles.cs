using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Windows.Forms.Design;
using static System.Net.Mime.MediaTypeNames;

namespace JetPirate
{
    /// <summary>
    /// Particles that can be attached to main jet. Can't be attached to enemy unfortunatelly. 
    /// </summary>
    internal class EngineParticles : Module
    {

        public ParticleSystem engineParticles;

        public EngineParticles(Object2D par, Vector2 shift, Texture2D tex ) : base( par, shift)
        {
                engineParticles = new ParticleSystem(parent.GetPosition(),parent.GetRotation(), tex,2f, 15f, 0.2f, 0.5f);
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
