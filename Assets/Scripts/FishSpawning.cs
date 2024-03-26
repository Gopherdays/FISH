using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawning : MonoBehaviour
{
    public GameObject[] fishTypes;

    private void Start()
    {
        for (int i = 3; i < 1000; i++)
        {
            SpawnFish(-i);
        }
    }

    void SpawnFish(float depth)
    {
        float randomFishIndex = (-depth + Mathf.Sqrt(Random.Range(0,9f) * Random.Range(0,2))) / 10;
        randomFishIndex = Mathf.RoundToInt(Mathf.Clamp(randomFishIndex, 0, fishTypes.Length - 1));
        GameObject fish = Instantiate(fishTypes[(int)randomFishIndex]);
        fish.transform.position = new Vector3(Random.Range(-45f, 45f), Mathf.Clamp(depth + Random.Range(-3f, 3f),-9999999,-1));
    }
}
