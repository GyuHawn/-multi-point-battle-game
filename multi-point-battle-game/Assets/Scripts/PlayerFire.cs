using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // �Ѿ� ���� ��ġ
    public GameObject bulletObj;

    // �ѱ�
    public GameObject firePosition;
    
    void Update()
    {
        // ��ư �Է½� �߻�
        if (Input.GetButtonDown("Fire1"))
        {
            // �Ѿ� ����
            GameObject bullet = Instantiate(bulletObj);

            // �Ѿ� �߻�
            bullet.transform.position = firePosition.transform.position;
        }
    }
}
