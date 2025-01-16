using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveRB : MonoBehaviour {
    //[SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float moveSpeed;

    Rigidbody rigid;
    //private InputAction moveAction;
    public float rotateSpeed = 10f; // ��]���x

    void Start() {
        Reset();
    }

    void Update() {
        HandleMove();
    }

    private void Reset() {
        moveSpeed = 5f;
        //characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        rigid = this.GetComponent<Rigidbody>();
    }

    private void HandleMove() {
        Vector3 moveDirection = GetMoveDirection();     // �ړ������̎擾
        Move(moveDirection);                            // �ړ�����
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
            //characterController.Move(move * moveSpeed * Time.deltaTime);
            rigid.velocity = move * moveSpeed;
            transform.rotation = Quaternion.LookRotation(move);

            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,rotateSpeed * Time.deltaTime);
        }
    }

}
