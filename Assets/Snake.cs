using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    public Joystick joystick;
    public Sprite sprite;
    float offset = 0.35f;
    float offsetAcc = 0.45f;
    float velocitat = 0.35f;
    Vector2Int Direccio = Vector2Int.right;
    Vector2Int Posicio;
    Vector2Int UltimaPosicio;
    int punts = 0;
    int temps = 0;
    public Text Punts;
    public Text Temps;
    public Text Nom;
    public Transform Area;
    public Spawn SpawnPomes;
    public Spawn SpawnObstacles;
    public GameObject JoystickGO;
    
    List<Transform> Cua;
    bool menjat = false;
    public GameObject cuaPrefab;

    void Start()
    {
        if(!Global.Joystick) JoystickGO.SetActive(false);
        Nom.text = Global.Nom;
        Cua = new List<Transform>();
        Posicio = new Vector2Int(0, 0);
        StartCoroutine(MoureTemps());
        StartCoroutine(TempsPunts());
    }

    void CanviarDireccio(){
        if(!Global.Joystick){
            if(Input.acceleration.x>offset){
                if(Direccio==Vector2Int.right){
                    Direccio = Vector2Int.down;
                }else if(Direccio == Vector2Int.down){
                    Direccio = Vector2Int.left;
                }else if(Direccio == Vector2Int.left){
                    Direccio = Vector2Int.up;
                }else Direccio = Vector2Int.right;
            }else if(Input.acceleration.x<-offset){
                if(Direccio==Vector2Int.right){
                    Direccio = Vector2Int.up;
                }else if(Direccio == Vector2Int.down){
                    Direccio = Vector2Int.right;
                }else if(Direccio == Vector2Int.left){
                    Direccio = Vector2Int.down;
                }else Direccio = Vector2Int.left;
            }
        }else{
            if((joystick.Horizontal>offset ) && Direccio!=Vector2Int.left){
                Direccio = Vector2Int.right;
            }
            else if((joystick.Horizontal<-offset) && Direccio!=Vector2Int.right) {
                Direccio = Vector2Int.left;
            }
            else if((joystick.Vertical>offset ) && Direccio!=Vector2Int.down) {
                Direccio = Vector2Int.up;
            }
            else if((joystick.Vertical<-offset) && Direccio!=Vector2Int.up) {
                Direccio = Vector2Int.down;
            }

        }

    }

    void Moure(){
        UltimaPosicio = Posicio;
        // for(int i=1;i<Cua.Count;i++){
        //     Cua[i].localPosition = Cua[i-1].localPosition;
        // }
        Posicio.x += Direccio.x*20;
        Posicio.y += Direccio.y*20;
        transform.localPosition = new Vector3(Posicio.x,Posicio.y);
        if(Cua.Count>0){
            Cua[Cua.Count-1].localPosition = Cua[0].localPosition;
            Cua.Insert(1,Cua[Cua.Count-1]);
            Cua.RemoveAt(Cua.Count-1);
            Cua[0].localPosition = new Vector3(UltimaPosicio.x,UltimaPosicio.y);
        }
    }

    IEnumerator MoureTemps(){
        yield return new WaitForSeconds(velocitat);
        if(menjat){
            Allargar();
            menjat = false;
        }
        CanviarDireccio();
        Moure();
        StartCoroutine(MoureTemps());
    }

    IEnumerator TempsPunts(){
        yield return new WaitForSeconds(1f);
        temps++;
        if(velocitat>0.1f && temps%7==0) velocitat -= 0.025f;
        Temps.text = temps.ToString();
        StartCoroutine(TempsPunts());
    }

    void Allargar(){
        GameObject go = Instantiate(cuaPrefab);
        go.transform.SetParent(transform.parent,false);
        Cua.Add(go.transform);
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.touchCount > 0){
            int i=0;
            while(i<Input.touchCount){
                Touch touch = Input.GetTouch(i);
                if(touch.phase!=TouchPhase.Began){
                    i++;
                    continue;
                }
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                Debug.Log(ray.origin);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit))
                {
                    GameObject o = hit.collider.gameObject;
                    Debug.Log(hit.point);
                    if (o.tag == "Obstacle")
                    {
                        Destroy(o);
                        punts++;
                        Punts.text = punts.ToString();
                        SpawnObstacles.elements--;
                        break;
                    }
                }
                i++;
            }

        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.name == "Poma") {
            punts++;
            Punts.text = punts.ToString();
            menjat = true;
            Destroy(coll.gameObject);
            SpawnPomes.elements--;
        }
        else {
            if(punts > Global.Punts) Global.Punts = punts;
            if(temps > Global.Temps) Global.Temps = temps;
            SceneManager.LoadScene("Start");
        }
    }
    
}
