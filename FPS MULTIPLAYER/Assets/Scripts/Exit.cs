using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // PhotonNetwork�� ����ϱ� ���� �߰�

public class Exit : MonoBehaviour
{
    public void OnExitButtonClick() // �Ű� ���� ���� public �Լ��� ���� ��ư OnClick()�� �����մϴ�.
    {
        PhotonNetwork.LoadLevel("Menu"); // "Menu" ������ �̵��ϴ� �ڵ��Դϴ�.
    }
}
