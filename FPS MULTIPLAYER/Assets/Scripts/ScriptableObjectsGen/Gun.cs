using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Gun", menuName ="Gun")]
public class Gun : ScriptableObject
{
    public string gunName; // 총 이름

    public int damage; // 데미지
    public float firerate; // 연사속도
    public float spread; // 탄 퍼짐
    public float recoil; // 총 반동 회전값 
    public float rebound; // 총 반동
    public float aimSpeed; // 마우스 감도(속도)

    public int maxAmmo; // 최대 탄창 수
    public int clipsize; // 장전시 최대 탄창 수
    private int clip; // 장전 탄창 수 초기화
    private int currentAmmo; // 현재 탄창 수 초기화

    public float reload; // 장전
    public int burst; // 총을 확인하여 총에 따라 설정 변경 (0 - 주무기(semi), 1 - 권총(auto), 2 - x(burst))

    public GameObject prefab; // 지정할 무기 (프리팹 지정)

    public void Initialize() // 설정한 총의 탄창 수 초기화 
    {
        currentAmmo = maxAmmo;
        clip = clipsize;
    }

    public bool FireBullet() // 탄창 없을때 총을 쏘지 못하도록 설정
    {
        if (clip > 0)
        {
            clip -= 1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Reload() // 총 장전시 탄창 수 줄이기
    {
        currentAmmo += clip;
        clip = Mathf.Min(clipsize, currentAmmo); // 현재 총알 수와 최대 탄창 수를 비교하여 작은 값을 반환
        currentAmmo -= clip;    
    }

    public int GetStash() // 현재 소유한 전체 탄약 수 반환
    {
        return currentAmmo;
    }

    public int GetClip() // 장전된 탄창 안의 총알 수를 반환
    {
        return clip;
    }
}
