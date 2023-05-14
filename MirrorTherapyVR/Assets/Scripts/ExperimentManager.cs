using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This script is part of MirrorTherapy
/// This script does higher level experiment management.
/// It switches from experimental task to experimental task,
/// and knows about all of our quadrant lists.
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated May 2023
/// </summary>



public class ExperimentManager : MonoBehaviour
{
    public enum Task {Sync, VTS, Buttons, UnimanualFireflies, BimanualFireflies, Drumming, Hazard};
    public List<Task> taskOrder = new List<Task> {Task.Sync, Task.VTS, Task.Buttons, Task.UnimanualFireflies, Task.BimanualFireflies, Task.Drumming, Task.Hazard};
    private int taskCounter = 0;
    public Task currentTask;//making it public lets us start somewhere else
    public VTS vts;
    public GameObject vtsGo;
    public ButtonsManager buttonsManager;
    public GameObject buttonsManagerGo;
    //you should probablt merge the unimanual and bimanual firefly tasks, and make it just a mode toggle at some point
    public FireflyManager fireflyManager;
    public GameObject fireflyGo;
    public Logger logger;

    public Text instructionsText;
    public GameObject nextTaskButton, allTasksGo;

    public bool useVTS;
    public bool mirrorHands;

    public Renderer rightHandReal, leftHandReal, rightHandMirrored, leftHandMirrored;

    //these are the orders of the quadrants for each of the 10 trials in each of the tasks
    //9 are random, the 10th is staged to get us all possible movements between quadrants
    private List<int> quadrantOrder1;
    private List<int> quadrantOrder2;
    private List<int> quadrantOrder3;
    private List<int> quadrantOrder4;
    private List<int> quadrantOrder5;
    private List<int> quadrantOrder6;
    private List<int> quadrantOrder7;
    private List<int> quadrantOrder8;
    private List<int> quadrantOrder9;
    private List<int> quadrantOrder10;

    public List<List<int>> quadrantOrders = new List<List<int>>();

    private int quadrantCounter;

    // Start is called before the first frame update
    void Start()
    {
      //TODO: replace these with real random numbers
      quadrantOrder1 =  new List<int>(){1, 2, 3, 4, 1, 2, 3, 4, 1, 2};
      quadrantOrder2 =  new List<int>(){1, 2, 3, 4, 1, 2, 3, 4, 1, 2};
      quadrantOrder3 =  new List<int>(){1, 2, 3, 4, 1, 2, 3, 4, 1, 2};
      quadrantOrder4 =  new List<int>(){1, 2, 3, 4, 1, 2, 3, 4, 1, 2};
      quadrantOrder5 =  new List<int>(){1, 2, 3, 4, 1, 2, 3, 4, 1, 2};
      quadrantOrder6 =  new List<int>(){1, 2, 3, 4, 1, 2, 3, 4, 1, 2};
      quadrantOrder7 =  new List<int>(){1, 2, 3, 4, 1, 2, 3, 4, 1, 2};
      quadrantOrder8 =  new List<int>(){1, 2, 3, 4, 1, 2, 3, 4, 1, 2};
      quadrantOrder9 =  new List<int>(){1, 2, 3, 4, 1, 2, 3, 4, 1, 2};
      quadrantOrder10 =  new List<int>(){1, 2, 3, 4, 1, 2, 3, 4, 1, 2};

      logger.Log("Experiment Start at " + Time.time);
      logger.Log("Use VTS: " + useVTS.ToString());
      logger.Log("Mirror Hands: " + mirrorHands.ToString());

      LoadQuadrantOrders();
      NextTask(Task.Sync);
    }//end start

    //called by other scripts, to let experiment manager know to advance to the next task
    public void FinishTask(){
      allTasksGo.SetActive(false);
      nextTaskButton.SetActive(true);
      instructionsText.text = "You've finished the task! Please remove the headset to answer some questions about your experience.";
    }

    //pressing the "next" button calls this
    public void GoToNextTask(){
      allTasksGo.SetActive(true);
      nextTaskButton.SetActive(false);
      taskCounter++;
      NextTask(taskOrder[taskCounter]);
    }

