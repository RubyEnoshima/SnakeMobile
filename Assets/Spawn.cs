using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
 {
    
    public GameObject Element;
    public Transform Area;

    // Borders
    int esq = -380;
    int dre = 380;
    int adalt = 180;
    int abaix = -180;

    public int elements = 1;
    public int maxelements = 4;
    public float temps = 4f;

    IEnumerator SpawnPoma(){
        yield return new WaitForSeconds(temps);
        Spawnejar();
        StartCoroutine(SpawnPoma());
    }

    void Start () {
        StartCoroutine(SpawnPoma());

    }
    

    // Spawn one piece of food
    void Spawnejar() {
        if(elements < maxelements){
            int x=Random.Range(esq,dre+1),y=Random.Range(abaix,adalt+1);

            x = x-(x%20);
            y = y-(y%20);

            if(Element.name=="Obstacle"){
                x+=10; y+=10;
            }

            GameObject go = Instantiate(Element);
            go.transform.SetParent(Area,false);
            go.transform.localPosition = new Vector3(x,y);
            go.name = Element.name;

            elements++;
        }
    }
}