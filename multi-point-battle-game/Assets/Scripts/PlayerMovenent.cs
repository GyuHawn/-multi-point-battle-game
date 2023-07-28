using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;
using Photon.Pun;
using Cinemachine;

public class PlayerMovenent : MonoBehaviour
{
    public int health = 100;
    public int currenthealth = 100;

    private CharacterController controller;
    private new Transform transform;
    private Animator animator;
    private new Camera camera;
    
    // ������ Plane�� ����ĳ�����ϱ� ���� ����
    private Plane plane;// ������ ������ ������ �ٴ��� �����ϱ� ���� Plane ����ü
    private Ray ray; // ����
    private Vector3 hitPoint; // ������ �ε����� ����

    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");

    // �̵��ӵ�
    public float moveSpeed = 10.0f;

    // ī�޶� ���� ����
    private PhotonView pv;
    private CinemachineVirtualCamera virtualCamera;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        camera = Camera.main;

        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
    }

    void Start()
    {
        // �ڽ��� ĳ������ ��� �ó׸ӽ� ī�޶� ����
        /*if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }*/
        virtualCamera.Follow = transform;
        virtualCamera.LookAt = transform;

        // ������ �ٴ��� �������� ���ΰ��� ��ġ�� ����
        plane = new Plane(transform.up, transform.position);
    }

    void Update()
    {
        // �ڽ��� ĳ����(��Ʈ��ũ ��ü)�� ��쿡�� ����
        /*if (pv.IsMine)
        {
            Move();
            Turn();
        }*/
        Move();
        Turn();

        // FŰ�� ���� ���� Ȱ��ȭ ����
        if (Input.GetKey(KeyCode.F))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.45f, transform.position.z);
        }

        // ĳ���Ͱ� ����� �������� ������
        if (currenthealth <= 0)
        {
            Debug.Log("���");
            transform.position = new Vector3(-70, 1.1f, 50);
            currenthealth = health;
        }
    }


    void Move()
    {
        Vector3 cameraForward = camera.transform.forward; // �յ�
        Vector3 cameraRight = camera.transform.right; // �翷

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        // ȸ���� �÷��̾ �������� �̵� ���� ���
        Vector3 moveDir = (transform.forward * v) + (transform.right * h);
        moveDir.Set(moveDir.x, 0.0f, moveDir.z);

        // ĳ���� �̵� ó��
        controller.SimpleMove(moveDir * moveSpeed);

        // ĳ���� �ִϸ��̼� ó��
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);
        animator.SetBool("Move", true);
    }


    void Turn()
    {
        // ���콺�� 2���� ��ǩ���� �̿��� 3���� ���̸� ����
        ray = camera.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ���� ���� ����
        float enter = 0.0f;

        // ������ �ٴڿ� ray�� �߻��� �浹 ������ �Ÿ��� enter ������ ��ȯ
        plane.Raycast(ray, out enter);

        // ������ �ٴڿ� ���̰� �浹�� ��ǩ���� ����
        hitPoint = ray.GetPoint(enter);

        // ȸ���ؾ� �� ������ ���͸� ���
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0.0f;

        // ĳ������ ȸ���� ����
        transform.localRotation = Quaternion.LookRotation(lookDir);
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
/*
  public int health = 100;
    public int currenthealth = 100;

    private CharacterController controller;
    private new Transform transform;
    private Animator animator;
    private new Camera camera;
    
    // ������ Plane�� ����ĳ�����ϱ� ���� ����
    private Plane plane;// ������ ������ ������ �ٴ��� �����ϱ� ���� Plane ����ü
    private Ray ray; // ����
    private Vector3 hitPoint; // ������ �ε����� ����

    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");

    // �̵��ӵ�
    public float moveSpeed = 10.0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    void Start()
    {
        // ������ �ٴ��� �������� ���ΰ��� ��ġ�� ����
        plane = new Plane(transform.up, transform.position);
    }

    void Update()
    {
        // �ڽ��� ĳ����(��Ʈ��ũ ��ü)�� ��쿡�� ����
        Move();
        Turn();

        if (Input.GetKey(KeyCode.F))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.45f, transform.position.z);
        }
        if (currenthealth <= 0)
        {
            Debug.Log("���");
            transform.position = new Vector3(-70, 1.1f, 50);
            currenthealth = health;
        }
    }


    void Move()
    {
        Vector3 cameraForward = camera.transform.forward; // �յ�
        Vector3 cameraRight = camera.transform.right; // �翷

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        // ȸ���� �÷��̾ �������� �̵� ���� ���
        Vector3 moveDir = (transform.forward * v) + (transform.right * h);
        moveDir.Set(moveDir.x, 0.0f, moveDir.z);

        // ĳ���� �̵� ó��
        controller.SimpleMove(moveDir * moveSpeed);

        // �÷��̾��� y��ġ�� ����ؼ� ���ҽ�ŵ�ϴ�.
        //transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);

        // ĳ���� �ִϸ��̼� ó��
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);
        animator.SetBool("Move", true);
    }


    void Turn()
    {
        // ���콺�� 2���� ��ǩ���� �̿��� 3���� ���̸� ����
        ray = camera.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ���� ���� ����
        float enter = 0.0f;

        // ������ �ٴڿ� ray�� �߻��� �浹 ������ �Ÿ��� enter ������ ��ȯ
        plane.Raycast(ray, out enter);

        // ������ �ٴڿ� ���̰� �浹�� ��ǩ���� ����
        hitPoint = ray.GetPoint(enter);

        // ȸ���ؾ� �� ������ ���͸� ���
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0.0f;

        // ĳ������ ȸ���� ����
        transform.localRotation = Quaternion.LookRotation(lookDir);
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
 */