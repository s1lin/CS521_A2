using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Wind wind;
    public StoneHengeVertices hengeVertices;

    //Ghost    
    public GameObject ghostPrefab;
    public GameObject[] ghostInstances;
    private float posX = -2f;
    private float posY = 3f;
    private float offset = 1f;

    private void Start() {
        InitGhosts();
    }
    private void InitGhosts() {
        ghostInstances = new GameObject[4];

        for (int i = 0; i < 4; i++) {

            float initX = Random.Range(posX, posX + i * offset);
            float initY = Random.Range(posY, posY + i * offset);

            ghostInstances[i] = Instantiate(ghostPrefab, new Vector3(initX, initY, 0), transform.rotation, null) as GameObject;
            ghostInstances[i].GetComponent<Ghost>().RootIndex = i;
        }

    }

    public void RespawnGhost(int index) {
        
        float initX = Random.Range(posX, posX + offset);
        float initY = Random.Range(posY, posY + offset);

        ghostInstances[index] = Instantiate(ghostPrefab, new Vector3(initX, initY, 0), transform.rotation, null) as GameObject;
        ghostInstances[index].GetComponent<Ghost>().RootIndex = index;
    }
}
