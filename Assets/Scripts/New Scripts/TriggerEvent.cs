using System.Collections;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] string prefabTag = "Prefab";
    [SerializeField] string starTag = "Star";
    [SerializeField] string heart = "Heart";
    [SerializeField] CosmoController2 cosmoController;
    [SerializeField] float flashDuration = 0.2f;
    [SerializeField] int flashCount = 3;
    [SerializeField] Color flashColor;
    [SerializeField] SpriteRenderer flashRenderer;
    [SerializeField] bool isCoroutineRunning = false;
    [SerializeField] GameData gameData;

    [System.Obsolete]
    private void Start()
    {
        flashRenderer = cosmoController.spriteRenderer;
        gameData = FindObjectOfType<GameData>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == prefabTag)
        {
            if (cosmoController != null && flashRenderer != null && !isCoroutineRunning && cosmoController.health > 0)
            {
                StartCoroutine(FlashSprite(flashRenderer));
                cosmoController.health--;
                if (cosmoController.health <= 0)
                {
                    Debug.Log("You Death!");
                }
                Debug.Log("Is Coroutine running");
            }
            for (int i = 0; i < cosmoController.heart.Length; i++)
            {
                cosmoController.heart[i].SetActive(false);
            }
        }
        else if (collision.tag == starTag)
        {
            cosmoController.starCount++;
            if(gameData != null && cosmoController.starCount > gameData.scoreStars)
            {
                gameData.scoreStars = cosmoController.starCount;
                gameData.SaveScore(gameData.scoreStars);
            }
            Destroy(collision.gameObject);
        }
        else if (collision.tag == heart)
        {
            if(cosmoController.health < 3)
            {
                cosmoController.health++;
            }
            Destroy(collision.gameObject);
        }
    }
    IEnumerator FlashSprite(SpriteRenderer sr)
    {
        isCoroutineRunning = true;

        Color originalColor = sr.color;

        for (int i = 0; i < flashCount; i++)
        {
            sr.color = flashColor;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.3f); // полупрозрачный
            yield return new WaitForSeconds(flashDuration);
            sr.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
        isCoroutineRunning = false;

    }
}
