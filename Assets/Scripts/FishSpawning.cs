using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawning : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject fishingHook;

    //Change this variable to change how often fish spawn
    public float fishDepthInterval = 2;

    public float maxDepth = -15;

    private void Start()
    {
        for (float i = 3; i < 18; i += fishDepthInterval)
        {
            SpawnFish(-i);
        }
    }

    private void Update()
    {
        if (fishingHook.transform.position.y < maxDepth + 9)
        {
            SpawnFish(maxDepth);
            maxDepth -= fishDepthInterval;
        }
    }

    void SpawnFish(float depth)
    {
        GameObject fish;
        List<GameObject> fishList = gameManager.FindFishAtDepth(depth);
        if (fishList.Count > 0)
        {
            fish = Instantiate(fishList[Random.Range(0, fishList.Count)]);
            fish.transform.position = new Vector3(Random.Range(-49f, 49f), Mathf.Clamp(depth + Random.Range(-3f, 3f), -9999999, -1));
            fish.GetComponent<Fish>().swimSpeed *= Random.Range(0.6f, 1.35f);
            if (Random.Range(0, 2) == 0)
            {
                Vector3 temp = fish.transform.localScale;
                temp.x *= -1;
                fish.transform.localScale = temp;
                fish.GetComponent<Fish>().swimSpeed *= -1;
            }
        }
    }
}
