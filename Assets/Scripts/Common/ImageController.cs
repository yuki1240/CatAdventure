using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    public bool scrollMoveRight;
    public bool scrollMoveLeft;
    public float scrollRangeMax;
    public float scrollRangeMin;
    public float scrollSpeed;
    public Image scrollImage;

    void Update()
    {
        Vector3 currentPos = this.transform.localPosition;
       
        if (scrollMoveRight)
        {
            var pos = scrollImage.transform.localPosition;
            pos.x += scrollSpeed * Time.deltaTime;
            scrollImage.transform.localPosition = pos;

            if (scrollRangeMax < scrollImage.transform.localPosition.x)
            {
                var to = scrollImage.transform.localPosition;
                to.x = scrollRangeMin;
                scrollImage.transform.localPosition = to;
            }

            if (scrollRangeMin > scrollImage.transform.localPosition.x)
            {
                var to = scrollImage.transform.localPosition;
                to.x = scrollRangeMax;
                scrollImage.transform.localPosition = to;
            }
        }
        else if(scrollMoveLeft)
        {
            var pos = scrollImage.transform.localPosition;
            pos.x -= scrollSpeed * Time.deltaTime;
            scrollImage.transform.localPosition = pos;

            if (scrollRangeMax > scrollImage.transform.localPosition.x)
            {
                var to = scrollImage.transform.localPosition;
                to.x = scrollRangeMin;
                scrollImage.transform.localPosition = to;
            }

            if (scrollRangeMin < scrollImage.transform.localPosition.x)
            {
                var to = scrollImage.transform.localPosition;
                to.x = scrollRangeMax;
                scrollImage.transform.localPosition = to;
            }
        }
    }
}