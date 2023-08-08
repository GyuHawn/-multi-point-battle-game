using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Manager : MonoBehaviour
{
    public string playerPrefab; // 플레이어
    public Transform[] spawnPoints; // 스폰위치

    private void Start()
    {
        Spawn();
        
    }

    public void Spawn()
    {
        Transform spawner = spawnPoints[Random.Range(0, spawnPoints.Length)]; // 생성되어 있는 스폰 포인트중 랜덤 스폰
        PhotonNetwork.Instantiate(playerPrefab, spawner.position, spawner.rotation); // 플레이어를 생성할 스폰포인트에 생성
    }
}
