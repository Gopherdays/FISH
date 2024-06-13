using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementPopup : MonoBehaviour
{
    public Achievement achievement;

    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;
    public Image image;

    RectTransform t;

    private void Start()
    {
        t = GetComponent<RectTransform>();
        GetComponent<Rigidbody2D>().velocity = Vector2.up * 150;
    }

    private void Update()
    {
        title.text = achievement.name;
        desc.text = achievement.desc;
        image.sprite = achievement.icon;
        if (t.anchoredPosition.y < -300)
            Destroy(gameObject);
        else if (t.anchoredPosition.y > 0)
            t.anchoredPosition = Vector2.zero;
    }
}
