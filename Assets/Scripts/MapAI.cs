using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 穴掘りアルゴリズム
public class MapAI : MonoBehaviour
{

	// チップ定数
	const int CHIP_NONE = 0; // 通過可能
	const int CHIP_wALL = 1; // 通行不可

	// チップ管理
	Dictionary<int, Token> chips = null;

	// 穴掘りが完了したかどうか
	bool _bDone = false;

	// チップ上のX座標を取得する.
	float GetChipX(int i)
	{
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		var spr = Util.GetSprite("", "Wall");
		var sprW = spr.bounds.size.x;

		return min.x + (sprW * i) + sprW / 2;
	}

	// チップ上のy座標を取得する.
	float GetChipY(int j)
	{
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
		var spr = Util.GetSprite("", "Wall");
		var sprH = spr.bounds.size.y;

		return max.y - (sprH * j) - sprH / 2;
	}

	// 壁を作る
	void AddWall(Layer2D layer, int i, int j)
	{
		if (layer.Get(i, j) == CHIP_wALL)
		{
			// すでに存在するので何もしない
			return;
		}
		layer.Set(i, j, CHIP_wALL);

		int idx = layer.ToIdx(i, j);
		float x = GetChipX(i);
		float y = GetChipY(j);
		chips[idx] = Util.CreateToken(x, y, "", "Wall", "Wall");
	}
	// 壁を削除
	void RemoveWall(Layer2D layer, int i, int j)
	{
		if (layer.Get(i, j) == CHIP_NONE)
		{
			// 何もないのでなにもしない
			return;
		}
		layer.Set(i, j, CHIP_NONE);

		int idx = layer.ToIdx(i, j);
		Token t = chips[idx];
		// インスタンス破棄
		t.DestroyObj();
		chips.Remove(idx);
	}

	// 穴掘り開始
	IEnumerator Start()
	{
		// チップ管理生成
		chips = new Dictionary<int, Token>();

		// ダンジョンを作る
		var layer = new Layer2D();
		// ダンジョンの幅と高さは奇数のみ
		layer.Create(16 + 1, 16 + 1);
		// すべて壁を埋める
		for (int j = 0; j < layer.Height; j++)
		{
			for (int i = 0; i < layer.Width; i++)
			{
				AddWall(layer, i, j);
			}
		}
		//layer.Fill(CHIP_wALL);

		// 開始地点を決める
		int xstart = 2; // 開始地点は偶数でないといけない
		int ystart = 4; // 開始地点は偶数でないといけない

		// 穴掘り開始
		yield return StartCoroutine(_Dig(layer, xstart, ystart));

		// 結果表示
		layer.Dump();

		// 穴掘り完了
		_bDone = true;
	}

	// 穴を掘る
	IEnumerator _Dig(Layer2D layer, int x, int y)
	{
		// 開始地点を掘る
		//layer.Set(x, y, CHIP_NONE);
		RemoveWall(layer, x, y);
		yield return new WaitForSeconds(0.1f);

		Vector2[] dirList = {
			new Vector2(-1, 0),
			new Vector2(0, -1),
			new Vector2(1, 0),
			new Vector2(0, 1)
		};
		// シャッフル
		for (int i = 0; i < dirList.Length; i++)
		{
			var tmp = dirList[i];
			var idx = Random.Range(0, dirList.Length - 1);
			dirList[i] = dirList[idx];
			dirList[idx] = tmp;
		}

		foreach (var dir in dirList)
		{
			int dx = (int)dir.x;
			int dy = (int)dir.y;
			if (layer.Get(x + dx * 2, y + dy * 2) == 1)
			{
				// 2マス先が壁なので掘れる
				//layer.Set(x+dx, y+dy, CHIP_NONE);
				RemoveWall(layer, x + dx, y + dy);

				yield return new WaitForSeconds(0.1f);

				// 次の穴を掘る
				//_Dig(layer, x + dx*2, y + dy*2);
				yield return StartCoroutine(_Dig(layer, x + dx * 2, y + dy * 2));
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	[System.Obsolete]
	void OnGUI()
	{
		if (_bDone)
		{
			if (GUI.Button(new Rect(160, 120, 128, 32), "もう一回"))
			{
				// もう一度やり直す
				Application.LoadLevel("Main");
			}
		}
	}
}
