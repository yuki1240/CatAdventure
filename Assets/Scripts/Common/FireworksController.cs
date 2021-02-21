using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksController : MonoBehaviour
{

    public bool isKaruta = false;
    public bool isJanken = false;

    public void RondomPositon()
    {
        if (isKaruta)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(-600f, 600f), UnityEngine.Random.Range(-600f, 450f), 0.0f);
            this.transform.localPosition = pos;
        }
        if (isJanken)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(-450f, 450f), UnityEngine.Random.Range(-250f, 500f), 0.0f);
            this.transform.localPosition = pos;
        }
    }
}
