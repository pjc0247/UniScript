using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

using ModPlayerSDK;

public class PPlayer : MonoBehaviourPunCallbacks
{
    public static PPlayer LocalPlayer;

    public Text nicknameText;

    public CharacterController controller;
    public Animator anim;
    public GameObject typingBubble;

    public GameObject cinemaLight;
    public GameObject thirdPersonCam;
    public Transform thirdPersonCamContainer;
    public GameObject lights;

    public int thirdPersonAngleXMin = -25;
    public int thirdPersonAngleXMax = 30;

    public string nickname;
    public string userId;
    public bool isDead { get; private set; }
    public Player owner { get; private set; }

    private ImpactReceiver impact;
    private bool canMove = false;

    void Awake()
    {
        thirdPersonCam.SetActive(SceneSetup.instance.cameraType == ModPlayerSDK.CameraType.ThirdPerson);
        cinemaLight.SetActive(Room.CustomProperties.SafeGet("type", "") == RoomType.Cinema);

        if (photonView.IsMine)
        {
            LocalPlayer = this;
            transform.LookAt(Camera.main.transform);
            transform.localEulerAngles = new Vector3(
                0,
                transform.localEulerAngles.y,
                0);
        }
        else
            Destroy(thirdPersonCam);

        impact = GetComponent<ImpactReceiver>();

        nickname = photonView.Controller.NickName;
        nicknameText.text = nickname;

        userId = photonView.Controller.UserId;
        owner = photonView.Controller;
        Room.players[userId] = this;

        LoadCharacterMesh();

        DontDestroyOnLoad(gameObject);
    }
    private void LoadCharacterMesh()
    {
        var name = owner.CustomProperties.SafeGet("character", "default");

        var prefab = Resources.Load<GameObject>($"Players/{name}");
        if (prefab == null)
            prefab = Resources.Load<GameObject>($"Players/default");

        var p = Instantiate(prefab, transform);
        p.transform.localPosition = Vector3.zero;
        var childAnim = p.GetComponent<Animator>();
        if (childAnim != null)
        {
            anim.avatar = childAnim.avatar;
            if (childAnim.runtimeAnimatorController != null)
                anim.runtimeAnimatorController = childAnim.runtimeAnimatorController;
            Destroy(childAnim);
        }
    }

    void OnDestroy()
    {
        Room.players.Remove(photonView.Controller.UserId);
    }

    void Update()
    {
        if (isDead) return;
        if (photonView.IsMine == false)
            return;
        if (Room.isControllable == false && canMove == false)
        {
            controller.SimpleMove(Vector3.zero);
            if (Room.is3rdPersonCam)
                Process3rdPersonCameraRotation();
            return;
        }

        if (impact.UpdateImpact())
            return;
        if (IsPlayingGesture() == false)
            anim.applyRootMotion = false;

        var input = InputManager.ActiveDevice;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC(nameof(DoInteraction), RpcTarget.All);
            // 일단 다 보냄
            photonView.RPC(nameof(NotifyAction), RpcTarget.All, 1);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            photonView.RPC(nameof(DoInteraction), RpcTarget.All);
            // 일단 다 보냄
            photonView.RPC(nameof(NotifyAction), RpcTarget.All, 2);
        }

