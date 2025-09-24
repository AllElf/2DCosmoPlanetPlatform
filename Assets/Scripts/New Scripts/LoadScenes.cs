using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    [System.Serializable]
    public class SceneEntry
    {
        public string key;
        public string sceneName;
    }
    [SerializeField] private List<SceneEntry> sceneList;
    private readonly string[] predefinedKeys = { "Moon", "Mars", "Jupiter", "Venus", "Saturn", "SceneMenu" };
    private readonly string[] predefinedsceneName = { "MoonCosmonautTourist", "MarsCosmonautTourist", "JupiterCosmonautTourist", "VenusCosmonautTourist", "SaturnCosmonautTourist", "SceneMenuCosmonautTourist" };

    void Awake()
    {
        for (int i = 0; i < sceneList.Count && i < predefinedKeys.Length; i++)
        {
            sceneList[i].key = predefinedKeys[i];
            sceneList[i].sceneName = predefinedsceneName[i];
        }
    }

    public string GetSceneNameByKey(string key)
    {
        foreach (var entry in sceneList)
        {
            if (entry.key == key)
                return entry.sceneName;
        }

        Debug.LogWarning("Ключ не найден: " + key);
        return null;
    }
    public void LoadMoon()
    {
        SceneManager.LoadScene(GetSceneNameByKey("Moon"));
    }
    public void LoadMars()
    {
        SceneManager.LoadScene(GetSceneNameByKey("Mars"));
    }
    public void LoadJupiter()
    {
        SceneManager.LoadScene(GetSceneNameByKey("Jupiter"));
    }
    public void SceneMenu()
    {
        SceneManager.LoadScene(GetSceneNameByKey("SceneMenu"));
    }
    public void LoadVenus()
    {
        SceneManager.LoadScene(GetSceneNameByKey("Venus"));
    }
}
