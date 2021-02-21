using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioSwitchController : MonoBehaviour
{
    public Sprite onImage;
    public Sprite onTapImage;
    public Sprite offImage;
    public Sprite offTapImage;

    public Image volumeObj;
    public GameObject audioAlert;

    // BGMがオンのとき = true
    public static bool volumeFlg = true;

    public string bgmName = "";
    public string resName = "";

    public bool alertEnable;
    public bool playBgm;

    public float volume = 1.0f;

    void Awake()
    {
        if (localSave.getIntData("Mute") == 0)
        {
            volumeFlg = false;
        }

        Sound.LoadBgm(bgmName, resName);
        Sound.StopBgm();

        // BGMがオフの状態だったら
        if (localSave.getIntData("Mute") == 0)
        {
            if (alertEnable)
            {
                audioAlert.SetActive(true);
            }
            if (playBgm)
            {
                Sound.StopBgm();
            }
            volumeObj.sprite = offImage;
            volumeFlg = false;
        }
        // BGMがオン
        else
        {
            if (alertEnable)
            {
                audioAlert.SetActive(false);
            }
            if (playBgm)
            {
                Sound.bgmVolume = volume;
                Sound.PlayBgm(bgmName);
            }
            volumeObj.sprite = onImage;
            volumeFlg = true;
        }
    }

    public void OnClick()
    {
        // BGMがオフの状態だったら
        if (localSave.getIntData("Mute") == 0)
        {
            // key = Muteに1を代入する（1 = オン）
            localSave.saveData("Mute", 1);
            if (alertEnable)
            {
                audioAlert.SetActive(false);
            }
            if (playBgm)
            {
                Sound.bgmVolume = volume;
                Sound.PlayBgm(bgmName);
            }
            volumeObj.sprite = onImage;
            volumeFlg = true;
        }
        // BGMがオン
        else
        {
            // key = Muteに0を代入する（0 = オフ）
            localSave.saveData("Mute", 0);
            if (alertEnable)
            {
                audioAlert.SetActive(true);
            }
            if (playBgm)
            {
                Sound.StopBgm();
            }
            volumeObj.sprite = offImage;
            volumeFlg = false;
        }
    }

    public void closeAudioAlert()
    {
        audioAlert.SetActive(false);
    }

    public void onTap()
    {
        if (volumeFlg == true)
        {
            volumeObj.sprite = onTapImage;
        }
        else
        {
            volumeObj.sprite = offTapImage;
        }
    }

    public void offTap()
    {
        if (volumeFlg == true)
        {
            volumeObj.sprite = onImage;
        }
        else
        {
            volumeObj.sprite = offImage;
        }
    }

    public void SetState()
    {
        if (volumeFlg == true)
        {
            Sound.PlayBgm(bgmName);
        }
    }

    public void StopBgm()
    {
        // BGMがオンだったら
        if (localSave.getIntData("Mute") == 1)
        {
            Sound.StopBgm();
            volumeFlg = true;
        }
        else 
        {
            volumeFlg = false;
        }
    }
}