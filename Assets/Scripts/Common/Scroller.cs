using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    public bool moveUp;
    public bool moveDown;
    public bool moveRight;
    public bool moveLeft;
    public bool isLoop;

    public float rangeMax;
    public float rangeMin;
    public float speed;

    Vector3 currentPos;

    void Update()
    {
        currentPos = this.transform.localPosition;
       
        // 上下のスクロール
        if (moveUp)
        {
            var pos = currentPos;
            pos.y += speed * Time.deltaTime;
            this.transform.localPosition = pos;

            if (rangeMax < currentPos.y)
            {
                var to = currentPos;
                to.y = rangeMin;
                currentPos = to;
            }

            if (rangeMin > currentPos.y)
            {
                var to = currentPos;
                to.y = rangeMax;
                currentPos = to;
            }
        }
        else if (moveDown)
        {
            var pos = currentPos;
            pos.y -= speed * Time.deltaTime;
            this.transform.localPosition = pos;

            if (rangeMax > currentPos.y)
            {
                var to = currentPos;
                to.y = rangeMin;
                currentPos = to;
            }

            if (rangeMin < currentPos.y)
            {
                var to = currentPos;
                to.y = rangeMax;
                currentPos = to;
            }
        }

        // 左右のスクロール
        if (moveRight)
        {
            var pos = currentPos;
            pos.x += speed * Time.deltaTime;
            this.transform.localPosition = pos;

            if (rangeMax < currentPos.x)
            {
                print("Max");
                var to = currentPos;
                to.x = rangeMin;
                currentPos = to;
            }

            if (rangeMin > currentPos.x)
            {
                print("Min");
                var to = currentPos;
                to.x = rangeMax;
                currentPos = to;
            }
        }
        else if(moveLeft)
        {
            var pos = currentPos;
            pos.x -= speed * Time.deltaTime;
            this.transform.localPosition = pos;

            if (rangeMax > currentPos.x)
            {
                var to = currentPos;
                to.x = rangeMin;
                currentPos = to;
            }

            if (rangeMin < currentPos.x)
            {
                var to = currentPos;
                to.x = rangeMax;
                currentPos = to;
            }
        }
    }
}