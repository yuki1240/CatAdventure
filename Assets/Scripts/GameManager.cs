using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject Content;
    public GameObject runButton;
    public GameObject reTryPanel;
    public GameObject clearPanel;
    public GameObject almostPanel;

    GameObject enemyPrefab;
    Animation enemyDeathAnime;

    public bool gameStopFlag = false;
    public bool gameClearFlag = false;

    // 音源
    AudioSource audioSource;
    public AudioClip mistakeSE;
    public AudioClip clearSE;
    public AudioClip enemyDeathSE;
    public AudioClip attackSE;
    public AudioClip actionSE;

    public StageCreater StageCreater;

    private PlayerManager playerSclipt;

    // セットされたコマンド画像を入れておく配列
    Image[] dropImageChild = new Image[20];

    // コマンドの情報を保持する配列
    [System.NonSerialized]
    public List<string> cmdList = new List<string>();

    public int CommandNumber = 20;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        enemyPrefab = GameObject.FindWithTag("Enemy").GetComponent<GameObject>();
        // enemyDeathAnime = enemyPrefab.GetComponent<Animation>();
    }

    public void OnClickRunButton()
    {
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
                dropImage = Content.transform.Find(frameImageName).GetChild(0).gameObject.GetComponent<Image>();
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
        // 一連のコマンド情報を猫に渡す
        playerSclipt.ReceaveCmd(cmdList, reTryPanel, clearPanel, almostPanel);
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

        return _hitInfo.transform.tag == "Block" || _hitInfo.transform.tag == "Wall" || _hitInfo.transform.tag == "Enemy";
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
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(enemyDeathSE);
        _hitInfo.transform.gameObject.GetComponent<Animator>().Play("EnemyRotate");
        Destroy(_hitInfo.transform.gameObject, 1.0f);
    }

    // flag = trueのとき、最後のコマンドで敵を倒したとき
    public IEnumerator ShowReTryPanel(bool _attackFlag)
    {
        if (_attackFlag)
        {
            print("wait 1.5f");
            yield return new WaitForSeconds(1.5f);
            reTryPanel.SetActive(true);
            audioSource.PlayOneShot(mistakeSE);
        } 
        else
        {
            print("wait 1.0f");
            yield return new WaitForSeconds(1.0f);
            reTryPanel.SetActive(true);
            audioSource.PlayOneShot(mistakeSE);
        }
    }

    public void ShowClearPanel()
    {
        clearPanel.SetActive(true);
    }

    public void OnClickReTryButton()
    {
        reTryPanel.SetActive(false);
        
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
