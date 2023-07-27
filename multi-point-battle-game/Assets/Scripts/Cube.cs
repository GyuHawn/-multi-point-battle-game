using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube: MonoBehaviour
{
    public int health = 100;
    public int currenthealth = 100;

    void Update()
    {
        if (currenthealth <= 0)
        {
            Debug.Log("���");
            transform.position = new Vector3(-70, 1.1f, 50);
            currenthealth = health;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullte"))
        {
            Debug.Log("�ǰ�");

            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            currenthealth -= bullet.damage;
            Debug.Log("���� ü�� : " + currenthealth);
        }
    }
}
