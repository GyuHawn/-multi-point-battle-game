using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PointGameStart : MonoBehaviourPunCallbacks
{
    private PlayerMovement playerMove;
    private Weapon weapon;

    public GameObject moveMap;
    public GameObject pointGamemanager;
    public GameObject waitFloor;
    public GameObject startUI;
    public GameObject pointGameUI;
    public Button startButton;
    public Transform[] pointGameSpawnPoints;

    private List<GameObject> playersOnFloor = new List<GameObject>();

    private void Start()
    {
        startUI.gameObject.SetActive(false);
        pointGameUI.gameObject.SetActive(false);
        pointGamemanager.gameObject.SetActive(false);
        startButton.onClick.AddListener(StartGame);

        playerMove = FindObjectOfType<PlayerMovement>();
        weapon = FindObjectOfType<Weapon>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PhotonView pv = collision.gameObject.GetComponent<PhotonView>();

            if (pv != null && pv.IsMine)
            {
                if (!playersOnFloor.Contains(collision.gameObject))
                {
                    playersOnFloor.Add(collision.gameObject);
                }

                startUI.gameObject.SetActive(true);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playersOnFloor.Remove(collision.gameObject);

            if (playersOnFloor.Count == 0)
            {
                startUI.gameObject.SetActive(false);
                pointGameUI.gameObject.SetActive(true);
            }
        }
    }

    private void StartGame()
    {
        RpcStartGame();
    }

    private void RpcStartGame()
    {
        moveMap.gameObject.SetActive(false);
        pointGamemanager.gameObject.SetActive(true);
        playerMove.isPointGame = true;
        weapon.isWeapon = true;
        
        List<Transform> availableSpawns = new List<Transform>(pointGameSpawnPoints);

        foreach (GameObject player in playersOnFloor)
        {
            int spawnIndex = Random.Range(0, availableSpawns.Count);

            player.transform.position = availableSpawns[spawnIndex].position;

            availableSpawns.RemoveAt(spawnIndex);
        }

        playersOnFloor.Clear();

        startUI.gameObject.SetActive(false);
    }
}