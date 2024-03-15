using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public bool fadeout;
    public float seconds;
    SpriteRenderer sprite;
    Color color = new(255, 255, 255);

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (fadeout)
            color.a = 0;
    }

    void FixedUpdate()
    {
        if (fadeout)
            color.a += Time.deltaTime / seconds;
        else
            color.a -= Time.deltaTime / seconds;
        sprite.color = color;
    }

    public IEnumerator Fade(float time)
    {
        while (color.a !> 1 && color.a !< 0)
        {
            color.a += Time.deltaTime * (1 / seconds);
            yield return new WaitForFixedUpdate();
        }
        yield return true;
    }
}
