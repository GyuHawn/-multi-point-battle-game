using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpwan : MonoBehaviour
{
    public Transform[] sPSP; // SkyPointSpawnPoints // 0 - 11.3 (26) / 1 - 20.3 (35)
    public Transform[] gPSP; // GroundPointSpawnPoints
    public GameObject[] SPoint;
    public GameObject GPoint;

    private List<Transform> usedSpawnPoints = new List<Transform>(); // Track used spawn points

    private void Start()
    {
        SpawnGPoint();
        SpawnSPoint();
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
            Instantiate(GPoint, spawnPoint.position, Quaternion.identity);
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

                // Apply rotation only for SpawnPoint1
                if (spawnpoint.name == "SpawnPoint1")
                    rotation = Quaternion.Euler(0f, 90f, 0f);
                else
                    rotation = Quaternion.identity; // 기본적인 회전값

                // Set the desired y-axis position for SPoint[0] and SPoint[1]
                Vector3 spawnPosition = spawnpoint.position;
                if (i == 0) // SPoint[0]
                    spawnPosition.y = 26f;
                else if (i == 1) // SPoint[1]
                    spawnPosition.y = 35f;

                GameObject spawnedObject = Instantiate(pointPrefab, spawnPosition, rotation);
                spawnedObject.transform.SetParent(transform);

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
