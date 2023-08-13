using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
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