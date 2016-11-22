using UnityEngine;
using System.Collections;

namespace CubeManProject
{
	public class PlayerCubeMan : MonoBehaviour
	{
		//	CubeManの向いている方向
		public enum MoveDirection
		{
			None,
			Left,
			Right
		}

		//	地面のGameObject
		[SerializeField]
		private GameObject groundObject;
		//	プレイヤーの移動速度
		[SerializeField]
		private float moveSpeed = 0.25f;
		//	腕と足を回転させるためのスクリプト
		[SerializeField]
		private LegAndArmAnimation rotAnimation;
		//	弾を発射するためのスクリプト
		[SerializeField]
		private ThrowAnimation throwScript;
		//	弾を連続して生成するまでの時間(秒)
		[SerializeField]
		private float bulletCoolTime = 0.5f;

		//	CubeManが実際に向いている方向
		private MoveDirection nowDirection = MoveDirection.None;
		//	次の弾を打てるようになるまでの時間。bulletCoolTime以上の時に発射可能とする
		private float nowCoolTime = 0f;
		//	弾の発射モーションフラグ
		private bool bulletAnimation = false;

		//	生成時に一度だけ呼ばれる初期化メソッド
		private void Start()
		{
			//	弾の発射可能間隔をリセット
			nowCoolTime = bulletCoolTime;
		}

		//	1フレームに1回呼ばれる更新メソッド
		private void Update()
		{
			//	移動中フラグ
			bool moveFlag = false;

			//	弾連続生成のクールタイムに加算
			nowCoolTime += Time.deltaTime;

			//	左移動チェック。弾の発射モーション中は移動できない
			if (Input.GetKey(KeyCode.LeftArrow) && !bulletAnimation)
			{
				//	左に移動しているので、移動状態を設定
				moveFlag = true;
				nowDirection = MoveDirection.Left;
				//	体の向いている角度を直指定する
				this.gameObject.transform.localEulerAngles = new Vector3(0, 90, 0);

				//	座標は計算してから入れる
				Vector3 nowPosition = this.gameObject.transform.localPosition;

				//	地面の左端より遠くに行かないように監視
				if (nowPosition.x > -(groundObject.transform.localScale.x / 2f))
				{
					nowPosition -= new Vector3(moveSpeed, 0, 0);
					this.gameObject.transform.localPosition = nowPosition;
				}
			}

			//	右移動チェック。弾の発射モーション中は移動できない
			if (Input.GetKey(KeyCode.RightArrow) && !bulletAnimation)
			{
				//	右に移動しているので、移動状態を設定
				moveFlag = true;
				nowDirection = MoveDirection.Right;
				//	角度を直指定する
				this.gameObject.transform.localEulerAngles = new Vector3(0, -90, 0);

				//	座標は計算してから入れる
				Vector3 nowPosition = this.gameObject.transform.localPosition;

				//	地面の右端より遠くに行かないように監視
				if (nowPosition.x < (groundObject.transform.localScale.x / 2f))
				{
					nowPosition += new Vector3(moveSpeed, 0, 0);
					this.gameObject.transform.localPosition = nowPosition;
				}
			}

			//	腕と足回転クラスに移動フラグをアニメーションフラグとして渡す
			rotAnimation.SetAnimationFlag(moveFlag);

			//	スペースキーと向きチェック。発射間隔や発射モーションの状態も考慮する
			if (Input.GetKey(KeyCode.Space) && (nowDirection != MoveDirection.None) &&
				(nowCoolTime >= bulletCoolTime) && !bulletAnimation)
			{
				//	弾の再生性までの時間を適応
				nowCoolTime = 0f;
				//	弾発射モーションフラグをON
				bulletAnimation = true;
				//	弾の発射アニメーションを開始する
				StartCoroutine(StartBulletAnimation());
			}
		}

		//	弾の発射アニメーションが終わるまで待つ
		private IEnumerator StartBulletAnimation()
		{
			if (throwScript != null)
			{
				yield return StartCoroutine(throwScript.StartThrowAnimation(nowDirection));
			}

			//	アニメーションが終わったら、弾発射状態をリセット
			bulletAnimation = false;
		}
	}
}
