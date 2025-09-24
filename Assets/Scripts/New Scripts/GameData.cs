using UnityEngine;

public class GameData : MonoBehaviour
{
    public int scoreStars;
    public string playerName;
    public bool audioLisener;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        scoreStars = LoadScore();
    }
    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt("TotalScore", score);
        PlayerPrefs.Save(); // необ€зательно, но можно вызвать вручную
    }

    public int LoadScore()
    {
        return PlayerPrefs.GetInt("TotalScore", 0); // 0 Ч значение по умолчанию
    }
}