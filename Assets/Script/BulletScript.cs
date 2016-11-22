using UnityEngine;
using System.Collections;

namespace CubeManProject
{
	public class BulletScript : MonoBehaviour
	{
		//	弾の進むスピード
		[SerializeField] private float moveSpeed = 0.5f;
		//	弾の生存する時間
		[SerializeField] private float moveTime = 3.0f;
		//	弾の進む方向
		private Vector3 moveDirection = Vector3.zero;
		//	弾の移動アニメーションを行うコルーチン用変数
		private IEnumerator moveAnime;

		private float nowMoveTime = 0f;

		//	アニメーション開始のためのメソッド
		public void StartAnimation(bool moveRight)
		{
			//	渡された向き(右向きフラグ)の状態で、移動方向を決める
			if (moveRight)
			{
				moveDirection = new Vector3(1, 0, 0);
			}
			else
			{
				moveDirection = new Vector3(-1, 0, 0);
			}

			//	敵に当たった場合は途中で自分を消すので、アニメーション終了用にコルーチンを保持する
			moveAnime = MoveAnimation();
			//	保持したものでアニメーションを開始する
			StartCoroutine(moveAnime);
		}

		//	実際に弾の移動を行うコルーチン
		private IEnumerator MoveAnimation()
		{
			//	最初に1フレーム待つ
			yield return null;

			//	生存時間が過ぎるまでは、無限ループで移動を続ける
			while (true)
			{
				//	弾の移動を行う
				Vector3 moveLength = moveDirection * moveSpeed;
                moveLength.y *= 0.98f;
				this.gameObject.transform.localPosition += moveLength;

				//	経過時間を集計する
				nowMoveTime += Time.deltaTime;

				if (nowMoveTime >= moveTime)
				{
					//	もし移動継続時間を過ぎていたら、ループを抜ける
                    
					break;
				}

				//	1フレーム待つ
				yield return null;
			}

			//	1フレーム待つ
			yield return null;

			//	生存時間が先に終了したので、コルーチン保持を解いておく
			moveAnime = null;
			//	弾削除を行う
			DeleteBullet();
		}

		//	この弾を削除する処理を行うメソッド
		public void DeleteBullet()
		{
			if (moveAnime != null)
			{
				//	真っ先にアニメーションを停止しておく
				StopCoroutine(moveAnime);
			}

			//	オブジェクトを削除
			Destroy(this.gameObject);
		}
	}
}
