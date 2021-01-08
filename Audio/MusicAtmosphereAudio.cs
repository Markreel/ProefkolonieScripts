using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAtmosphereAudio : DefaultAudioScript
{
    private void Update()
    {
        LerpLowPassCutoffFrequency(22000f,1870f, GameManager.Instance.CameraController.NormalizedZoomValue);
    }
}
