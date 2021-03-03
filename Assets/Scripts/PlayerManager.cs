using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // 各方向の猫画像
    public Sprite frontImage;
    public Sprite backImage;
    public Sprite rightImage;
    public Sprite leftImage;

    // 一連のコマンド情報が入ったリスト
    List<string> cmdList = new List<string>();

    // ゲームクリアフラグ
    bool clearFlag = false;

    // StageCreaterへの参照
    StageCreater StageCreater;

    // GamaManagerへの参照
    GameManager gameManager;

    // プレイヤーの方向
    string currentDirection = "front";

    // 今の状態のプレイヤー画像
    Image playerImage;

    // プレイヤーの位置情報
    Vector3 playerPosition = Vector3.zero;

    int playerCellX = 0;
    int playerCellY = 0;

    private readonly float CharacterMoveUnit = 120f;

    StageCreater.CellType cellType = StageCreater.CellType.Empty;

    void Start()
    {
        Sound.LoadSe("attackSE", "attackSE");
        Sound.LoadSe("actionSE", "actionSE");
        Sound.LoadSe("clearSE", "clearSE");
        Sound.LoadSe("mistakeSE", "mistakeSE");

        playerPosition = this.transform.localPosition;
        StageCreater = GameObject.FindWithTag("Wall").GetComponent<StageCreater>();
        playerImage = StageCreater.PlayerObj.GetComponent<Image>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // コマンド情報をGameManagerから受け取り、PlayerMove()へ渡して動かす
    public void ReceaveCmd(List<string> _cmdList)
    {
        cmdList = _cmdList;

        // 受け取ったコマンド情報を元に猫を動かす
        StartCoroutine(PlayerMove());
    }

    IEnumerator PlayerMove()
    {
        for (int i = 0; i < cmdList.Count; i++)
        {
            // 攻撃コマンド
            if (cmdList[i] == "Attack")
            {
                PlayerAttack();
            }

            // 1歩前に進む
            else if (cmdList[i] == "Walk1")
            {
                PlayerWalk();
                if (GetJuweryBoxCheck() == true)
                {
                    ShowClearPanel();
                    yield break;
                }
            }

            // 2歩前に進む
            else if (cmdList[i] == "Walk2")
            {
                PlayerWalk();
                
                yield return new WaitForSeconds(0.5f);

                // ここでtrueだったら、下のPlayerWork()は飛ばしたいんだけど、
                // IEnumeratorで普通にruturnできないからどうすればいい？
                if (GetJuweryBoxCheck() == true)
                {
                    
                    ShowClearPanel();
                    // ここでこの関数を抜けたい
                    yield break;    // これでCoroutineから抜けられるはず
                    // ぁ、そうなんだ！
                
                }

                PlayerWalk();
                
                if (GetJuweryBoxCheck() == true)
                {
                    ShowClearPanel();
                    // ここでこの関数を抜けたい
                    yield break;
                }

            }

            // 右回転
            else if (cmdList[i] == "TrunRight")
            {
                PlayerTrun(cmdList[i]);
            }

            // 左回転
            else if (cmdList[i] == "TrunLeft")
            {
                PlayerTrun(cmdList[i]);
            }

            // 他の処理が終わるのを待つ
            yield return new WaitForSeconds(0.1f);

            // 最後のコマンドのとき
            if (i == cmdList.Count - 1)
            {
                // 宝箱まであと1マスのとき
                if (GetFrontObject() == StageCreater.CellType.JuweryBox)
                {
                    StartCoroutine(gameManager.ShowAlmostPanel());
                    yield return new WaitForSeconds(1.0f);
                }
                StartCoroutine(gameManager.ShowReTryPanel());
                yield break;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    void PlayerAttack()
    {
        Sound.PlaySe("attackSE");

        // 目の前が敵だったとき
        if (GetFrontObject() == StageCreater.CellType.Enemy)
        {
            int enemyPosX = 0;
            int enemyPosY = 0;

            switch (GetCurrentDirection())
            {
                case "front":
                    enemyPosY = playerCellY + 1;
                    enemyPosX= playerCellX;
                    break;
                case "back":
                    enemyPosY = playerCellY - 1;
                    enemyPosX = playerCellX;
                    break;
                case "right":
                    enemyPosY = playerCellY;
                    enemyPosX = playerCellX + 1;
                    break;
                case "left":
                    enemyPosY = playerCellY;
                    enemyPosX = playerCellX - 1;
                    break;
            }

            // 敵オブジェの削除
            StartCoroutine(gameManager.DestoryEnemy(enemyPosX, enemyPosY));

            UpdateEnemyPosition();
        }
    }

    void PlayerWalk()
    {
        Sound.PlaySe("actionSE");

        if (GetFrontObject() == StageCreater.CellType.Empty || GetFrontObject() == StageCreater.CellType.JuweryBox)
        {
            if (GetCurrentDirection() == "front")
            {
                this.transform.localPosition += new Vector3(0f, CharacterMoveUnit, 0f);
            }
            else if (GetCurrentDirection() == "back")
            {
                this.transform.localPosition += new Vector3(0f, -CharacterMoveUnit, 0f);
            }
            else if (GetCurrentDirection() == "right")
            {
                this.transform.localPosition += new Vector3(CharacterMoveUnit, 0f, 0f);
            }
            else if (GetCurrentDirection() == "left")
            {
                this.transform.localPosition += new Vector3(-CharacterMoveUnit, 0f, 0f);
            }
            UpdatePlayerPosition();
            Debug.Log($"[Player位置] X:{playerCellX}, Y:{playerCellY}");
            Debug.Log($"[宝箱位置] X:{StageCreater.juweryBoxPos.x}, Y:{StageCreater.juweryBoxPos.y}");
        }
    }

    void PlayerTrun(string _commandDirection)
    {
        gameManager.gameStopFlag = false;
        Sound.PlaySe("actionSE");

        if (_commandDirection == "TrunRight")
        {
            switch (currentDirection)
            {
                case "front":
                    playerImage.sprite = rightImage;
                    break;

                case "back":
                    playerImage.sprite = leftImage;
                    break;

                case "right":
                    playerImage.sprite = backImage;
                    break;

                case "left":
                    playerImage.sprite = frontImage;
                    break;
            }
        }
        if (_commandDirection == "TrunLeft")
        {
            switch (currentDirection)
            {
                case "front":
                    playerImage.sprite = leftImage;
                    break;

                case "back":
                    playerImage.sprite = rightImage;
                    break;

                case "right":
                    playerImage.sprite = frontImage;
                    break;

                case "left":
                    playerImage.sprite = backImage;
                    break;
            }
        }
        currentDirection = GetCurrentDirection();
    }

    // プレイヤーがいる位置のセル情報を取得
    void GetPlayerPos()
    {
        for (int y = 0; y < StageCreater.mapHeight; y++)
        {
            for (int x = 0; x < StageCreater.mapWidth; x++)
            {
                if (StageCreater.cells[y, x] == StageCreater.CellType.Player)
                {
                    playerCellX = x;
                    playerCellY = y;
                    // Debug.Log($"[Player位置] X:{playerCellX}, Y:{playerCellY}");
                    break;
                }
            }
        }
    }

    // 前方のセル情報の取得
    StageCreater.CellType GetFrontObject()
    {
        GetPlayerPos();

        int mapH = StageCreater.mapHeight;
        int mapW = StageCreater.mapWidth;

        switch (GetCurrentDirection())
        {
            case "front":
                if (playerCellY + 1 >= mapH)
                {
                    return StageCreater.CellType.Block;
                }
                cellType = StageCreater.cells[playerCellY + 1, playerCellX];
                break;
            case "back":
                if (playerCellY - 1 <= -1)
                {
                    return StageCreater.CellType.Block;
                }
                cellType = StageCreater.cells[playerCellY - 1, playerCellX];
                break;
            case "right":
                if (playerCellX + 1 >= mapW)
                {
                    return StageCreater.CellType.Block;
                }
                cellType = StageCreater.cells[playerCellY, playerCellX + 1];
                break;
            case "left":
                if (playerCellX - 1 <= -1)
                {
                    return StageCreater.CellType.Block;
                }
                cellType = StageCreater.cells[playerCellY, playerCellX - 1];
                break;
        }

        switch(cellType)
        {
            case StageCreater.CellType.Empty:
                return StageCreater.CellType.Empty;
            case StageCreater.CellType.Enemy:
                return StageCreater.CellType.Enemy;
            case StageCreater.CellType.Block:
                return StageCreater.CellType.Block;
            case StageCreater.CellType.JuweryBox:
                return StageCreater.CellType.JuweryBox;
            case StageCreater.CellType.Player:
                return StageCreater.CellType.Player;
            default:
                return StageCreater.CellType.Empty;
        }
    }

    // プレイヤーの前方に障害物がないかをチェック
    public bool CollisionCheck(string _command)
    {
        if (GetFrontObject() != StageCreater.CellType.Empty)
        {
            return true;
        }    
        return false;
    }

    // プレイヤー位置の更新
    void UpdatePlayerPosition()
    {
        StageCreater.cells[playerCellY, playerCellX] = StageCreater.CellType.Empty;
       
        switch (GetCurrentDirection())
        {
            case "front":
                playerCellY += 1;
                break;
            case "back":
                playerCellY -= 1;
                break;
            case "right":
                playerCellX += 1;
                break;
            case "left":
                playerCellX -= 1;
                break;
        }

        StageCreater.cells[playerCellY, playerCellX] = StageCreater.CellType.Player;
    }

    void UpdateEnemyPosition()
    {
        switch (GetCurrentDirection())
        {
            case "front":
                StageCreater.cells[playerCellY + 1, playerCellX] = StageCreater.CellType.Empty;
                break;
            case "back":
                StageCreater.cells[playerCellY - 1, playerCellX] = StageCreater.CellType.Empty;
                break;
            case "right":
                StageCreater.cells[playerCellY, playerCellX + 1] = StageCreater.CellType.Empty;
                break;
            case "left":
                StageCreater.cells[playerCellY, playerCellX - 1] = StageCreater.CellType.Empty;
                break;
        }
    }

    // 今の猫の向きを取得
    string GetCurrentDirection()
    {
        // 前を向いているとき
        if (playerImage.sprite.name == frontImage.name)
        {
            return "front";
        }
        // 後ろを向いているとき
        else if (playerImage.sprite.name == backImage.name)
        {
            return "back";
        }
        // 右を向いているとき
        else if (playerImage.sprite.name == rightImage.name)
        {
            return "right";
        }
        // 左を向いているとき
        else if (playerImage.sprite.name == leftImage.name)
        {
            return "left";
        }
        return "";
    }

    // 宝箱にたどり着いているかの確認
    private bool GetJuweryBoxCheck()
    {
        if (playerCellX == StageCreater.juweryBoxPos.x && playerCellY == StageCreater.juweryBoxPos.y)
        {
            return true;
        }
        return false;
    }

    public void ShowClearPanel()
    {
        Sound.PlaySe("clearSE");
        StartCoroutine(gameManager.ShowClearPanel());
    }
}