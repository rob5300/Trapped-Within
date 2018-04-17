using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour {

    public bool DoRotation = true;
    public bool DoMovement = true;
    public float RotateSensitivity = 5f;
    public float MoveSensitivity = 3f;
    public CharacterControllerPreset CrouchedState;

    [System.NonSerialized]
    public CharacterControllerPreset NormalState;
    [System.NonSerialized]
    public CharacterControllerPreset CurrentState;
    [System.NonSerialized]
    public bool Crouched = false;

    private CharacterController _charController;
    private Player player;

    private float _yRotation = 0f;
    private float _xRotation = 0f;

    private float _horizontal = 0f;
    private float _vertical = 0f;
    private Vector3 _desiredMove;
    private RaycastHit hitInfo;

    private bool _tryUncroutch;
    private Ray _croutchCheckRay;
    private float _croutchCheckDistance;

    public void Start () {
        _charController = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        //We record the normal state of the player to let us go back to this when we uncrouch.
        NormalState = new CharacterControllerPreset(_charController.height, _charController.center, player.camera.transform.localPosition);
        CurrentState = NormalState;
        //We need to do this via a method and not in the constructor as Unity does not call the contrsuctor when it applies its serialised values.
        CrouchedState.CalculateTopPosition();

        _croutchCheckDistance = Vector3.Distance(CrouchedState.TopPosition, NormalState.TopPosition);
        LockCursor();
    }
	
	public void Update () {
        if (DoRotation) Rotation();
        if (DoMovement) Movement();
        else if (Crouched) TryUnCroutch();

        if (_tryUncroutch && Crouched)
        {
            TryUnCroutch();
        }
	}

    private void Rotation()
    {
        #region Camera Rotation
        //For Up Down rotation.
        _xRotation += Input.GetAxis("Mouse Y") * RotateSensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -90, 90);
        player.camera.transform.localRotation = Quaternion.Euler(-_xRotation, 0f, 0f); 
        #endregion

        #region Player Rotation
        //For Left Right rotation.
        _yRotation = Input.GetAxis("Mouse X") * RotateSensitivity;
        transform.rotation *= Quaternion.Euler(0f, _yRotation, 0f);
        #endregion
    }

    private void Movement()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        //Causes movement to be relative to the camera position.
        _desiredMove = transform.forward * _vertical + transform.right * _horizontal;

        //From Unitys FirstPersonController class.
        Physics.SphereCast(transform.position, _charController.radius, Vector3.down, out hitInfo,
                           _charController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        _desiredMove = Vector3.ProjectOnPlane(_desiredMove, hitInfo.normal).normalized;

        _charController.SimpleMove(_desiredMove * (MoveSensitivity * CurrentState.MoveSpeedModifier));

        //Croutching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Croutch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            TryUnCroutch();
        }
    }

    public void ApplyControllerPreset(CharacterControllerPreset preset)
    {
        _charController.height = preset.Height;
        _charController.center = preset.Center;
        player.camera.transform.localPosition = preset.CameraPositionLocalSpace;
        CurrentState = preset;
    }

    public void Croutch()
    {
        Crouched = true;
        _tryUncroutch = false;
        ApplyControllerPreset(CrouchedState);
    }

    public void TryUnCroutch()
    {
        if (EnoughCroutchSpace())
        {
            ApplyControllerPreset(NormalState);
            Crouched = false;
            _tryUncroutch = false;
        }
        else
        {
            _tryUncroutch = true;
        }
    }

    public bool EnoughCroutchSpace()
    {
        _croutchCheckRay = new Ray(player.transform.TransformPoint(CrouchedState.TopPosition), transform.up);
        RaycastHit hit;
        bool a = !Physics.Raycast(_croutchCheckRay, out hit, _croutchCheckDistance);
        return a;
    }

    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

[System.Serializable]
public class CharacterControllerPreset
{
    public float Height;
    public Vector3 Center;
    public Vector3 CameraPositionLocalSpace;
    [System.NonSerialized]
    public Vector3 TopPosition;
    public float MoveSpeedModifier;

    public CharacterControllerPreset(float height, Vector3 center, Vector3 cameraPositionLocal) : this(height, center, cameraPositionLocal, 1) {}

    public CharacterControllerPreset(float height, Vector3 center, Vector3 cameraPositionLocal, float moveSpeedModifier)
    {
        Height = height;
        Center = center;
        CameraPositionLocalSpace = cameraPositionLocal;
        MoveSpeedModifier = moveSpeedModifier;

        CalculateTopPosition();
    }

    /// <summary>
    /// Calculate the top of the collider.
    /// </summary>
    public void CalculateTopPosition()
    {
        TopPosition = new Vector3(0, Height / 2 + Center.y, 0);
    }
}
