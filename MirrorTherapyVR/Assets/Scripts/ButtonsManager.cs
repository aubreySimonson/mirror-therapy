using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is part of MirrorTherapy
/// This script keeps track of buttons, and logs information about when they're pressed
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated January 2023
/// </summary>

public class ButtonsManager : MonoBehaviour
{
    //ref to logger once it exists
    public ExperimentManager experimentManager;
    public PressableButton button1, button2, button3, button4;
    private PressableButton currentButton;
    public List<int> quadrantOrder;

    private int buttonsCounter = 0;//how many buttons have we pressed in this round?
    private int roundsCounter = 0;//how many rounds have there been?

    public Text debugText;

    //start the next round of 10 button presses
    public void NextRound(){
      if(roundsCounter < 3){//0 indexed, 0-9
        buttonsCounter = 0;
        roundsCounter++;
        quadrantOrder = experimentManager.GetNextOrder();
        NextButton();
      }
      else{
        experimentManager.FinishTask();
      }
    }

    //pressable buttons call this.
    public void NextButton(){
      if(buttonsCounter < 36){//0 indexed, 0-9
        //TODO: log the button press
        //figure out which button is next
        if(quadrantOrder[buttonsCounter] == 1){
          currentButton = button1;
        }
        if(quadrantOrder[buttonsCounter] == 2){
          currentButton = button2;
        }
        if(quadrantOrder[buttonsCounter] == 3){
          currentButton = button3;
        }
        if(quadrantOrder[buttonsCounter] == 4){
          currentButton = button4;
        }
        //highlight the next button
        currentButton.Highlight();
        buttonsCounter++;
        debugText.text = "buttons counter: " + buttonsCounter + " rounds counter: " + roundsCounter;
      }
      else{
        NextRound();
      }
    }
}
