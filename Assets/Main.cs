using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public Text Punts;
    public Text Temps;
    private void Start() {
        Punts.text = Global.Punts.ToString();
        Temps.text = Global.Temps.ToString();
    }
    public void JugarJ(){
        Global.Joystick = true;
        SceneManager.LoadScene("Joc");
    }

    public void JugarA(){
        Global.Joystick = false;
        SceneManager.LoadScene("Joc");
    }
}
