using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Closes table controls, also starts the study
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated January 2024
/// </summary>

public class CloseTableControls : MonoBehaviour
{
    public GameObject closeThis;
    public Collider handCollider;
    public ExperimentManager experimentManager;

    public void OnTriggerEnter(Collider other){
        if(other==handCollider){
            experimentManager.GoToNextTask();//something you can't find turns on sync at the start of the game, so this, while optimal, hasn't been working
            //you just have to be careful to not accidentally pinch anything while adjusting the table height
            closeThis.SetActive(false);
        }
    }
}
