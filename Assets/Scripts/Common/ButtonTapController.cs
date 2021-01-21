using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTapController : MonoBehaviour
{
    public Sprite defaultImage;
    public Sprite pushImage;
    public Image imageObj;

    public bool tapUI;
    public bool tapLogo;
    private bool tapChangedImage;

    private void Start()
    {
        if (tapUI)
        {
            Sound.LoadSe("buttonSe", "Common/Clicks_13");
        }
        else if (tapLogo)
        {
            Sound.LoadSe("logoSe", "Common/Coin Award 24_trim");
        }
        if (pushImage != null)
        {
            tapChangedImage = true;
        }
    }

    public void PushButton()
    {
        if (tapUI && AudioSwitchController.volumeFlg)
        {
            Sound.PlaySe("buttonSe", 0);
        }
        else if (tapLogo && AudioSwitchController.volumeFlg)
        {
            Sound.PlaySe("logoSe", 0);
        }

        if (tapChangedImage)
        {
            imageObj.sprite = pushImage;
        }
    }

    public void NonPushButton()
    {
        if (tapChangedImage)
        {
            imageObj.sprite = defaultImage;
        }
    }
}
