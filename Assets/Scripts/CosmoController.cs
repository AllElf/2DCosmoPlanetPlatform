using UnityEngine;
using System.Collections;

public class CosmoController : MonoBehaviour
{
    [Header("Скорость персонажа")]
    [SerializeField] float speed = 5f;
    //[SerializeField] Rigidbody2D rb2d;
    public float currentHorizontalSpeed;

    [SerializeField] float horizontalAcceleration = 5f;
    [SerializeField] float horizontalDeceleration = 5f;

    public bool range;

    [Header("Настройки роста персонажа")]
    [SerializeField] float minScale = 5f, maxScale = 8f;
    [SerializeField] float minHeightUp, maxHeightUp;
    [SerializeField] float minHeightForward, maxHeightForward;

    [Header("Настройки прыжка")]
    [SerializeField] float jumpForce = 10f, fallSpeed = 5f;
    [SerializeField] float startY, targetJumpY;
    [SerializeField] bool isJumping, isFalling;

    [Header("Спрайты")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer _sprite;

    public float horizontal, vertical;
    Coroutine spriteCoroutine;


    [SerializeField] bool isOnGroundPlatform = false;
    [SerializeField] bool isOnGround = true;

    Color originalColor;
    public Color flashColor = Color.red; // Цвет мерцания
    public float flashDuration = 0.2f; // Длительность мерцания



    void Start()
    {
        originalColor = sprite.color;
        //rb2d ??= GetComponent<Rigidbody2D>();
        sprite ??= GetComponent<SpriteRenderer>();
        startY = transform.position.y;
    }

    void Update()
    {
        GetInput();
        Move();
        Flip();
        CheckRange();
        SwitchSprite();

        if (!isJumping && !isFalling && isOnGround)
        {
            AdjustScale();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                isOnGround = false;
                targetJumpY = transform.position.y + jumpForce;
            }
        }

        HandleJump();
        // Обновляем координату Y земли, если стоим на земле и не на платформе
        if (isOnGround && !isOnGroundPlatform && !isJumping && !isFalling)
        {
            startY = transform.position.y;
        }

    }

    void GetInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void Flip()
    {
        if (horizontal < 0) _sprite.flipX = true;
        if (horizontal > 0) _sprite.flipX = false;
    }

    void CheckRange()
    {
        float x = transform.position.x;
        range = x == minHeightForward || x == maxHeightForward;
    }

    void SwitchSprite()
    {
        bool isMoving = horizontal != 0 || vertical != 0;

        if (isMoving && spriteCoroutine == null)
            spriteCoroutine = StartCoroutine(SwitchSprites());
        else if (!isMoving && spriteCoroutine != null)
        {
            StopCoroutine(spriteCoroutine);
            spriteCoroutine = null;
            sprite.sprite = sprites[0];
        }
    }

    IEnumerator SwitchSprites()
    {
        while (horizontal != 0 || vertical != 0)
        {
            sprite.sprite = sprites[0];
            yield return new WaitForSeconds(0.2f);
            sprite.sprite = sprites[1];
            yield return new WaitForSeconds(0.2f);
        }
    }

    void Move()
    {
        float inputX = horizontal;

        currentHorizontalSpeed = Mathf.MoveTowards(
            currentHorizontalSpeed,
            inputX * speed,
            (Mathf.Abs(inputX) > 0.01f ? horizontalAcceleration : horizontalDeceleration) * Time.deltaTime
        );

        float newX = Mathf.Clamp(transform.position.x + currentHorizontalSpeed * Time.deltaTime, minHeightForward, maxHeightForward);
        float newY = transform.position.y;

        if (!isJumping && !isFalling)
        {
            float inputY = vertical;

            if (!isOnGroundPlatform)
                newY = Mathf.Clamp(transform.position.y + inputY * speed * Time.deltaTime, minHeightUp, maxHeightUp);
        }

        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    void AdjustScale()
    {
        float scaleDelta = -vertical * speed * Time.deltaTime;
        float newScale = Mathf.Clamp(transform.localScale.x + scaleDelta, minScale, maxScale);
        transform.localScale = new Vector3(newScale, newScale, 1f);
    }

    void HandleJump()
    {
        if (isJumping)
        {
            transform.position += Vector3.up * jumpForce * Time.deltaTime;
            if (transform.position.y >= targetJumpY)
            {
                isJumping = false;
                isFalling = true;
            }
        }
        else if (isFalling)
        {
            transform.position -= Vector3.up * fallSpeed * Time.deltaTime;

            if (!isOnGroundPlatform && transform.position.y <= startY)
            {
                transform.position = new Vector3(transform.position.x, startY, transform.position.z);
                isFalling = false;
                isOnGround = true;
            }

            if (isOnGroundPlatform)
            {
                isFalling = false;
                isOnGround = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "Comet1" || collision.gameObject.name == "Comet2")
        {
            Debug.Log("Попала комета");
            sprite.color = originalColor;
            StartCoroutine(FlashEffect());
        }
        if (collision.CompareTag("Ground"))
        {
            isOnGroundPlatform = true;
            isFalling = false;
            isOnGround = true;
        }
    }
    private IEnumerator FlashEffect()
    {
        sprite.color = flashColor; // Меняем цвет на красный
        yield return new WaitForSeconds(flashDuration); // Ждём заданное время
        sprite.color = originalColor; // Возвращаем исходный цвет
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isOnGroundPlatform = false;
            isFalling = true;
            isOnGround = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isFalling)
            {
                isJumping = true;
                isOnGround = false;
                targetJumpY = transform.position.y + jumpForce;
            }
        }
    }
    


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Удар головой — потолок
            if (contact.normal.y < -0.5f)
            {
                if (isJumping)
                {
                    isJumping = false;
                    isFalling = true;
                }
            }

            // Удар в стену сбоку (слева или справа)
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                if (isJumping)
                {
                    isJumping = false;
                    isFalling = true;
                }
            }

            // Приземление снизу (не обрабатываем здесь, а в триггере под ногами)
        }
    }


}
