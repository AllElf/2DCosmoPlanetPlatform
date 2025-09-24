using System.Collections;
using UnityEngine;

public class ChangingTheSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sprites;
    [SerializeField] float speedNext = 0.2f;
    int currentIndex = 0;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (sprites != null && sprites.Length > 0)
            StartCoroutine(NextSprites());
    }


    IEnumerator NextSprites()
    {
        while(true)
        {
            spriteRenderer.sprite = sprites[currentIndex];
            currentIndex++;
            if (currentIndex >= sprites.Length)
                currentIndex = 0;
            yield return new WaitForSeconds(speedNext);
        }
    }
}
