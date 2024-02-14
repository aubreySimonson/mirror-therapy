using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownButton : MonoBehaviour
{
    public TableAdjust tableAdjust;
    public Collider handCollider;
    public float adjustAmount;

    public void OnTriggerEnter(Collider other){
        if(other == handCollider){
            tableAdjust.GoDown(adjustAmount);
        }
    }
}
