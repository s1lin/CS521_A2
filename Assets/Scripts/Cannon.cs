using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public GameObject cannonBallPrefab;
    public GameObject cannon;

    private float cannonAngle; 
    void Update() {

        if (Input.GetKey(KeyCode.UpArrow)) {
            cannonAngle += 100f * Time.deltaTime;
            if (cannonAngle > 90f)
                cannonAngle = 90f;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            cannonAngle -= 100f * Time.deltaTime;
            if (cannonAngle < 0f)
                cannonAngle = 0f;
        }

        cannon.transform.localRotation = Quaternion.Euler(0, -180, cannonAngle);

        if (Input.GetKeyDown(KeyCode.Space)) {
            Vector3 posistion = cannon.transform.Find("Frontmost").position;
            Instantiate(cannonBallPrefab, posistion, cannon.transform.localRotation, null);
        }


    }
}
