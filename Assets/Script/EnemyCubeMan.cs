using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CubeManProject
{
	public class EnemyCubeMan : MonoBehaviour
	{
		private class GameObjectPrams
		{
			public Vector3 position;
			public Vector3 rotation;
			public Vector3 scale;

			public GameObjectPrams(Vector3 pos, Vector3 rot, Vector3 scl)
			{
				this.position = pos;
				this.rotation = rot;
				this.scale = scl;
			}
		}

		//	頭Cubeオブジェクト
		[SerializeField] private Rigidbody headObject;
		//	胴体Cubeオブジェクト
		[SerializeField] private Rigidbody bodyObject;
		//	左腕Cubeオブジェクト
		[SerializeField] private Rigidbody leftArmObject;
		//	右腕Cubeオブジェクト
		[SerializeField] private Rigidbody rightArmObject;
		//	左足Cubeオブジェクト
		[SerializeField] private Rigidbody leftLegObject;
		//	右足Cubeオブジェクト
		[SerializeField] private Rigidbody rightLegObject;
		//	自信の持つBoxCollider
		[SerializeField] private BoxCollider myBoxCollider;
		//	オブジェクトが復元するまでの時間(秒)
		[SerializeField] private float respawnTime = 20.0f;

		private List<GameObjectPrams> objectStatesList;


		//	生成時に一度だけ呼ばれる初期化メソッド
		private void Start()
		{
			objectStatesList = new List<GameObjectPrams>();
			//	念のため、全オブジェクトに停止処理をしておく
			OffAllPartsRigidbody();
		}

		//	当たり判定にヒットしたとき呼ばれるメソッド
		private void OnCollisionEnter(Collision collision)
		{
			BulletScript checkBullet = collision.gameObject.GetComponent<BulletScript>();

			if (checkBullet != null)
			{
				//	弾オブジェクトとぶつかったので、まずは弾を削除
				checkBullet.DeleteBullet();

				//	アニメーション開始
				StartDown();
			}
		}

		//	崩れ落ちるアニメーションを開始する
		private void StartDown()
		{
			//	自身のBoxColliderを無効化する
			myBoxCollider.enabled = false;

			//	最初に位置情報などを保存しておく
			Transform getTransform = headObject.gameObject.transform;
			objectStatesList.Add(new GameObjectPrams(getTransform.localPosition, getTransform.localEulerAngles, getTransform.localScale));
			getTransform = bodyObject.gameObject.transform;
			objectStatesList.Add(new GameObjectPrams(getTransform.localPosition, getTransform.localEulerAngles, getTransform.localScale));
			getTransform = leftArmObject.gameObject.transform;
			objectStatesList.Add(new GameObjectPrams(getTransform.localPosition, getTransform.localEulerAngles, getTransform.localScale));
			getTransform = rightArmObject.gameObject.transform;
			objectStatesList.Add(new GameObjectPrams(getTransform.localPosition, getTransform.localEulerAngles, getTransform.localScale));
			getTransform = leftLegObject.gameObject.transform;
			objectStatesList.Add(new GameObjectPrams(getTransform.localPosition, getTransform.localEulerAngles, getTransform.localScale));
			getTransform = rightLegObject.gameObject.transform;
			objectStatesList.Add(new GameObjectPrams(getTransform.localPosition, getTransform.localEulerAngles, getTransform.localScale));

			OnAllPartsRigidbody();

			//	崩れ落ちた状態の継続時間経過を待ち、その後復旧する
			StartCoroutine(WaitAndResetCroutine());
		}

		//	全てのパーツのRigidbodyの演算を止める
		private void OffAllPartsRigidbody()
		{
			headObject.velocity = Vector3.zero;
			headObject.constraints = RigidbodyConstraints.FreezeAll;
			headObject.useGravity = false;
			bodyObject.velocity = Vector3.zero;
			bodyObject.constraints = RigidbodyConstraints.FreezeAll;
			bodyObject.useGravity = false;
			leftArmObject.velocity = Vector3.zero;
			leftArmObject.constraints = RigidbodyConstraints.FreezeAll;
			leftArmObject.useGravity = false;
			rightArmObject.velocity = Vector3.zero;
			rightArmObject.constraints = RigidbodyConstraints.FreezeAll;
			rightArmObject.useGravity = false;
			leftLegObject.velocity = Vector3.zero;
			leftLegObject.constraints = RigidbodyConstraints.FreezeAll;
			leftLegObject.useGravity = false;
			rightLegObject.velocity = Vector3.zero;
			rightLegObject.constraints = RigidbodyConstraints.FreezeAll;
			rightLegObject.useGravity = false;
		}

		//	全てのパーツのRigidbodyの演算を再開する
		private void OnAllPartsRigidbody()
		{
			headObject.constraints = RigidbodyConstraints.None;
			headObject.useGravity = true;
			bodyObject.constraints = RigidbodyConstraints.None;
			bodyObject.useGravity = true;
			leftArmObject.constraints = RigidbodyConstraints.None;
			leftArmObject.useGravity = true;
			rightArmObject.constraints = RigidbodyConstraints.None;
			rightArmObject.useGravity = true;
			leftLegObject.constraints = RigidbodyConstraints.None;
			leftLegObject.useGravity = true;
			rightLegObject.constraints = RigidbodyConstraints.None;
			rightLegObject.useGravity = true;
		}

		//	リセット時間まで待って、オブジェクトの位置を復元する
		private IEnumerator WaitAndResetCroutine()
		{
			yield return new WaitForSeconds(respawnTime);
			
			//	物理演算の影響を切る
			OffAllPartsRigidbody();

			//	位置を復元
			ResetAllPartsParam();

			//	自身のBoxColliderを有効化する
			myBoxCollider.enabled = true;
		}

		//	全てのパーツの情報を復元する
		private void ResetAllPartsParam()
		{
			GameObjectPrams param;

			param = objectStatesList[0];
			headObject.gameObject.transform.localPosition = param.position;
			headObject.gameObject.transform.localEulerAngles = param.rotation;
			headObject.gameObject.transform.localScale = param.scale;
			param = objectStatesList[1];
			bodyObject.gameObject.transform.localPosition = param.position;
			bodyObject.gameObject.transform.localEulerAngles = param.rotation;
			bodyObject.gameObject.transform.localScale = param.scale;
			param = objectStatesList[2];
			leftArmObject.gameObject.transform.localPosition = param.position;
			leftArmObject.gameObject.transform.localEulerAngles = param.rotation;
			leftArmObject.gameObject.transform.localScale = param.scale;
			param = objectStatesList[3];
			rightArmObject.gameObject.transform.localPosition = param.position;
			rightArmObject.gameObject.transform.localEulerAngles = param.rotation;
			rightArmObject.gameObject.transform.localScale = param.scale;
			param = objectStatesList[4];
			leftLegObject.gameObject.transform.localPosition = param.position;
			leftLegObject.gameObject.transform.localEulerAngles = param.rotation;
			leftLegObject.gameObject.transform.localScale = param.scale;
			param = objectStatesList[5];
			rightLegObject.gameObject.transform.localPosition = param.position;
			rightLegObject.gameObject.transform.localEulerAngles = param.rotation;
			rightLegObject.gameObject.transform.localScale = param.scale;
			
			objectStatesList.Clear();

			objectStatesList = new List<GameObjectPrams>();
		}
	}
}
