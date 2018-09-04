using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace StickyHandGame_C9_RP7.Source.Managers.Classes
{
    class SoundManager
    {
        public static String BackGroundMusic = "BackgroundMusic";
        public static String ShotEffect = "Shoot";
        private static SoundManager instance;
        public static SoundManager Instance
        {
            get
            {
                if (SoundManager.instance == null)
                    SoundManager.instance = new SoundManager();
                return SoundManager.instance;
            }
        }
        private SoundManager()
        {

        }
        public Song BackGround;
        public List<SoundEffect> soundEffects = new List<SoundEffect>();
        public void LoadContent()
        {
            this.BackGround = GameManager.Instance.Content.Load<Song>(SoundManager.BackGroundMusic);
            SoundEffect se = GameManager.Instance.Content.Load<SoundEffect>(SoundManager.ShotEffect);
            soundEffects.Add(se);
            MediaPlayer.Play(this.BackGround);
            MediaPlayer.MediaStateChanged += this.MediaPlayer_MediaStateChanged;
        }
        public void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(this.BackGround);
        }
        public void Play(int i)
        {
            soundEffects[i].CreateInstance().Play();
        }
    }
}
