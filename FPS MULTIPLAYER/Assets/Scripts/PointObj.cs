using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class PointObj : MonoBehaviourPunCallbacks, IPunObservable
{
    private string currentScene;

    public float max_health;
    public float current_health;

    private Vector3 initialPosition;
    private bool moveForward = true;

    public float moveDistance = 10f;
    public float moveSpeed = 200f;

    public float coolTime = 1f;
    private float lastDamageTime = 0f; // ������ ������ ó�� �ð� ����� ����

    private void Awake()
    {
        // ���� ���� �̸��� �����ɴϴ�.
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        // ���� ���� �̸��� "Map"�� ��쿡�� PDamage ��ũ��Ʈ�� Ȱ��ȭ�մϴ�.
        if (currentScene == "Map")
        {
            enabled = true;
        }
        else
        {
            enabled = false;
        }
    }

    void Start()
    {
        gameObject.layer = 11;
        current_health = max_health;
        // �ʱ� ��ġ ����
        initialPosition = transform.position;
    }

    void Update()
    {
        if (moveForward)
        {
            // ������ �̵�
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            // �̵� �Ÿ� Ȯ���Ͽ� ó�� ��ġ�� ���ư�
            if (transform.position.z >= initialPosition.z + moveDistance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, initialPosition.z + moveDistance);
                moveForward = false; // �ڷ� �̵��ϱ� ���� ���� ����
            }
        }
        else
        {
            // �ڷ� �̵�
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

            // �̵� �Ÿ� Ȯ���Ͽ� ó�� ��ġ�� ���ư�
            if (transform.position.z <= initialPosition.z - moveDistance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, initialPosition.z - moveDistance);
                moveForward = true; // ������ �̵��ϱ� ���� ���� ����
            }
        }

        // X�� Y ���� ������ ���� (������ �κ�)
        var newPosition = new Vector3(initialPosition.x, initialPosition.y, transform.position.z);
        transform.SetPositionAndRotation(newPosition, Quaternion.identity);
    }

    [PunRPC]
    public void PointUp(int p_damage)
    {
        float currentTime = Time.time; // ���� �ð� ����
        float timePassed = currentTime - lastDamageTime; // ������ ������ ó�� ���� ��� �ð� ���

        if (timePassed >= coolTime) // ������ ó�� Cool Time �� �����ٸ�
        {
            current_health -= p_damage;

            Debug.Log("������");
            if (current_health <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            lastDamageTime = currentTime; // ������ ������ ó�� �ð� ����
        }
        else // ���� ������ ó�� Cool Time �� ������ �ʾҴٸ�
        {
            Debug.Log("Cool Time Not Over!");
        }


    }

    [PunRPC]
    public void PointUpWithFloat(float p_damage)
    {
        current_health -= (int)p_damage;

        if (current_health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (currentScene == "Map")
        {
            if (stream.IsWriting)
            {
                // ���� �÷��̾��� ������ ����
                stream.SendNext((int)current_health); // int�� ����ȯ�Ͽ� ����
            }
            else
            {
                // ���� �÷��̾��� ������ ����
                current_health = (int)stream.ReceiveNext(); // int�� ����ȯ�Ͽ� �޾ƿ�
            }
        }
    }
}
