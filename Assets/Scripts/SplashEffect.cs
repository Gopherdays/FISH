using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashEffect : MonoBehaviour
{
    //Separate the splash effect from the hook so it can be used elsewhere
    public GameObject splash;
    public GameObject parent;

    public float splashHeight = 0;
    public float intensity = 1;
    public float scale = 1;

    public bool velocityBased;
    public float velocityMod;

    public bool canvasOverride;

    bool splashed = false;

    private void FixedUpdate()
    {
        if (!splashed && transform.position.y < splashHeight)
            Splash();
        splashed = transform.position.y < splashHeight;
    }

    public void Splash()
    {
        splashed = true;
        for (int i = 0; i < Mathf.Clamp(Random.Range(40 * intensity, 61 * intensity),1,5000); i++)
        {
            GameObject g = Instantiate(splash, transform.position, Quaternion.identity);
            if (parent != null)
                g.transform.parent = parent.transform;
            if (canvasOverride)
            {
                g.transform.position = Camera.main.ScreenToWorldPoint(transform.position);
                g.transform.position = new Vector2(g.transform.position.x, g.transform.position.y);
            }
                
            float rand = Random.Range(0.15f, 0.35f) * scale;
            g.transform.localScale = new Vector2(rand, rand);
            float xVal = Random.Range(-0.5f, 0.5f) * intensity * scale;
            Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
            if (velocityBased)
                rb.AddForce(new Vector2(xVal, 2 + Random.Range(0f, 1f) - Mathf.Abs(2 * xVal)) * (30 + Random.Range(0, 50)) * intensity * GetComponent<Rigidbody2D>().velocity.magnitude * velocityMod);
            else
                rb.AddForce(new Vector2(xVal, 2 + Random.Range(0f, 1f) - Mathf.Abs(2 * xVal)) * (30 + Random.Range(0, 50)) * intensity);
            Debug.Log(xVal);
            Debug.Log(rand);
            Destroy(g, 4);
        }
    }
}
