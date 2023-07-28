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
    
    // 가상의 Plane에 레이캐스팅하기 위한 변수
    private Plane plane;// 지정한 지역에 가상의 바닥을 생성하기 위한 Plane 구조체
    private Ray ray; // 광선
    private Vector3 hitPoint; // 광선이 부딪히는 지점

    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");

    // 이동속도
    public float moveSpeed = 10.0f;

    // 카메라를 위한 변수
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
        // 자신의 캐릭터일 경우 시네머신 카메라 연결
        /*if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }*/
        virtualCamera.Follow = transform;
        virtualCamera.LookAt = transform;

        // 가상의 바닥을 기준으로 주인공의 위치를 생성
        plane = new Plane(transform.up, transform.position);
    }

    void Update()
    {
        // 자신의 캐릭터(네트워크 객체)일 경우에만 실행
        /*if (pv.IsMine)
        {
            Move();
            Turn();
        }*/
        Move();
        Turn();

        // F키를 눌러 거점 활성화 시작
        if (Input.GetKey(KeyCode.F))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.45f, transform.position.z);
        }

        // 캐릭터가 사망시 거점으로 리스폰
        if (currenthealth <= 0)
        {
            Debug.Log("사망");
            transform.position = new Vector3(-70, 1.1f, 50);
            currenthealth = health;
        }
    }


    void Move()
    {
        Vector3 cameraForward = camera.transform.forward; // 앞뒤
        Vector3 cameraRight = camera.transform.right; // 양옆

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        // 회전된 플레이어를 기준으로 이동 방향 계산
        Vector3 moveDir = (transform.forward * v) + (transform.right * h);
        moveDir.Set(moveDir.x, 0.0f, moveDir.z);

        // 캐릭터 이동 처리
        controller.SimpleMove(moveDir * moveSpeed);

        // 캐릭터 애니메이션 처리
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);
        animator.SetBool("Move", true);
    }


    void Turn()
    {
        // 마우스의 2차원 좌푯값을 이용해 3차원 레이를 생성
        ray = camera.ScreenPointToRay(Input.mousePosition); // 마우스 위치값에 레이 생성
        float enter = 0.0f;

        // 가상의 바닥에 ray를 발사해 충돌 지점의 거리를 enter 변수로 반환
        plane.Raycast(ray, out enter);

        // 가상의 바닥에 레이가 충돌한 좌푯값을 추출
        hitPoint = ray.GetPoint(enter);

        // 회전해야 할 방향의 백터를 계산
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0.0f;

        // 캐릭터의 회전값 지정
        transform.localRotation = Quaternion.LookRotation(lookDir);
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
/*
  public int health = 100;
    public int currenthealth = 100;

    private CharacterController controller;
    private new Transform transform;
    private Animator animator;
    private new Camera camera;
    
    // 가상의 Plane에 레이캐스팅하기 위한 변수
    private Plane plane;// 지정한 지역에 가상의 바닥을 생성하기 위한 Plane 구조체
    private Ray ray; // 광선
    private Vector3 hitPoint; // 광선이 부딪히는 지점

    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");

    // 이동속도
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
        // 가상의 바닥을 기준으로 주인공의 위치를 생성
        plane = new Plane(transform.up, transform.position);
    }

    void Update()
    {
        // 자신의 캐릭터(네트워크 객체)일 경우에만 실행
        Move();
        Turn();

        if (Input.GetKey(KeyCode.F))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.45f, transform.position.z);
        }
        if (currenthealth <= 0)
        {
            Debug.Log("사망");
            transform.position = new Vector3(-70, 1.1f, 50);
            currenthealth = health;
        }
    }


    void Move()
    {
        Vector3 cameraForward = camera.transform.forward; // 앞뒤
        Vector3 cameraRight = camera.transform.right; // 양옆

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        // 회전된 플레이어를 기준으로 이동 방향 계산
        Vector3 moveDir = (transform.forward * v) + (transform.right * h);
        moveDir.Set(moveDir.x, 0.0f, moveDir.z);

        // 캐릭터 이동 처리
        controller.SimpleMove(moveDir * moveSpeed);

        // 플레이어의 y위치를 계속해서 감소시킵니다.
        //transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);

        // 캐릭터 애니메이션 처리
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);
        animator.SetBool("Move", true);
    }


    void Turn()
    {
        // 마우스의 2차원 좌푯값을 이용해 3차원 레이를 생성
        ray = camera.ScreenPointToRay(Input.mousePosition); // 마우스 위치값에 레이 생성
        float enter = 0.0f;

        // 가상의 바닥에 ray를 발사해 충돌 지점의 거리를 enter 변수로 반환
        plane.Raycast(ray, out enter);

        // 가상의 바닥에 레이가 충돌한 좌푯값을 추출
        hitPoint = ray.GetPoint(enter);

        // 회전해야 할 방향의 백터를 계산
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0.0f;

        // 캐릭터의 회전값 지정
        transform.localRotation = Quaternion.LookRotation(lookDir);
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
 */