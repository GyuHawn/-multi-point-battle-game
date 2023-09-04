using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class PointSpwan : MonoBehaviourPunCallbacks
{
    public Transform[] sPSP; // SkyPointSpawnPoints
    public Transform[] gPSP; // GroundPointSpawnPoints
    public GameObject[] SPoint;
    public GameObject GPoint;

    public bool isTargetDestroyed = false; // ǥ���� ���ŵǾ����� ���θ� ��Ÿ���� ����
    private const float RespawnDelay = 1f; // ǥ�� ����� ������ �ð� (��)
    private float respawnTimer = 0f; // ǥ�� ����� Ÿ�̸�

    private List<Transform> usedSpawnPoints = new List<Transform>(); // Track used spawn points

    private void Start()
    {
        SpawnGPoint();
        SpawnSPoint();
    }

    void Update()
    {
        if (isTargetDestroyed)
        {
            respawnTimer += Time.deltaTime;

            if (respawnTimer >= RespawnDelay)
            {
                usedSpawnPoints.Clear(); // Reset the list of used spawn points

                SpawnGPoint();
                SpawnSPoint();

                isTargetDestroyed = false;
                respawnTimer = 0f;
            }
        }
    }

    private void SpawnGPoint()
    {
        if (gPSP.Length == 0)
        {
            Debug.LogWarning("No Ground Point Spawn Points assigned.");
            return;
        }

        if (GPoint == null)
        {
            Debug.LogWarning("Ground Point prefab is not assigned.");
            return;
        }

        int randomIndex = Random.Range(0, gPSP.Length);
        Transform spawnPoint = gPSP[randomIndex];

        if (!usedSpawnPoints.Contains(spawnPoint))
        {
            PhotonNetwork.Instantiate(GPoint.name, spawnPoint.position, Quaternion.identity);
            usedSpawnPoints.Add(spawnPoint);
        }
        
    }
    private void SpawnSPoint()
    {
        if (sPSP.Length == 0)
        {
            Debug.LogWarning("No Sky Point Spawn Points assigned.");
            return;
        }

        if (SPoint.Length == 0)
        {
            Debug.LogWarning("No Sky Point prefabs assigned.");
            return;
        }

        for (int i = 0; i < SPoint.Length; i++)
        {
            if (usedSpawnPoints.Count >= sPSP.Length)
                break;

            GameObject pointPrefab = SPoint[i];

            Transform spawnpoint = GetRandomUnusedSkySpawnpoint();

            if (spawnpoint != null && pointPrefab != null)
            {
                Quaternion rotation = Quaternion.identity;

                if (spawnpoint.name == "SpawnPoint1")
                    rotation = Quaternion.Euler(0f, 90f, 0f);
                else
                    rotation = Quaternion.identity; // �⺻���� ȸ����

                Vector3 spawnPosition = spawnpoint.position;
                if (i == 0) // SPoint[0]
                    spawnPosition.y = 26f;
                else if (i == 1) // SPoint[1]
                    spawnPosition.y = 35f;

                GameObject spawnedObject = PhotonNetwork.Instantiate(pointPrefab.name, spawnPosition, rotation);

                if (!usedSpawnPoints.Contains(spawnpoint))
                    usedSpawnPoints.Add(spawnpoint);
            }
        }
    }

    private Transform GetRandomUnusedSkySpawnpoint()
     {
         List<Transform> unusedSkySpawnpints = new List<Transform>(sPSP);

         foreach(Transform used in usedSpawnPoints)
        {
             unusedSkySpawnpints.Remove(used);
         }

         if(unusedSkySpawnpints.Count > 0)
         {
             int randomIndex= Random.Range(0, unusedSkySpawnpints.Count);
             return unusedSkySpawnpints[randomIndex];
          }
          else
              return null;
      }
}
