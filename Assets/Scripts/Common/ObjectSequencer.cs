using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSequencer : MonoBehaviour
{
    public Animator beforeAnimator;

    public GameObject afterShowObj;

    private void Start()
    {
        beforeAnimator.enabled = true;
        afterShowObj.SetActive(false);
    }

    public void StartAfterShowObj()
    {
        if (AudioSwitchController.volumeFlg == false)
        {
            Debug.Log("showAleat");
            beforeAnimator.enabled = false;
            afterShowObj.SetActive(true);
        }
    }
}
