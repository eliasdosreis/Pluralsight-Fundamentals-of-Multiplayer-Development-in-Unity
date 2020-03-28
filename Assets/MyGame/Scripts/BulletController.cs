using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletController : MonoBehaviour {


	public float speedBullet = 100f;
	public Rigidbody2D rigid;

	public float bulletTimeLife = 50f;
	float bulletTimeCount;

    public float bulletDamage;

    private PhotonView photonview;

	void Start () {
		rigid = GetComponent<Rigidbody2D> ();

		rigid.AddForce (transform.up * speedBullet, ForceMode2D.Force);

        photonview = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {
		if (bulletTimeCount >= bulletTimeLife) {
			Destroy (gameObject);
		}

		bulletTimeCount += Time.deltaTime;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && other.GetComponent<PlayerController>() && other.GetComponent<PhotonView>().IsMine )
        {
            other.GetComponent<PlayerController>().TakeDamage(bulletDamage);
            DestroyBullet();
            Debug.Log("Player.:"+ other.GetComponent<PhotonView>().Owner.NickName   );
        }
    }
    
    void DestroyBullet()
    {
        photonview.RPC("DestroyBulletNetwork", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void DestroyBulletNetwork()
    {
        Destroy(gameObject);
    }

}
