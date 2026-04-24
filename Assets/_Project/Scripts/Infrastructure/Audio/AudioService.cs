using System.Collections;
using _Project.Scripts.Configs;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Infrastructure.Audio
{
    public class AudioService : MonoBehaviour
    {
        private static AudioService _instance;

        private AudioConfig _config;
        private AudioSource _musicSource;
        private AudioSource _sfx2DSource;
        private Coroutine _musicTransitionCoroutine;

        private bool _muted;
        private float _musicLogicalVolume = 1f;
        
        public bool Muted { get => _muted; set => _muted = value; }
        
        
        [Inject]
        public void Construct(AudioConfig config) => _config = config;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            _musicSource = CreateSource("MusicSource", loop: true);
            _sfx2DSource = CreateSource("Sfx2DSource", loop: false);
        }

        public void SetMuted(bool muted)
        {
            Muted = muted;
            
            _musicSource.mute = muted;
            _sfx2DSource.mute = muted;
            
            ApplyMusicVolume();
        }
        
        private void ApplyMusicVolume()
        {
            _musicSource.volume = Muted ? 0f : _musicLogicalVolume;
        }

        public void PlayMusic(SoundId id, bool loop = true, float fadeInSeconds = 0.5f, float fadeOutSeconds = 0.5f)
        {
            var entries = _config.GetAll(id);

            if (entries.Count == 0)
            {
                Debug.LogWarning("No sound for " + id);
                return;
            }

            var startIndex = Random.Range(0, entries.Count);

            if (_musicTransitionCoroutine != null)
                StopCoroutine(_musicTransitionCoroutine);

            _musicTransitionCoroutine = StartCoroutine(PlayMusicRoutine(entries, startIndex, loop, fadeOutSeconds, fadeInSeconds));
        }

        public void StopMusic(float fadeOutSeconds = 0.5f)
        {
            if (_musicTransitionCoroutine != null)
                StopCoroutine(_musicTransitionCoroutine);

            _musicTransitionCoroutine = StartCoroutine(StopMusicRoutine(fadeOutSeconds));
        }

        public void PlaySfx(SoundId id)
        {
            if (Muted) 
                return;
            
            var entry = _config.Get(id);

            _sfx2DSource.pitch = entry.Pitch + Random.Range(-0.25f, 0.25f);
            _sfx2DSource.PlayOneShot(entry.Clip, entry.Volume);
        }

        public void PlaySfxAt(SoundId id, Vector3 worldPos, float volume = 1f)
        {
            if (Muted) 
                return;
            
            var entry = _config.Get(id);

            AudioSource.PlayClipAtPoint(entry.Clip, worldPos, volume * entry.Volume);
        }

        private IEnumerator PlayMusicRoutine(
            System.Collections.Generic.List<AudioConfig.Entry> entries,
            int currentIndex,
            bool loop,
            float fadeOutSeconds,
            float fadeInSeconds)
        {
            if (_musicSource.isPlaying)
                yield return FadeMusicRoutine(0f, fadeOutSeconds);

            while (true)
            {
                var entry = entries[currentIndex];

                _musicSource.Stop();
                _musicSource.clip = entry.Clip;
                _musicSource.loop = false;
                _musicSource.pitch = entry.Pitch;
                _musicSource.volume = 0f;
                _musicSource.Play();

                yield return FadeMusicRoutine(entry.Volume, fadeInSeconds);
                yield return new WaitWhile(() => _musicSource.isPlaying);

                if (!loop)
                    break;

                currentIndex = GetRandomIndexExcluding(entries.Count, currentIndex);
            }

            _musicTransitionCoroutine = null;
        }

        private IEnumerator StopMusicRoutine(float fadeOutSeconds)
        {
            if (_musicSource.isPlaying)
                yield return FadeMusicRoutine(0f, fadeOutSeconds);

            _musicSource.Stop();
            _musicSource.clip = null;

            _musicTransitionCoroutine = null;
        }

        private IEnumerator FadeMusicRoutine(float targetVolume, float duration)
        {
            float startVolume = _musicSource.volume;
            float elapsed = 0f;

            if (duration <= 0f)
            {
                _musicSource.volume = targetVolume;
                ApplyMusicVolume();
                yield break;
            }

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                _musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
                ApplyMusicVolume();
                yield return null;
            }

            _musicSource.volume = targetVolume;
            ApplyMusicVolume();
        }

        private AudioSource CreateSource(string name, bool loop)
        {
            var go = new GameObject(name);
            go.transform.SetParent(transform, false);
            var source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = loop;
            source.spatialBlend = 0f;
            return source;
        }
        
        private int GetRandomIndexExcluding(int count, int excludedIndex)
        {
            if (count <= 1)
                return excludedIndex;

            int index;
            
            do
                index = Random.Range(0, count);
            while (index == excludedIndex);

            return index;
        }
    }
}