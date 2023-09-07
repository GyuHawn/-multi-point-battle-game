using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    private string currentSceneName;

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
    public float current_health;
    public Manager manager;
    private float slide_time;
    private float movementCounter;
    private float idleCounter;
    private float baseFOV;
    private float sprintFOVModifier = 1.5f;
    private bool crouched;
    private bool sliding;
    private Rigidbody rig;

    private Transform ui_healthbar;
    private Text ui_ammo;

    public Vector3 weaponParentCurrentPos;
    private Vector3 origin;
    private Vector3 weaponParentOrigin;
    private Vector3 slide_dir;
    private Vector3 targetWeaponBobPosition;

    private Weapon weapon;
    private float aimAngle;

    private Animator anim;

    private ChatManager chatManager;

    public TMP_Text playerNameTextPrefab;
    private TMP_Text playerNameTextInstance;

    #region Photon Callbacks
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }


    #endregion
    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        chatManager = GameObject.Find("ChatPanel").GetComponent<ChatManager>();

        weapon = GetComponent<Weapon>();
        anim = GetComponent<Animator>();
        current_health = max_health;
        cameraParent.SetActive(photonView.IsMine);
        if (!photonView.IsMine) gameObject.layer = 9;
        baseFOV = normalCam.fieldOfView;
        origin = normalCam.transform.localPosition;
        if (Camera.main) Camera.main.enabled = false;

        if (!photonView.IsMine)
        {
            gameObject.layer = 9;
            standingCollider.layer = 9;
            crouchingCollider.layer = 9;
        }

        // Camera.main.enabled = false;
        rig = GetComponent<Rigidbody>();
        weaponParentOrigin = weaponParent.localPosition;
        weaponParentCurrentPos = weaponParentOrigin;

        if (photonView.IsMine)
        {
            if (currentSceneName == "Map")
            {
                ui_healthbar = GameObject.Find("HUD/Health/bar").transform;
                ui_ammo = GameObject.Find("HUD/Ammo/Text").GetComponent<Text>();
                RefreshHealthBar();
            }
            else if (currentSceneName == "World")
            {
                // 로컬 플레이어인 경우 닉네임 설정
                playerNameTextInstance = Instantiate(playerNameTextPrefab, transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
                playerNameTextInstance.text = PhotonNetwork.NickName;
            }
        }
        else
        {
            if (currentSceneName == "World")
            {
                // 원격 플레이어인 경우 PhotonView의 Owner의 Nickname을 사용합니다.
                playerNameTextInstance = Instantiate(playerNameTextPrefab, transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
                playerNameTextInstance.text = photonView.Owner.NickName;
            }
        }

        anim.SetBool("Run", false);
    }

    private void Update()
    {
        if (chatManager.isInputtingChat) { return; }

        if (!photonView.IsMine)
        {
            RefreshMultiplayerState();
            return;
        }

        if (playerNameTextInstance != null)
        {
            // 텍스트 중앙 정렬
            playerNameTextInstance.alignment = TextAlignmentOptions.Center;
            // 텍스트 사이즈
            playerNameTextInstance.fontSize = 5;
            // 이름 태그가 항상 캐릭터 위에 위치하도록 위치를 조정합니다.
            playerNameTextInstance.transform.position = transform.position + new Vector3(0f, 3f, 0f);

            // 플레이어와 같은 방향으로 회전
            playerNameTextInstance.transform.rotation = transform.rotation;
        }

        // Axis
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");

        anim.SetBool("Run", t_hmove != 0 || t_vmove != 0);

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

            //Head Bob

            if (sliding)
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
            else if (crouched)
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

        if (currentSceneName == "Map")
        {
            //UI Refresh HealthBar
            RefreshHealthBar();
            weapon.RefreshAmmo(ui_ammo);
        }
    }
    void FixedUpdate()
    {
        if (!photonView.IsMine || chatManager.isInputtingChat) return;
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
            else if (crouched)
            {
                t_adjustedSpeed *= crouchModifier;
            }

        }
        else
        {
            t_direction = slide_dir;
            t_adjustedSpeed *= slideModifier;
            slide_time -= Time.deltaTime;
            if (slide_time <= 0)
            {
                sliding = false;
                weaponParentCurrentPos = weaponParentOrigin;

            }
        }

        Vector3 t_targetVelocity = t_direction * t_adjustedSpeed * Time.deltaTime;
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
        if (sliding)
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

    void HeadBob(float p_z, float p_x_intensity, float p_y_intensity)
    {
        float t_aim_adjust = 1f;
        if (weapon.isAiming) t_aim_adjust = 0.1f;
        targetWeaponBobPosition = weaponParentCurrentPos + new Vector3(Mathf.Cos(p_z) * p_x_intensity * t_aim_adjust, Mathf.Sin(p_z * 2) * p_y_intensity * t_aim_adjust, 0);
    }

    public void RefreshHealthBar()
    {
        if (!photonView.IsMine)
            return;

        float t_health_ratio = (float)current_health / (float)max_health;
        ui_healthbar.localScale = Vector3.Lerp(ui_healthbar.localScale, new Vector3(t_health_ratio, 1, 1), Time.deltaTime * 8f);
    }

    [PunRPC]
    void SetCrouch(bool p_state)
    {
        if (crouched = p_state) return;

        crouched = p_state;

        if (crouched)
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