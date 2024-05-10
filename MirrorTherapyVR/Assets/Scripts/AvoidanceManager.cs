using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is part of MirrorTherapy
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated May 2023
/// </summary>
public class AvoidanceManager : MonoBehaviour
{
    public List<HazardHandler> hazardHandlers;
    public List<int> hazardOrder;//which hazard to display
    public List<int> hazardHandlerOrder;//where to display it
    //ref to logger once it exists
    public ExperimentManager experimentManager;
    public PressableButton button1, button2, button3, button4;
    private PressableButton currentButton;
    public List<int> quadrantOrder;

    private int buttonsCounter = 0;//how many buttons have we pressed in this round?
    private int roundsCounter = 0;//how many rounds have there been?

    public Text debugText;

    void Start(){
      //1 = no hazard
      //2 = first hazard
      //3 = second hazard
      hazardOrder = new List <int> {3, 1, 3, 2, 3, 3, 1, 2, 1, 2, 3, 1, 1, 3, 1, 3, 2, 3, 2, 2, 1, 1, 2, 2, 1, 1, 3, 1, 3, 2, 2, 2, 3, 1, 2, 3};
      hazardHandlerOrder = new List<int> {3, 4, 1, 2, 2, 5, 3, 5, 4, 5, 1, 5, 5, 4, 5, 2, 3, 4, 5, 2, 1, 1, 5, 3, 2, 5, 5, 2, 5, 1, 1, 4, 3, 3, 4, 5};
    }

    //pressable buttons call this.
    public void NextButton(){
      foreach(HazardHandler hazardHandler in hazardHandlers){
        hazardHandler.DeactivateAllHazards();
      }
      hazardHandlers[buttonsCounter].ActivateHazardAfterDelay(hazardOrder[buttonsCounter]);
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
        experimentManager.FinishTask();
      }
    }
}
