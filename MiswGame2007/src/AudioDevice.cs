using System;
using Yanesdk.Ytl;
using Yanesdk.Sound;

namespace MiswGame2007
{
    public class AudioDevice
    {
        private static string[] SOUND_PATH = {
            "sounds/test.wav",
            "sounds/pistol.wav",
            "sounds/explode.wav",
            "sounds/shotgun.wav",
            "sounds/rocket.wav",
            "sounds/atfield.wav",
            "sounds/flame.wav",
            "sounds/damage.wav",
            "sounds/weapon.wav",
            "sounds/glass.wav",
            "sounds/hammer.wav",
            "sounds/rjump.wav",
            "sounds/impact.wav",
            "sounds/mvoice.wav",
            "sounds/rvoice.wav",
            "sounds/svoice.wav",
            "sounds/fvoice.wav",
            "sounds/okvoice.wav",
            "sounds/explode2.wav",
            "sounds/baaka.wav",
            "sounds/byaa.wav",
            "sounds/nurunuru.wav",
            "sounds/hi1.wav",
            "sounds/hi2.wav",
            "sounds/kue.wav",
            "sounds/horay.wav",
            "sounds/shuhu.wav",
            "sounds/duely.wav",
            "sounds/tvoice.wav",
            "sounds/close.wav",
            "sounds/mr.wav",
            "sounds/warp.wav",
            "sounds/laser.wav"
        };

        private static string[] MUSIC_PATH = {
            "musics/test.ogg",
            "musics/title.ogg",
            "musics/stage1.ogg",
            "musics/stage2.ogg",
            "musics/stage3.ogg",
            "musics/stage4.ogg",
            "musics/stage5.ogg",
            "musics/boss1.ogg",
            "musics/boss2.ogg",
            "musics/clear.ogg",
            "musics/ending.ogg"
        };

        private static float[] MUSIC_VOLUME = {
            0.0f,
            1.0f,
            1.0f,
            1.0f,
            0.8f,
            0.8f,
            0.6f,
            1.0f,
            0.8f,
            1.0f,
            1.0f
        };

        private Sound[] sounds;
        private Sound[] musics;

        private GameMusic currentMusic;

        public AudioDevice()
        {
            LoadSounds();
            LoadMusics();
            currentMusic = GameMusic.None;
        }

        private void LoadSounds()
        {
            sounds = new Sound[SOUND_PATH.Length];
            for (int i = 1; i < SOUND_PATH.Length; i++)
            {
                sounds[i] = LoadSoundByPath(Settings.RESOURCE_PATH + "/" + SOUND_PATH[i]);
            }
        }

        private void LoadMusics()
        {
            musics = new Sound[MUSIC_PATH.Length];
            for (int i = 1; i < MUSIC_PATH.Length; i++)
            {
                musics[i] = LoadMusicByPath(Settings.RESOURCE_PATH + "/" + MUSIC_PATH[i]);
                musics[i].Volume = MUSIC_VOLUME[i];
            }
        }

        private Sound LoadSoundByPath(string path)
        {
            Sound sound = new Sound();
            YanesdkResult result = sound.Load(path);
            if (result == YanesdkResult.NoError)
            {
                if (path.IndexOf("voice") != -1)
                {
                    sound.Volume = 0.9f;
                }
                else
                {
                    sound.Volume = 0.4f;
                }
                return sound;
            }
            else
            {
                throw new Exception("効果音「" + path + "」をロードできません＞＜");
            }
        }

        private Sound LoadMusicByPath(string path)
        {
            Sound sound = new Sound();
            YanesdkResult result = sound.Load(path, -1);
            if (result == YanesdkResult.NoError)
            {
                if (path.IndexOf("title") != -1 || path.IndexOf("clear") != -1 || path.IndexOf("ending") != -1)
                {
                    sound.Loop = 0;
                }
                else
                {
                    sound.Loop = -1;
                }
                return sound;
            }
            else
            {
                throw new Exception("BGM「" + path + "」をロードできません＞＜");
            }
        }

        public void PlaySound(GameSound sound)
        {
            sounds[(int)sound].Play();
        }

        public void PlayMusic(GameMusic music)
        {
            if (music != currentMusic)
            {
                musics[(int)music].PlayFade(1000);
                currentMusic = music;
            }
        }

        public void StopMusic()
        {
            if (currentMusic == GameMusic.None)
            {
                return;
            }
            Sound.FadeMusic(250);
            currentMusic = GameMusic.None;
        }

        public void Update()
        {
        }
    }
}
