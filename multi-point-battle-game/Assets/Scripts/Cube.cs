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
            Debug.Log("사망");
            transform.position = new Vector3(-70, 1.1f, 50);
            currenthealth = health;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullte"))
        {
            Debug.Log("피격");

            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            currenthealth -= bullet.damage;
            Debug.Log("현재 체력 : " + currenthealth);
        }
    }
}
