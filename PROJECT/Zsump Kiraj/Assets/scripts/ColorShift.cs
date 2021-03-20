
using UnityEngine;
using UnityEngine.UI;

public class ColorShift : MonoBehaviour
{
    float Speed = 0.5f;
    public Image rend;

    void Start()
    {

    }

    void Update()
    {
        rend.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * Speed, 0.5f), 1f, 0.6f));
        //rend.color(HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * Speed, 1), 1, 1)));
    }
}