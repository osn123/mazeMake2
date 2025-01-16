using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_HitCheck : MonoBehaviour {
	Image 		GameOverImage;
	Image		GameClearImage;
	bool		is_Goal = false;		//ゴールしたかのフラグ
	bool		is_GameOver = false;	//ゲームオーバーになったかのフラグ
	AudioSource	Aud_SE;
	public AudioSource	Aud_BGM;
	public AudioClip	SE_Goal;
	public AudioClip	SE_GameOver;

	// Use this for initialization
	void Start () {
        GameOverImage = GameObject.Find("GameOver_Image").GetComponent<Image>();
        GameOverImage.enabled = false;
        GameClearImage = GameObject.Find("Goal_Image").GetComponent<Image>();
		GameClearImage.enabled = false;
		is_Goal = false;
		is_GameOver = false;
		Aud_SE = GetComponent<AudioSource>();
	}
	
	//何かと接触した時に呼び出される
	void OnCollisionEnter(Collision collision)
	{
		//ゴールとプレイヤーが触れた（ゲームオーバー後でない）
		if (!is_GameOver && collision.gameObject.tag == "Goal") {
			if(!is_Goal){   //ゴールした直後に一度だけ発動
				//ゴールからエフェクトを出す
                GameObject goal = GameObject.Find("Goal");
                ParticleSystem goalParticle = goal.GetComponent<ParticleSystem>();
                if (goalParticle != null)	goalParticle.Play();

				//SEを出し、BGMを止める
                if (SE_Goal != null)        Aud_SE.PlayOneShot(SE_Goal);
				if (Aud_BGM != null)        Aud_BGM.Stop();
			}
			is_Goal = true;
			GameClearImage.enabled = true;
		}
		//ハンターとプレイヤーが触れた（ゴール後でない）
		if (!is_Goal && collision.gameObject.tag == "Hunter") {
			if(!is_GameOver){   //ゲームオーバー直後に一度だけ発動
				//SEを出し、BGMを止める
                if (SE_GameOver != null)    Aud_SE.PlayOneShot(SE_GameOver);
                if (Aud_BGM != null)        Aud_BGM.Stop();
			}
			is_GameOver = true;
			GameOverImage.enabled = true; 
		}
	}
}
