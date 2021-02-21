using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localSave
{
    // データの保存
    // int型の保存
    public static bool saveData(string key, int value)
    {
        try
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs save int ERROR : " + err);
            return false;
        }
        return true;
    }
    // float型の保存
    public static bool saveData(string key, float value)
    {
        try
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs save float ERROR : " + err);
            return false;
        }
        return true;
    }
    // string型の保存
    public static bool saveData(string key, string value)
    {
        try
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs save string ERROR : " + err);
            return false;
        }
        return true;
    }
    // int型listの保存
    public static bool saveData(string key, List<int> valueList)
    {
        try
        {
            int i = 0;
            while (PlayerPrefs.HasKey(key + i))
            {
                PlayerPrefs.DeleteKey(key + i);
                i++;
            }
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs delete int list ERROR : " + err);
            return false;
        }

        try
        {
            int i = 0;
            foreach (int value in valueList)
            {
                if (PlayerPrefs.HasKey(key + i)) PlayerPrefs.DeleteKey(key + i);
                PlayerPrefs.SetInt(key + i, value);
                i++;
            }
            if (PlayerPrefs.HasKey(key + i)) PlayerPrefs.DeleteKey(key + i);
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs save int list ERROR : " + err);
            return false;
        }
        return true;
    }
    // float型listの保存
    public static bool saveData(string key, List<float> valueList)
    {
        try
        {
            int i = 0;
            while (PlayerPrefs.HasKey(key + i))
            {
                PlayerPrefs.DeleteKey(key + i);
                i++;
            }
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs delete float list ERROR : " + err);
            return false;
        }

        try
        {
            int i = 0;
            foreach (float value in valueList)
            {
                if (PlayerPrefs.HasKey(key + i)) PlayerPrefs.DeleteKey(key + i);
                PlayerPrefs.SetFloat(key + i, value);
                i++;
            }
            if (PlayerPrefs.HasKey(key + i)) PlayerPrefs.DeleteKey(key + i);
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs save float list ERROR : " + err);
            return false;
        }
        return true;
    }
    // string型listの保存
    public static bool saveData(string key, List<string> valueList)
    {
        try
        {
            int i = 0;
            while (PlayerPrefs.HasKey(key + i))
            {
                PlayerPrefs.DeleteKey(key + i);
                i++;
            }
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs delete string list ERROR : " + err);
            return false;
        }

        try
        {
            int i = 0;
            foreach (string value in valueList)
            {
                if (PlayerPrefs.HasKey(key + i)) PlayerPrefs.DeleteKey(key + i);
                PlayerPrefs.SetString(key + i, value);
                i++;
            }
            if (PlayerPrefs.HasKey(key + i)) PlayerPrefs.DeleteKey(key + i);
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs save string list ERROR : " + err);
            return false;
        }
        return true;
    }

    // 保存済みデータの取得
    // int型の取得
    public static int getIntData(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    // float型の取得
    public static float getFloatData(string key, float defaultValue = 0.0f)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }
    // string型の取得
    public static string getStringData(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }
    // int型listの取得
    public static List<int> getIntListData(string key)
    {
        List<int> returnList = new List<int>();
        try
        {
            int i = 0;
            while (PlayerPrefs.HasKey(key + i))
            {
                returnList.Add(PlayerPrefs.GetInt(key + i, 0));
                i++;
            }
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs get int list ERROR : " + err);
            return null;
        }
        return returnList;
    }
    // float型listの取得
    public static List<float> getFloatListData(string key)
    {
        List<float> returnList = new List<float>();
        try
        {
            int i = 0;
            while (PlayerPrefs.HasKey(key + i))
            {
                returnList.Add(PlayerPrefs.GetFloat(key + i, 0.0f));
                i++;
            }
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs get float list ERROR : " + err);
            return null;
        }
        return returnList;
    }
    // string型listの取得
    public static List<string> getStringListData(string key)
    {
        List<string> returnList = new List<string>();
        try
        {
            int i = 0;
            while (PlayerPrefs.HasKey(key + i))
            {
                returnList.Add(PlayerPrefs.GetString(key + i, ""));
                i++;
            }
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs get string list ERROR : " + err);
            return null;
        }
        return returnList;
    }

    // 保存済みデータの削除
    // キーを指定して削除
    public static bool deleteData(string key)
    {
        try
        {
            PlayerPrefs.DeleteKey(key);
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs delete ERROR : " + err);
            return false;
        }
        return true;
    }
    // すべてのローカルデータを削除
    public static bool deleteAllData(string key)
    {
        try
        {
            PlayerPrefs.DeleteAll();
        }
        catch (System.Exception err)
        {
            Debug.Log("localSave.cs delete all ERROR : " + err);
            return false;
        }
        return true;
    }

}
