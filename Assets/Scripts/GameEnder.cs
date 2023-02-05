using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEnder : MonoBehaviour
{
    public TMP_Text textfield;
    public GameObject textfieldObj;

    public GameObject[] corpses;
    private int score1 = 0;
    private int score2 = 0;

    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
            corpses = GameObject.FindGameObjectsWithTag("corpse");           

    }

    public void AddScore1(){
         score1+=1;
    }

    public void AddScore2(){
         score2+=1;
    }

    // Update is called once per frame
    void Update()
    {
        if(corpses.Length<=score1+score2){
            GameOver();            
        }
    }

    void GameOver(){
         textfieldObj.SetActive(true);
         if(score2>=score1){
            textfield.text = "Yellow won!";
         }else{
            textfield.text = "Brown won!";
         }
         
    }
}
