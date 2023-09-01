using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PointCount : MonoBehaviourPunCallbacks
{
    public Text killText;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Map")
        {
            killText = GameObject.Find("KillCount").GetComponent<Text>();
        }
    }

    private void OnDestroy()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
        }
    }

    private void OnEventReceived(EventData photonEvent)
    {
        if (photonEvent.Code == 1)
        {
            object[] data = (object[])photonEvent.CustomData;

            int playerId = (int)data[0];
            int score = (int)data[1];

            if (playerId == PhotonNetwork.LocalPlayer.ActorNumber && SceneManager.GetActiveScene().name == "Map")
            {
                UpdateKillTextRPC(score);
            }
        }
    }

    [PunRPC]
    public void UpdateKillTextRPC(int score)
    {
        if (SceneManager.GetActiveScene().name == "Map")
        {
            killText.text = score.ToString();
        }
    }
}
