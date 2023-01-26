using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is part of MirroTherapy.cs
/// This script keeps track of buttons, and logs information about when they're pressed
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated January 2023
/// </summary>

public class ButtonsManager : MonoBehaviour
{
    //ref to logger once it exists
    //ref to Experiement Manager once it exists
    public List<PressableButton> q1Buttons;//pressable buttons in quadrant 1
    public List<PressableButton> q2Buttons;//in quadrant 2
    public List<PressableButton> q3Buttons;//you get the idea
    public List<PressableButton> q4Buttons;

    private PressableButton currentButton;

    private List<int> tempQuadrantOrder;
    private int counter = 0;


    // Start is called before the first frame update
    void Start()
    {
      tempQuadrantOrder =  new List<int>(){1, 2, 3, 4, 1, 2, 3, 4, 1, 1, 1, 1};
      Object [] allButtons = FindObjectsOfType<PressableButton>();
      foreach(Object buttonObject in allButtons){

        PressableButton thisButton = (PressableButton) buttonObject;

        if(thisButton.quadrant == 1){
          q1Buttons.Add(thisButton);
        }
        else if(thisButton.quadrant == 2){
          q2Buttons.Add(thisButton);
        }
        else if(thisButton.quadrant == 3){
          q3Buttons.Add(thisButton);
        }
        else if(thisButton.quadrant == 4){
          q4Buttons.Add(thisButton);
        }
        else{
          Debug.Log("a button has an invalid quadrant!");
        }
      }
    }

    //pressable buttons call this.
    //the name is a bit weird because the oculus integration has terrible namespace management
    //and you end up breaking some part of it any time anything related to buttons has a vaguely
    //predictable name
    public void NextButton(){
      //log the button press
      //get the next quadrant from the experiment manager.
      //highlight the next button
      if(tempQuadrantOrder[counter] == 1){
        currentButton = q1Buttons[Random.Range(0, q1Buttons.Count)];
      }
      if(tempQuadrantOrder[counter] == 2){
        currentButton = q2Buttons[Random.Range(0, q2Buttons.Count)];
      }
      if(tempQuadrantOrder[counter] == 3){
        currentButton = q3Buttons[Random.Range(0, q3Buttons.Count)];
      }
      if(tempQuadrantOrder[counter] == 4){
        currentButton = q4Buttons[Random.Range(0, q4Buttons.Count)];
      }
      currentButton.Highlight();
      counter ++;
    }
}
