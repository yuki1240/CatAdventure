using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using DG.Tweening;
using System;
using System.Text;
using System.Diagnostics;

public class NameCheack : MonoBehaviour
{
    public TMP_InputField nameFiled;
    public GameObject errorMessage;
    public GameObject lengthErrorMessage;
    //public AtodashiJankenResultManager atodashiJankenResultManager;
    //public KarutaGameManager karutaGameManager;
    //public NurieMain nurieMain;

    public bool snsFlg = false;

    // すべてひらがなであるかを判別
    private bool IsNameCheack(string text)
    {
        var isJapanese = Regex.IsMatch(text, @"^\p{IsHiragana}+$");
        return isJapanese;
    }

    // 文字の長さをバイトで判定
    public static int CountCheack(string text)
    {
        Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
        int num = sjisEnc.GetByteCount(text);
        return num;
    }

    public void Cheak(string s)
    {
        if (IsNameCheack(nameFiled.text))
        {
            var count = nameFiled.text.Length;

            if (count <= 8)
            {
                print("正常に登録できました");
                //if (atodashiJankenResultManager != null)
                //{
                //    atodashiJankenResultManager.PushSaveButton();
                //}
                //else if (karutaGameManager != null)
                //{
                //    karutaGameManager.PushSaveButton();
                //}
                //else if (nurieMain != null && s == "save")
                //{
                //    nurieMain.SaveImage();
                //}
                //else if (nurieMain != null && s == "sns")
                //{
                //    nurieMain.ShareInSNS();
                //}
            }
            else
            {
                print("文字数が長いです");
                StartCoroutine(ShowMessage(lengthErrorMessage, 4.0f));
            }
        }
        else
        {
            print("※ひらがなのみです");
            StartCoroutine(ShowMessage(errorMessage, 4.0f));
        }
    }

    IEnumerator ShowMessage(GameObject obj, float waitTime)
    {
        var showDuration = 0.5f;
        var objPos = obj.transform.localPosition;
        var movePosX = 5.0f;

        obj.SetActive(true);
        obj.transform.DOLocalMove(new Vector3(movePosX, objPos.y, objPos.z), showDuration);
        yield return new WaitForSeconds(0.2f);
        obj.transform.DOLocalMove(new Vector3(-movePosX*2, objPos.y, objPos.z), showDuration);
        yield return new WaitForSeconds(0.2f);
        obj.transform.DOLocalMove(new Vector3(movePosX, objPos.y, objPos.z), showDuration);
        yield return new WaitForSeconds(0.2f);
        obj.transform.DOLocalMove(new Vector3(-movePosX * 2, objPos.y, objPos.z), showDuration);
        yield return new WaitForSeconds(0.2f);
        obj.transform.DOLocalMove(new Vector3(objPos.x, objPos.y, objPos.z), showDuration);
        yield return new WaitForSeconds(waitTime);
        obj.SetActive(false);
    }
}
