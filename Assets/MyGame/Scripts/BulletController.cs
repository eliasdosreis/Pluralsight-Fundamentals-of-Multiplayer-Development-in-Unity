using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {


	public float speedBullet = 100f;
	public Rigidbody2D rigid;

	public float bulletTimeLife = 5f;
	float bulletTimeCount;

	void Start () {
		rigid = GetComponent<Rigidbody2D> ();

		rigid.AddForce (transform.up * speedBullet, ForceMode2D.Force);
	}
	
	// Update is called once per frame
	void Update () {
		if (bulletTimeCount >= bulletTimeLife) {
			Destroy (gameObject);
		}

		bulletTimeCount += Time.deltaTime;
	}
}
