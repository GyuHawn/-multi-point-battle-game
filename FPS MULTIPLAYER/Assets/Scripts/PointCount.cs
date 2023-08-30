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

        if (PhotonNetwork.IsConnected)
        {
            UpdateKillText();
            // Subscribe to OnPlayerPropertiesChanged event to handle real-time updates of scores from other players.
            PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
        }
    }

    private void OnDestroy()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Unsubscribe from OnPlayerPropertiesChanged event when the script is destroyed.
            PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
        }
    }

    private void OnEventReceived(EventData photonEvent)
    {
        if (photonEvent.Code == 1) // Custom event code for score update
        {
            object[] data = (object[])photonEvent.CustomData;

            int playerId = (int)data[0];
            int score = (int)data[1];

            if (playerId == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                UpdateKillText();
            }
        }
    }

    public void UpdateKillText()
    {
        int score = 0;

        if (PhotonNetwork.IsConnected)
        {
            score = PhotonNetwork.LocalPlayer.GetScore();

            // Update the kill count text on all clients using a custom event
            object[] data = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, score };
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(1, data, options, SendOptions.SendReliable);
        }

        killText.text = score.ToString();
    }
}
