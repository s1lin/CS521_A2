using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {
    
    public float maxSpeed = 2f;
    public GameObject cloudPrefab;

    private float windSpeed;
    private List<GameObject> cloudInstance;

    void Start() {
        windSpeed = 0;
        cloudInstance = new List<GameObject>();

        for (int i = 0; i < 2; i++) {
            Vector3 position = cloudPrefab.transform.position + new Vector3(i * 2, i * 3, 0);
            cloudInstance.Add(Instantiate(cloudPrefab, position, cloudPrefab.transform.rotation, null) as GameObject);
        }
        InvokeRepeating("WindGenerator", 0f, 2f); //update every 2 seconds
    }

    void Update() {
        for (int i = 0; i < 2; i++) {
            cloudInstance[i].transform.position += new Vector3(windSpeed * Time.deltaTime, 0, 0);
        }
    }

    private void WindGenerator() {
        windSpeed = Random.Range(-maxSpeed, maxSpeed);
    }

    public float WindSpeed() {
        return windSpeed;
    }
}
