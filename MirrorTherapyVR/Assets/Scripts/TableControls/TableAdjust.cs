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
    
    public void GoUp(){
        table.transform.Translate(Vector3.up * speed, Space.World);
    }

    public void GoDown(){
        table.transform.Translate(Vector3.up * -speed, Space.World);
    }
}
