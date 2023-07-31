using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class JoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] bool isMultiplayer;
    [SerializeReference] Vector3 spawnPositionOffset;
    [SerializeField] GameObject vrMultiplayerPrefab;


    void Awake()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Instantiate(vrMultiplayerPrefab.name, transform.position + spawnPositionOffset, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    void JoinCreateRoom()
    {    
        if (PhotonNetwork.CountOfRooms < 1)
        {
            PhotonNetwork.CreateRoom("0000");
        }
        else
        {
            PhotonNetwork.JoinRoom("0000");
        }
    }

    public override void OnConnectedToMaster()
    {
        JoinCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);    
    }
}
