using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] animalPrefabs;
    private float xPosRange = 13;
    private PlayerController playerCtrl;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomAnimal", 3.0f, 1.5f);
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnRandomAnimal()
    {
        if (playerCtrl.gameOver == false)
        {
            float randXPos = Random.Range(-xPosRange, xPosRange);
            int animalPrefabIndex = Random.Range(0, animalPrefabs.Length);
            Vector3 randPos = new Vector3(randXPos, 1, 18);
            Instantiate(animalPrefabs[animalPrefabIndex], randPos,
                animalPrefabs[animalPrefabIndex].transform.rotation);
        }
    }

}
