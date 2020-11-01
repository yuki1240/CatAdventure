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
    public GameObject runButtonObj;
    public Button runButton;
    GameObject enemyPrefab;
    Animation enemyDeathAnime;

    // セットされたコマンド画像を入れておく配列
    Image[] dropImageChild = new Image[20];

    // コマンドの情報を保持する配列
    // [System.NonSerialized]
    

    public int CommandNumber = 20;

    [System.NonSerialized]
    public bool gameStopFlag = false;
    [System.NonSerialized]
    public bool gameClearFlag = false;

    // 宝箱まであと1マスだったら、true
    [System.NonSerialized]
    public bool isAlmostCollision = false;

    // 音源
    AudioSource audioSource;
    public AudioClip mistakeSE;
    public AudioClip clearSE;
    public AudioClip enemyDeathSE;
    public AudioClip attackSE;
    public AudioClip actionSE;

    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        enemyPrefab = GameObject.FindWithTag("Enemy").GetComponent<GameObject>();
        runButton = runButtonObj.GetComponent<Button>();
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
        }

        // 一連のコマンド情報を猫に渡す
        playerSclipt.ReceaveCmd(cmdList);
    }

    // 進もうとしているマスに、オブジェクトがあるかどうかをチェック
    public bool CollisionCheck(RaycastHit2D _hitInfo, string _command)
    {
        if (_hitInfo.collider == null)
        {
            return false;
        }

        if (_command == "attack")
        {
            return _hitInfo.transform.tag == "Enemy";
        }
        return _hitInfo.transform.tag == "Block" || _hitInfo.transform.tag == "Wall" || _hitInfo.transform.tag == "Enemy" || _hitInfo.transform.tag == "Player";
    }

    public void CallSound(String _soundName)
    {
        switch (_soundName)
        {
            case "mistakeSE":
                audioSource.PlayOneShot(mistakeSE);
                break;
            case "attackSE":
                audioSource.PlayOneShot(attackSE);
                break;
            case "enemyDeathSE":
                audioSource.PlayOneShot(enemyDeathSE);
                break;
            case "actionSE":
                audioSource.PlayOneShot(actionSE);
                break;
            case "clearSE":
                audioSource.PlayOneShot(clearSE);
                break;
        }
    }

    public IEnumerator DestoryEnemy(RaycastHit2D _hitInfo)
    {
        yield return new WaitForSeconds(0.4f);
        audioSource.PlayOneShot(enemyDeathSE);
        _hitInfo.transform.gameObject.GetComponent<Animator>().Play("EnemyRotate");
        Destroy(_hitInfo.transform.gameObject, 0.4f);
    }

    // flag = trueのとき、最後のコマンドで敵を倒したとき
    public IEnumerator ShowReTryPanel(bool _attackFlag)
    {
        if (_attackFlag)
        {
            yield return new WaitForSeconds(1.5f);
            reTryPanelObj.SetActive(true);
            audioSource.PlayOneShot(mistakeSE);
        } 
        else
        {
            yield return new WaitForSeconds(1.0f);
            reTryPanelObj.SetActive(true);
            audioSource.PlayOneShot(mistakeSE);
        }
    }

    //// Almostパネルを表示するかのチェック（各方向の確認）
    //public void ShowDisplayCheck(string _currentDirection)
    //{
    //    print("playerSclipt.JuweryBoxCheck() : " + playerSclipt.JuweryBoxCheck());
    //    if (playerSclipt.JuweryBoxCheck())
    //    {
    //        gameStopFlag = true;
    //        StartCoroutine(DisplayAlmostPanel());
    //    }
    //}

    //// Almostパネルを表示するかのチェック（1マス先が宝箱だったら、2秒間表示する）
    //IEnumerator DisplayAlmostPanel()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    almostPanel.SetActive(true);
    //    yield return new WaitForSeconds(2.0f);
    //    almostPanel.SetActive(false);
    //    // StageCreater.CreateMapObjects();
    //}

    
    public IEnumerator ShowClearPanel()
    {
        yield return new WaitForSeconds(0.5f);
        clearPanelObj.SetActive(true);
    }

    public void OnClickReTryButton()
    {
        reTryPanelObj.SetActive(false);

        // 実行ボタンを再表示
        runButton.interactable = true;

        // 初期化処理
        StageCreater.CreateMapObjects();
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
