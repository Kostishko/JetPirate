using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace JetPirate
{
    static class AudioManager
    {

        //listener and position
        private static float maxDistance;
        private static ListenerModule listener;


        private static Song song;

        private static float volume;
        public static float Volume
        {
            get => volume;
            set
            {
                volume = Math.Clamp(value, 0f, 1f);
            }
        }

        
        public static float MusicVolume
        {
            get=> MediaPlayer.Volume;
            set 
            {
                MediaPlayer.Volume = Math.Clamp(value, 0f, 1f);
            }
        }


      //  private static List<AudioModule> audioModules;


        static AudioManager()
        {
          //  audioModules = new List<AudioModule>();
        }

        



        //public static void AddObject(AudioModule obj)
        //{
        //    audioModules.Add(obj);
        //}

        public static void PlayMusic()
        {
            MediaPlayer.Play(song);
        }

        public static void SetMusicVolume(float vol)
        {
            MusicVolume = vol;
        }

        public static void SetVolume(float vol)
        {
            Volume = vol;
        }

        public static void Initialize(float maxDistance, ListenerModule listener)
        {
            AudioManager.maxDistance = maxDistance;
            AudioManager.listener = listener; 
        }

        
        public static void PlaySound(SoundEffectInstance instance, Vector2 sourcePos)
        {
            float distance = Vector2.Distance(sourcePos, listener.GetPosition());
            float noramalizedDist = 1 - distance / maxDistance;
            switch (maxDistance-distance)
            {
                case 0:
                    instance.Volume = 1f*Volume;
                    break;
                case > 0:
                    instance.Volume = (1 - noramalizedDist) * Volume;
                    if (listener.GetPosition().X > sourcePos.X)
                        instance.Pan = -((1 - noramalizedDist) * Volume);
                    else
                        instance.Pan = (1 - noramalizedDist) * Volume;
                    break;
                case < 0:
                    break;                
            }

            instance.Play();
        }

        public static void StopSound(SoundEffectInstance instance) 
        {
            instance.Stop();        
        }


    }


    internal class AudioModule : Module
    {

        private List<SoundEffectInstance> soundEffectList;

        public AudioModule(Object2D parent, Vector2 shift, List<SoundEffect> sounds): base(parent, shift)
        {
            // AudioManager.AddObject(this);
            soundEffectList = new List<SoundEffectInstance>();
            for (int i = 0; i < sounds.Count; i++)
            {
                soundEffectList.Add(sounds[i].CreateInstance());
            }
        }

        public void PlaySound(byte soundId)
        {
            AudioManager.PlaySound(soundEffectList[soundId], parent.GetPosition());
        }

        public void StopSound(int soundId)
        {
            
        }



    }

    /// <summary>
    /// We need it if we want to add some effects from listener
    /// </summary>
    public class ListenerModule : Module
    {
        public ListenerModule(Object2D parent, Vector2 shift) : base(parent, shift)
        {
        }
    }
}
