using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public Transform[] itemSpawnPoints;
    public GameObject spdObj;
    public GameObject dfsObj;

    private List<Transform> availableSpawnPoints = new List<Transform>();
    private float respawnTime = 10.0f; // 생성 대기 시간

    void Start()
    {
        // 시작 시 모든 생성 위치를 availableSpawnPoints 리스트에 추가
        foreach (Transform spawnPoint in itemSpawnPoints)
        {
            availableSpawnPoints.Add(spawnPoint);
        }

        // 최초에 5개의 아이템 생성
        for (int i = 0; i < 5; i++)
        {
            SpawnRandomItem();
        }
    }

    void Update()
    {
        // 생성 위치가 부족할 경우 새로운 생성 위치를 추가한다.
        if (availableSpawnPoints.Count == 0)
        {
            foreach (Transform spawnPoint in itemSpawnPoints)
            {
                availableSpawnPoints.Add(spawnPoint);
            }
        }

        // 아이템 생성 대기 시간이 지나면 랜덤한 위치에 아이템을 생성한다.
        respawnTime -= Time.deltaTime;
        if (respawnTime <= 0)
        {
            SpawnRandomItem();
            respawnTime = 10.0f;
        }
    }

    private void SpawnRandomItem()
    {
        if (availableSpawnPoints.Count > 0)
        {
            // 생성 가능한 위치 중 랜덤한 위치를 선택
            int index = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[index];

            // 생성 위치에서 아이템 생성
            if (Random.value < 0.5f) // 50%의 확률로 SpdObj, 50%의 확률로 DfsObj 생성
            {
                Instantiate(spdObj, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Instantiate(dfsObj, spawnPoint.position, Quaternion.identity);
            }

            // 생성한 위치를 사용된 위치 리스트에서 제거
            availableSpawnPoints.RemoveAt(index);
        }
    }

    // 아이템이 파괴될 경우 사용되지 않는 위치 리스트에 추가한다.
    public void AddAvailableSpawnPoint(Transform spawnPoint)
    {
        if (!availableSpawnPoints.Contains(spawnPoint))
        {
            availableSpawnPoints.Add(spawnPoint);
        }
    }
}
