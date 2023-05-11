using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is part of MirrorTherapy
/// This script goes on the "Next Task" button, and causes it to progress to
/// the next task when touched by anything.
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated May 2023
/// </summary>

public class NextTaskButton : MonoBehaviour
{
    public ExperimentManager experimentManager;

    public void OnTriggerEnter(Collider other){
      experimentManager.GoToNextTask();
    }
}
