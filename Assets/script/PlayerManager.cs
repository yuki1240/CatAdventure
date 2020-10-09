using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float speed = 1.0f;
    private Animator animator = null;
    private Rigidbody2D rb = null;


    // ぶつかっているかのフラグ
    bool collisionFlag = false;

   // 
    string commandStatus = null;

    // 今どの方向を向いているか
    // string nowStatus = null;

    void Start()
    {
        animator = this.transform.GetComponent<Animator>();
        rb = this.transform.GetComponent<Rigidbody2D>();
        // rb.AddForce(new Vector2(0, 50));
    }

    void Update()
    {
        // 現在の座標を取得
        Vector3 position = transform.position;

        // 方向転換するか？

        // 移動（キー操作受付部分）
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            position += new Vector3(0f, 1f, 0f) * speed;
            animator.Play("Up", 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            position += new Vector3(0f, -1f, 0f) * speed;
            animator.Play("Down", 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            position += new Vector3(-1f, 0f, 0f) * speed;
            animator.Play("TrunLeft", 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            position += new Vector3(1f, 0f, 0f) * speed;
            animator.Play("TrunRight", 0);
        }

        transform.position = position;
    }

    void FixedUpdate()
    {
        if (commandStatus == "Atack")
        {

        }
        else if (commandStatus == "Walk")
        {
            Walk();
        }
        else if (commandStatus == "TrunRight")
        {

        }
        else if (commandStatus == "TrunLeft")
        {

        }
    }

    private void setStateToAnimator(Vector2? vector)
    {
        if (!vector.HasValue)
        {
            // this.animator.speed = 0.0f;
            return;
        }

        Debug.Log(vector.Value);
        // this.animator.speed = 1.0f;
        // this.animator.SetFloat("TrunRight", True);
        // this.animator.SetFloat("TrunLeft", vector.Value.y);

    }

    private Vector2? actionKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) return Vector2.up;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) return Vector2.left;
        if (Input.GetKeyDown(KeyCode.DownArrow)) return Vector2.down;
        if (Input.GetKeyDown(KeyCode.RightArrow)) return Vector2.right;
        return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 宝箱にぶつかっていたら
        if (collision.transform.tag == "box")
        {
            collisionFlag = true;
            TrunRight();
        }
    }

    void Walk()
    {
        if (!collisionFlag)
        {
            animator.SetInteger("Walk", 1);
            print("Go");
        }
        else
        {
            animator.SetInteger("Walk", 0);
            print("Stop");
        }
    }

    void Atack()
    {

    }

    void TrunRight()
    {
        animator.SetBool("TrunRight", true);
    }

    void TrunLeft()
    {

    }
}
