using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        curHp = maxHp;
        isWalking = false;
        isRunning = false;
        isJumping = false;
        isExhausted = false;
        prevTime = 0f;
        speedMultiplier = 1f;
        speedDivider = 1f;
    }

    void Update()
    {
        Move();
        Rotate();
        Jump();
        AutoHeal();
    }

    void OnCollisionEnter(Collision collision)
    {
        var gameObject = collision.gameObject;
        if (gameObject.CompareTag(Tag.GROUND) || gameObject.CompareTag(Tag.ROCK))
            isJumping = false;
    }

    public void LoseHp(float loseAmount)
    {
        loseAmount = Mathf.Abs(loseAmount);
        curHp -= loseAmount;
        if (curHp < 0f)
            curHp = 0f;
        isExhausted = curHp == 0f ? true : false;
    }

    public void HealHp(float healAmount)
    {
        healAmount = Mathf.Abs(healAmount);
        curHp += healAmount;
        isExhausted = false;
        if (curHp > maxHp)
            curHp = maxHp;
    }

    void Move()
    {
        float h = Input.GetAxis(Axis.HORIZONTAL);
        float v = Input.GetAxis(Axis.VERTICAL);

        if (Input.GetKey(KeyCode.LeftShift))
            isRunning = true;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;

        #region ������ ������ ü�� ������ ����
        if (!(h == 0 && v == 0)) // ������ ��
        {
            isWalking = true;
            speedDivider = isExhausted ? 0.5f : 1f;
            if (isRunning && !isExhausted) // �� �� & ��ġ�� �ʾ��� ��
            {
                LoseHp(0.02f);
                speedMultiplier = 2f;
            }
            else // ���� ��
            {
                LoseHp(0.01f);
                speedMultiplier = 1f;
            }
            Canvas.instance.UpdateTiredBar(curHp, maxHp);
        }
        else // �������� ��
            isWalking = false;
        #endregion

        transform.Translate((Vector3.forward * v + Vector3.right * h).normalized * moveSpeed * speedMultiplier * speedDivider * Time.deltaTime);
    }

    void Rotate()
    {
        float vr = Input.GetAxis(Axis.MOUSE_X);

        transform.Rotate(Vector3.up * vr * rotateSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    void AutoHeal()
    {
        if (!isWalking && !isJumping) // ������ ���� ��
        {
            if (Time.time - prevTime > 3f) // 3�ʰ� �����ٸ�
            {
                HealHp(autoHealDelta);
                Canvas.instance.UpdateTiredBar(curHp, maxHp);
            }
        }
        else // �����̰� �Ǹ�
            prevTime = Time.time;
    }

    [Header("Player Info")]
    public float maxHp;
    public float curHp;
    public float moveSpeed;
    public float rotateSpeed;
    public float jumpPower;
    public float autoHealDelta;

    [Header("State Variables")]
    public bool isWalking;
    public bool isRunning;
    public bool isJumping;
    public bool isExhausted;
    
    Rigidbody rigidbody;
    float prevTime;
    float speedMultiplier = 1f; // �ٰų� ���� ��� ������ ����(�̵� �ӵ� ���)
    float speedDivider = 1f; // ü���� Ư�� �ۼ�Ʈ �̸��� ��� ������ ����(�̵� �ӵ� ����)
}
