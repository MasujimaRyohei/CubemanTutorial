using UnityEngine;
using System.Collections;

namespace CubeManProject
{
	public class ThrowAnimation : MonoBehaviour
	{
		//	当的モーションを行う腕のオブジェクト
		[SerializeField] private GameObject throwArm;
		//	腕を回し終わるまでの時間(秒)
		[SerializeField] private float rotMotionTime = 0.3f;
		//	腕の初期回転角度(オイラー各)
		[SerializeField] private float firstArmRot = 150.0f;
		//	弾発射タイミング
		[SerializeField] private float fireRotEuler = 120.0f;
		//	弾を発生させる基準点
		[SerializeField] private GameObject putBulletPosition;
		//	生成する弾用Prefabオブジェクト
		[SerializeField] private GameObject bulletObject;

		public IEnumerator StartThrowAnimation(PlayerCubeMan.MoveDirection nowDirection)
		{
			//	アニメーションの経過時間を管理する変数
			float animationTime = 0;
			//	最初の角度を設定
			throwArm.transform.localEulerAngles = new Vector3(firstArmRot, 0.0f, 0.0f);
			//	弾を撃ったかどうかを管理するフラグ
			bool fireBullet = false;

			//	表示のために1フレーム待つ
			yield return null;

			//	指定された時間が経過するまでループ
			while(animationTime < rotMotionTime)
			{
				//	経過時間を計測
				animationTime += Time.deltaTime;

				//	腕がきっちり真下を向くように、オーバーした場合は補正しておく
				if (animationTime >= rotMotionTime)
				{
					animationTime = rotMotionTime;
				}

				//	現在の腕の角度を算出する
				float nowArmRotate = firstArmRot - (firstArmRot * (animationTime / rotMotionTime));

				//	腕の角度を調べ、弾発射角度を過ぎていたら一度だけ弾を撃つ
				if (!fireBullet && nowArmRotate <= fireRotEuler)
				{
					fireBullet = true;

					FireBulletAnimation(nowDirection);
				}

				throwArm.transform.localEulerAngles = 
					new Vector3(nowArmRotate, 0.0f, 0.0f);

				//	1フレーム待つ
				yield return null;
			}
		}
		private void FireBulletAnimation(PlayerCubeMan.MoveDirection nowDirection)
		{
			//	弾を飛ばす処理を行う
			GameObject bulletInstance = Instantiate<GameObject>(bulletObject);
			//	親の設定を行っておく
			bulletInstance.transform.position = putBulletPosition.transform.position;
			//	BulletScriptスクリプトを取得する
			BulletScript burretScript = bulletInstance.GetComponent<BulletScript>();

			//	弾の移動アニメーションを開始する
			burretScript.StartAnimation(nowDirection == PlayerCubeMan.MoveDirection.Right);
		}
	}
}
