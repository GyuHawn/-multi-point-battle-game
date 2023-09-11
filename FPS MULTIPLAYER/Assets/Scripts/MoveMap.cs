using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveMap : MonoBehaviourPunCallbacks
{
    public GameObject mMenu;
    public Transform[] moveMapPoints;

    public void MoveMenu()
    {
        if (mMenu != null)
        {
            mMenu.SetActive(!mMenu.activeSelf);
        }
    }

    public void PointGameMove()
    {
        if (moveMapPoints.Length > 0 && Manager.currentPlayer != null)
        {
            if (mMenu != null)
            {
                mMenu.SetActive(false);
            }

            Manager.currentPlayer.transform.position = moveMapPoints[0].position;
        }
    }

    public void DodgeGameMove()
    {
        if (moveMapPoints.Length > 0 && Manager.currentPlayer != null)
        {
            if (mMenu != null)
            {
                mMenu.SetActive(false);
            }

            Manager.currentPlayer.transform.position = moveMapPoints[1].position;
        }
    }
}
