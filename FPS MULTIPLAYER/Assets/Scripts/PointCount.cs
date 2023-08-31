using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class PointCount : MonoBehaviourPunCallbacks
{
    public Text killText;

    void Start()
    {
        killText = GameObject.Find("KillCount").GetComponent<Text>();
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

            if (playerId == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                UpdateKillTextRPC(score);
            }
        }
    }

    [PunRPC]
    public void UpdateKillTextRPC(int score)
    {
        killText.text = score.ToString();
    }
}
