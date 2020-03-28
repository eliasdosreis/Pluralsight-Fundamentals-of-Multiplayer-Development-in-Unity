using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;

public class NetworkController : MonoBehaviourPunCallbacks {

	public bool isConnected;

	[SerializeField]
	private GameObject[] windowsNetwork;

	[SerializeField]
	private InputField playerName;

	[SerializeField]
	private InputField roomName;


	void Start () {
		windowsNetwork = new GameObject[2];
		windowsNetwork [0] = GameObject.Find ("Login").gameObject;
		windowsNetwork [1] = GameObject.Find ("Partidas").gameObject;

		playerName = GameObject.Find ("playerName").GetComponent<InputField> ();
		roomName = GameObject.Find ("NameRoom").GetComponent<InputField> ();

		windowsNetwork [0].SetActive (true);
		windowsNetwork [1].SetActive (false);
		
		isConnected = false;
		//PhotonNetwork.ConnectUsingSettings();
	}

	void Update () {
		if(isConnected){
			//PhotonNetwork.ConnectUsingSettings ();
		}
	}

	public void Login(){
		if (playerName.text != null || playerName.text != "") {
			PhotonNetwork.NickName = playerName.text;
			PhotonNetwork.ConnectUsingSettings ();
		} else {
			PhotonNetwork.NickName = "Player"+ UnityEngine.Random.Range(1,100);
			PhotonNetwork.ConnectUsingSettings ();
		}
	}

	public void BuscarPartidas(){
		PhotonNetwork.JoinRandomRoom ();
	}

	public void CriarSala(){
		if (roomName.text != null || roomName.text != "") {
			PhotonNetwork.CreateRoom (roomName.text);
		} else {
			int roomTemp = UnityEngine.Random.Range (1, 100);
			PhotonNetwork.CreateRoom ("Sala"+roomTemp);
		}
	}

	//------------------------- Pullcallbacks ----------------------------------//
	public override void OnConnected ()
	{
		// Chamado quando conexão com photon realizada.
		Debug.Log("Conectado");
		isConnected = false;
	}

	public override void OnConnectedToMaster ()
	{
		// Quando conexao e realizada e validada
		Debug.Log ("Conectado e Validado");
		Debug.Log ("Sever.:" + PhotonNetwork.CloudRegion + " - Ping.:" + PhotonNetwork.GetPing());

		try{
			windowsNetwork [1].SetActive (true);
			windowsNetwork [0].SetActive (false);
		}
		catch(Exception error) {

		}

		// Criando sala para obter interação entre outros jogadores
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedLobby ()
	{
		Debug.Log ("Entrou no lobby");

	}

	public override void OnJoinRandomFailed (short returnCode, string message)
	{
		
	}


	public GameObject myPlayer;
	public override void OnJoinedRoom ()
	{
		Debug.Log ("Conectado a uma sala.:");
		windowsNetwork [1].SetActive (false);

		//Instantiate (myPlayer, myPlayer.transform.position, myPlayer.transform.rotation);
		PhotonNetwork.Instantiate (myPlayer.name,myPlayer.transform.position, myPlayer.transform.rotation,0	);
	}

	public override void OnDisconnected (DisconnectCause cause)
	{
		Debug.Log ("OnDisconnected.: " + cause);
		//PhotonNetwork.ConnectToRegion ("us");
		isConnected = true;
		try{
			windowsNetwork [1].SetActive (false);
			windowsNetwork [0].SetActive (true);
		}catch(Exception error){
			
		}

	}



}
