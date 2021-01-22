using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject follow; // Az objektum amit a kamera k�vet (teh�t a karakter)

    private Camera m_camera;
    private float m_verticalUnits; // f�gg�leges egys�gek sz�ma amit a kamera �rz�kel

    void Start()
    {
        m_camera = GetComponent<Camera>();
        m_verticalUnits = m_camera.orthographicSize * 2;
    }

    void Update()
    {
        var level = (int)(follow.transform.position.y / m_verticalUnits);
        transform.position = new Vector3(0, level * m_verticalUnits + m_verticalUnits / 2, -10);
    }
}
