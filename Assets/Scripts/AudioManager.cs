using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public string bgmName = "";
    public string resName = "";

    public float volume = 1.0f;

    void Awake()
    {
        Sound.LoadBgm(bgmName, resName);
        Sound.StopBgm();

        Sound.bgmVolume = volume;
        Sound.PlayBgm(bgmName);
    }
}