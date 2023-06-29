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

    //hazard stuff
    public bool avoidanceTask;
    public List<HazardHandler> hazardHandlers;
    public List<int> hazardOrder;
    public List<int> hazardLocations;//you still need to figure these out


    void Start(){
      //1 is no hazard
      //2 is hazards[0]
      //3 is hazards[1]
      hazardOrder = new List <int> {3, 1, 3, 2, 3, 3, 1, 2, 1, 2, 3, 1, 1, 3, 1, 3, 2, 3, 2, 2, 1, 1, 2, 2, 1, 1, 3, 1, 3, 2, 2, 2, 3, 1, 2, 3};
      hazardLocations = new List<int>{0, 3, 4, 1, 2, 2, 5, 3, 5, 4, 5, 1, 5, 5, 4, 5, 2, 3, 4, 5, 2, 1, 1, 5, 3, 2, 5, 5, 2, 5, 1, 1, 4, 3, 3, 4, 5};
    }

    //start the next round of 10 button presses
    public void NextRound(){
      debugText.text = "next round called";
      if(roundsCounter < 3 && !avoidanceTask){//0 indexed, 0-9-- we only do avoidanceTask once
        buttonsCounter = 0;
        roundsCounter++;
        quadrantOrder = experimentManager.GetNextOrder();
        NextButton();
      }
      else{
        experimentManager.FinishTask();
      }
    }

    public void StartHazardsTask(){
      debugText.text = "Start hazards task called";
      avoidanceTask = true;
      quadrantOrder = experimentManager.GetNextOrder();//throw out one
      quadrantOrder = experimentManager.GetNextOrder();//throw out two
      quadrantOrder = experimentManager.GetNextOrder();//throw out three
      quadrantOrder = experimentManager.GetNextOrder();//use the fourth
      NextButton();
    }

    //pressable buttons call this.
    public void NextButton(){
      debugText.text = "Next button called";
      //make sure all hazards are off
      foreach(HazardHandler handler in hazardHandlers){
        debugText.text = "We are in that foreach loop";
        handler.DeactivateAllHazards();
      }

      debugText.text = "buttonsCounter = " + buttonsCounter;

      if(buttonsCounter < 36){//0 indexed, 0-35
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
        if(avoidanceTask && hazardLocations[buttonsCounter]!=0){
          int location = hazardLocations[buttonsCounter];
          hazardHandlers[location-1].ActivateHazard(hazardOrder[buttonsCounter]);
        }
        buttonsCounter++;
        debugText.text = "buttons counter: " + buttonsCounter + " rounds counter: " + roundsCounter;
      }
      else{
        NextRound();
      }
    }
}
