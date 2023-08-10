using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Manager : MonoBehaviourPunCallbacks
{
    public string playerPrefab; // 플레이어
    public Transform[] spawnPoints; // 스폰위치

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Spawn();
        }
    }

    public override void OnJoinedRoom()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (spawnPoints.Length <= 0)
        {
            Debug.LogError("Spawn points are not set");
            return;
        }

        Transform spawner = spawnPoints[Random.Range(0, spawnPoints.Length)]; // 생성되어 있는 스폰 포인트중 랜덤 스폰
        PhotonNetwork.Instantiate(playerPrefab, spawner.position, spawner.rotation); // 플레이어를 생성할 스폰포인트에 생성
    }
}
