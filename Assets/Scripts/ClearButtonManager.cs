using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearButtonManager : MonoBehaviour
{
    public Image dropImage;

    public void Remove()
    {
        dropImage.sprite = null;
        dropImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
}
