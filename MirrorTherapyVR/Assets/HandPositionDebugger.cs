using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HandPositionDebugger : MonoBehaviour
{
    public GameObject realHand;//should be a gameobject with OVRSkeleton on it
    public GameObject fakeHand;//should not have OVRSkeleton
    public List<Transform> bonesToSkip;//there are more fake bones than real bones
    OVRSkeleton skeleton;

    //every transform in the fake hand
    public List<Transform> fakeBones;

    //this doesn't show up in the inspector?
    List<OVRBone> realBones;

    public GameObject fakeHandPointPrefab;//the spheres that show us where the bones are.
    public List<GameObject> fakeHandPoints;//points that match the position of all of the bones on the real hand
    public List<GameObject> fakeHandPointsMirrored;//a mirror of the fakeHandPoints


    //fake bones, real bones
    public Text debugText, debugText2, debugText3;

    // Start is called before the first frame update
    void Start()
    {
      skeleton = realHand.GetComponent<OVRSkeleton>();

      debugText.text = "fake bones: ";

      //make the fakeBones skeleton
      fakeBones = new List<Transform>();
      foreach(Transform bone in fakeHand.transform){
        GetMoreBones(bone);
      }

      debugText2.text += "real bones: ";

      //make real Bones skeleton
      realBones = new List<OVRBone>();
      SortBones();
    }

    // Update is called once per frame
    void Update()
    {
        //position the fake bones relative to the real bones
        int bonesCounter = 0;
        foreach(OVRBone bone in realBones){

          //put a hand point at each bone position
          fakeHandPoints[bonesCounter].transform.position = bone.Transform.position;

          //mirror the positon of the fake hand point
          Vector3 mirroredPosition = new Vector3((fakeHandPoints[bonesCounter].transform.position.x) * -1.0f, fakeHandPoints[bonesCounter].transform.position.y, fakeHandPoints[bonesCounter].transform.position.z);
          fakeHandPointsMirrored[bonesCounter].transform.position = mirroredPosition;

          //put the fake bones at the mirrored positions
          fakeBones[bonesCounter].transform.position = mirroredPosition;
          bonesCounter++;
        }
    }//end update

    //a functon which does the recursive loop of getting all fake bones
    private void GetMoreBones(Transform bone){
      if(!bonesToSkip.Contains(bone)){
        fakeBones.Add(bone);

        GameObject newFakeHandPoint = Instantiate(fakeHandPointPrefab);
        fakeHandPoints.Add(newFakeHandPoint);

        GameObject newFakeHandPointMirrored = Instantiate(fakeHandPointPrefab);
        fakeHandPointsMirrored.Add(newFakeHandPointMirrored);

        debugText.text += "fake bone: " + bone.gameObject.name + ",\n";

        if(bone.transform.childCount!=0){
          foreach(Transform smallerBone in bone){
            GetMoreBones(smallerBone);
          }
        }//end if
      }//end if
    }//end get more bones

    //looks at all of the bones in the skeleton and put just the ones in the left hand
    //into realBones. The order is super important because it has to match the fake bones
    //it will be a while to get these in the right order
    //you might think: we can refactor this, and make it much more elegant-- you would be wrong.
    //OVR is sort of weird and fussy about bones?
    private void SortBones(){
      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_WristRoot)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the wrist!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_ForearmStub)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the forearm!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Index1)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the index1!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Index2)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the index2!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Index3)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the index3!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Middle1)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the middle 1!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Middle2)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the Middle2!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Middle3)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the middle3!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Pinky0)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the pinky0!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Pinky1)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the pinky1!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Pinky2)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the pinky2!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Pinky3)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the pinky3!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Ring1)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the Ring1!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Ring2)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the ring2!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Ring3)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the ring3!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Thumb0)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the thumb0!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Thumb1)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the thumb1!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Thumb2)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the thumb2!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Thumb3)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the thumb3!";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_ThumbTip)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the ThumbTip!";
        }//end if
      }//end foreach


      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the IndexTip! \n";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_MiddleTip)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the MiddleTip! \n";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_RingTip)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the RingTip! \n";
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_PinkyTip)
        {
          realBones.Add(bone);
          debugText2.text += "\n found the PinkyTip! \n";
        }//end if
      }//end foreach

      // AddBone(OVRSkeleton.BoneId.Hand_End);
    }

    //we pull this out as a helper function for sort bones to make it less... long--
    //it does not work, and is currently not in use, but maybe someday
    private void AddBone(OVRSkeleton.BoneId boneId){
      debugText2.text += "boneId: " + boneId + ",\n";
      foreach(OVRBone bone in skeleton.Bones){
        if(bone.Id == boneId){
          realBones.Add(bone);
          debugText2.text += "real bone: " + boneId + ",\n";
        }
      }
    }
}
