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
    public enum Task {Sync, VTS, Buttons, UnimanualFireflies, BimanualFireflies, Hazard, Drumming};//no matter what we do with taskOrder, it always uses this list<--that's because you set it in the inspector, idiot    //shorter list for debugging
    public List<Task> taskOrder = new List<Task> {Task.Sync, Task.VTS, Task.Buttons, Task.VTS, Task.UnimanualFireflies, Task.VTS, Task.BimanualFireflies, Task.VTS, Task.Hazard, Task.VTS, Task.Drumming};
    private int taskCounter = 0;
    public Task currentTask;//making it public lets us start somewhere else
    public float timeOut = 1200.0f;//number of seconds a participant can spend on a task before we make them do the next task
    private float taskStartTime;//when the current task was started
    private bool betweenTasks = false;
    public VTS vts;
    public GameObject vtsGo;
    public ButtonsManager buttonsManager;
    public GameObject buttonsManagerGo;
    public FireflyManager fireflyManager;
    public GameObject fireflyGo;
    public GameObject hazardsGo;
    public Logger logger;


    public Text instructionsText, debugText;
    public GameObject nextTaskButton, allTasksGo;

    public bool useVTS;
    public bool mirrorHands;

    public Renderer rightHandReal, leftHandReal, rightHandMirrored, leftHandMirrored;

    private List<int> quadrantOrder1;
    private List<int> quadrantOrder2;
    private List<int> quadrantOrder3;
    private List<int> quadrantOrder4;
    private List<int> quadrantOrder5;//you'll need a 5th one for the drumming--do we?


    public List<List<int>> quadrantOrders = new List<List<int>>();

    private int quadrantCounter;

    // Start is called before the first frame update
    void Start()
    {
      taskStartTime = Time.time;
      //these are our random numbers-- they're calculated by hand, and deserve further scrutiny
      quadrantOrder1 =  new List<int>(){2, 4, 1, 2, 1, 3, 4, 2, 3, 1, 4, 3, 2, 1, 3, 2, 4, 1, 2, 3, 4, 3, 1, 4, 2,  3, 1, 3,  2, 4,  1, 2, 1, 4, 3, 4, 2};
      quadrantOrder2 =  new List<int>(){2, 1, 3, 4, 3, 1, 2, 4, 1, 4, 2, 3, 2, 1, 2, 3, 1, 4, 3, 4,  1, 3,  2, 4, 2,  3, 4, 2, 1, 3,  2, 4,  1, 4, 3, 1, 2};
      quadrantOrder3 =  new List<int>(){3, 4, 2, 1, 3, 1, 2, 4, 1, 4, 3, 2, 3, 2, 3, 4, 1, 2, 1, 3, 1, 4,  2, 4, 3, 1, 2, 4, 2, 1, 4, 3,  2,  3, 4,  1,3};
      quadrantOrder4 =  new List<int>(){4, 3, 1, 2, 4, 2, 3, 4, 1, 3, 2, 1, 4, 1, 3, 2, 4, 3, 1, 4, 2, 1, 2,  3, 4, 2,  3,  2, 4,  1, 2, 1, 3, 4, 3, 1, 4};

      logger.Log("Experiment Start at " + Time.time);
      logger.Log("Use VTS: " + useVTS.ToString());
      logger.Log("Mirror Hands: " + mirrorHands.ToString());

      LoadQuadrantOrders();
      SetHandsToNormal();
      //debugText.text = "first item in task order: " + taskOrder[0].ToString();
      debugText.text = "time out time is " + timeOut.ToString() + ". We this task should time out at " + (timeOut+taskStartTime).ToString();
      NextTask(Task.Sync);
    }//end start

    void Update(){
      if(!betweenTasks && Time.time>timeOut+taskStartTime){
        fireflyManager.TurnOffTheLights();
        buttonsManager.TurnOffTheLights();
        FinishTask();
      }
    }


    //called by other scripts, to let experiment manager know to advance to the next task
    public void FinishTask(){
      betweenTasks=true;
      fireflyGo.SetActive(false);
      buttonsManagerGo.SetActive(false);
      allTasksGo.SetActive(false);
      hazardsGo.SetActive(false);
      nextTaskButton.SetActive(true);
      instructionsText.text = "You've finished the task! Please remove the headset to answer some questions about your experience.";
    }

    //pressing the "next" button calls this
    public void GoToNextTask(){
      betweenTasks = false;
      taskStartTime = Time.time;
      allTasksGo.SetActive(true);
      nextTaskButton.SetActive(false);
      SetHandsToNormal();
      taskCounter++;
      quadrantCounter = -1;//we start at negative 1 so that we can increment, then return in GetNextOrder
      debugText.text = "now going to task: " + taskOrder[taskCounter].ToString();
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
          LoadHazardsTask();
          break;
      default:
          print ("This shouldn't happen");
          break;
      }
    }//end next task

    public void LoadStartInstructions(){
      currentTask = Task.Sync;

      instructionsText.text = "Hi, and welcome to our user study! ";
      instructionsText.text+= "This is an excellent spot for any additional instructions we might need to include at the start. ";
      instructionsText.text += "Take a look at your hand. And try to touch your thumb to each finger.";

      //make sure everything that should be turned off is turned off
      vtsGo.SetActive(false);
      buttonsManagerGo.SetActive(false);
      fireflyGo.SetActive(false);
      fireflyManager.enabled = false;
    }

    //make the hands behave in the way they do for most tasks
    public void SetHandsToNormal(){
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
    }

    public void LoadVTS(){
      currentTask = Task.VTS;
      if(!useVTS){//if we're not doing synchronous visuotactile stimulation, skip straight to buttons task
        taskCounter++;//skip VTS in the list
        //LoadButtonsTask();
        GoToNextTask();//trying this instead of the above to see if that lets us do VTS after every step
      }
      else{
        instructionsText.text = "Some kind of explanation of how to do the VTS situation, which you plan on writing later";
        vtsGo.SetActive(true);

        //make sure everything that should be turned off is turned off
        buttonsManagerGo.SetActive(false);
        fireflyGo.SetActive(false);
      }
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
      fireflyManager.SetUnimanual();
      fireflyManager.Restart();
  
      instructionsText.text = "Some kind of explanation of how to do the unimanual fireflies task";

      //make sure everything that should be turned off is turned off
      vtsGo.SetActive(false);
      buttonsManagerGo.SetActive(false);

    }

    public void LoadBimanualFirefliesTask(){
      //in the mirrored condition, both fake hands should be on
      //int he real condition, both real hands should be on
      if(mirrorHands){
        rightHandMirrored.enabled = true;
        rightHandReal.enabled = false;
        leftHandMirrored.enabled = true;
        leftHandReal.enabled = false;
      }
      else{
        rightHandMirrored.enabled = false;
        rightHandReal.enabled = true;
        leftHandMirrored.enabled = false;
        leftHandReal.enabled = true;
      }
      currentTask = Task.BimanualFireflies;
      fireflyGo.SetActive(true);
      fireflyManager.enabled = true;
      fireflyManager.SetBimanual();
      fireflyManager.Restart();

      instructionsText.text = "Some kind of explanation of how to do the bimanual fireflies task";

      //make sure everything that should be turned off is turned off
      vtsGo.SetActive(false);
      buttonsManagerGo.SetActive(false);
    }

    public void LoadHazardsTask(){
      currentTask = Task.Hazard;
      instructionsText.text = "Some kind of explanation of how to do the hazard task";
      hazardsGo.SetActive(true);
      buttonsManagerGo.SetActive(true);
      buttonsManager.StartHazardsTask();

      //make sure everything that should be turned off is turned off
      fireflyGo.SetActive(false);
      vtsGo.SetActive(false);
    }

    private void LoadQuadrantOrders(){
      quadrantOrders.Add(quadrantOrder1);
      quadrantOrders.Add(quadrantOrder2);
      quadrantOrders.Add(quadrantOrder3);
      quadrantOrders.Add(quadrantOrder4);
      quadrantCounter = -1;//we start at negative 1 so that we can increment, then return in GetNextOrder
    }


    public List<int> GetNextOrder(){
      quadrantCounter++;
      return quadrantOrders[quadrantCounter];
    }
}
