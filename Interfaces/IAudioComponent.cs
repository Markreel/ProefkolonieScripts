using UnityEngine;

public interface IAudioComponent
{
    AudioSource Source { get; }
    AudioClip[] AudioClips { get; }
    AudioLowPassFilter LowPassFilter { get; }

    bool PlayOnAwake { get; }
    bool Loop { get; }
    bool IgnorePause { get; }

    void SetVolume(float _value);
    void SetLowPassCutoffFrequency(float _value);
}
