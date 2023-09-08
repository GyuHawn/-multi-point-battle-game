using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PointGamePortal : MonoBehaviourPunCallbacks
{
    public string sceneName;
    private bool isPLeaving = false;

    private void Start()
    {
        sceneName = "Map";
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPLeaving)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (collision.contacts[0].thisCollider.gameObject == this.gameObject)
                {
                    isPLeaving = true;
                    collision.gameObject.GetComponent<PlayerMovement>().currentSceneName = sceneName;  // Add this line before leaving the room.
                    PhotonNetwork.LeaveRoom();
                }
            }
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined the lobby.");

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        string newRoomName = "Room" + Random.Range(0, 1000);

        if (!PhotonNetwork.JoinRandomRoom())
        {
            Debug.Log("No rooms available. Attempting to create one...");
        }

        StartCoroutine(DelayedSceneLoad(sceneName));

        isPLeaving = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No rooms available. Creating a new one...");

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        string newRoomName = "Room" + Random.Range(0, 1000);

        PhotonNetwork.CreateRoom(newRoomName, options);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left the room.");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Reconnected to master server.");
        PhotonNetwork.JoinLobby();
    }

    private IEnumerator DelayedSceneLoad(string scene)
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.LoadLevel(scene);
    }
}