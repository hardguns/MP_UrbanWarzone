using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMan : Photon.MonoBehaviour
{
    public string verNum = "0.1";
    public string roomName = "room1";
    public string playerName = "Player001";
    public Transform spawnPoint;
    public GameObject playerPrefab;
    public bool isConnected = false;

    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        if (playerName.Equals("Player001"))
        {
            playerName = "Player" + Random.Range(0, 999);
        }
        roomName = "Room " + Random.Range(0, 999);
        PhotonNetwork.gameVersion = verNum;
        PhotonNetwork.ConnectUsingSettings(verNum);
        Debug.Log("Starting connection...");
    }

    public void OnJoinedLobby()
    {
        Debug.Log("Enter on join");
        isConnected = true;
        //PhotonNetwork.JoinOrCreateRoom(roomName, null, null);
        Debug.Log("Starting server!");
    }

    public void OnJoinedRoom()
    {
        PhotonNetwork.playerName = playerName;
        isConnected = false;
        spawnPlayer();
    }

    public void spawnPlayer()
    {
        GameObject pl = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Length)].position, spawnPoint.rotation, 0) as GameObject;
        pl.GetComponent<FPSWalker>().enabled = true;
        pl.GetComponent<FPSWalker>().fpsCam.SetActive(true);
        pl.GetComponent<FPSWalker>().graphics.SetActive(false);
    }

    public void OnGUI()
    {
        if (isConnected)
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2 - 250, (Screen.height / 2) - 250, 500, 500));

            playerName = GUILayout.TextField(playerName);
            roomName = GUILayout.TextField(roomName);

            if (GUILayout.Button("Create"))
            {
                PhotonNetwork.JoinOrCreateRoom(roomName, null, null);
            }

            foreach (RoomInfo game in PhotonNetwork.GetRoomList())
            {
                if (GUILayout.Button(game.Name + " " + game.PlayerCount + "/" + game.MaxPlayers))
                {
                    PhotonNetwork.JoinOrCreateRoom(game.Name, null, null);
                }
            }

            GUILayout.EndArea();
        }
    }
}
