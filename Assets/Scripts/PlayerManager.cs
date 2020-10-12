﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float speed = 1.0f;
    private Animator animator = null;
    private Rigidbody2D rb = null;

    // 一連のコマンド情報が入ったリスト
    List<string> cmdList = new List<string>();

    // ぶつかっているかのフラグ
    // bool boxCollisionFlag = false;

    // 今実行中のコマンド
    // string nowCommand = null;

    // 今どの方向を向いているか
    // string nowStatus = null;

    void Start()
    {
        animator = this.transform.GetComponent<Animator>();
        rb = this.transform.GetComponent<Rigidbody2D>();
        
    }

    void FixedUpdate()
    {
        // 現在の座標を取得
        Vector3 position = transform.position;

        // 方向転換するか？
        if (Input.GetKeyDown(KeyCode.W))
        {
            position += new Vector3(0.0f, 1.0f, 0.0f) * speed;
            animator.Play("Up", 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            position += new Vector3(0.0f, -1.0f, 0.0f) * speed;
            animator.Play("Down", 0);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            position += new Vector3(-1.0f, 0.0f, 0.0f) * speed;
            animator.Play("TrunLeft", 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            position += new Vector3(1.0f, 0.0f, 0.0f) * speed;
            animator.Play("TrunRight", 0);
        }


        // 移動（キー操作受付部分）
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            position += new Vector3(0.0f, 1.0f, 0.0f) * speed;
            animator.Play("Up", 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            position += new Vector3(0.0f, -1.0f, 0.0f) * speed;
            animator.Play("Down", 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            position += new Vector3(-1.0f, 0.0f, 0.0f) * speed;
            animator.Play("TrunLeft", 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            position += new Vector3(1.0f, 0.0f, 0.0f) * speed;
            animator.Play("TrunRight", 0);
        }

        transform.position = position;
    }

    IEnumerator PlayerMove()
    {
        for (int i = 0; i < 4; i++)
        {
            string nowCmd = cmdList[i];
            if (nowCmd == "Attack")
            {
                
            }
            else if (nowCmd == "Walk")
            {

            }
            else if (nowCmd == "TrunRight")
            {
                animator.Play("TrunRight", 0);


            }
            else if (nowCmd == "TrunLeft")
            {
                animator.Play("TrunLeft", 0);
            }

            // ここで一旦待つ
            yield return new WaitForSeconds(1.0f);
        }
    }

        private void OnCollisionEnter2D(Collision2D collision)
    {
        // 宝箱にぶつかっていたら
        if (collision.transform.tag == "box")
        {
            // BoxCollisionFlag = true;
        }
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
        // print(cmdList.Count);

        // 受け取ったコマンド情報を元にプレイヤーを動かす
        StartCoroutine(PlayerMove());
    }

}
