using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is part of MirrorTherapy
/// This script does higher level experiment management.
/// It switches from experimental task to experimental task,
/// and knows about all of our quadrant lists.
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated February 2023
/// </summary>

public class ExperimentManager : MonoBehaviour
{
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

    public ButtonsManager buttonsManager;

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

      //Debug.Log("all quadrant orders: " + quadrantOrders);
      //Debug.Log("quadrant order # 7:" +quadrantOrders[7]);

      buttonsManager.NextRound();//we call this from here in order to ensure that all of the quadrant orders have been loaded before we start asking for them
    }

    public List<int> GetNextOrder(){
      quadrantCounter++;
      return quadrantOrders[quadrantCounter];
    }
}
