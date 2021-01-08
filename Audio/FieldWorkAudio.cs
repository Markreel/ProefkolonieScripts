using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldWorkAudio : DefaultAudioScript
{
    private void Update()
    {
        LerpVolume(0.5f, 0f, GameManager.Instance.CameraController.NormalizedZoomValue);
    }
}
