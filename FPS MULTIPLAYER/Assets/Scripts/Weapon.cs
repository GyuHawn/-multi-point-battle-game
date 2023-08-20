using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class Weapon : MonoBehaviourPunCallbacks
{
    public Gun[] loadout; // 사용할 총의 배열 변수
    public Transform weaponParent; // 총을 배치할 위치
    private GameObject currentWeapon; // 현재 총의 상태
    private int gunIndex; // loadout 배열의 번호
    public GameObject bulletHolePrefab; // 총탄 자국 오브젝트
    public LayerMask canBeShot; // 총알 충돌 레이어
    private float shotDelay; // 발사 딜레이
    private bool isReloading; // 재장전
    public bool isAiming = false; // 조준중인지

    private PDamage pDamage;
    private PlayerMovement playerMove;

    private void Awake()
    {
        pDamage = FindObjectOfType<PDamage>();
        playerMove = FindObjectOfType<PlayerMovement>();
    }

    void Start()
    {
        foreach (Gun a in loadout) a.Initialize(); // 배열의 각 총 초기화
        Equip(0); // 초기총(1번) 장착
    }

    void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha1)) // 1번 키로 총 장착
        {
            photonView.RPC("Equip", RpcTarget.All, 0);
        }
        if(photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha2)) // 2번 키로 권총 장착
        {
            photonView.RPC("Equip", RpcTarget.All, 1);
        }
           
        if(currentWeapon != null) // 총이 있을때
        {
            if(photonView.IsMine)  // 자신일때
            {
                Aim(Input.GetMouseButton(1)); // 조준(삭제 예정)
                if(loadout[gunIndex].burst != 1)
                {

                }
                if (Input.GetMouseButtonDown(0) && shotDelay <= 0 ) // 왼쪽키를 누르고 딜레이 0일때
                {
                    if (loadout[gunIndex].FireBullet())
                        photonView.RPC("Shoot", RpcTarget.All);
                    else StartCoroutine(Reload(loadout[gunIndex].reload));

                }
                else
                {
                    if (Input.GetMouseButton(0) && shotDelay < 0)
                    {
                        if (loadout[gunIndex].FireBullet())
                            photonView.RPC("Shoot", RpcTarget.All);
                        else StartCoroutine(Reload(loadout[gunIndex].reload));
                    }
                }

                if(Input.GetKeyDown(KeyCode.R)) // R키로 장전
                {
                    StartCoroutine(Reload(loadout[gunIndex].reload));
                }

                // 발사 딜레이(연사력)
                if (shotDelay > 0)
                {
                    shotDelay -= Time.deltaTime;
                }
            }
            
            //무기 반동
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);                
        }   
    }

    IEnumerator Reload(float p_wait) // 재장전 코루틴
    {
        isReloading = true; // 재장전 중
        currentWeapon.SetActive(false); // 총 비활성화

        yield return new WaitForSeconds(p_wait);

        loadout[gunIndex].Reload(); // 선택한 총 재장전
        currentWeapon.SetActive(true); // 총 활성화
        isReloading = false; // 재정전 끝
    }

    [PunRPC]
    void Equip(int p_ind) // 네트워크에서 총기 장착 처리
    {
        if (currentWeapon != null) // 총이 있을때
        {
            if(isReloading) // 재장전 멈춤
            StopCoroutine("Reload"); // 코루틴 정지
            Destroy(currentWeapon); // 현재 총 파괴

        }

        gunIndex = p_ind; // 장착할 총의 번호 저장
        // 새로운 총 생성
        GameObject t_newWeapon = Instantiate(loadout[p_ind].prefab,weaponParent.position,weaponParent.rotation,weaponParent) as GameObject;
        t_newWeapon.transform.localPosition = Vector3.zero; // 총을 원래 위치로 이동
        t_newWeapon.transform.localEulerAngles = Vector3.zero;
        t_newWeapon.GetComponent<Sway>().enabled = photonView.IsMine; // 총의 Sway 설정

        currentWeapon = t_newWeapon; // 현재의 총 상태를 새로운 총으로 변경
    }

    void Aim(bool p_isAiming) // 조준
    {
        isAiming = p_isAiming; // 조준 중인지
        Transform t_anchor = currentWeapon.transform.Find("Anchor"); // 총의 위치 찾기
        Transform t_state_ads = currentWeapon.transform.Find("States/Ads");
        Transform t_state_hip = currentWeapon.transform.Find("States/Hip");
        if (p_isAiming)
        {
            // aim
            //t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ads.position, Time.deltaTime * loadout[gunIndex].aimSpeed);
        }
        else
        {
            // 비 조준 중일때
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_hip.position, Time.deltaTime * loadout[gunIndex].aimSpeed);
        }


    }

    [PunRPC]
    void Shoot() // 발사
    {
        Transform t_spawn = transform.Find("Normal Camera"); // 총의 발사 위치

        // 탄 퍼짐
        Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f; // 총알이 날아가는 위치
            t_bloom += Random.Range(-loadout[gunIndex].spread, loadout[gunIndex].spread) * t_spawn.up; // 탄퍼짐 정도 설정
            t_bloom += Random.Range(-loadout[gunIndex].spread, loadout[gunIndex].spread) * t_spawn.right;
            t_bloom -= t_spawn.position; // 총알이 날아가는 방향
            t_bloom.Normalize(); // 방향 정규화

        // 연사력
        shotDelay = loadout[gunIndex].firerate; // 연사력 설정

        // 레이케스트
        RaycastHit t_hit = new RaycastHit(); // 총알 충돌 정보
        if (Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 1000f, canBeShot)) // 충돌 확인
        {
            if (Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 1000f, canBeShot))
            {
                if (t_hit.collider.gameObject.layer == 9) // 총알이 다른 플레이어 충돌시
                {
                    t_hit.collider.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[gunIndex].damage); // 다른 플레이어 데미지 입힘
                }
                else if (t_hit.collider.CompareTag("Player"))
                {
                    // do nothing if the hit object is player
                }
                else
                {
                    // 총탄자국 생성
                    GameObject t_newBulletHole = Instantiate(bulletHolePrefab, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject; // 충돌 지점 총탄자국 생성
                    t_newBulletHole.transform.LookAt(t_hit.point + t_hit.normal); // 총알 충돌 지점을 기준으로 회전
                    Destroy(t_newBulletHole, 5f); // 5초후 자국 파괴
                }
            }


            if (photonView.IsMine) // 자신인지
            { 
                if(t_hit.collider.gameObject.layer == 9) // 총알이 다른 플레이어 충돌시
                {
                    t_hit.collider.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[gunIndex].damage); // 다른 플레이어 데미지 입힘
                }
            }
        }
        currentWeapon.transform.Rotate(-loadout[gunIndex].recoil,0,0); // 무기의 회전값 지정
        currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[gunIndex].rebound; // 무기 반동값 지정 
    }

    public void RefreshAmmo(Text p_text) // 현재 탄창 정보 UI 업데이트
    {
        int t_clip = loadout[gunIndex].GetClip(); // 탄창 정보를 변수에 저장
        int t_stache = loadout[gunIndex].GetStash();

        p_text.text = t_clip.ToString() + " / " + t_stache.ToString(); // 탄창 정보를 문자열로 표시
    }


   
}
