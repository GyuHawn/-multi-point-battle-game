using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class PointGameStart : MonoBehaviourPunCallbacks
{
    public Transform pointWaitSpawnPoints;
    public Transform[] pointGameSpawnPoints;

    void Start()
    {
        pointWaitSpawnPoints = GameObject.Find("WaitPoint").transform;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {

            }
        }
    }
}
