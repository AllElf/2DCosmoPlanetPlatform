using UnityEngine;
using UnityEngine.UI;

public class CosmoController2 : MonoBehaviour
{
    [Header("Игрок")]
    [SerializeField] GameObject player;
    public int health = 3;
    public int starCount = 0;
    [SerializeField] Text textStarsCount;
    [SerializeField] public GameObject[] heart;
    

    [Header("Настройки прыжка")]
    [SerializeField] float maxJumpHeight = 5f;
    [SerializeField] float jumpAcceleration = 20f;
    [SerializeField] float fallAcceleration = 10f;

    [SerializeField] float startY;
    [SerializeField] float currentVelocityY = 0f;
    [SerializeField] float previousY = 0;
    [SerializeField] bool isPushing = false;

    [Header("Спрайты")]
    public SpriteRenderer spriteRenderer;
    [SerializeField] Sprite jumpSprite;
    [SerializeField] Sprite fallSprite;
    [SerializeField] Sprite pushSprite;

    //[Header("Загрузка данных")]
    //[SerializeField] GameData gameData;

    //[System.Obsolete]
    void Start()
    {
        //gameData = FindObjectOfType<GameData>();
        if (player == null)
        {
            Debug.LogError("Player GameObject не назначен в инспекторе!");
            enabled = false;
            return;
        }
        else if (player != null)
        {
            startY = player.transform.position.y;
            previousY = startY;
        }

        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer не найден на объекте игрока.");
        }
    }
    public void Heart()
    {
        if (heart != null)
        {
            for (int i = 0; i < health; i++)
            {
                heart[i].SetActive(true);
            }
        }
    }
    void StarsCount()
    {
        textStarsCount.text = starCount.ToString();
    }
    void Update()
    {
        StarsCount();
        float currentY = player.transform.position.y;

        // Толчок — при первом нажатии
        if (Input.GetMouseButtonDown(0))
        {
            isPushing = true;
            if (spriteRenderer != null)
                spriteRenderer.sprite = pushSprite;
        }

        // Подъём — когда нажали кнопку
        if (Input.GetMouseButtonDown(0) && currentY < startY + maxJumpHeight)
        {
            currentVelocityY += jumpAcceleration * Time.deltaTime;

            if (!isPushing && spriteRenderer != null)
                spriteRenderer.sprite = jumpSprite;
        }
        else
        {
            currentVelocityY -= fallAcceleration * Time.deltaTime;

            if (!Input.GetMouseButton(0) && spriteRenderer != null)
                spriteRenderer.sprite = fallSprite;

            isPushing = false;
        }

        // Ограничение по высоте
        if (currentY >= startY + maxJumpHeight && currentVelocityY > 0)
        {
            currentVelocityY = 0;
        }

        // Приземление
        if (currentY <= startY && currentVelocityY < 0)
        {
            currentVelocityY = 0;
            player.transform.position = new Vector3(player.transform.position.x, startY, player.transform.position.z);
        }
        else
        {
            player.transform.position += Vector3.up * currentVelocityY * Time.deltaTime;
        }

        previousY = currentY;
        Heart();
        if (player.transform.position.y == startY && !Input.GetMouseButton(0) && currentVelocityY <= 0 && isPushing == false)
        {
            currentVelocityY += jumpAcceleration  * Time.deltaTime;
            isPushing = true;
        }
    }
}