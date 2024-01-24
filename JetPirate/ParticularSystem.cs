using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;

namespace JetPirate
{
    /// <summary>
    /// This is a system that produce particles for visual effect
    /// </summary>
    internal class ParticleSystem : Object2D
    {
        //emission

        //private Particle _particle;
        private List<Particle> _particles;
        public Vector2 Direction;
        private bool isPlaying;

        public float timeBetweenEmission;
        private float timer;

        private Random rng;
        private float emissionCone;

        //Particle setting
        private Texture2D partTex;
        private float opacitySpeed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos">Start position for particles</param>
        /// <param name="rot">Rotation of emission (direction of particle flying)</param>
        /// <param name="tex"></param>
        /// <param name="speed">Particles speed</param>
        /// <param name="opacitySpeed">Particles shading speed</param>
        /// <param name="emissionTime">Pause between emission</param>
        /// /// <param name="emissionCone">Random direction</param>
        public ParticleSystem(Vector2 pos, float rot, Texture2D tex, float speed, float opacitySpeed, float emissionTime, float emissionCone) : base(pos, rot)
        {
            partTex = tex;
            //emission
            timeBetweenEmission = emissionTime;
            timer = timeBetweenEmission;
            this.emissionCone = emissionCone;
            rng=new Random();
            //system control
            isPlaying = false;
            _particles = new List<Particle>();
            for (int i = 0; i < 10; i++)
            {
                _particles.Add(new Particle(partTex, position, speed, Rotation, opacitySpeed ));
            }            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos">current position of system</param>
        /// <param name="rot">current rotation of system</param>
        public void UpdateMe(Vector2 pos, float rot)
        {
            //Update positions
            position = pos;
            Rotation = rot;
            //Update direction of particles' flying
            Direction = new Vector2((float)Math.Sin(Rotation), (float)Math.Cos(Rotation));
            Direction.Normalize();

            //Update for all flying particles
            for (int i =0; i<_particles.Count;i++)
            {
                if (_particles[i].GetState())
                _particles[i].UpdateMe(position);
                
            }

            // update id it is turned on
            if(isPlaying)
            {
                if (timer <= 0) //timer of emission
                {
                    for (int i = 0; i < _particles.Count; i++)
                    {
                        //check if we have particles which not in fly
                        if (!_particles[i].GetState())
                        {
                            //Update direction of particles' flying
                            float rotateShift = (float)(rng.NextDouble() - 0.5f) * emissionCone * 2;
                            Direction = new Vector2((float)Math.Sin(Rotation+rotateShift), (float)Math.Cos(Rotation + rotateShift));
                            Direction.Normalize();
                            //if it is not in a flying - we trigger it to start
                            _particles[i].TriggerMe(position, Direction); //trigger back particle to current system pos
                            timer = timeBetweenEmission; 
                            break;
                        }
                    }
                }
                else
                {
                    timer -= 0.1f;
                }
            }
        }

        public void DrawMe(SpriteBatch sp)
        {   
            //drawing for all particles
            for(int i=0; i < _particles.Count;i++)
            {
                _particles[i].DrawMe(sp);   
            }
            //debug strings
            //DebugManager.DebugString("isPlaying?: " + isPlaying, new Vector2(0, 0));
            //DebugManager.DebugString("Timer to next particle: "+timer, new Vector2(0, 22));
            //DebugManager.DebugString("direction: "+ Direction, new Vector2(0, 44));
        }

        //trigger to start an emission
        public void Play()
        {
            isPlaying=true;
        }
        //trigger to stop an emission
        public void Stop()
        {
            isPlaying = false;
        }
    }

    /// <summary>
    /// Particle object
    /// </summary>
    class Particle : Object2D
    {

        private Texture2D _texture;
        private Vector2 origin;

        //speed of movement and speed of fading
        private float speed;
        private float opacitySpeed;

        //direction
        private Vector2 direction;
        
        
        private bool isFlying; //is it triggered particle?
        
        private Color color;


        public Particle(Texture2D tex, Vector2 pos, float speed, float rot, float opacitySpeed) : base (pos,rot)
        {
            _texture = tex;
            position = pos;
            origin = new Vector2(tex.Width/2, tex.Height/2);
            direction = Vector2.Zero;
            this.speed = speed;
            this.opacitySpeed = opacitySpeed;
            color = Color.Transparent;            

            isFlying = false;
        }


        public void UpdateMe( Vector2 startPos)
        {
            if (isFlying) //if it's triggered
            {
                // movement update
                position.X += direction.X * speed;
                position.Y += direction.Y * speed;
                
                //Rotation update (maybe change it to avoid rotation after triggering, add a startDirection variable maybe?)
                Rotation = (float)Math.Atan2(direction.Y, direction.X);
                
                //color fading
                color.A = (byte)Math.Clamp(color.A - opacitySpeed, 0, byte.MaxValue);
                color.B = (byte)Math.Clamp(color.B - opacitySpeed, 0, byte.MaxValue);
                color.R = (byte)Math.Clamp(color.R - opacitySpeed, 0, byte.MaxValue);
                color.G = (byte)Math.Clamp(color.G - opacitySpeed, 0, byte.MaxValue);

                //if it's faded to transparent - it's back in untriggered particles pool
                if (color==Color.Transparent) 
                {
                    TriggerMe(startPos, Vector2.Zero);
                }
            }
            //untriggered particles are faded and always on a particle system position
            else
            {   
                position = startPos;
            }

        }

        public void DrawMe(SpriteBatch sp)
        {
            sp.Draw(_texture, position, null, color, Rotation, origin, 1f, SpriteEffects.None, 1f);
        }

        public void TriggerMe(Vector2 startPos, Vector2 dir)
        {
            isFlying = isFlying ? false : true;

            position = startPos;
            direction = dir;
            if (isFlying) { color=Color.White; }            
        }

        public bool GetState()
        {
            return isFlying;
        }

    }
}
