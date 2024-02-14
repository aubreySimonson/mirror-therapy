using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableAdjust : MonoBehaviour
{
    public GameObject table;
    public float speed;

    void Start(){
        if(speed<0.0001f){
            speed = 0.0001f;//presumably you forgot to pick a speed
        }
    }
    
    public void GoUp(float amount){
        table.transform.Translate(Vector3.up * amount, Space.World);
    }

    public void GoDown(float amount){
        table.transform.Translate(Vector3.up * -amount, Space.World);
    }
}
