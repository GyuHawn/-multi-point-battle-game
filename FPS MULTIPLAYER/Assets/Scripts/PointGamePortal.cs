using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PointGamePortal : MonoBehaviourPunCallbacks
{
    public Transform pointWaitSpawnPoints;

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
                if (collision.contacts[0].thisCollider.gameObject == this.gameObject)
                {
                    PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
                    playerMovement.transform.position = pointWaitSpawnPoints.position;
                }
            }
        }
    }

}