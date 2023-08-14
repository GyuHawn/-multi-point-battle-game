using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    private PlayerMovement player;
    public Text p_text;

    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    [PunRPC]
    public void TakeDamage(int p_damage)
    {
        if (photonView.IsMine)
        {
            player.current_health -= p_damage;
            player.RefreshHealthBar();
            Debug.Log(player.current_health);

            if (player.current_health <= 0)
            {
                player.manager.Spawn();
                PhotonNetwork.Destroy(gameObject);
                Debug.Log("YOU DIED");
            }
        }
    }
}