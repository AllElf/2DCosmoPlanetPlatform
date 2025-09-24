using UnityEngine;
using UnityEngine.UI;

public class ManagerInfo : MonoBehaviour
{
    GameData gameData;
    [SerializeField] Text textScoreMoon;

    [System.Obsolete]
    private void Start()
    {
        gameData = FindObjectOfType<GameData>();
        TextScoreMoon();
    }
    public void TextScoreMoon()
    {
        if(gameData != null && textScoreMoon != null)
        {
            textScoreMoon.text = $"STAR  {gameData.scoreStars.ToString()}";
        }
        
    }
}
