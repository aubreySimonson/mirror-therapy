using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is part of MirrorTherapy
/// This script keeps track of fireflies, and logs information about when they're grabbed
///
///This script and buttons manager could probably be made to inherit from the same class,
///rather than sharing so much copy-pasted code, if you wanted to do some refactoring.
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated April 2023
/// </summary>

public class FireflyManager : MonoBehaviour
{
    public ExperimentManager experimentManager;
    public Transform fireflyLocation1, fireflyLocation2, fireflyLocation3, fireflyLocation4;
    public Transform currentFireflyLocation;
    public GameObject fireflyPrefab;
    public GameObject currentFirefly;
    public AudioSource goodSound;

    //this is sort of spaghetti, but we need to tell the hands which type of mirroring to do
    public HandPositionMirror leftHand, rightHand;

    public List<int> quadrantOrder;

    private int fireflyCounter = 0;//how many fireflies have we grabbed in this round?
    private int roundsCounter = 0;//how many rounds have there been?

    public FireflyGrabber rightHandFireflyGrabber, leftHandFireflyGrabber;
    public bool isBimanual;

    public Text debugText;

    //start the next round of 10 button presses
    public void NextRound(){
      if(roundsCounter < 4){//we increment from 0 to 1 before the first round, making this 0 indexed
        fireflyCounter = 0;
        roundsCounter++;
        quadrantOrder = experimentManager.GetNextOrder();
        Debug.Log("quadrant order at firefly counter is " + quadrantOrder[fireflyCounter]);

        //the following workaround is gross, and I'm sorry
        currentFirefly = Instantiate(fireflyPrefab);
        NextFirefly(currentFirefly);
      }
      else{
        experimentManager.FinishTask();
      }
    }

    public void Update(){
      if(isBimanual){
        if(rightHandFireflyGrabber.grabbingFirefly && leftHandFireflyGrabber.grabbingFirefly){
          debugText.text = "both hands grabbing firefly";
          NextFirefly(rightHandFireflyGrabber.firefly);
        }
      }
      else{
        if(rightHandFireflyGrabber.grabbingFirefly){
          debugText.text = "one hand grabbing firefly";
          NextFirefly(rightHandFireflyGrabber.firefly);
        }
        if(leftHandFireflyGrabber.grabbingFirefly){
          debugText.text = "one hand grabbing firefly";
          NextFirefly(leftHandFireflyGrabber.firefly);
        }
      }
    }

    //firefly grabber calls this.
    public void NextFirefly(GameObject currentFirefly){
      rightHandFireflyGrabber.touchingFirefly = false;
      leftHandFireflyGrabber.touchingFirefly = false;
      rightHandFireflyGrabber.grabbingFirefly = false;
      leftHandFireflyGrabber.grabbingFirefly = false;

      goodSound.Play();
      Destroy(currentFirefly);
      if(fireflyCounter < 36){//0 indexed, 0-9
        //TODO: log
        //figure out which button is next
        if(quadrantOrder[fireflyCounter] == 1){
          currentFireflyLocation = fireflyLocation1;
        }
        if(quadrantOrder[fireflyCounter] == 2){
          currentFireflyLocation = fireflyLocation2;
        }
        if(quadrantOrder[fireflyCounter] == 3){
          currentFireflyLocation = fireflyLocation3;
        }
        if(quadrantOrder[fireflyCounter] == 4){
          currentFireflyLocation = fireflyLocation4;
        }
        //generate the next firefly
        currentFirefly = Instantiate(fireflyPrefab, currentFireflyLocation.position, Quaternion.identity);
        fireflyCounter++;
        debugText.text = "firefly counter: " + fireflyCounter + " rounds counter: " + roundsCounter;
      }
      else{
        NextRound();
      }
    }

    public void SetBimanual(){
      isBimanual = true;
      rightHand.trueMirror = true;
      leftHand.trueMirror = true;
    }

    public void SetUnimanual(){
      isBimanual = false;
      rightHand.trueMirror = false;
      leftHand.trueMirror = false;
    }

    public void TurnOffTheLights(){
      debugText.text = "firefly turn off the lights called";
      fireflyCounter = 0;
      roundsCounter = 0;
      if(currentFirefly!=null){
        Destroy(currentFirefly);
      }
      RemoveStrayFireflies();
      debugText.text = "firefly lights turned off";
    }

    public void Restart(){
      fireflyCounter = 0;
      roundsCounter = 0;
      if(currentFirefly!=null){
        Destroy(currentFirefly);
      }
      RemoveStrayFireflies();
      NextRound();
    }

    //I don't know where this bug is coming from and at this point I am debugging with a hammer <3
    public void RemoveStrayFireflies(){
      GameObject[] fireflies;
      fireflies = GameObject.FindGameObjectsWithTag("firefly");
      foreach(GameObject firefly in fireflies){
        debugText.text = "stray firefly found and destroyed";
        Destroy(firefly);
      }
    }//end remove stray fireflies
}
