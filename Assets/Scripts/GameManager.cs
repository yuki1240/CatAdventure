using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public StageCreater StageCreater;
    private PlayerManager playerSclipt;

    public GameObject ContentObj;
    public GameObject reTryPanelObj;
    public GameObject clearPanelObj;
    public GameObject almostPanelObj;
    public Text clearedStageNumber;
    public Button runButton;
    public Button refreshButton;
    public Image scrollView;

    Animation enemyDeathAnime;
    int clearedStageNum = 0;

    // セットされたコマンド画像を入れておく配列
    Image[] dropImageChild = new Image[20];

    // コマンドの情報を保持する配列
    [System.NonSerialized]
    public int CommandNumber = 20;

    [System.NonSerialized]
    public bool gameStopFlag = false;

    [System.NonSerialized]
    public bool gameClearFlag = false;

    // 宝箱まであと1マスだったら、true
    [System.NonSerialized]
    public bool isAlmostCollision = false;

    private void Start()
    {
        Sound.LoadSe("enemyDeathSE", "EnemyDeathSE");
        Sound.LoadSe("mistakeSE", "MistakeSE");
        clearedStageNumber.text = "クリア済みステージ：" + PlayerPrefs.GetInt("clearedStageNumber".ToString());
    }

    public void OnClickRunButton()
    {
        List<string> cmdList = new List<string>();

        playerSclipt = StageCreater.PlayerObj.GetComponent<PlayerManager>();

        // コマンドリストの初期化
        cmdList.Clear();

        Image dropImage;

        // Framesの子のFremeImage1～Nまでを取得
        for (var i = 0; i < CommandNumber; i++)
        {
            // FremeImage1～20まで子のDropImageを取得
            var frameImageName = $"FrameImage_{i + 1}";

            try
            {
                dropImage = ContentObj.transform.Find(frameImageName).GetChild(0).gameObject.GetComponent<Image>();
            }
            catch
            {
                break;
            }

            // 枠の中に画像がセットされているとき、その画像名を取得
            if (dropImage.sprite != null)
            {
                cmdList.Add(dropImage.sprite.name);
            }

        }

        if (cmdList.Count != 0)
        {
            // 実行ボタンを終わるまでグレーアウト
            runButton.interactable = false;
            refreshButton.interactable = false;
        }

        // 一連のコマンド情報をプレイヤー（猫）に渡す
        playerSclipt.ReceaveCmd(cmdList);
    }

    public IEnumerator DestoryEnemy(int enemyPosX, int enemyPosY)
    {
        GameObject enemy = StageCreater.enemyList[enemyPosY, enemyPosX];
        yield return new WaitForSeconds(0.3f);
        Sound.PlaySe("enemyDeathSE");
        yield return new WaitForSeconds(0.1f);

        // 消失アニメーションの再生
        enemy.transform.gameObject.GetComponent<Animator>().Play("EnemyRotate");
        Destroy(enemy.gameObject, 0.4f);
        // マップ情報から敵を削除する
        StageCreater.cells[enemyPosY, enemyPosX] = StageCreater.CellType.Empty;
    }

    public IEnumerator ShowReTryPanel()
    {
        yield return new WaitForSeconds(1.0f);
        reTryPanelObj.SetActive(true);
        Sound.PlaySe("mistakeSE");
    }

    public IEnumerator ShowAlmostPanel()
    {
        yield return new WaitForSeconds(0.5f);
        almostPanelObj.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        almostPanelObj.SetActive(false);
    }

    public IEnumerator ShowClearPanel()
    {
        yield return new WaitForSeconds(0.5f);
        clearPanelObj.SetActive(true);
        // 今のクリア済みステージ数に＋1した値を保存する
        localSave.saveData("clearedStageNumber", localSave.getIntData("clearedStageNumber") + 1);
        // PlayerPrefs.SetInt("clearedStageNumber", PlayerPrefs.GetInt("clearedStageNumber") + 1);
        // PlayerPrefs.Save();
    }

    public void OnClickReTryButton()
    {
        reTryPanelObj.SetActive(false);
        runButton.interactable = true;
        refreshButton.interactable = true;
        StageCreater.Retry();
    }

    public void OnClickRefleshButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnClickClearButton()
    {
        SceneManager.LoadScene("Main");
    }
}
