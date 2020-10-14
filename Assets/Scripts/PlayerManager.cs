using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public float speed = 1.0f;

    SpriteRenderer playerImage;

    // 一連のコマンド情報が入ったリスト
    List<string> cmdList = new List<string>();

    private Animator animator = null;
    private Rigidbody2D rb = null;

    void Start()
    {
        animator = this.transform.GetComponent<Animator>();
        rb = this.transform.GetComponent<Rigidbody2D>();
        playerImage = this.transform.GetComponent<SpriteRenderer>();

    }

    void FixedUpdate()
    {
        //// 現在の座標を取得
        //Vector3 position = transform.position;

        //// 方向転換するか？
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    position += new Vector3(0.0f, 1.0f, 0.0f) * speed;
        //    animator.Play("Up", 0);
        //}
        //else if (Input.GetKeyDown(KeyCode.S))
        //{
        //    position += new Vector3(0.0f, -1.0f, 0.0f) * speed;
        //    animator.Play("Down", 0);
        //}
        //else if (Input.GetKeyDown(KeyCode.A))
        //{
        //    position += new Vector3(-1.0f, 0.0f, 0.0f) * speed;
        //    animator.Play("TrunLeft", 0);
        //}
        //else if (Input.GetKeyDown(KeyCode.D))
        //{
        //    position += new Vector3(1.0f, 0.0f, 0.0f) * speed;
        //    animator.Play("TrunRight", 0);
        //}


        //// 移動（キー操作受付部分）
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    position += new Vector3(0.0f, 1.0f, 0.0f) * speed;
        //    animator.Play("Up", 0);
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    position += new Vector3(0.0f, -1.0f, 0.0f) * speed;
        //    animator.Play("Down", 0);
        //}
        //else if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    position += new Vector3(-1.0f, 0.0f, 0.0f) * speed;
        //    animator.Play("TrunLeft", 0);
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    position += new Vector3(1.0f, 0.0f, 0.0f) * speed;
        //    animator.Play("TrunRight", 0);
        //}

        //transform.position = position;


        //if (flag)
        //{
        //    float present_Location = (Time.time) / 1.0f;
        //    transform.position = Vector3.Lerp(new Vector3(1.88f, 0.27f, 0.0f), new Vector3(0.0f, 0.27f, 0.0f), present_Location);
        //}
    }

    IEnumerator PlayerMove()
    {

        for (int i = 0; i < 4; i++)
        {
            string nowCmd = cmdList[i];
            string plaeyInfo = GetPlayerInfo();

            // 攻撃
            if (nowCmd == "Attack")
            {
                
            }
            else if (nowCmd == "Walk")
            {
                            }
            // 右回転
            else if (nowCmd == "TrunRight")
            {
                
            }
            // 左回転
            else if (nowCmd == "TrunLeft")
            {
                
            }

            // ここで一旦待つ
            yield return new WaitForSeconds(1.0f);
        }
    }

    string GetPlayerInfo()
    {
        // 前を向いているとき
        if (playerImage.sprite.name == "Player_10")
        {
            return "front";
        }
        // 後ろを向いているとき
        else if (playerImage.sprite.name == "Player_1")
        {
            return "back";
        }
        // 右を向いているとき
        else if (playerImage.sprite.name == "Player_4")
        {
            return "Right";
        }
        // 左を向いているとき
        else if (playerImage.sprite.name == "Player_7")
        {
            return "Left";
        }

        return "";
    }


    public void ReceaveCmd(List<string> _cmdList)
    {
        cmdList = _cmdList;

        // foreach (string cmd in cmdList)
        for (int i = 0; i < 4; i++)
        {
            // print(cmd);
            print(i+1 + "番目：" + cmdList[i]);
        }

        // 受け取ったコマンド情報を元にプレイヤーを動かす
        StartCoroutine(PlayerMove());
    }
}
