using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //Ghost    
    public GameObject ghostPrefab;
    public GameObject[] ghostInstances;

    private float posX = 0f;
    private float posY = 1f;

    private void Start() {
        InitGhosts();
    }
    private void InitGhosts() {
        ghostInstances = new GameObject[4];

        for (int i = 0; i < 4; i++) {

            float initX = Random.Range(posX, posX + 5f);
            float initY = Random.Range(posY, posY + i);

            ghostInstances[i] = Instantiate(ghostPrefab, new Vector3(initX, initY, 0), transform.rotation, null) as GameObject;
            ghostInstances[i].GetComponent<Ghost>().RootIndex = i;
        }
    }

    public void RespawnGhost(int index) {

        float initX = Random.Range(posX, posX + 5f);
        float initY = Random.Range(posY, posY + 1f);

        ghostInstances[index] = Instantiate(ghostPrefab, new Vector3(initX, initY, 0), transform.rotation, null) as GameObject;
        ghostInstances[index].GetComponent<Ghost>().RootIndex = index;
       
    }
}
