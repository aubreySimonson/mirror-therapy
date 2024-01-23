using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpButton : MonoBehaviour
{
    public TableAdjust tableAdjust;
    public Collider handCollider;


    public void OnTriggerEnter(Collider other){
        if(other == handCollider){
            tableAdjust.GoUp();
        }
    }
}
