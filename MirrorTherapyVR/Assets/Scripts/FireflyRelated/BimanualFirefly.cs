using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
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
    public FireflyGrabber realHandFireflyGrabber, fakeHandFireflyGrabber;
    public FireflyManager fireflyManager;

    void Update(){
      if(realHandFireflyGrabber.touchingFirefly && fakeHandFireflyGrabber.touchingFirefly){
        realHandFireflyGrabber.touchingFirefly = false;
        fakeHandFireflyGrabber.touchingFirefly = false;
        //we never actually grab the firefly with the 
        fireflyManager.NextFirefly();
      }
    }
}
