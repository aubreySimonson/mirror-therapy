using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is part of MirroTherapy.cs
/// This script goes on each button.
/// Buttons should have rigidbodies, and colliders with isTrigger checked.
/// Remember to set the tags.
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated January 2023
/// </summary>

public class PressableButton : MonoBehaviour
{
  public int quadrant;//which of our 4 quadrants is this in?
  public Material highlightedMat, defaultMat;
  public ButtonsManager buttonsManager;
  private bool highlighted = false;

  public void Highlight(){
    gameObject.GetComponent<Renderer>().material = highlightedMat;
    highlighted = true;
  }
  public void UnHighlight(){
    gameObject.GetComponent<Renderer>().material = defaultMat;
    highlighted = false;
  }

  void OnTriggerEnter(Collider other){
    if(highlighted && other.gameObject.tag == "canPress"){
      buttonsManager.NextButton();
      UnHighlight();
    }
  }
}
