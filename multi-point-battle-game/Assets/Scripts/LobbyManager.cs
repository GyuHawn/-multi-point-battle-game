using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerNameInputField;
    public TMP_InputField roomNameInputField;
    public Button joinButton;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnPlayButtonClicked()
    {
        string playerName = playerNameInputField.text;
        string roomName = roomNameInputField.text;
        byte maxPlayers = 20;

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000 };

        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
        PhotonNetwork.LocalPlayer.NickName = playerName;
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Main");
        }
    }
}

