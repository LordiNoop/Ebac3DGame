using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MuteAudio : MonoBehaviour
{
    public void MuteGameAudio()
    {
        AudioListener.pause = !AudioListener.pause;
    }
}
