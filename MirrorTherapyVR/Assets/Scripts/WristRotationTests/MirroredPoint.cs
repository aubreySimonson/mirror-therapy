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

    public OVRBone realBone;//these aren't exposed in the inspector, which will make this super annoying to assign
    public string boneName;//this is for letting the points manager map the OVR skeleton bones. It isn't the most elegant solution

    public MirroredPointsManager pointsManager;

    public UnderlyingHandedness underlyingHandedness;//match the handedness of the real hand, not the fake hand-- god this is all confusing

    public GameObject fakeHandPoint;//do geometric stuff on this instead of the actual fake bone, assign fake bone to this position after.

    public Text debugText;//for debugging
    public Material finalizedMat, unfinalizedMat;//also for debugging
    public bool isFinalized;

    void Start(){
        if(fakeHandBone == null){
            fakeHandBone = gameObject;//assume that we put the script on the fake hand bone, and were just lazy
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

    public void matchFakeBonesToRealBones(){
        //put them both in the same place
        fakeHandBone.transform.position = realBone.Transform.position;
        fakeHandBone.transform.rotation = realBone.Transform.rotation;//this is quaternions

        if(debugText!=null){
            debugText.text = "matched fake bones to real bones";
        }
    }

    public void PutFakeHandPointsAtRealBones(){
        fakeHandPoint.transform.position = realBone.Transform.position;
        fakeHandPoint.transform.rotation = realBone.Transform.rotation;
        if(debugText!=null){
            debugText.text = "put fake hand points at real bones";
        }
    }

    //mirrors this point across the world origin for... one of the axis. The right one.
    //first of the two matrix transformations we do every frame...
    public void Reflect(){

        //flip it across the axis
        fakeHandPoint.transform.rotation = ReflectRotation(fakeHandPoint.transform.rotation, Vector3.right);
        fakeHandPoint.transform.position = Vector3.Reflect(fakeHandPoint.transform.position, Vector3.right);
        if(debugText!=null){
            debugText.text = "reflected bones";
        }
        isFinalized = false;
        //translate
        // if(!trueMirror){
        //     Vector3 adjustedPosition = new Vector3(fakeHandBone.transform.position.x+(pointsManager.adjust*2.00f), fakeHandBone.transform.position.y, fakeHandBone.transform.position.z);
        //     fakeHandBone.transform.position = adjustedPosition;
        // }

    }

    public void FinalizePosition(){
        if(debugText!=null){
            debugText.text = "finalize called";
        }
        fakeHandBone.transform.position = fakeHandPoint.transform.position;
        fakeHandBone.transform.rotation = fakeHandPoint.transform.rotation;
        if(debugText!=null){
            debugText.text = "fake hand point position is " + fakeHandPoint.transform.position + " and fake hand bone position is " + fakeHandBone.transform.position;
        }
        isFinalized = true;
    }

    //figures out the distance between the fake wrist and this point
    //called *after* reflect
    public void RecalculateOffset(){
        positionOffset = GetPositionOffset();
		rotationOffset = fakeWrist.localEulerAngles + fakeHandBone.transform.localEulerAngles;
    }

    //moves this point based on however we rotated the fake hand wrist this frame
    //call *after* points have been reflected, offset has been recalculated, and fake wrist has been rotated
    //second of two matrix transformaitons we do every frame...
    //this function currently isn't being used, and it might be better to delete it...
    public void RotateFromWrist(){
        if(boneName!="wrist"){
            // Calculate the combined transformation matrix
            Matrix4x4 transformationMatrix = Matrix4x4.TRS(
                fakeWrist.position + fakeWrist.TransformVector(positionOffset),
                fakeWrist.rotation * Quaternion.Euler(rotationOffset),
                Vector3.one
            );
            // Apply the transformation to the finger
            fakeHandBone.transform.position = transformationMatrix.GetColumn(3);
            fakeHandBone.transform.rotation = transformationMatrix.rotation;
        }
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
