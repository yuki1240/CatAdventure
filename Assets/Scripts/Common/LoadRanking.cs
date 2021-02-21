using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadRanking : MonoBehaviour
{
    List<string> nameList = new List<string>();
    List<int> pointList = new List<int>();
    public TextMeshProUGUI[] names = new TextMeshProUGUI[5];
    public TextMeshProUGUI[] points = new TextMeshProUGUI[5];

    // Start is called before the first frame update
    void Start()
    {
        LoadSaveData();
    }

    void LoadSaveData()
    {
        // リストの初期化
        nameList.Clear();
        pointList.Clear();

        // データのロード
        nameList = localSave.getStringListData("Karuta");
        pointList = localSave.getIntListData("Karuta");

        // Unity内のTextに反映
        for (int i = 0; i < nameList.Count; i++)
        {
            names[i].text = nameList[i] + "：";
            points[i].text = (pointList[i]).ToString();
        }
    }
}
