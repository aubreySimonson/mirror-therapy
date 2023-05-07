//PART OF MIRROR THERAPY VR
//This is the main script-- it takes the bones of one hand and
//animates a model of the other from it.
//put this script on the same gameobject that the OVRHand script is on of the real hand.
//Last edited January 2023
//???-->followspotfour@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HandPositionMirror : MonoBehaviour
{


    public GameObject realHand;//should be a gameobject with OVRSkeleton on it
    public GameObject fakeHand;//should not have OVRSkeleton
    public List<Transform> bonesToSkip;//there are more fake bones than real bones
    OVRSkeleton skeleton;

    //every transform in the fake hand
    public List<Transform> fakeBones;
    List<OVRBone> realBones;

    public GameObject fakeHandPointPrefab;//the spheres that show us where the bones are.
    public List<GameObject> fakeHandPoints;//points that match the position of all of the bones on the real hand
    public List<GameObject> fakeHandPointsMirrored;//a mirror of the fakeHandPoints

    public float comfortableDistance;//distance over from the world origin that's most comfortable to keep the hand at.
    private float adjust;//distance between hand points and comfortableDistance
    private Vector3 adjustedPosition;

    public GameObject worldOrigin;

    private Logger logger;

    public Text debugText;

    // Start is called before the first frame update
    void Start()
    {
      skeleton = realHand.GetComponent<OVRSkeleton>();

      //make the fakeBones skeleton
      fakeBones = new List<Transform>();
      foreach(Transform bone in fakeHand.transform){
        GetMoreBones(bone);
      }

      //make real Bones skeleton
      realBones = new List<OVRBone>();
      SortBones();

      //turn off the points on the real hand
      foreach(GameObject point in fakeHandPoints){
        point.GetComponent<Collider>().enabled = false;
      }

      if(logger == null){
        logger = (Logger)FindObjectOfType(typeof(Logger));
      }
      logger.SetUpDataCollection();
    }

    //all quaternion math stays in here-- don't touch it.
    private Quaternion ReflectRotation(Quaternion source, Vector3 normal)
    {
        Quaternion reflected = Quaternion.LookRotation(Vector3.Reflect(source * Vector3.forward, normal), Vector3.Reflect(source * Vector3.up, normal));
        Quaternion rotateBy = new Quaternion(1,0,0,0);//x, y, z, w
        return reflected * rotateBy;
    }

    // Update is called once per frame
    void Update()
    {
        //position the fake bones relative to the real bones
        int bonesCounter = 0;

        Vector3 realWristPosition = realBones[0].Transform.position;
        adjust = realWristPosition.x - comfortableDistance;

        foreach(OVRBone bone in realBones){
          //put a hand point at each bone position
          fakeHandPoints[bonesCounter].transform.position = bone.Transform.position;
          fakeHandPoints[bonesCounter].transform.rotation = bone.Transform.rotation;//this is quaternions

          fakeHandPointsMirrored[bonesCounter].transform.rotation = ReflectRotation(fakeHandPoints[bonesCounter].transform.rotation, Vector3.right);
          fakeHandPointsMirrored[bonesCounter].transform.position = Vector3.Reflect(fakeHandPoints[bonesCounter].transform.position, Vector3.right);

          //translate
          adjustedPosition = new Vector3(fakeHandPointsMirrored[bonesCounter].transform.position.x+(adjust*2.00f), fakeHandPointsMirrored[bonesCounter].transform.position.y, fakeHandPointsMirrored[bonesCounter].transform.position.z);
          fakeHandPointsMirrored[bonesCounter].transform.position = adjustedPosition;

          fakeBones[bonesCounter].transform.position = fakeHandPointsMirrored[bonesCounter].transform.position;
          fakeBones[bonesCounter].transform.rotation = fakeHandPointsMirrored[bonesCounter].transform.rotation;
          bonesCounter++;
        }
    }//end update

    public List<OVRBone> GetRealBones(){
      return realBones;
    }

    //a functon which does the recursive loop of getting all fake bones
    private void GetMoreBones(Transform bone){
      if(!bonesToSkip.Contains(bone)){
        fakeBones.Add(bone);

        GameObject newFakeHandPoint = Instantiate(fakeHandPointPrefab);
        fakeHandPoints.Add(newFakeHandPoint);

        GameObject newFakeHandPointMirrored = Instantiate(fakeHandPointPrefab);
        fakeHandPointsMirrored.Add(newFakeHandPointMirrored);

        if(bone.transform.childCount!=0){
          foreach(Transform smallerBone in bone){
            GetMoreBones(smallerBone);
          }
        }//end if
      }//end if
    }//end get more bones

    //looks at all of the bones in the skeleton and put just the ones in the left hand
    //into realBones. The order is super important because it has to match the fake bones
    //you might think: we can refactor this, and make it much more elegant-- you would be wrong.
    //OVR is sort of weird and fussy about bones?
    private void SortBones(){
      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_WristRoot)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_ForearmStub)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Index1)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Index2)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Index3)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Middle1)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Middle2)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Middle3)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Pinky0)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Pinky1)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Pinky2)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Pinky3)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Ring1)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Ring2)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Ring3)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Thumb0)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Thumb1)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Thumb2)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_Thumb3)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach

      foreach (OVRBone bone in skeleton.Bones)
      {
        if (bone.Id == OVRSkeleton.BoneId.Hand_ThumbTip)
        {
          realBones.Add(bone);
        }//end if
      }//end foreach
    }//end sortbones

    //we pull this out as a helper function for sort bones to make it less... long--
    //it does not work, and is currently not in use, but maybe someday
    private void AddBone(OVRSkeleton.BoneId boneId){
      foreach(OVRBone bone in skeleton.Bones){
        if(bone.Id == boneId){
          realBones.Add(bone);
        }
      }
    }
}
