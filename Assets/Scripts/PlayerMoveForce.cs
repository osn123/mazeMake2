using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveForce : MonoBehaviour {
    //[SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float downSpeed;    
    [SerializeField] private float rotateSpeed = 10f;

    Rigidbody rigid;
    //private InputAction moveAction;

    void Start() {
        Reset();
    }

    void Update() {
        HandleMove();
    }

    private void Reset() {
        moveSpeed = 1f;
        maxSpeed = 5f;
        downSpeed = 5f;
        rotateSpeed = 10f;
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
            // �J�����̌����Ɋ�Â��Ĉړ������𒲐�
            Vector3 move = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * direction;

            // AddForce���g�p���ė͂�������
            rigid.AddForce(move * moveSpeed,ForceMode.VelocityChange);

            // �ő呬�x�𐧌�����i�I�v�V�����j
            rigid.velocity = Vector3.ClampMagnitude(rigid.velocity,maxSpeed);

            // �L�����N�^�[�̌������ړ������ɍ��킹��
            if (rigid.velocity.magnitude > 0.1f) {
                Quaternion targetRotation = Quaternion.LookRotation(rigid.velocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,rotateSpeed * Time.deltaTime);
            }
        } else {
            // ���͂��Ȃ��ꍇ�͏��X�Ɍ�������
            rigid.velocity = Vector3.Lerp(rigid.velocity,Vector3.zero,Time.deltaTime * downSpeed);
        }
    }
}