    private void NextTask(Task nextTask){
      switch (nextTask)
      {
      case Task.Sync:
          LoadStartInstructions();
          break;
      case Task.VTS:
          LoadVTS();
          break;
      case Task.Buttons:
          LoadButtonsTask();
          break;
      case Task.UnimanualFireflies:
          LoadUnimanualFirefliesTask();
          break;
      case Task.BimanualFireflies:
          LoadBimanualFirefliesTask();
          break;
      case Task.Drumming:
          print ("We haven't made this yet");
          break;
      case Task.Hazard:
          print ("We haven't made this yet");
          break;
      default:
          print ("This shouldn't happen");
          break;
      }
    }//end next task

    public void LoadStartInstructions(){
      currentTask = Task.Sync;
      if(mirrorHands){
        rightHandMirrored.enabled = true;
        rightHandReal.enabled = false;
      }
      else{
        rightHandMirrored.enabled = false;
        rightHandReal.enabled = true;
      }

      //we start with only the right hand, so we turn the left hand off
      leftHandMirrored.enabled = false;
      leftHandReal.enabled = false;

      instructionsText.text = "Hi, and welcome to our user study! ";
      instructionsText.text+= "This is an excellent spot for any additional instructions we might need to include at the start. ";
      instructionsText.text += "Take a look at your hand. And try to touch your thumb to each finger.";

      //make sure everything that should be turned off is turned off
      vtsGo.SetActive(false);
      buttonsManagerGo.SetActive(false);
      fireflyGo.SetActive(false);
      fireflyManager.enabled = false;
    }

    public void LoadVTS(){
      currentTask = Task.VTS;
      if(!useVTS){//if we're not doing synchronous visuotactile stimulation, skip straight to buttons task
        LoadButtonsTask();
      }
      instructionsText.text = "Some kind of explanation of how to do the VTS situation, which you plan on writing later";
      vtsGo.SetActive(true);

      //make sure everything that should be turned off is turned off
      buttonsManagerGo.SetActive(false);
      fireflyGo.SetActive(false);
    }

    public void LoadButtonsTask(){
      currentTask = Task.Buttons;
      instructionsText.text = "Some kind of explanation of how to do the buttons task";
      vtsGo.SetActive(false);//this doesn't run
      buttonsManagerGo.SetActive(true);
      buttonsManager.NextRound();

      //make sure everything that should be turned off is turned off
      fireflyGo.SetActive(false);
    }

    public void LoadUnimanualFirefliesTask(){
      currentTask = Task.UnimanualFireflies;
      fireflyGo.SetActive(true);
      fireflyManager.enabled = true;
      fireflyManager.isBimanual = false;
      fireflyManager.Restart();

      instructionsText.text = "Some kind of explanation of how to do the unimanual fireflies task";

      //make sure everything that should be turned off is turned off
      vtsGo.SetActive(false);
      buttonsManagerGo.SetActive(false);

    }

    public void LoadBimanualFirefliesTask(){
      currentTask = Task.BimanualFireflies;
      fireflyGo.SetActive(true);
      fireflyManager.enabled = true;
      fireflyManager.isBimanual = true;
      fireflyManager.Restart();

      instructionsText.text = "Some kind of explanation of how to do the bimanual fireflies task";

      //make sure everything that should be turned off is turned off
      vtsGo.SetActive(false);
      buttonsManagerGo.SetActive(false);
    }

    private void LoadQuadrantOrders(){
      quadrantOrders.Add(quadrantOrder1);
      quadrantOrders.Add(quadrantOrder2);
      quadrantOrders.Add(quadrantOrder3);
      quadrantOrders.Add(quadrantOrder4);
      quadrantOrders.Add(quadrantOrder5);
      quadrantOrders.Add(quadrantOrder6);
      quadrantOrders.Add(quadrantOrder7);
      quadrantOrders.Add(quadrantOrder8);
      quadrantOrders.Add(quadrantOrder9);
      quadrantOrders.Add(quadrantOrder10);
      quadrantCounter = -1;//we start at negative 1 so that we can increment, then return in GetNextOrder
    }


    public List<int> GetNextOrder(){
      quadrantCounter++;
      return quadrantOrders[quadrantCounter];
    }
}
