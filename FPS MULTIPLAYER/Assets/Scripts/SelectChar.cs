using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectChar : MonoBehaviour
{
    public Toggle[] toggles;
    public int tNum = 1;
    void Update()
    {
        if (toggles != null && toggles.Length >= 3) // �迭�� �����Ǿ���, �迭�� ũ��� 3 �̻��� ��� 
        {
            if (toggles[0].isOn)
            {
                tNum = 1;
                PlayerPrefs.SetInt("tNum", tNum); // PlayerPrefs�� ����
            }
            else if (toggles[1].isOn)
            {
                tNum = 2;
                PlayerPrefs.SetInt("tNum", tNum); // PlayerPrefs�� ����
            }
            else if (toggles[2].isOn)
            {
                tNum = 3;
                PlayerPrefs.SetInt("tNum", tNum); // PlayerPrefs�� ����
            }
        }
    }
}