using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // PhotonNetwork를 사용하기 위해 추가

public class Exit : MonoBehaviour
{
    public void OnExitButtonClick() // 매개 변수 없는 public 함수를 만들어서 버튼 OnClick()에 연결합니다.
    {
        PhotonNetwork.LoadLevel("Menu"); // "Menu" 씬으로 이동하는 코드입니다.
    }
}
