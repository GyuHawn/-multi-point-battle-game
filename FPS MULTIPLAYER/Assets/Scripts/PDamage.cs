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
    private float lastDamageTime = 0f; // 마지막 데미지 처리 시간 저장용 변수

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

            // PlayerMovement 스크립트 접근 방식을 변경합니다.
            player = GetComponent<PlayerMovement>();

            if (player != null)
            {
                float currentTime = Time.time; // 현재 시간 저장
                float timePassed = currentTime - lastDamageTime; // 마지막 데미지 처리 이후 경과 시간 계산
                if (timePassed >= coolTime) // 데미지 처리 Cool Time 이 지났다면
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
                    lastDamageTime = currentTime; // 마지막 데미지 처리 시간 갱신
                }
                else // 아직 데미지 처리 Cool Time 이 지나지 않았다면
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

            // PlayerMovement 스크립트 접근 방식을 변경합니다.
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

            // 현재 킬 카운트를 다른 플레이어들에게 전파
            photonView.RPC("OnKill", RpcTarget.OthersBuffered, this.killCount);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 로컬 플레이어의 데이터 전송
            stream.SendNext(player.current_health);
            stream.SendNext(killCount);
        }
        else
        {
            // 원격 플레이어의 데이터 수신
            int health = (int)stream.ReceiveNext();
            player.current_health = health;
            player.RefreshHealthBar();

            int count = (int)stream.ReceiveNext();
            killCount = count;
        }
    }
}