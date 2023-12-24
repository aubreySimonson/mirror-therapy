using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One for each point to be mirrored. 
/// Doesn't have to go on the bit of the fake hand it controls, 
/// but that's nice for organizational reasons.
/// Managed by Mirrored Points Manager.
/// 
/// Last edited November 2023
/// ???-->simonson.au@northeastern.edu
/// </summary>
public class MirroredPoint : MonoBehaviour
{
    private Transform realWrist;
    private Transform fakeWrist;
    private Vector3 positionOffset, rotationOffset;
    public GameObject fakeHandBone;//the part of the fake hand model controlled by this script.

    public bool trueMirror;//if true, moving the real hand right moves the fake hand left, etc-- turn on for drumming

    public OVRBone realBone;//these aren't exposed in the inspector, so this is assigned by a sort of messy process
    public string boneName;//this is for letting the points manager map the OVR skeleton bones. It isn't the most elegant solution

    public MirroredPointsManager pointsManager;

    public UnderlyingHandedness underlyingHandedness;//match the handedness of the real hand, not the fake hand

    public GameObject fakeHandPoint;//do transformations to this instead of the actual fake bone, assign fake bone to this position after.

    void Start(){
        if(fakeHandBone == null){
            fakeHandBone = gameObject;//if there's no bone connected, assume we at least put this script on the fakeHandBone gameobject
        }
    }

    //setters
    public void SetRealWrist(Transform rW){
        realWrist = rW;
    }

    public void SetFakeWrist(Transform fW){
        fakeWrist = fW;
    } 

    public void SetRealBone(OVRBone rB){
        realBone = rB;
    }

    public void PutFakeHandPointsAtRealBones(){
        fakeHandPoint.transform.position = realBone.Transform.position;
        fakeHandPoint.transform.rotation = realBone.Transform.rotation;
    }

    //mirrors this point across the world origin for... one of the axis. The right one.
    public void Reflect(){

        //flip it across the axis
        fakeHandPoint.transform.rotation = ReflectRotation(fakeHandPoint.transform.rotation, Vector3.right);
        fakeHandPoint.transform.position = Vector3.Reflect(fakeHandPoint.transform.position, Vector3.right);

        //translate
        // if(!trueMirror){
        //     Vector3 adjustedPosition = new Vector3(fakeHandBone.transform.position.x+(pointsManager.adjust*2.00f), fakeHandBone.transform.position.y, fakeHandBone.transform.position.z);
        //     fakeHandBone.transform.position = adjustedPosition;
        // }

    }

    public void FinalizePosition(){
        fakeHandBone.transform.position = fakeHandPoint.transform.position;
        fakeHandBone.transform.rotation = fakeHandPoint.transform.rotation;
    }

    //figures out the distance between the fake wrist and this point
    //called *after* reflect
    public void RecalculateOffset(){
        positionOffset = GetPositionOffset();
		rotationOffset = fakeWrist.localEulerAngles + fakeHandBone.transform.localEulerAngles;
    }

    private Vector3 GetPositionOffset(){
        //There's no reasonable explanation for the following mess of code.
        //You basically tried things until it worked. 
        Vector3 tempPositionOffset = fakeWrist.position - fakeHandBone.transform.position;
        float distanceInX;
        if(fakeHandBone.transform.position.x > fakeHandBone.transform.position.x){
            distanceInX = Mathf.Abs(tempPositionOffset.x);
        }
        else{
            distanceInX = distanceInX = Mathf.Abs(tempPositionOffset.x) * -1.0f;
        }
        float distanceInY;
        if(fakeHandBone.transform.position.y > fakeWrist.position.y){
            distanceInY = Mathf.Abs(tempPositionOffset.y);
        }
        else{
            distanceInY = distanceInY = Mathf.Abs(tempPositionOffset.y) * -1.0f;
        }
        float distanceInZ;
        if(fakeHandBone.transform.position.z > fakeWrist.position.z){
            distanceInZ = Mathf.Abs(tempPositionOffset.z);
        }
        else{
            distanceInZ = distanceInZ = Mathf.Abs(tempPositionOffset.z) * -1.0f;
        }
        return new Vector3(distanceInX, distanceInY, distanceInZ);
    }

    //all quaternion math stays in here-- don't touch it.
    private Quaternion ReflectRotation(Quaternion source, Vector3 normal)
    {
        Quaternion reflected = Quaternion.LookRotation(Vector3.Reflect(source * Vector3.forward, normal), Vector3.Reflect(source * Vector3.up, normal));
        Quaternion rotateBy = new Quaternion(1,0,0,0);//x, y, z, w
        return reflected * rotateBy;
    }
}
