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

        //Generate Cloud at random position
        cloudInstance = new List<GameObject>();
        float x = cloudPrefab.transform.position.x;
        float y = cloudPrefab.transform.position.y;

        for (int i = 0; i < 4; i++) {

            float initX = Random.Range(9f, 10f);
            float initY = Random.Range(y - 1f, y + 1f);
            Vector3 position = cloudPrefab.transform.position + new Vector3(initX * i, initY, 0);
            cloudInstance.Add(Instantiate(cloudPrefab, position, cloudPrefab.transform.rotation, null) as GameObject);
        }

        //update speed of wind every 2 seconds
        InvokeRepeating("WindGenerator", 0f, 2f);
    }

    void Update() {
        for (int i = 0; i < 4; i++) {
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
