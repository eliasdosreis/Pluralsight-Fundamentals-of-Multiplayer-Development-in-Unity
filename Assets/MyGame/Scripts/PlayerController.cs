using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class PlayerController : MonoBehaviourPun,IPunObservable	 {

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

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody2D> ();
		photonview = GetComponent<PhotonView> ();

		HealthManager (playerHealthmax);
	}
	
	// Update is called once per frame
	void Update () {
		if (photonview.IsMine) {	
			PlayerMove ();
			PlayerTurn ();

			Shotting ();
		}
	}

	void IPunObservable.OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo	 info){
		if (stream.IsReading) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);

			stream.SendNext (rigid.velocity); 
		} else {
			
				Vector3 temp = (Vector3)stream.ReceiveNext ();
			transform.position = Vector3.Lerp(transform.position,temp,Mathf.Abs( (float)(PhotonNetwork.Time - info.timestamp)));
			transform.rotation = (Quaternion)stream.ReceiveNext ();

			rigid.velocity = (Vector2)stream.ReceiveNext ();

			//float lag = Mathf.Abs ((float)(PhotonNetwork.Time - info.timestamp));
			//rigid.position += rigid.velocity * lag;
		}
	}

	void Shotting()
	{
		if (Input.GetMouseButton (0)) {
			HealthManager (-10);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			//Instantiate (bulletPhotonView, spawBullet.transform.position, spawBullet.transform.rotation); 
			PhotonNetwork.Instantiate (bullet.name, spawBullet.transform.position, spawBullet.transform.rotation, 0);
		}
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
