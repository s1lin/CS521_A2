﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour {

    public float initialVelocity;
    public float restitution = 0.5f;
    public float gravity;

    private float vX, vY, angle;

    private float posX, posY;
    private float nextPosX, nextPosY;
    private Vector2 nextPos;

    private float windSpeed;
    private bool top = false;

    //collision handling

    private Wind wind;

    void Start() {
        nextPosX = transform.position.x;
        nextPosY = transform.position.y;
        wind = GameObject.FindGameObjectWithTag("game").GetComponent<Wind>();

        angle = transform.rotation.eulerAngles.z;

        vX = initialVelocity * Mathf.Cos(Mathf.Deg2Rad * angle);
        vY = initialVelocity * Mathf.Sin(Mathf.Deg2Rad * angle);
    }

    // Update is called once per frame
    void Update() {
        posY = nextPosY;
        posX = nextPosX;

        nextPos = new Vector2(nextPosX, +nextPosY);

        windSpeed = wind.WindSpeed();
        Movement();  
    }

    private void FixedUpdate() {
        if (IsOutOfBound(transform.position))
            Destroy(gameObject);
        if (IsLostKinetic())
            Destroy(gameObject);
    }
    void Movement() {

        vY -= gravity;

        if (posY > 7.0f)
            vX -= windSpeed * Time.deltaTime;

        nextPosX -= vX;
        nextPosY += vY - gravity;

        transform.position = Vector3.Lerp(transform.position, new Vector3(nextPosX, nextPosY, 0), 0.0001f);
    }

    private bool IsOutOfBound(Vector3 position) {
        return position.y <= 1.0f || position.x < 0 || position.x > 30;
    }

    private bool IsLostKinetic() {
        return false;
    }

    public void Bounce(Vector3 vertex, int i) {
        restitution = Random.Range(0.5f, 0.95f);
        posX = nextPosX;
        posY = nextPosY;
        Vector3 vertexNom = vertex.normalized;

        //hit the top stone
        if (i > 242 && i < 484 || top) {
            //top = true;
            vY = -vY * restitution * vertexNom.y * 1.5f;
            nextPosY = transform.position.y + vY;
        } else {//hit the right stone
            vX = -vX * restitution * 1.5f;
            vY = vertexNom.y * restitution;
            nextPosY = transform.position.y - vY;
        }

        nextPosX = transform.position.x - vX;
        

        transform.position = Vector3.Lerp(transform.position, new Vector3(nextPosX, nextPosY, 0), 0.05f);
    }

}
