using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is part of MirrorTherapy
/// This script goes on a sphere on the center of the fake hand.
/// It recognizes when the player grabs fireflies, and informs
/// Firefly Manager of this fact.
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated March 2023
/// </summary>

public class FireflyGrabber : MonoBehaviour
{
    public bool touchingFirefly = false;
    public bool grabbingFirefly = false;
    public bool isBimanual;//if we're currently doing the bimanual task, we need to have this script not directly contact the firefly manager
    public GameObject fingerMarker;
    public GameObject firefly;
    public FireflyManager fireflyManager;
    public Text debugText;


    void OnTriggerEnter(Collider other){
      if(other.gameObject.tag == "firefly"){
        debugText.text = "firefly touched";
        touchingFirefly = true;
        firefly = other.gameObject;
      }
      if(other.gameObject == fingerMarker && touchingFirefly){
        GrabFirefly();
      }
    }
    void OnTriggerExit(Collider other){
      if(other.gameObject.tag == "firefly"){
        touchingFirefly = false;
        grabbingFirefly = false;
      }
    }

    private void GrabFirefly(){
      grabbingFirefly = true;
      debugText.text = "firefly grabbed";
    }
}
