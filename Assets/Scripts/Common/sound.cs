using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sound {

    // SEチャンネル数はとりあえず4にしてある
    const int SE_CHANNEL = 4;

    public static float seVolume = 1.0f;
    public static float bgmVolume = 1.0f;

    // サウンド種
    enum eType {
        Bgm,
        Se,
    }

    // シングルトン関連処理
    static Sound _singleton = null;
    
    public static Sound GetInstance()
    {
        return _singleton ?? (_singleton = new Sound());
    }

    // サウンド再生のためのゲームオブジェクト
    GameObject _object = null;
  
    // サウンドリソース
    AudioSource _sourceBgm = null; // BGM
    AudioSource _sourceSeDefault = null; // SE (デフォルト)
    AudioSource[] _sourceSeArray; // SE (チャンネル)
  
    static Dictionary<string, _Data> _poolBgm = new Dictionary<string, _Data>(); 
    static Dictionary<string, _Data> _poolSe = new Dictionary<string, _Data>();

    class _Data {
        public string Key;
        public string ResName;
        public AudioClip Clip;

        public _Data(string key, string res) {
            Key = key;
            ResName = "Sounds/" + res;
            Clip = Resources.Load(ResName) as AudioClip;
        }
    }

    /// コンストラクタ
    public Sound() {
        _sourceSeArray = new AudioSource[SE_CHANNEL];
    }

    /// AudioSourceを取得する
    AudioSource _GetAudioSource(eType type, int channel=-1) {
        if(_object == null) {
            _object = new GameObject("Sound");
            GameObject.DontDestroyOnLoad(_object);
            _sourceBgm = _object.AddComponent<AudioSource>();
            _sourceSeDefault = _object.AddComponent<AudioSource>();
            for (int i = 0; i < SE_CHANNEL; i++)
            {
                _sourceSeArray[i] = _object.AddComponent<AudioSource>();
            }
        }

        if(type == eType.Bgm) {
            // BGM
            _sourceBgm.volume = bgmVolume;
            return _sourceBgm;
        } else {
        // SE
            if (0 <= channel && channel < SE_CHANNEL)
            {
                // チャンネル指定
                _sourceSeArray[channel].volume = seVolume;
                return _sourceSeArray[channel];
            } else {
                // デフォルト
                _sourceSeDefault.volume = seVolume;
                return _sourceSeDefault;
            }
        }
    }

    // サウンドのロード
    // sound用フォルダはResources/Soundsフォルダに配置
    public static void LoadBgm(string key, string resName) {
        GetInstance()._LoadBgm(key, resName);
    }
    public static void LoadSe(string key, string resName) {
        GetInstance()._LoadSe(key, resName);
    }

    void _LoadBgm(string key, string resName) {
        if (_poolBgm.ContainsKey(key))
        {
            // すでに登録済ならいったん消す
            _poolBgm.Remove(key);
        }
        _poolBgm.Add(key, new _Data(key, resName));
    }
    void _LoadSe(string key, string resName) {
        if (_poolSe.ContainsKey(key))
        {
            // すでに登録済みならいったん消す
            _poolSe.Remove(key);
        }
        _poolSe.Add(key, new _Data(key, resName));
    }

    /// BGMの再生
    /// ※事前にLoadBgmでロードしておくこと
    public static bool PlayBgm(string key) {
        return GetInstance()._PlayBgm(key);
    }
    bool _PlayBgm(string key) {
        
        
        if(_poolBgm.ContainsKey(key) == false) {
        // 対応するキーがない
            return false;
        }

        // Debug.Log("BGM再生2");
        // いったん止める
        _StopBgm();

        // リソースの取得
        var _data = _poolBgm[key];

        // 再生
        var source = _GetAudioSource(eType.Bgm);
        source.loop = true;
        source.clip = _data.Clip;
        source.Play();

        // Debug.Log("BGM再生");
        return true;
    }

    /// BGMの停止
    public static bool StopBgm() {
        return GetInstance()._StopBgm();
    }
    bool _StopBgm() {
        _GetAudioSource(eType.Bgm).Stop();
        // Debug.Log("BGM停止");
        return true;
    }

    public static bool PlaySe(string key, int channel=-1) {
        return GetInstance()._PlaySe(key, channel);
    }
    bool _PlaySe(string key, int channel=-1) {
        if(_poolSe.ContainsKey(key) == false) {
            return false;
        }

        var _data = _poolSe[key];

        if (0 <= channel && channel < SE_CHANNEL)
        {
            var source = _GetAudioSource(eType.Se, channel);
            source.clip = _data.Clip;
            source.Play();
        } else {
            var source = _GetAudioSource(eType.Se);
            source.PlayOneShot(_data.Clip);
        }
        // Debug.Log("SE再生");
        return true;
    }

    public static bool StopSe(string key, int channel=-1) {
        return GetInstance()._StopSe(key, channel);
    }
    bool _StopSe(string key, int channel=-1) {
        if (0 <= channel && channel < SE_CHANNEL)
        {
            var source = _GetAudioSource(eType.Se, channel);
            source.Stop();
        } else {
            var source = _GetAudioSource(eType.Se);
            source.Stop();
        }
        return true;
    }
}