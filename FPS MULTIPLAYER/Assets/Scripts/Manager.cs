using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Random = UnityEngine.Random;

public class Manager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public Transform[] mainWorldSpawnPoints;

    private SelectChar select;

    public static GameObject currentPlayer;

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            select = FindObjectOfType<SelectChar>();
            if (select != null)
            {
                LoadTNum();
                Spawn();
            }
        }
    }

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        Spawn();
    }

    void LoadTNum()
    {
        select.tNum = PlayerPrefs.GetInt("tNum", 1);
    }


    public void Spawn()
    {
        if (mainWorldSpawnPoints.Length <= 0)
        {
            Debug.LogError("Spawn points are not set");
            return;
        }
        if (select.tNum == 1)
        {
            Transform spawner = mainWorldSpawnPoints[Random.Range(0, mainWorldSpawnPoints.Length)]; // 생성되어 있는 스폰 포인트중 랜덤 스폰
            GameObject selectedPlayerPrefab = playerPrefabs[0];
            currentPlayer = PhotonNetwork.Instantiate(selectedPlayerPrefab.name, spawner.position, spawner.rotation); // 선택된 플레이어 프리팹 생성
        }
        else if (select.tNum == 2)
        {
            Transform spawner = mainWorldSpawnPoints[Random.Range(0, mainWorldSpawnPoints.Length)]; // 생성되어 있는 스폰 포인트중 랜덤 스폰
            GameObject selectedPlayerPrefab = playerPrefabs[1];
            currentPlayer = PhotonNetwork.Instantiate(selectedPlayerPrefab.name, spawner.position, spawner.rotation); // 선택된 플레이어 프리팹 생성
        }
        else if (select.tNum == 3)
        {
            Transform spawner = mainWorldSpawnPoints[Random.Range(0, mainWorldSpawnPoints.Length)]; // 생성되어 있는 스폰 포인트중 랜덤 스폰
            GameObject selectedPlayerPrefab = playerPrefabs[2];
            currentPlayer = PhotonNetwork.Instantiate(selectedPlayerPrefab.name, spawner.position, spawner.rotation); // 선택된 플레이어 프리팹 생성
        }

    }

    public void RespawnPlayer()
    {
        if (mainWorldSpawnPoints.Length <= 0)
        {
            Debug.LogError("Spawn points are not set");
            return;
        }

        Transform spawner = mainWorldSpawnPoints[Random.Range(0, mainWorldSpawnPoints.Length)];
        currentPlayer.transform.position = spawner.position;
    }
}
