using System.Collections;
using TMPro;
using UnityEngine;

public class Hints : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private ParallaxBackground2 _background;
    [SerializeField] private CosmoController _cosmoController;
    [SerializeField] int _count = 0;
    [SerializeField] float delay, saveText;
    [SerializeField] public bool _corutinStop, _request, saveTextBool;
    [Header("Список всплывающих подсказок")]
    [Multiline][SerializeField] string[] hint;
    [Header("Текст и цвета для текста")]
    [SerializeField] TextMeshPro textComponent;
    [SerializeField] private Color[] colors; // Массив цветов

    private void Awake()
    {
        saveTextBool = false;
        //_corutinStop = true;
        _cosmoController = GameObject.FindObjectOfType<CosmoController>();
    }
    private void Start()
    {
        _background = GetComponent<ParallaxBackground2>();
    }
    void Background()
    {
        if (_corutinStop)
        {
            if (_background != null)
            {
                _background.enabled = true;
            }
        }
        else
        {
            _background.enabled = false;
        }
    }
    private void Update()
    {
        Request();
        Background();
    }
    IEnumerator TypeText()
    {
        if (_count < hint.Length)
        {
            string fullHint = hint[_count];
            string[] words = fullHint.Split(' '); // Разделяем на слова
            string coloredText = "";
            _corutinStop = false;

            for (int i = 0; i < words.Length; i++)
            {
                string currentWord = words[i];
                string colorTagStart = $"<color=#{ColorUtility.ToHtmlStringRGB(colors[i % colors.Length])}>"; // Используем Color вместо строки
                string colorTagEnd = "</color>";

                for (int j = 0; j < currentWord.Length; j++)
                {
                    coloredText += colorTagStart + currentWord[j] + colorTagEnd;
                    
                    yield return new WaitForSeconds(delay);
                    textComponent.text = coloredText;
                }
                coloredText += " "; // Добавляем пробел после слова
            }

            StartCoroutine(TextClear());
            _corutinStop = true;
            _count++;
        }
    }
    IEnumerator TextClear()
    {
        saveTextBool = true;
        yield return new WaitForSeconds(saveText);
        textComponent.text = "";
        saveTextBool = false;
    }
    public void Request()
    {
        if (_request && _corutinStop && !saveTextBool)
        {
            _request = false;
            Debug.Log("Start Request");
            StopAllCoroutines();
            textComponent.text = "";
            StartCoroutine(TypeText()); 
            _corutinStop = false;
            Debug.Log("End Request");
        }   
    }
}