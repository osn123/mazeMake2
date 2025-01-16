using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unitychan_Controller : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    public Vector3 moveDirection = Vector3.zero;
    public float gravity = 8;
    public float rotateForce = 300; //回転量
    public float runForce = 2.5f; //前進量
    public float maxRunSpeed = 2; //前進速度の制限
    public float jumpforce = 5; //ジャンプ量

    Quaternion defaultCameraDir;
    Vector3 defaultCameraOffset;
    float charaDir = 0;

    void Start()
    {
        Application.targetFrameRate = 60;
        // 必要なコンポーネントを自動取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        defaultCameraDir = Camera.main.transform.rotation;
        defaultCameraOffset = Camera.main.transform.position - transform.position;
    }

    void Update()
    {
        if (controller == null) return;  //キャラコントローラーが入っていない場合は終了

        //横方向の入力で方向転換する
        //transform.Rotate(0, Input.GetAxis("Horizontal") * rotateForce * Time.deltaTime, 0);

        //ジャンプ
        if (controller.isGrounded)  //地面に着地していたら
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpforce;
                animator.SetTrigger("Jump");
            }
        }

        //カメラ回転
        if (Input.GetKey(KeyCode.Z))
        {
            charaDir -= 120 * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.X))
        {
            charaDir += 120 * Time.deltaTime;
        }
        Camera.main.transform.rotation = Quaternion.Euler(0, charaDir, 0) * defaultCameraDir;

        //上方向の入力で進む
        bool isRun = false; //入力があるかどうか

        //if (Input.GetAxis("Vertical") > 0.0f)
        if (Input.GetAxis("Vertical")!=0f || Input.GetAxis("Horizontal") != 0f)
        {
            isRun = true;
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (input.magnitude>1.0f)
            {
                input = input.normalized;
            }
                moveDirection.z = input.z * runForce;
            moveDirection.x = input.x * runForce;

            //キャラの向きに
            float Dir = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, charaDir + Dir, 0);
        }
        else
        {
            moveDirection.z = 0;
            moveDirection.x = 0;
        }

        moveDirection.y-=gravity*Time.deltaTime;

        //移動
        //Vector3 globalDirection= transform.TransformDirection(moveDirection);
        //キャラクターの向きに前進
        Vector3 globalDirection = Quaternion.Euler(0, charaDir, 0) * moveDirection;
        controller.Move(globalDirection*Time.deltaTime);

        //地面に着地していたらy方向移動をリセットする
        if (controller.isGrounded) moveDirection.y = 0;

        //カメラ
        Camera.main.transform.position = transform.position + Quaternion.Euler(0, charaDir, 0) * defaultCameraOffset;

        //走っているかどうかのアニメーション設定
        animator.SetBool("Run", isRun);

        //アニメーション速度を調整（アニメーション名で判別）
        string anim_name = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (anim_name.Contains("Run") && isRun)
        {
            Vector3 vel_xz = new Vector3(moveDirection.x, 0, moveDirection.z);
            animator.speed = vel_xz.magnitude / runForce;
        }
        else
        {
            animator.speed = 1.0f;
        }
    }
}