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
    public GameObject enemyPrefab;
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
        clearedStageNumber.text = "クリア済みステージ：" + StringWidthConverter.ConvertToFullWidth(PlayerPrefs.GetInt("clearedStageNumber").ToString());
        
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

    // 一歩先が宝箱かどうかを返す関数
    public bool JuweryBoxCheck(Vector3 _currentPos, string _currentDirection)
    {
        Vector2 direction = Vector2.zero;
        float distance = 1.0f;

        switch (_currentDirection)
        {
            case "front":
                direction = Vector3.up;
                break;
            case "right":
                direction = Vector3.right;
                break;
            case "left":
                direction = Vector3.left;
                break;
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(_currentPos, direction, distance);

        if (hitInfo.collider == null)
        {
            return false;
        }
        return hitInfo.transform.tag == "JuweryBox";
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

    // _attackFlag = trueのとき、最後のコマンドで敵を倒したとき
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
        PlayerPrefs.SetInt("clearedStageNumber", PlayerPrefs.GetInt("clearedStageNumber") + 1);
        PlayerPrefs.Save();
    }

    public void OnClickReTryButton()
    {
        reTryPanelObj.SetActive(false);
        runButton.interactable = true;
        refreshButton.interactable = true;
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



    // 半角 ←→ 全角変換
    public class StringWidthConverter : MonoBehaviour
    {
        const int ConvertionConstant = 65248;

        static public string ConvertToFullWidth(string halfWidthStr)
        {
            string fullWidthStr = null;

            for (int i = 0; i < halfWidthStr.Length; i++)
            {
                fullWidthStr += (char)(halfWidthStr[i] + ConvertionConstant);
            }

            return fullWidthStr;
        }

        static public string ConvertToHalfWidth(string fullWidthStr)
        {
            string halfWidthStr = null;

            for (int i = 0; i < fullWidthStr.Length; i++)
            {
                halfWidthStr += (char)(fullWidthStr[i] - ConvertionConstant);
            }

            return halfWidthStr;
        }
    }
}
