using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAudio : DefaultAudioScript
{
    private void Update()
    {
        LerpVolume(0.1f, 0.5f, GameManager.Instance.CameraController.NormalizedZoomValue);
    }
}
