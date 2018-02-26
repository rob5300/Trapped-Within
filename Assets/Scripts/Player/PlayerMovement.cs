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

    private CharacterController _charController;
    private Player player;

    private float _yRotation = 0f;
    private float _xRotation = 0f;
    private float _rotationSensitivityFrame;

    private float _horizontal = 0f;
    private float _vertical = 0f;
    private Vector3 _desiredMove;
    private RaycastHit hitInfo;

    void Start () {
        _charController = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        LockCursor();
    }
	
	void Update () {
        if (DoRotation) Rotation();
        if (DoMovement) Movement();
	}

    private void Rotation()
    {
        _rotationSensitivityFrame = RotateSensitivity * Time.deltaTime;

        #region Camera Rotation
        //For Up Down rotation.
        _xRotation += Input.GetAxis("Mouse Y") * _rotationSensitivityFrame;
        player.camera.transform.localRotation = Quaternion.Euler(-Mathf.Clamp(_xRotation, -90, 90), 0f, 0f); 
        #endregion

        #region Player Rotation
        //For Left Right rotation.
        _yRotation = Input.GetAxis("Mouse X") * _rotationSensitivityFrame;
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

        _charController.SimpleMove(_desiredMove * MoveSensitivity);
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
