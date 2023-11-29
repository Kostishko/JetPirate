using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace JetPirate
{
    internal class ParticleSystem : Object2D
    {
        //emission

        private Particle _particle;
        private List<Particle> _particles;
        private Vector2 _direction;
        private bool isPlaying;

        public float timeBetweenEmission;
        private float timer;

        //Particle setting
        private Texture2D partTex;        

        public ParticleSystem(Vector2 pos, float rot, Texture2D tex): base(pos, rot)
        {
            partTex = tex;
            timeBetweenEmission = 2f;
            timer = timeBetweenEmission;
            isPlaying = false;
            _particles = new List<Particle>();
            for (int i = 0; i < 10; i++)
            {
                _particles.Add(new Particle(partTex, position, 1f, rotation));
            }
        }



        public void UpdateMe(Vector2 pos, float rot)
        {
            position = pos;
            rotation = rot;
            _direction = new Vector2((float)Math.Sin(rotation), (float)Math.Cos(rotation));

            for (int i =0; i<_particles.Count;i++)
            {
                _particles[i].UpdateMe(_direction, position);
                
            }

            if(isPlaying)
            {
                if (timer <= 0)
                {
                    for (int i = 0; i < _particles.Count; i++)
                    {
                        if (!_particles[i].GetState())
                        {
                            _particles[i].TriggerMe();
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

            
            for(int i=0; i < _particles.Count;i++)
            {
                _particles[i].DrawMe(sp);   
            }


            DebugManager.DebugString("isPlaying?: " + isPlaying, new Vector2(0, 0));
            DebugManager.DebugString("Timer to next particle: "+timer, new Vector2(0, 22));
            DebugManager.DebugString("direction: "+ _direction, new Vector2(0, 44));
        }

        public void Play()
        {
            isPlaying=true;
        }

        public void Stop()
        {
            isPlaying = false;
        }


    }

    class Particle : Object2D
    {
        private Texture2D _texture;
        private float speed;
        
        private bool isFlying;

        private Color color;

        public Particle(Texture2D tex, Vector2 pos, float speed, float rot) : base (pos,rot)
        {
            _texture = tex;
            position = pos;
            this.speed = speed;
            color = Color.Transparent;
            

            isFlying = false;
        }


        public void UpdateMe(Vector2 direction, Vector2 startPos)
        {
            if (isFlying)
            {
                position.X += direction.X * speed;
                position.Y += direction.Y * speed;
                color.A--;
                color.B--;
                color.R--; 
                color.G--; 
                if(color.A==0)
                {
                    TriggerMe();
                }
            }
            else
            {   
                position = startPos;
            }

        }

        public void DrawMe(SpriteBatch sp)
        {
            sp.Draw(_texture, position, null, color, rotation, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        public void TriggerMe()
        {
            isFlying = isFlying ? false : true;
            if (isFlying) { color=Color.White; }
        }

        public bool GetState()
        {
            return isFlying;
        }

    }
}
