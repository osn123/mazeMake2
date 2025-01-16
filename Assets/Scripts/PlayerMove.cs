using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour {
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float moveSpeed;

    //private InputAction moveAction;
    public float rotateSpeed = 10f; // ‰ñ“]‘¬“x

    void Start() {
        Reset();
    }

    void Update() {
        HandleMove();
    }

    private void Reset() {
        moveSpeed = 5f;
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void HandleMove() {
        Vector3 moveDirection = GetMoveDirection();     // ˆÚ“®•ûŒü‚Ìæ“¾       
        Move(moveDirection);                            // ˆÚ“®ˆ—
    }

    public Vector3 GetMoveDirection() {
        float horizontal = Input.GetAxis("Horizontal");
        horizontal = InputManager.Instance.MoveTest.Player.Move.ReadValue<Vector2>().x;

        float vertical = Input.GetAxis("Vertical");
        vertical = InputManager.Instance.MoveTest.Player.Move.ReadValue<Vector2>().y;

        return new Vector3(horizontal,0,vertical).normalized;
    }

    public void Move(Vector3 direction) {
        if (direction.magnitude >= 0.1f) {
            Vector3 move = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * direction;
            characterController.Move(move * moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(move);

            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,rotateSpeed * Time.deltaTime);
        }
    }

}
