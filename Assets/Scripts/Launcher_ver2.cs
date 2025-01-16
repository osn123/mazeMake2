using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

public class Launcher_ver2 : MonoBehaviour {
	public GameObject	WallPrefab;
	public GameObject	HunterPrefab;

	//デフォルトマップ
	private int mapWidth = 15;
	private int mapLength = 15;
	private char[,] mapData = {
		{'1','1','1','1','1','1','1','1','1','1','1','1','1','1','1'},
		{'1','0','0','0','0','0','0','0','1','0','0','0','0','0','1'},
		{'1','0','1','1','1','1','1','0','1','0','1','1','1','0','1'},
		{'1','0','1','0','0','0','1','0','0','0','1','0','0','0','1'},
		{'1','0','1','0','1','0','1','0','1','1','1','0','1','0','1'},
		{'1','0','1','0','1','0','0','0','0','0','0','0','1','0','1'},
		{'1','0','0','0','0','0','1','0','1','1','1','0','1','0','1'},
		{'1','0','1','1','1','0','1','0','1','0','0','0','1','0','1'},
		{'1','0','1','0','0','0','0','0','1','0','1','0','1','0','1'},
		{'1','0','1','0','1','0','1','1','1','1','1','0','0','0','1'},
		{'1','0','1','1','1','0','1','0','0','0','1','1','1','0','1'},
		{'1','0','1','s','0','0','1','0','0','0','1','g','1','0','1'},
		{'1','0','1','0','1','1','1','1','1','0','1','0','1','0','1'},
		{'1','0','0','0','0','0','0','0','0','0','0','0','0','h','1'},
		{'1','1','1','1','1','1','1','1','1','1','1','1','1','1','1'},
	};

	//壁を設置
	private void SetWalls()
	{
		Vector3 vec = Vector3.zero;
		for (int z = 0; z < mapLength; z++) {
			for (int x = 0; x < mapWidth; x++) {
				//該当マスの位置を先に計算しておく
				vec.Set(x*1.0f - mapWidth*0.5f
				    , 0.5f 
				    , -(z*1.0f - mapLength*0.5f));

				switch (mapData [z, x]) {
				case '0':	//通路（何もしない）
					break;
				case '1':	//壁ブロックの生成
					Instantiate(this.WallPrefab , vec , Quaternion.identity);
					break;
				case 'h':	//ハンターの出現
					Instantiate(this.HunterPrefab , vec , Quaternion.identity);
					break;
				case 's':	//プレイヤーの初期位置
					GameObject player = GameObject.FindWithTag ("Player");
					player.transform.position = vec;
					break;
				case 'g':	//ゴール位置
					GameObject goal = GameObject.FindWithTag ("Goal");
					goal.transform.position = vec;
					break;
				}
			}
		}
	}

	//テキストファイルからマップをロードする
	private void LoadMap_FromTextFile(string filename)
	{
		string txtMapData = "";
		FileInfo fi = new FileInfo(Application.dataPath + "/MapData/" + filename);
		
		try {
			// 一行毎読み込み
			using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8)){
				txtMapData = sr.ReadToEnd();
			}
		} catch (Exception e){
			Debug.LogWarning(e.Message);
		}
		
		if (txtMapData.Length != 0) {
			// 空の要素を削除するためのオプション.
			System.StringSplitOptions	option = System.StringSplitOptions.RemoveEmptyEntries;

			// 改行コードで１行ごとに切り出す..
			string[] lines = txtMapData.Split(new char[] {'\r','\n'},option);

			// 一行目はマップの大きさ.
			// "," で１文字ごとに切り出す.
			string[] sizewh = lines[0].Split(new char[] {','},option);
			mapWidth = int.Parse(sizewh[0]);
			mapLength = int.Parse(sizewh[1]);
			
			char[,] mapdata = new char[mapLength,mapWidth];
			
			for (int lineCnt = 0; lineCnt < mapLength; lineCnt++) {
				// "," で１文字ごとに切り出す.
				string[] data = lines[1+lineCnt].Split(new char[] {','},option);	//一行目はマップの大きさなので２行目から

				for (int col = 0; col < mapWidth; col++) {
					mapdata[lineCnt,col] = data[col][0];
				}
			}
			
			// ゲームで使う配列に丸ごとコピー
			mapData = mapdata;
		} else {
			//テキストが空だった
			Debug.LogWarning("txtMapData is null");
		}
	}

	//生成された初回だけ呼び出される
	void Start () {
		LoadMap_FromTextFile("map_Stage1Test.txt");	//テキストファイルからマップをロードする

		SetWalls();
	}
}
