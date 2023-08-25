using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WorldGame : MonoBehaviour
{
    public GameObject Press_Key;

    public GameObject pointGame;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject == pointGame)
        {
            Press_Key.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene("Map");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Press_Key.SetActive(false);
        }
    }
}
