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
        pdamage = FindObjectOfType<PDamage>(); // PDamage ��ũ��Ʈ reference ����
        killText.text = "Kills: 0";
    }

    void Update()
    {
        if (pdamage != null)
        {
            // PDamage ��ũ��Ʈ���� �޾ƿ� killCount ���� UI Text�� �ݿ�
            killText.text = pdamage.killCount.ToString();
        }
    }
}
