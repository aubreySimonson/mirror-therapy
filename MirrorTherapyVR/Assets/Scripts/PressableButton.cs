using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is part of MirrorTherapy
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
  public Text debugText;

  public void Highlight(){
    gameObject.GetComponent<Renderer>().material = highlightedMat;
    highlighted = true;
    //debugText.text = "highlighted";
  }
  public void UnHighlight(){
    gameObject.GetComponent<Renderer>().material = defaultMat;
    highlighted = false;
    //debugText.text = "unhighlighted";
  }

  void OnTriggerEnter(Collider other){
    if(highlighted && other.gameObject.tag == "canPress"){
      UnHighlight();
      buttonsManager.NextButton();
    }
  }
}
