using UnityEngine;

public class CameraAutoFit : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float posX;
    [SerializeField] private float posY;
    [SerializeField] private float posZ;
    [SerializeField] Vector3 pozCam;
    [SerializeField] bool orientation;
    

    void Start()
    {
        pozCam = mainCamera.transform.position;
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            ScreenOrientation();
        }        
    }

    void Update()
    {
        ScreenOrientation();
    }
    void ScreenOrientation()
    {
        // Проверяем ориентацию экрана
        if (Screen.width > Screen.height && orientation == true) // Горизонтальная ориентация
        {
            mainCamera.transform.position = pozCam;
            Debug.Log("Перевернул горизонтально");
            orientation = false;
        }
        else if (Screen.width < Screen.height && orientation == false) // Вертикальная ориентация
        {
            mainCamera.transform.position = new Vector3(posX, posY, posZ);
            Debug.Log("Перевернул вертикально");
            orientation = true;
        }
    }
}
 