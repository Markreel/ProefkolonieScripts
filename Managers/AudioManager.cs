using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType { Ambience, Music, SFX }

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource ambienceSource;
    [SerializeField] float ambienceVolume = 1;
    [SerializeField] AudioSource musicSource;
    [SerializeField] float musicVolume = 1;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] float sfxVolume = 1;

    [Header("Ambience Settings: ")]
    [SerializeField] AudioClip ambienceClipDay;
    [SerializeField] AudioClip ambienceClipNight;
    [SerializeField] AudioClip windClip;
    [SerializeField] float fadeAmbienceDuration = 1.5f;
    private float defaultAmbienceVolume;

    [Header("Music Settings: ")]
    [SerializeField] AudioClip defaultMusicClip;
    [SerializeField] AudioClip creditsMusicClip;
    [SerializeField] AudioClip rollCreditsMusicClip;
    [SerializeField] float fadeMusicDuration = 1.5f;
    private float defaultMusicVolume;

    private Coroutine fadeAmbienceRoutine;
    private Coroutine fadeMusicRoutine;

    private void Awake()
    {
        //ambienceSource.ignoreListenerPause = true;
        ambienceSource.loop = true;

        //musicSource.ignoreListenerPause = true;
        musicSource.loop = true;

        ambienceSource.volume = ambienceVolume;
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;

        defaultAmbienceVolume = musicSource.volume;
        defaultMusicVolume = musicSource.volume;

        PlayMusicClip(defaultMusicClip);
        PlayAmbienceClip(ambienceClipDay);
    }

    private void FadeAmbience(AudioClip _newClip = null)
    {
        if (fadeAmbienceRoutine != null) StopCoroutine(fadeAmbienceRoutine);
        fadeAmbienceRoutine = StartCoroutine(IEFadeAudioSource(_newClip, ambienceSource, fadeAmbienceDuration));
    }

    public void FadeMusic(AudioClip _newClip = null)
    {
        if (fadeMusicRoutine != null) StopCoroutine(fadeMusicRoutine);
        fadeMusicRoutine = StartCoroutine(IEFadeAudioSource(_newClip, musicSource, fadeMusicDuration));
    }

    private IEnumerator IEFadeAudioSource(AudioClip _newClip, AudioSource _source, float _duration)
    {
        float _startVolume = _source.volume;
        AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        float _lerpTime = 0;
        while (_lerpTime < 1)
        {
            _lerpTime += Time.unscaledDeltaTime / _duration;
            float _lerpKey = _curve.Evaluate(_lerpTime);

            _source.volume = Mathf.Lerp(_startVolume, 0, _lerpKey);
            yield return null;
        }

        _source.Stop();

        if (_newClip != null)
        {
            _source.clip = _newClip;
            _source.Play();
            _lerpTime = 0;

            while (_lerpTime < 1)
            {
                _lerpTime += Time.deltaTime / _duration;
                float _lerpKey = _curve.Evaluate(_lerpTime);

                _source.volume = Mathf.Lerp(0, defaultMusicVolume, _lerpKey);
                yield return null;
            }
        }
        else { _source.volume = defaultMusicVolume; }

        yield return null;
    }

    public void PlayAmbienceClip(AudioClip _clip)
    {
        if (ambienceSource.isPlaying) { FadeAmbience(_clip); }
        else
        {
            ambienceSource.clip = _clip;
            ambienceSource.Play();
        }
    }

    public void PlayMusicClip(AudioClip _clip)
    {
        if (musicSource.isPlaying) { FadeMusic(_clip); }
        else
        {
            musicSource.clip = _clip;
            musicSource.Play();
        }
    }

    public void PlayClip(AudioClip _clip)
    {
        sfxSource.PlayOneShot(_clip);
    }

    public void PlayClip(AudioClip _clip, float _volume)
    {
        sfxSource.PlayOneShot(_clip, _volume);
    }

    public void PlayRandomClip(AudioClip[] _clips)
    {
        sfxSource.PlayOneShot(_clips[Random.Range(0, _clips.Length)]);
    }

    public void PlayCreditsMusic()
    {
        musicSource.Stop();
        musicSource.ignoreListenerPause = true;

        PlayMusicClip(creditsMusicClip);
    }

    public void PlayRollCreditsMusic()
    {
        PlayMusicClip(rollCreditsMusicClip);
    }

    public void RestartMusic()
    {
        musicSource.Stop();
        musicSource.Play();
    }
}
