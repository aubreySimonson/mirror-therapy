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
    private bool touchingFirefly = false;
    public GameObject fingerMarker;
    private GameObject firefly;
    public FireflyManager fireflyManager;

    void OnTriggerEnter(Collider other){
      if(other.gameObject.tag == "firefly"){
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
      }
    }

    private void GrabFirefly(){
      Destroy(firefly);
      touchingFirefly = false;
      //play win sound
      //rell firefly manager about it
      fireflyManager.NextFirefly();
    }
}
