using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillText : MonoBehaviour
{
    private PDamage pdamage;
    public Text killText;

    void Start()
    {
        pdamage = FindObjectOfType<PDamage>(); // PDamage 스크립트 reference 설정
        killText.text = "Kills: 0";
    }

    void Update()
    {
        if (pdamage != null)
        {
            // PDamage 스크립트에서 받아온 killCount 값을 UI Text에 반영
            killText.text = pdamage.killCount.ToString();
        }
    }
}
