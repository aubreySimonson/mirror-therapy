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
    public AudioSource goodSound;

    public List<int> quadrantOrder;

    private int fireflyCounter = 0;//how many fireflies have we grabbed in this round?
    private int roundsCounter = 0;//how many rounds have there been?

    public Text debugText;

    //start the next round of 10 button presses
    public void NextRound(){
      if(roundsCounter < 10){//we increment from 0 to 1 before the first round, making this 0 indexed
        fireflyCounter = 0;
        roundsCounter++;
        quadrantOrder = experimentManager.GetNextOrder();
        Debug.Log("quadrant order at firefly counter is " + quadrantOrder[fireflyCounter]);
        NextFirefly();
      }
    }

    //firefly grabber calls this.
    public void NextFirefly(){
      goodSound.Play();
      if(fireflyCounter < 9){//0 indexed, 0-9
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
        Instantiate(fireflyPrefab, currentFireflyLocation.position, Quaternion.identity);
        fireflyCounter++;
        debugText.text = "firefly counter: " + fireflyCounter + " rounds counter: " + roundsCounter;
      }
      else{
        NextRound();
      }
    }
}
