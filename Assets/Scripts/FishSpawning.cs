using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawning : MonoBehaviour
{
    public GameObject[] fishTypes;
    public GameObject[] rareFishTypes;
    GameObject fishingHook;

    public int maxDepth = -15;

    private void Start()
    {
        for (int i = 3; i < 18; i += 3)
        {
            SpawnFish(-i);
        }
        fishingHook = GameObject.Find("Fishing Hook");
    }

    private void Update()
    {
        if (fishingHook.transform.position.y < maxDepth + 9)
        {
            SpawnFish(maxDepth);
            maxDepth -= 3;
        }
    }

    void SpawnFish(float depth)
    {
        float randomFishIndex = ((-depth*0.75f) + Mathf.Sqrt(Random.Range(0,9f) * Random.Range(0,2))) / 10; // uuuummm... you know maybe I should turn off my math brain and think simpler every once in a while
        randomFishIndex = Mathf.RoundToInt(Mathf.Clamp(randomFishIndex, 0, fishTypes.Length - 1));
        GameObject fish;
        if (Random.Range(0f,1f) > 0.99f)
        {
            fish = Instantiate(rareFishTypes[Random.Range(0,rareFishTypes.Length)]);
        } else
        {
            fish = Instantiate(fishTypes[(int)randomFishIndex]);
        }
        fish.transform.position = new Vector3(Random.Range(-49f, 49f), Mathf.Clamp(depth + Random.Range(-3f, 3f),-9999999,-1));
        fish.GetComponent<Fish>().swimSpeed *= Random.Range(0.6f, 1.35f);
        if (Random.Range(0,2) == 0)
        {
            Vector3 temp = fish.transform.localScale;
            temp.x *= -1;
            fish.transform.localScale = temp;
            fish.GetComponent<Fish>().swimSpeed *= -1;
        }
    }
}
