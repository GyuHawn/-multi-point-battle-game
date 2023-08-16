using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Random = UnityEngine.Random;

public class Manager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs; // 플레이어
    public Transform[] spawnPoints; // 스폰위치

    private SelectChar select;

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            LoadTNum();
            Spawn();
        }
    }

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        LoadTNum(); // PlayerPrefs에서 값을 읽어옴
    }

    public override void OnJoinedRoom()
    {
        Spawn();
    }

    void LoadTNum()
    {
        select.tNum = PlayerPrefs.GetInt("tNum", 1); // PlayerPrefs에서 값을 읽어옴, 저장된 값이 없으면 1을 기본값으로 사용
    }

    public void Spawn()
    {
        if (spawnPoints.Length <= 0)
        {
            Debug.LogError("Spawn points are not set");
            return;
        }
        if (select.tNum == 1)
        {
            Transform spawner = spawnPoints[Random.Range(0, spawnPoints.Length)]; // 생성되어 있는 스폰 포인트중 랜덤 스폰
            GameObject selectedPlayerPrefab = playerPrefabs[0];
            PhotonNetwork.Instantiate(selectedPlayerPrefab.name, spawner.position, spawner.rotation); // 선택된 플레이어 프리팹 생성
        }
        else if (select.tNum == 2)
        {
            Transform spawner = spawnPoints[Random.Range(0, spawnPoints.Length)]; // 생성되어 있는 스폰 포인트중 랜덤 스폰
            GameObject selectedPlayerPrefab = playerPrefabs[1];
            PhotonNetwork.Instantiate(selectedPlayerPrefab.name, spawner.position, spawner.rotation); // 선택된 플레이어 프리팹 생성
        }
        else if (select.tNum == 3)
        {
            Transform spawner = spawnPoints[Random.Range(0, spawnPoints.Length)]; // 생성되어 있는 스폰 포인트중 랜덤 스폰
            GameObject selectedPlayerPrefab = playerPrefabs[2];
            PhotonNetwork.Instantiate(selectedPlayerPrefab.name, spawner.position, spawner.rotation); // 선택된 플레이어 프리팹 생성
        }

    }
}
