using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Text text;

	public void OnClick(int number)
    {
        switch (number)
        {
            case 0:
                text.text = "A";
                break;
            case 1:
                text.text = "B";
                break;
            default:
                break;
        }
    }
}