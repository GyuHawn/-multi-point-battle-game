using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public float speed;
    public float sprintModifier = 2;
    public float jumpForce;
    public float max_health;
    public float lenghtOfSlide;
    public float slideModifier;
    public float crouchModifier;
    public float slideAmount;
    public float crounchAmount;
    public LayerMask ground;
    public Transform groundDetector;
    public Transform weaponParent;
    public GameObject cameraParent;
    public GameObject standingCollider;
    public GameObject crouchingCollider;
    public Camera normalCam;
    private float current_health;
    private float slide_time;
    private float movementCounter;
    private float idleCounter;
    private float baseFOV;
    private float sprintFOVModifier = 1.5f;
    private bool crouched;
    private bool sliding;
    private Manager manager;
    private Text ui_ammo;
    private Rigidbody rig;
    private Transform ui_healthbar;
    public Vector3 weaponParentCurrentPos;
    private Vector3 origin;
    private Vector3 weaponParentOrigin;
    private Vector3 slide_dir;
    private Vector3 targetWeaponBobPosition;

    private Weapon weapon;
    private float aimAngle;

    #region Photon Callbacks
    public void OnPhotonSerializeView(PhotonStream p_stream, PhotonMessageInfo p_message)
    {
        if (p_stream.IsWriting)
        {
            p_stream.SendNext((int)(weaponParent.transform.localEulerAngles.x * 100f));
        }
        else
        {
            aimAngle = (int)p_stream.ReceiveNext() / 100f;
        }
    }

    #endregion
    private void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        weapon = GetComponent<Weapon>();
        current_health = max_health;
        cameraParent.SetActive(photonView.IsMine);
        if (!photonView.IsMine) gameObject.layer = 9;
        baseFOV = normalCam.fieldOfView;
        origin = normalCam.transform.localPosition;
        if (Camera.main) Camera.main.enabled = false;

        if(!photonView.IsMine)
        {
            gameObject.layer = 9;
            standingCollider.layer = 9;
            crouchingCollider.layer = 9;
        }
       
      // Camera.main.enabled = false;
        rig = GetComponent<Rigidbody>();
        weaponParentOrigin = weaponParent.localPosition;
        weaponParentCurrentPos = weaponParentOrigin;


        if(photonView.IsMine)
        {
            ui_healthbar = GameObject.Find("HUD/Health/bar").transform;
            ui_ammo = GameObject.Find("HUD/Ammo/Text").GetComponent<Text>();
            RefreshHealthBar();
        }

    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            RefreshMultiplayerState();
            return;
        }

        // Axis
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");

        // Controls
        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool jump = Input.GetKeyDown(KeyCode.Space);
        //bool crouch = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl);

        // States
        bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.15f, ground);
        bool isJumping = jump && isGrounded;
        bool isSprinting = sprint && t_vmove > 0 && isGrounded && !isJumping;
       // bool isCrouching = crouch && !isSprinting && !isJumping && isGrounded;
       /*
          //Crouching
          if(isCrouching)
        {
            photonView.RPC("SetCrouch", RpcTarget.All, !crouched);
        }*/
        //Jumping
        if (isJumping)
        {
            if (crouched) photonView.RPC("SetCrouch", RpcTarget.All, false);
            rig.AddForce(Vector3.up * jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.U))
            TakeDamage(100);

        //Head Bob
        
        if(sliding) 
        {
            //SLIDING
            HeadBob(movementCounter, 0.15F, 0.075F);
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
        }
        else if (t_hmove == 0 && t_vmove == 0)
        {
            //IDLING
            HeadBob(idleCounter, 0.025f, 0.025f);
            idleCounter += Time.deltaTime;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);
        }
        else if (!isSprinting && !crouched)
        {
            //WALKING
            HeadBob(movementCounter, 0.035f, 0.035f);
            movementCounter += Time.deltaTime * 3f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 6f);
        }
        else if(crouched)
        {
            //CROUNCHING
            HeadBob(movementCounter, 0.035f, 0.035f);
            movementCounter += Time.deltaTime * 3f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 6f);
        }
        else
        {
            //SPRINTING
            HeadBob(movementCounter, 0.15f, 0.075f);
            movementCounter += Time.deltaTime * 7f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
        }


        //UI Refresh HealthBar
        RefreshHealthBar();
        weapon.RefreshAmmo(ui_ammo);
    }
    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        // Axis
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");

        // Controls
        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool jump = Input.GetKeyDown(KeyCode.Space);
        //bool slide = Input.GetKey(KeyCode.LeftControl);

        // States
        bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
        bool isJumping = jump && isGrounded;
        bool isSprinting = sprint && t_vmove > 0 && isGrounded && !isJumping;
       // bool isSliding = isSprinting && slide && !sliding;

        // Movement
        Vector3 t_direction = Vector3.zero;
        float t_adjustedSpeed = speed;
        if (!sliding)
        {
            t_direction = new Vector3(t_hmove, 0, t_vmove);
            t_direction.Normalize();
            t_direction = transform.TransformDirection(t_direction);
            
            if (isSprinting)
            {
                if (crouched) photonView.RPC("SetCrouch", RpcTarget.All, false);
                t_adjustedSpeed *= sprintModifier;
            }
            else if(crouched)
            {
                t_adjustedSpeed *= crouchModifier;
            }
           
        }
       else
        {
            t_direction = slide_dir;
            t_adjustedSpeed *= slideModifier;
            slide_time -= Time.deltaTime;
            if(slide_time <= 0)
            {
                sliding = false;
                weaponParentCurrentPos = weaponParentOrigin;
                
            }
        }

        Vector3 t_targetVelocity =t_direction * t_adjustedSpeed * Time.deltaTime;
        t_targetVelocity.y = rig.velocity.y;
        rig.velocity = t_targetVelocity;

        //Sliding
        /*if (isSliding)
        {
            sliding = true;
            slide_dir = t_direction;
            slide_time = lenghtOfSlide;
            weaponParentCurrentPos += Vector3.down * (slideAmount - crounchAmount);
            if (!crouched) photonView.RPC("SetCrouch", RpcTarget.All, true);
            
            
            //adjust camera

        }*/

        // Camera Stuff
        if(sliding)
        {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier * 1.15f, Time.deltaTime * 8f);
            normalCam.transform.localPosition = Vector3.Lerp(normalCam.transform.localPosition, origin + Vector3.down * 0.75f, Time.deltaTime * 6f);
        }
        else
        {
            //Sprinting
            if (isSprinting)
            {
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f);
            }
            else
            {
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
            }
            if (crouched) normalCam.transform.localPosition = Vector3.Lerp(normalCam.transform.localPosition, origin + Vector3.down * crounchAmount, Time.deltaTime * 6f);
            else normalCam.transform.localPosition = Vector3.Lerp(normalCam.transform.localPosition, origin, Time.deltaTime * 6f);


        }

       

       
    }

    void RefreshMultiplayerState()
    {
        float cacheEulY = weaponParent.localEulerAngles.y;

        Quaternion targetRotation = Quaternion.identity * Quaternion.AngleAxis(aimAngle, Vector3.right);
        weaponParent.rotation = Quaternion.Slerp(weaponParent.rotation, targetRotation, Time.deltaTime * 8f);

        Vector3 finalRotation = weaponParent.localEulerAngles;
        finalRotation.y = cacheEulY;

        weaponParent.localEulerAngles = finalRotation;
    }
    void HeadBob (float p_z , float p_x_intensity , float p_y_intensity)
    {
        float t_aim_adjust = 1f;
        if (weapon.isAiming) t_aim_adjust = 0.1f;
        targetWeaponBobPosition = weaponParentCurrentPos + new Vector3(Mathf.Cos(p_z) * p_x_intensity *t_aim_adjust, Mathf.Sin(p_z * 2) * p_y_intensity *t_aim_adjust, 0);
    }

    [PunRPC]
    public void TakeDamage(int p_damage)
    {
        if (photonView.IsMine)
        {
            current_health -= p_damage;
            RefreshHealthBar();
            Debug.Log(current_health);

            if(current_health <= 0)
            {
                manager.Spawn();
                PhotonNetwork.Destroy(gameObject);
                Debug.Log("YOU DIED");
            }
        }
        
    }

    void RefreshHealthBar()
    {
        float t_health_ratio = (float)current_health / (float)max_health;
        ui_healthbar.localScale = Vector3.Lerp(ui_healthbar.localScale, new Vector3(t_health_ratio, 1, 1), Time.deltaTime * 8f);
    }

    [PunRPC]
    void SetCrouch( bool p_state)
    {
        if(crouched = p_state) return;

        crouched = p_state;
        
        if(crouched)
        {
            standingCollider.SetActive(false);
            crouchingCollider.SetActive(true);
            weaponParentCurrentPos += Vector3.down * crounchAmount;
        }
        else
        {
            standingCollider.SetActive(true);
            crouchingCollider.SetActive(false);
            weaponParentCurrentPos -= Vector3.down * crounchAmount;
        }
    }
}
/*
using 문 - System.Collections과 System.Collections.Generic 네임스페이스를 사용함을 나타낸다.
UnityEngine 네임스페이스를 사용한다.
Photon.Pun 네임스페이스를 사용한다.
UnityEngine.UI 네임스페이스를 사용한다.
Player 클래스를 MonoBehaviourPunCallbacks와 IPunObservable로 상속한다.
float 형 변수 speed를 정의한다.
float 형 변수 sprintModifier를 정의한다.
float 형 변수 jumpForce를 정의한다.
float 형 변수 max_health를 정의한다.
float 형 변수 lenghtOfSlide를 정의한다.
float 형 변수 slideModifier를 정의한다.
float 형 변수 crouchModifier를 정의한다.
float 형 변수 slideAmount를 정의한다.
float 형 변수 crouchAmount를 정의한다.
LayerMask 형 변수 ground를 정의한다.
Transform 형 변수 groundDetector를 정의한다.
Transform 형 변수 weaponParent를 정의한다.
GameObject 형 변수 cameraParent를 정의한다.
GameObject 형 변수 standingCollider를 정의한다.
GameObject 형 변수 crouchingCollider를 정의한다.
Camera 형 변수 normalCam을 정의한다.
float 형 변수 current_health를 정의하고 max_health의 값을 대입한다.
float 형 변수 slide_time을 정의한다.
float 형 변수 movementCounter를 정의한다.
float 형 변수 idleCounter를 정의한다.
float 형 변수 baseFOV를 정의하고 normalCam의 fieldOfView 값을 대입한다.
float 형 변수 sprintFOVModifier를 정의하고 1.5f 값을 대입한다.
bool 형 변수 crouched를 정의한다.
bool 형 변수 sliding을 정의한다.
Manager 클래스의 인스턴스를 참조하는 Manager 형 변수 manager를 정의한다.
Text 컴포넌트를 참조하는 ui_ammo 변수를 정의한다.
Rigidbody 컴포넌트를 참조하는 rig 변수를 정의한다.
Transform 컴포넌트를 참조하는 ui_healthbar 변수를 정의한다.
Vector3 형 변수 weaponParentCurrentPos를 정의하고 weaponParent의 localPosition 값을 대입한다.
Vector3 형 변수 origin을 정의하고 normalCam의 localPosition 값을 대입한다.
Vector3 형 변수 weaponParentOrigin을 정의하고 weaponParent의 localPosition 값을 대입한다.
Vector3 형 변수 slide_dir를 정의한다.
Vector3 형 변수 targetWeaponBobPosition을 정의한다.
Weapon 클래스 변수 weapon을 정의한다.
float 형 변수 aimAngle를 정의한다.
PhotonCallBacks 영역을 정의한다.
OnPhotonSerializeView 메서드를 정의한다. PhotonStream과 PhotonMessageInfo 매개변수를 받는다.
p_stream이 쓰기 모드인 경우 (p_stream.IsWriting == true), (int)(weaponParent.transform.localEulerAngles.x * 100f)를 전송한다.
p_stream이 읽기 모드인 경우 (p_stream.IsWriting == false), (int)p_stream.ReceiveNext() / 100f 값을 aimAngle에 대입한다.
Start() 메서드를 정의한다.
Manager 게임 오브젝트를 찾고, 그것의 Manager 컴포넌트를 manager 변수에 대입한다.
Weapon 클래스의 인스턴스를 참조하는 weapon 변수를 정의하고 GetComponent<Weapon>() 메서드를 호출하여 참조값을 대입한다.
current_health에 max_health를 대입한다.
만약 photonView가 로컬 플레이어에 해당하는 경우, cameraParent를 활성화한다.
photonView가 로컬 플레이어가 아닌 경우, gameObject의 레이어를 9로 설정한다.
만약 photonView가 로컬 플레이어인 경우, ui_healthbar를 GameObject.Find(string) 메서드로 찾아 대입한다.
ui_ammo를 GameObject.Find(string).GetComponent<Text>() 메서드로 찾아 대입한다.
TakeDamage(int) 메서드를 호출하여 체력바를 초기화한다.
Update() 메서드를 정의한다.
photonView가 로컬 플레이어가 아닌 경우, RefreshMultiplayerState() 메서드를 호출하고, 이후 코드를 실행하지 않는다.
float 형 변수 t_hmove를 정의하고, "Horizontal" 축의 입력값을 받아와서 초기화한다.
float 형 변수 t_vmove를 정의하고, "Vertical" 축의 입력값을 받아와서 초기화한다.
bool 형 변수 sprint을 정의하고, KeyCode.LeftShift 또는 KeyCode.RightShift 키를 눌렀는지 여부에 따라 초기화한다.
bool 형 변수 jump을 정의하고, KeyCode.Space 키를 눌렀는지 여부에 따라 초기화한다.
bool 형 변수 isGrounded를 정의하고, groundDetector에서 아래쪽으로 Raycast를 발사하여 바닥이 있는지 여부에 따라 초기화한다.
bool 형 변수 isJumping을 정의하고, jump 키를 누르고, isGrounded가 참일 경우, true를 대입한다.
bool 형 변수 isSprinting을 정의하고, sprint이 참이면서, t_vmove가 양수이며, isGrounded가 참이면서, isJumping이 거짓일 경우, true를 대입한다.
bool 형 변수 isCrouching은 주석 처리되어 있다.
만약 isJumping이 참일 경우, crouched가 참인 경우, SetCrouch(bool) 메서드를 호출하고, false를 인수로 전달하여 crouched 값을 false로 변경한다. 그리고 rig의 상승 힘을 더해준다.
만약 U 키를 눌렀을 경우, TakeDamage(int) 메서드를 호출하여, p_damage 값을 100으로 설정한다.
만약 sliding이 참일 경우, HeadBob() 메서드를 호출하여, targetWeaponBobPosition 값을 설정한다.
만약 t_hmove와 t_vmove가 0인 경우, HeadBob() 메서드를 호출하여, targetWeaponBobPosition 값을 설정하고, idleCounter 값을 증가시킨다.
만약 isSprinting와 crouched가 거짓인 경우, HeadBob() 메서드를 호출하여 targetWeaponBobPosition 값을 설정하고, movementCounter 값을 증가시킨다.
만약 crouched가 참일 경우, HeadBob() 메서드를 호출하여 targetWeaponBobPosition 값을 설정하고, movementCounter 값을 증가시킨다.
위의 어느 경우에도 해당하지 않는 경우, HeadBob() 메서드를 호출하여 targetWeaponBobPosition 값을 설정하고, movementCounter 값을 증가시킨다.
UI RefreshHealthBar() 메서드를 호출한다.
weapon.RefreshAmmo(Text) 메서드를 호출한다.
FixedUpdate() 메서드를 정의한다.
photonView가 로컬 플레이어가 아닌 경우, 함수의 실행을 중지한다.
float 형 변수 t_hmove를 정의하고, "Horizontal" 축의 입력값을 받아와서 초기화한다.
float 형 변수 t_vmove를 정의하고, "Vertical" 축의 입력값을 받아와서 초기화한다.
bool 형 변수 sprint을 정의하고, KeyCode.LeftShift 또는 KeyCode.RightShift 키를 눌렀는지 여부에 따라 초기화한다.
bool 형 변수 jump을 정의하고, KeyCode.Space 키를 눌렀는지 여부에 따라 초기화한다.
bool 형 변수 isGrounded를 정의하고, groundDetector에서 아래쪽으로 Raycast를 발사하여 바닥이 있는지 여부에 따라 초기화한다.
bool 형 변수 isJumping을 정의하고, jump 키를 누르고, isGrounded가 참일 경우, true를 대입한다.
bool 형 변수 isSprinting을 정의하고, sprint이 참이면서, t_vmove가 양수이며, isGrounded가 참이면서, isJumping이 거짓일 경우, true를 대입한다.
마지막과 근접한 Comment로 작성된 bool 형 변수 isSliding 주석을 해제하고, 변수를 정의하고 초기화한다.
Vector3 형 변수 t_direction을 정의하고, x, y, z 구성요소를 갖는 (t_hmove, 0, t_vmove) 값을 대입한다.
t_direction을 노멀라이즈한다.
t_direction에 transform.TransformDirection(t_direction)을 대입한다.
매개변수 t_vmove가 양수이고, sprint이 참이며, isJumping이 거짓이며, isSprinting이 참일 경우, crouched를 만약 참이면, SetCrouch(bool) 메서드를 호출하여 crouched 값을 거짓으로 바꾼다. t_adjustedSpeed를 sprintModifier로 조정한다.
만약 crouched가 참일 경우, t_adjustedSpeed를 crouchModifier로 조정한다.
방향 벡터 t_direction과 t_adjustedSpeed와 Time.deltaTime을 곱한 값을 t_targetVelocity 변수에 대입한다.
t_targetVelocity.y 값을 rig.velocity.y로 설정한다.
rig.velocity 값을 t_targetVelocity로 설정한다.
만약 isSliding이 참일 경우, slide_dir를 t_direction으로 설정하고, slide_time을 길이OfSlide로 설정한다. weaponParentCurrentPos 변수에 slideAmount - crouchAmount 값을 더한다. crouched가 거짓일 경우, crouched 값을 참으로 바꾼다.
sliding이 거짓일 경우, sliding을 참으로 설정하고, weaponParentCurrentPos 값을 weaponParentOrigin 으로 설정한다.
마지막과 근접한 Comment로 작성된 Camera Stuff 영역의 코드 부분을 참조하여, normalCam의 fieldOfView 값을 조정한다.
만약 crouched가 참일 경우, normalCam의 transform.localPosition 값을 조정한다.
rigidbody의 velocity 값에 따라 플레이어의 이동을 처리한다.
RefreshMultiplayerState() 메서드를 호출한다.
TakeDamage(int) 메서드를 호출한다.
RefreshHealthBar() 메서드를 호출한다.
SoundPlay() 메서드를 호출한다.
SetCrouch(bool) 메서드를 호출한다.
RefreshAmmo() 메서드를 호출한다.
RefreshMultiplayerState() 메서드를 정의한다.
float 형 변수 cacheEulY를 정의하고, weaponParent의 localEulerAngles.y 값을 대입한다.
Quaternion 형 변수 targetRotation을 정의하고, Quaternion.identity * Quaternion.AngleAxis(aimAngle, Vector3.right)의 값을 대입한다.
weaponParent.rotation 값을 targetRotation과 Time.deltaTime, 8f를 인수로하여 Quaternion.Slerp() 메서드로 보간한다.
Vector3 형 변수 finalRotation은 weaponParent의 localEulerAngles 값을 대입한다.
finalRotation의 y 구성요소를 cacheEulY 값으로 설정한다.
weaponParent의 localEulerAngles 값을 finalRotation으로 설정한다.
TakeDamage() 메서드를 정의한다. int 형 매개변수 p_damage를 받는다.
photonView가 로컬 플레이어일 경우, current_health 값을 p_damage 만큼 감소시키고,체력바를 갱신한다.
만약 current_health이 0 이하일 경우, manager.Spawn()을 호출하여, 다시 스폰하고, PhotonNetwork.Destroy(gameObject) 메서드로 자신을 삭제한다.
로컬 플레이어가 아닐 경우, TakeDamage() 메서드를 무시한다.
RefreshHealthBar() 메서드를 정의한다.
current_health의 비율을 t_health_ratio에 대입한다.
ui_healthbar의 localScale 값을 t_health_ratio로 설정한다.
SetCrouch(bool) 메서드를 정의한다. bool 형 매개변수 p_state를 받는다.
만약 crouched가 p_state와 같은 값을 가진 경우, 메서드의 수행을 중지한다.
crouched 값을 p_state으로 변경한다.
crouched가 참일 경우, standingCollider를 비활성화하고, crouchingCollider를 활성화한다.
weaponParentCurrentPos에 Vector3.down * crounchAmount 값을 더한다.
crouched가 거짓일 경우, standingCollider를 활성화하고, crouchingCollider를 비활성화한다.
weaponParentCurrentPos에 Vector3.down * crounchAmount 값을 뺀다.
 */