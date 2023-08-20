using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class PDamage : MonoBehaviourPunCallbacks, IPunObservable
{
    private PlayerMovement player;
    public float coolTime = 1f;
    private float lastDamageTime = 0f; // ������ ������ ó�� �ð� ����� ����

    public int killCount = 0;

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

            // PlayerMovement ��ũ��Ʈ ���� ����� �����մϴ�.
            player = GetComponent<PlayerMovement>();

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
                        if (photonView.IsMine)
                        {
                            photonView.RPC("OnKill", RpcTarget.AllBuffered, killCount);
                        }
                        player.manager.Spawn();
                        PhotonNetwork.Destroy(gameObject);
                        killCount++;
                        Debug.Log("YOU KILLED! Kill Count: " + killCount);
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

            // PlayerMovement ��ũ��Ʈ ���� ����� �����մϴ�.
            player = GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.current_health -= (int)p_damage;
                player.RefreshHealthBar();
                Debug.Log(player.current_health);

                if (player.current_health <= 0)
                {
                    if (photonView.IsMine)
                    {
                        photonView.RPC("OnKill", RpcTarget.AllBuffered, killCount);
                    }
                    player.manager.Spawn();
                    PhotonNetwork.Destroy(gameObject);
                    killCount++;
                    Debug.Log("YOU DIED");

                }
            }
        }
    }

    [PunRPC]
    void OnKill(int killCount)
    {
        if (photonView && photonView.IsMine)
        {
            this.killCount = killCount + 1;
            Debug.Log("YOU DIED. Kill Count: " + killCount);

            // ���� ų ī��Ʈ�� �ٸ� �÷��̾�鿡�� ����
            photonView.RPC("OnKill", RpcTarget.OthersBuffered, this.killCount);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ���� �÷��̾��� ������ ����
            stream.SendNext(player.current_health);
            stream.SendNext(killCount);
        }
        else
        {
            // ���� �÷��̾��� ������ ����
            int health = (int)stream.ReceiveNext();
            player.current_health = health;
            player.RefreshHealthBar();

            int count = (int)stream.ReceiveNext();
            killCount = count;
        }
    }
}