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
            experimentManager.GoToNextTask();
            closeThis.SetActive(false);
        }
    }
}
