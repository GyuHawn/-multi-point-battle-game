using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PDamage : MonoBehaviourPunCallbacks, IPunObservable
{
    private PlayerMovement player;
    public float coolTime = 1f;
    private float lastDamageTime = 0f; // ������ ������ ó�� �ð� ����� ����
    private string currentScene;
    
    private void Awake()
    {
        // ���� ���� �̸��� �����ɴϴ�.
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        player = GetComponent<PlayerMovement>();
        // ���� ���� �̸��� "Map"�� ��쿡�� PDamage ��ũ��Ʈ�� Ȱ��ȭ�մϴ�.
        if (currentScene == "Map")
        {
            enabled = true;
        }
        else
        {
            enabled = false;
        }
    }
    

    [PunRPC]
    public void TakeDamage(int p_damage)
    {
        if (photonView.IsMine)
        {
            PhotonView pv = GetComponent<PhotonView>();
            if (!pv.ObservedComponents.Contains(this))
            {
                pv.ObservedComponents.Add(this);
            }

            if (player != null)
            {
                float currentTime = Time.time; // ���� �ð� ����
                float timePassed = currentTime - lastDamageTime; // ������ ������ ó�� ���� ��� �ð� ���
                if (timePassed >= coolTime) // ������ ó�� Cool Time �� �����ٸ�
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
                    lastDamageTime = currentTime; // ������ ������ ó�� �ð� ����
                }
                else // ���� ������ ó�� Cool Time �� ������ �ʾҴٸ�
                {
                    Debug.Log("Cool Time Not Over!");
                }
            }
        }
    }

    [PunRPC]
    public void TakeDamageWithFloat(float p_damage)
    {
        if (photonView.IsMine)
        {
            PhotonView pv = GetComponent<PhotonView>();
            if (!pv.ObservedComponents.Contains(this))
            {
                pv.ObservedComponents.Add(this);
            }

            if (player != null)
            {
                player.current_health -= (int)p_damage;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (currentScene == "Map")
        {
            if (stream.IsWriting)
            {
                // ���� �÷��̾��� ������ ����
                stream.SendNext((int)player.current_health); // int�� ����ȯ�Ͽ� ����
            }
            else
            {
                // ���� �÷��̾��� ������ ����
                player.current_health = (int)stream.ReceiveNext(); // int�� ����ȯ�Ͽ� �޾ƿ�
                player.RefreshHealthBar();
            }
        }
    }

}