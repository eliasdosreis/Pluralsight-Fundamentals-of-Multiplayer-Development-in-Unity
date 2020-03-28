using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class PlayerController : MonoBehaviourPun {
    
    public float playerSpeed = 5f;

	[SerializeField]
	Rigidbody2D rigid;

	[SerializeField]
	PhotonView	photonview;

	[Header("Health")]

	[SerializeField]
	private float playerHealthmax = 100;

	[SerializeField]
	private float playerHealthCurrent;

	[SerializeField]
	private Image playerHealthField;	

	[Header("Bullet")]
	public GameObject spawBullet;
	public GameObject bullet;
	public GameObject bulletPhotonView;

	void Start () {
		rigid = GetComponent<Rigidbody2D> ();
		photonview = GetComponent<PhotonView> ();

		HealthManager (playerHealthmax);
	}
	
	void Update () {
		if (photonview.IsMine) {	
			PlayerMove ();
			PlayerTurn ();

			Shotting ();
            gameObject.name = photonview.Owner.NickName.ToString();
            NickName();
        }
	}

    void NickName()
    {
        photonview.RPC("NickNameNetwork", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void NickNameNetwork()
    {
        gameObject.name = photonview.Owner.NickName.ToString();
    }

	void Shotting()
	{
		if (Input.GetMouseButton (0)) {
			HealthManager (-10);
		}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonview.RPC("Shoot",RpcTarget.All);  
        }		
	}

    [PunRPC]
    void Shoot()
    {
       Instantiate(bullet, spawBullet.transform.position, spawBullet.transform.rotation);
    }


    public void TakeDamage(float value)
    {
        photonview.RPC("TakeDamageNetwork", RpcTarget.AllBuffered,value);
    }

    [PunRPC]
    void TakeDamageNetwork(float value)
    {
        HealthManager(value);
    }

    void HealthManager(float value){
		playerHealthCurrent += value;
		playerHealthField.fillAmount = playerHealthCurrent/100;
	}
	 
	void PlayerMove()
	{
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		rigid.velocity = new Vector2 (x, y);
	}

	void PlayerTurn(){
		Vector3 mousePosition = Input.mousePosition;

		mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);

		Vector2 direction = new Vector2 (
			mousePosition.x - transform.position.x,
			mousePosition.y - transform.position.y
		);

		transform.up = direction;
	}
}
