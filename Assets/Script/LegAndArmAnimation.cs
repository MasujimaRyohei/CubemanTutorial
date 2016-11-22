using UnityEngine;
using System.Collections;

namespace CubeManProject
{
	//	腕と足を回転させるためのスクリプト
	public class LegAndArmAnimation : MonoBehaviour
	{
		//	回転アニメーションに使うカーブ
		[SerializeField] private AnimationCurve rotateCurve;
		//	左足回転用ゲームオブジェクト
		[SerializeField] private GameObject leftLeg;
		//	右足回転用ゲームオブジェクト
		[SerializeField] private GameObject rightLeg;
		//	左腕回転用ゲームオブジェクト
		[SerializeField] private GameObject leftArm;
		//	右腕回転用ゲームオブジェクト
		[SerializeField] private GameObject rightArm;
		//	カーブの1回実行にかかる時間(秒)
		[SerializeField] private float roopTime = 2.0f;
		//	カーブの波が1になったとき、回る足の角度(デグリー)
		[SerializeField] private float rotateDeguree = 45.0f;

		//	現在のカーブ値取得に使う経過時間
		private float nowCurveTime = 0f;
		//	アニメーション再生フラグ
		private bool animationFlag = false;

		//	生成時に一度だけ呼ばれる初期化メソッド
		private void Start()
		{
		}

		//	1フレームに1回呼ばれる更新メソッド
		private void Update()
		{
			if (animationFlag)
			{
				//	カーブに渡す基準の時間に経過時間を渡す
				nowCurveTime += Time.deltaTime;

				//	1回実行にかかる時間を過ぎていたら、その分を引いておく
				if (nowCurveTime >= roopTime)
				{
					nowCurveTime -= roopTime;
				}

				//	カーブに、基準時間を1回実行にかかる時間で割った数を渡す
				float nowCurveValue = rotateCurve.Evaluate(nowCurveTime / roopTime);

				//	取得したカーブ値から、足の回転角を出す
				float legRotateValue = nowCurveValue * rotateDeguree;

				//	左足と右腕に回転角を渡す
				leftLeg.transform.localEulerAngles = new Vector3(legRotateValue, 0, 0);
				rightArm.transform.localEulerAngles = new Vector3(legRotateValue, 0, 0);
				//	右足と左腕回転角を渡す
				rightLeg.transform.localEulerAngles = new Vector3(-legRotateValue, 0, 0);
				leftArm.transform.localEulerAngles = new Vector3(-legRotateValue, 0, 0);
			}
			else
			{
				nowCurveTime = 0f;
				//	全ての箇所にデフォルトの回転角を渡す
				leftLeg.transform.localEulerAngles = new Vector3(0, 0, 0);
				rightArm.transform.localEulerAngles = new Vector3(0, 0, 0);
				rightLeg.transform.localEulerAngles = new Vector3(0, 0, 0);
				leftArm.transform.localEulerAngles = new Vector3(0, 0, 0);
			}
		}

		//	アニメーションの状態をON/OFFするためのメソッド
		public void SetAnimationFlag(bool flag)
		{
			animationFlag = flag;
		}
	}
}