        if (SceneSetup.instance.cameraType == ModPlayerSDK.CameraType.ThirdPerson)
            Process3rdPersonCameraMovement();
        else
            ProcessStaticCameraMovement();
    }
    private void ProcessStaticCameraMovement()
    {
        var input = InputManager.ActiveDevice;

        if (controller.isGrounded == false)
        {
            controller.SimpleMove(Vector3.zero);
            return;
        }
        if (input.Direction.IsPressed == false)
        {
            controller.SimpleMove(Vector3.zero);
            anim.SetFloat("Speed_f", 0);
            return;
        }

        var camAngle = Camera.main.transform.eulerAngles.y;
        var dir = new Vector3(
            Mathf.Sin(Mathf.PI / 180 * (input.Direction.Angle + camAngle)) * (Mathf.Abs(input.Direction.X) + Mathf.Abs(input.Direction.Y)),
            0,
            Mathf.Cos(Mathf.PI / 180 * (input.Direction.Angle + camAngle)) * (Mathf.Abs(input.Direction.X) + Mathf.Abs(input.Direction.Y))
            );
#if UNITY_EDITOR
        //이유몰름
        var move = dir * 75 * Time.deltaTime;
#else
        var move = dir * 300 * Time.deltaTime;
#endif

        controller.SimpleMove(move);

        CancelGestureIfPlaying();
        anim.SetFloat("Speed_f", move.magnitude);
        var angle = Mathf.Atan2(input.Direction.X, input.Direction.Y) *
            Mathf.Rad2Deg;

        transform.localEulerAngles = new Vector3(0, angle + camAngle, 0);
    }
    private void Process3rdPersonCameraMovement()
    {
        Process3rdPersonCameraRotation();
        anim.applyRootMotion = true;

        var input = InputManager.ActiveDevice;

        if (controller.isGrounded == false)
        {
            controller.SimpleMove(Vector3.zero);
            return;
        }
        if (input.Direction.IsPressed == false)
        {
            controller.SimpleMove(Vector3.zero);
            anim.SetFloat("Speed_f", 0);
            return;
        }

        var dir = 
            transform.forward * input.Direction.Y +
            transform.right * input.Direction.X;
        var move = dir * 100 * Time.deltaTime;
        controller.SimpleMove(move);

        CancelGestureIfPlaying();
        anim.SetFloat("Speed_f", move.magnitude);
    }

    private Vector3 prevPosition;
    private void Process3rdPersonCameraRotation()
    {
        var input = InputManager.ActiveDevice;

        /*
        transform.localEulerAngles +=
            new Vector3(0, input.RightStick.X * 1.55f, 0);
        thirdPersonCamContainer.localEulerAngles +=
            new Vector3(-input.RightStick.Y * 1.15f, 0, 0);
        */
        //thirdPersonCamContainer.localEulerAngles =
        //    new Vector3(Mathf.Clamp(thirdPersonCamContainer.localEulerAngles.x, -25, 30), 0, 0);
        /*
        var angles = thirdPersonCamContainer.localEulerAngles;
        if (angles.x >= 180 && angles.x < 360 + thirdPersonAngleXMin)
            angles.x = 360 + thirdPersonAngleXMin;
        if (angles.x < 180 && angles.x >= thirdPersonAngleXMax)
            angles.x = thirdPersonAngleXMax;
        thirdPersonCamContainer.localEulerAngles = angles;
        */

        var delta = prevPosition - Input.mousePosition;
        prevPosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(1))
            delta = Vector3.zero;
        if (Input.GetMouseButton(1))
        {
            transform.localEulerAngles += new Vector3(
                0, -delta.x * 0.45f, 0);
        }
    }

    public Ray GetForwardRay()
    {
        return new Ray(
            transform.position + new Vector3(0, 1.7f, 0),
            transform.forward);
    }
    public void SetEnableLights(bool enable)
    {
        lights.SetActive(enable);
    }

    public void SetMovable(bool value)
    {
        photonView.RPC(nameof(RpcSetMovable), owner, value);
    }
    [PunRPC]
    private void RpcSetMovable(bool value)
    {
        canMove = value;
    }

    public void LocalTeleport(Vector3 position)
    {
        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;

        transform.localEulerAngles = Vector3.zero;
        thirdPersonCamContainer.transform.localEulerAngles = Vector3.zero;
    }

    public void Teleport(Vector3 position)
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
        if (photonView.IsMine)
            LocalTeleport(position);
        photonView.RPC(nameof(RpcTeleport), owner, position);
    }
    [PunRPC]
    private void RpcTeleport(Vector3 position)
    {
        Debug.Log($"[Teleport] {position}");
        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;

        thirdPersonCamContainer.transform.localEulerAngles = Vector3.zero;

        // 삼인칭에서는 잠깐 없을 수도 있음
        if (Camera.main != null)
            transform.LookAt(Camera.main.transform);

        var angles = transform.localEulerAngles;
        angles.x = 0;
        transform.localEulerAngles = angles;
    }

    public void SetVisible(bool visible)
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
        photonView.RPC(nameof(RpcSetVisible), RpcTarget.All, visible);
    }
    [PunRPC]
    private void RpcSetVisible(bool visible)
    {
        transform.localScale = visible ? Vector3.one : Vector3.zero;
    }

    public void PlayAnimation(string id)
    {
        if (photonView.IsMine)
            photonView.RPC(nameof(RpcPlayAnimation), RpcTarget.All, id);
    }
    [PunRPC]
    private void RpcPlayAnimation(string id)
    {
        anim.applyRootMotion = true;
        anim.SetTrigger(id);
    }
    private bool IsPlayingGesture()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsTag("gesture");
    }
    private void CancelGestureIfPlaying()
    {
        if (IsPlayingGesture()) anim.Play("Idle", 0);
    }

    public void AwakeController()
    {
        controller.Move(Vector3.zero);
    }

    private void OnInteraction(InteractionData data)
    {
        if (Room.isPvpAvaliable == false) return;

        var diff = transform.position - data.sender.transform.position;
        diff = diff.normalized;

        photonView.RPC(nameof(RpcAddImpact), 
            RpcTarget.All,
            data.contact, diff, 55.0f);
    }
    [PunRPC]
    private void RpcAddImpact(Vector3 contact, Vector3 dir, float force)
    {
        Debug.Log($"AddImpact: {contact} / {force}");

        Fx.Create(Fx.Punch, contact);
        GetComponent<ImpactReceiver>()
            .AddImpact(dir, force);
    }

    [PunRPC]
    private void DoInteraction()
    {
        Debug.Log("[DoInteraction]");
        CancelGestureIfPlaying();
        anim.SetTrigger("Attack");

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(DelayedInteraction());
    }
    private IEnumerator DelayedInteraction()
    {
        yield return new WaitForSeconds(0.25f);

        var ray = new Ray(
                transform.position + new Vector3(0, 1.7f, 0),
                transform.forward);
        /*
        var hits = Physics.BoxCastAll(
            ray.origin, Vector3.one * 0.75f, 
            ray.direction, Quaternion.identity, 0.85f);
            */
        var hits = Physics.RaycastAll(
            ray, 1.25f);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
                continue;

            Debug.Log(hit.collider.name);

            var data = new InteractionData()
            {
                sender = this,
                contact = hit.point,
                target = hit.collider.gameObject
            };

            GameBaseNotifiable.instance?
                .NotifyInteraction(this, data);
            hit.collider.gameObject.SendMessage("OnInteraction",
                data,
                SendMessageOptions.DontRequireReceiver);
        }
    }
    [PunRPC]
    private void NotifyAction(int actionId)
    {
        if (GameBaseNotifiable.instance == null)
            return;

        if (PhotonNetwork.IsMasterClient)
            GameBaseNotifiable.instance.NotifyPlayerAction(this, actionId);
    }

    public void SetTypingState(bool isTyping)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new PhotonHashtable()
        {
            ["isTyping"] = isTyping
        });
    }
    public override void OnPlayerPropertiesUpdate(Player target, PhotonHashtable changedProps)
    {
        if (target != owner) return;
        if (changedProps.ContainsKey("isTyping"))
        {
            var isTyping = (bool)changedProps["isTyping"];
            ShowTypingBubble(isTyping);
        }

        if (photonView.IsMine &&
            changedProps.ContainsKey("isPlaying"))
        {
            if ((bool)changedProps["isPlaying"] == true)
                transform.localScale = Vector3.one;
        }
    }
    public override void OnRoomPropertiesUpdate(PhotonHashtable changes)
    {
        if (changes.ContainsKey("is3rdPersonCam"))
            thirdPersonCam.SetActive(changes.SafeGet("is3rdPersonCam", false));
    }

    private void ShowTypingBubble(bool isTyping)
    {
        typingBubble.SetActive(isTyping);
    }

    public void Die(FxType type = FxType.None)
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;

        Fx.CreateAll(type, transform.position + new Vector3(0, 0.5f, 0));
        photonView.RPC(nameof(RpcDie), RpcTarget.All);
    }
    [PunRPC]
    private void RpcDie()
    {
        isDead = true;
        transform.localScale = Vector3.zero;
    }

    public void Revive()
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;

        photonView.RPC(nameof(RpcRevive), RpcTarget.All);
    }
    [PunRPC]
    private void RpcRevive()
    {
        isDead = false;
        transform.localScale = Vector3.one;
    }
}
