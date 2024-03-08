using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawning : MonoBehaviour
{
    public GameObject[] fishTypes;

    private void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            SpawnFish(-i);
        }
    }

    void SpawnFish(float depth)
    {
        float randomFishIndex = depth + (Mathf.Sqrt(Random.Range(0,9f) * Random.Range(-1,2)));
        Debug.Log(randomFishIndex);
        randomFishIndex = Mathf.RoundToInt(Mathf.Clamp(randomFishIndex, 0, fishTypes.Length - 1));
        Debug.Log(randomFishIndex);
        Debug.Log((int)randomFishIndex);
        GameObject fish = Instantiate(fishTypes[(int)randomFishIndex]);
        fish.transform.position = new Vector3(Random.Range(-15f, 15f), Mathf.Clamp(depth + Random.Range(-3f, 3f),-9999999,-1));
    }
}
