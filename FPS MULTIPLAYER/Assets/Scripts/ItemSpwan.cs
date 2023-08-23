using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public Transform[] itemSpawnPoints;
    public GameObject spdObj;
    public GameObject dfsObj;

    private List<Transform> availableSpawnPoints = new List<Transform>();
    private float respawnTime = 10.0f; // ���� ��� �ð�

    void Start()
    {
        // ���� �� ��� ���� ��ġ�� availableSpawnPoints ����Ʈ�� �߰�
        foreach (Transform spawnPoint in itemSpawnPoints)
        {
            availableSpawnPoints.Add(spawnPoint);
        }

        // ���ʿ� 5���� ������ ����
        for (int i = 0; i < 5; i++)
        {
            SpawnRandomItem();
        }
    }

    void Update()
    {
        // ���� ��ġ�� ������ ��� ���ο� ���� ��ġ�� �߰��Ѵ�.
        if (availableSpawnPoints.Count == 0)
        {
            foreach (Transform spawnPoint in itemSpawnPoints)
            {
                availableSpawnPoints.Add(spawnPoint);
            }
        }

        // ������ ���� ��� �ð��� ������ ������ ��ġ�� �������� �����Ѵ�.
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
            // ���� ������ ��ġ �� ������ ��ġ�� ����
            int index = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[index];

            // ���� ��ġ���� ������ ����
            if (Random.value < 0.5f) // 50%�� Ȯ���� SpdObj, 50%�� Ȯ���� DfsObj ����
            {
                Instantiate(spdObj, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Instantiate(dfsObj, spawnPoint.position, Quaternion.identity);
            }

            // ������ ��ġ�� ���� ��ġ ����Ʈ���� ����
            availableSpawnPoints.RemoveAt(index);
        }
    }

    // �������� �ı��� ��� ������ �ʴ� ��ġ ����Ʈ�� �߰��Ѵ�.
    public void AddAvailableSpawnPoint(Transform spawnPoint)
    {
        if (!availableSpawnPoints.Contains(spawnPoint))
        {
            availableSpawnPoints.Add(spawnPoint);
        }
    }
}
