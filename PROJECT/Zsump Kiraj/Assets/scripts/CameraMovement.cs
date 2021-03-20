using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject follow; // Az objektum amit a kamera követ (tehát a karakter)

    private Camera _camera;
    private float m_verticalUnits; // függõleges egységek száma amit a kamera érzékel
    public bool isGame;

    public Vector2 targetAspect = new Vector2(4, 3);


    void Start()
    {
        _camera = GetComponent<Camera>();
        if (isGame)
        {
            m_verticalUnits = _camera.orthographicSize * 2;
        }


        UpdateCrop();
    }

    void Update()
    {
        if (isGame)
        {
            var level = (int)(follow.transform.position.y / m_verticalUnits);
            transform.position = new Vector3(0, level * m_verticalUnits + m_verticalUnits / 2, -10);
        }

    }

    // Call this method if your window size or target aspect change.
    public void UpdateCrop()
    {
        // Determine ratios of screen/window & target, respectively.
        float screenRatio = Screen.width / (float)Screen.height;
        float targetRatio = targetAspect.x / targetAspect.y;

        if (Mathf.Approximately(screenRatio, targetRatio))
        {
            // Screen or window is the target aspect ratio: use the whole area.
            _camera.rect = new Rect(0, 0, 1, 1);
        }
        else if (screenRatio > targetRatio)
        {
            // Screen or window is wider than the target: pillarbox.
            float normalizedWidth = targetRatio / screenRatio;
            float barThickness = (1f - normalizedWidth) / 2f;
            _camera.rect = new Rect(barThickness, 0, normalizedWidth, 1);
        }
        else
        {
            // Screen or window is narrower than the target: letterbox.
            float normalizedHeight = screenRatio / targetRatio;
            float barThickness = (1f - normalizedHeight) / 2f;
            _camera.rect = new Rect(0, barThickness, 1, normalizedHeight);
        }
    }
}
