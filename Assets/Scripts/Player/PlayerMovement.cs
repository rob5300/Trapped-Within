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

    private CharacterController _charController;
    private Player player;

    private float _yRotation = 0f;
    private float _xRotation = 0f;

    private float _horizontal = 0f;
    private float _vertical = 0f;
    private Vector3 _desiredMove;
    private RaycastHit hitInfo;

    public void Start () {
        _charController = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        //We record the normal state of the player to let us go back to this when we uncrouch.
        NormalState = new CharacterControllerPreset(_charController.height, _charController.center, player.camera.transform.localPosition);
        CurrentState = NormalState;
        LockCursor();
    }
	
	public void Update () {
        if (DoRotation) Rotation();
        if (DoMovement) Movement();
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
    }

    public void ApplyControllerPreset(CharacterControllerPreset preset)
    {
        _charController.height = preset.Height;
        _charController.center = preset.Center;
        player.camera.transform.localPosition = preset.CameraPositionLocalSpace;
        CurrentState = preset;
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
public struct CharacterControllerPreset
{
    public float Height;
    public Vector3 Center;
    public Vector3 CameraPositionLocalSpace;
    public float MoveSpeedModifier;

    public CharacterControllerPreset(float height, Vector3 center, Vector3 cameraPositionLocal) : this(height, center, cameraPositionLocal, 1) {}

    public CharacterControllerPreset(float height, Vector3 center, Vector3 cameraPositionLocal, float moveSpeedModifier)
    {
        Height = height;
        Center = center;
        CameraPositionLocalSpace = cameraPositionLocal;
        MoveSpeedModifier = moveSpeedModifier;
    }
}
