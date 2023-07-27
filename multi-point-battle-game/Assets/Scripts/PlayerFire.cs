using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // �Ѿ� ���� ��ġ
    public GameObject bulletObj;

    // �ѱ�
    public GameObject firePosition;

    // �Ѿ� �̵� �ӵ�
    public float bulletSpeed = 10f;

    void Update()
    {
        // ��ư �Է½� �߻�
        if (Input.GetButtonDown("Fire1"))
        {
            // �Ѿ� ����
            GameObject bullet = Instantiate(bulletObj);

            // ī�޶� ���� ������ ������ �߻�ǵ��� ����
            bullet.transform.forward = Camera.main.transform.forward;

            // �Ѿ� �߻� ��, �ӵ��� �߰�����.
            Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
            rigidbody.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);

            // �Ѿ� �߻�
            bullet.transform.position = firePosition.transform.position;
        }
    }
}
