using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public bool isPaused  = false;
    [SerializeField] private AudioListener cameraLisener;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprite;
    [SerializeField] private GameData data;

    [System.Obsolete]
    private void Start()
    {
        data = FindObjectOfType<GameData>();
        cameraLisener = Camera.main.GetComponent<AudioListener>();
    }
    public void Paused()
    {
        isPaused = !isPaused;
    }
    public void Sound()
    {
        if (cameraLisener != null && data != null)
        {
            data.audioLisener = !data.audioLisener;
        }
    }
    void EnabledSound()
    {
        cameraLisener.enabled = data.audioLisener;
        if (sprite != null && image != null && cameraLisener.enabled)
        {
            image.sprite = sprite[0];
            data.audioLisener = true;
        }
        else if (sprite != null && image != null && !cameraLisener.enabled)
        {
            image.sprite = sprite[1];
            data.audioLisener = false;
        }
    }
    void EnabledTimeGame()
    {
        if (isPaused)
        {
            Time.timeScale = 0.0f;
        }
        else if (!isPaused)
        {
            Time.timeScale = 1.0f;
        }
    }
    private void Update()
    {
        EnabledTimeGame();
        EnabledSound();
    }
}
