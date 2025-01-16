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
        Vector3 moveDirection = GetMoveDirection();     // 移動方向の取得
        Move(moveDirection);                            // 移動処理
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
            // カメラの向きに基づいて移動方向を調整
            Vector3 move = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * direction;

            // AddForceを使用して力を加える
            rigid.AddForce(move * moveSpeed,ForceMode.VelocityChange);

            // 最大速度を制限する（オプション）
            rigid.velocity = Vector3.ClampMagnitude(rigid.velocity,maxSpeed);

            // キャラクターの向きを移動方向に合わせる
            if (rigid.velocity.magnitude > 0.1f) {
                Quaternion targetRotation = Quaternion.LookRotation(rigid.velocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,rotateSpeed * Time.deltaTime);
            }
        } else {
            // 入力がない場合は徐々に減速する
            rigid.velocity = Vector3.Lerp(rigid.velocity,Vector3.zero,Time.deltaTime * downSpeed);
        }
    }
}
