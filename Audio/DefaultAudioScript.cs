using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource),typeof(AudioLowPassFilter))]
public class DefaultAudioScript : MonoBehaviour
{
    [SerializeField] private bool playOnAwake;
    [SerializeField] private bool loop;
    [SerializeField] private bool ignorePause;

    [SerializeField] private AudioClip[] audioClips;

    private AudioSource source;
    private AudioLowPassFilter lowPassFilter;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        if(source == null) { source = gameObject.AddComponent<AudioSource>(); }

        lowPassFilter = GetComponent<AudioLowPassFilter>();
        if (lowPassFilter == null) { lowPassFilter = gameObject.AddComponent<AudioLowPassFilter>(); }

        source.loop = loop;
        source.ignoreListenerPause = ignorePause;

        if (playOnAwake && audioClips.Length != 0)
        {
            foreach (var _clip in audioClips)
            {
                if(_clip != null) { PlayClip(_clip); break; }
            }
        }
    }

    private void PlayClip(AudioClip _clip)
    {
        source.clip = _clip;
        source.Play();
    }

    public void LerpLowPassCutoffFrequency(float _a, float _b, float _t)
    {
        lowPassFilter.cutoffFrequency = Mathf.Lerp(_a, _b, _t);
    }

    public void LerpVolume(float _a, float _b, float _t)
    {
        source.volume = Mathf.Lerp(_a, _b, _t);
    }
}
