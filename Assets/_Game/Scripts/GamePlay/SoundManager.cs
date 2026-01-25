using System.Collections;
using UnityEngine;
using Doozy.Engine.Soundy;
using DG.Tweening;
using System.Xml.Linq;

namespace TwoCore
{
    public class SoundManager : SingletonMono<SoundManager>
    {
        public SoundyPlayer BgSound;
        public SoundyPlayer BgBattleSound;
        private SoundyController prevSfx;
        private SoundyPlayer currentMusic;

        private void Reset()
        {
            gameObject.name = "SoundManager";
        }

        public void ResetSound()
        {
            prevSfx = null;
            currentMusic = null;
        }

        // ============================================
        // MUSIC
        // ============================================
        public void PlayBGMusic()
        {
            PlayMusic(BgSound);
        }

        public void PlayBGBattleMusic()
        {
            PlayMusic(BgBattleSound);
        }

        public void PlayMusic(SoundyPlayer sound)
        {
            if (sound == null) return;

            StartCoroutine(PlayMusicRoutine(sound));
        }

        private IEnumerator PlayMusicRoutine(SoundyPlayer sound)
        {
            if (currentMusic != null && currentMusic != sound)
            {
                currentMusic.Stop();
            }

            SoundyController.StopAll();
            yield return new WaitForSeconds(0.2f);

            sound.Play();
            sound.loop = true;
            currentMusic = sound;
        }

        public void StopMusic()
        {
            currentMusic.Stop();
            currentMusic = null;
        }

        // ============================================
        // SFX
        // ============================================
        public void PlayOneShot(string sfxName)
        {
            SoundyManager.Play("Sfx", sfxName);
        }

        public void PlayOneShot(string sfxName, float delay)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                SoundyManager.Play("Sfx", sfxName);
            });
        }

        public void StopAll()
        {
            SoundyManager.StopAllSounds();
            currentMusic = null;
            prevSfx = null;
        }

        public void PlaySoundSfx(string sfxName, float delay = 0)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                if (prevSfx != null) prevSfx.Stop();
                prevSfx = SoundyManager.Play("Sfx", sfxName);
            });
        }

        public void PlaySoundGameSfx(string sfxName, float delay = 0)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                if (prevSfx != null) prevSfx.Stop();
                prevSfx = SoundyManager.Play("Game Sfx", sfxName);
            });
        }

        public SoundyController PlaySoundSfxReturn(string sfxName)
        {
            if (prevSfx != null) prevSfx.Stop();
            prevSfx = SoundyManager.Play("Sfx", sfxName);
            return prevSfx;
        }

        public SoundyController PlaySoundSfxReturnValue(string sfxName)
        {
            return SoundyManager.Play("Sfx", sfxName);
        }

        public void PlayMutiSound(string sfxName, float turn = 0, float duration = 0.1f)
        {
            StartCoroutine(RunPlayMutiSound(sfxName, turn, duration));
        }

        IEnumerator RunPlayMutiSound(string sfxName, float turn = 0, float duration = 0.1f)
        {
            for (int i = 0; i < turn; i++)
            {
                PlaySoundSfxReturn(sfxName);
                yield return new WaitForSeconds(duration);
            }
        }
    }

}
