using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; // Сколько времени игрок может висеть в воздухе перед прыжком
    private float coyoteCounter; // Сколько времени прошло с момента как игрок сошел с края

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; // Горизонтальная сила прыжка от стены
    [SerializeField] private float wallJumpY; // Вертикальная сила прыжка от стены

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    [Header("Death")]
    [SerializeField] private UIManager uiManager;

    [Header("Player Scale")]
    [SerializeField] private Vector3 playerScale = new Vector3(5, 5, 1); // Фиксированный размер 5

    private void Awake()
    {
        // Получаем ссылки на rigidbody и animator из объекта
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Устанавливаем фиксированный размер при старте
        transform.localScale = playerScale;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Переворот игрока при движении влево-вправо (только меняем знак X, сохраняя размер)
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(playerScale.x), playerScale.y, playerScale.z);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(playerScale.x), playerScale.y, playerScale.z);

        // Установка параметров аниматора
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Прыжок
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Регулируемая высота прыжка
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; // Сброс счетчика койота при нахождении на земле
                jumpCounter = extraJumps; // Сброс счетчика прыжков до значения дополнительных прыжков
            }
            else
                coyoteCounter -= Time.deltaTime; // Начало уменьшения счетчика койота при отсутствии на земле
        }
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;
        // Если счетчик койота 0 или меньше и не на стене и нет дополнительных прыжков - ничего не делаем

        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                // Если не на земле и счетчик койота больше 0 - делаем обычный прыжок
                if (coyoteCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else
                {
                    if (jumpCounter > 0) // Если есть дополнительные прыжки, то прыгаем и уменьшаем счетчик
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }

            // Сброс счетчика койота в 0 чтобы избежать двойных прыжков
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(Mathf.Sign(transform.localScale.x), 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

    // Метод для принудительной установки размера 
    public void SetFixedScale(float scaleValue)
    {
        playerScale = new Vector3(scaleValue, scaleValue, 1);
        transform.localScale = new Vector3(Mathf.Abs(scaleValue) * Mathf.Sign(transform.localScale.x),
                                          scaleValue, 1);
    }
}

