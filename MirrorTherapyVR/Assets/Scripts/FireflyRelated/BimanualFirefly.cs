using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// OBSCELETE-- Firefly manager now incorporates this
/// This script is part of MirrorTherapy
/// Put this script wherever.
/// During the bimanual firefly task, it keeps track of both firefly scripts.
/// When we're grabbing the firefly with both hands,
/// this script informs firefly manager of that fact
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated April 2023
/// </summary>

public class BimanualFirefly : MonoBehaviour
{
    public FireflyGrabber rightHandFireflyGrabber, leftHandFireflyGrabber;
    public FireflyManager fireflyManager;
    public Text debugText;


    void Update(){
      if(rightHandFireflyGrabber.grabbingFirefly && leftHandFireflyGrabber.grabbingFirefly){
        debugText.text = "both hands grabbing firefly";
        rightHandFireflyGrabber.touchingFirefly = false;
        leftHandFireflyGrabber.touchingFirefly = false;
        rightHandFireflyGrabber.grabbingFirefly = false;
        leftHandFireflyGrabber.grabbingFirefly = false;

        //we never actually grab the firefly with the
        fireflyManager.NextFirefly(rightHandFireflyGrabber.firefly);
      }
    }
}
