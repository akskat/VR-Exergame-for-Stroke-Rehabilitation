/*
  AvatarVRFollower.cs
  
  Montering:
  1. Legg dette scriptet på Character-objektet (root) i scenen.
  2. I Inspector for AvatarVRFollower:
     - Drag "headPivot" til HeadPivot-feltet (ditt mixamo 'HeadPivot' under HeadTop_End)
     - Drag "eyeMount" til EyeMount-feltet (objektet "EyeMount").
     - Dra inn "mainCamera" fra XR Origin -> Camera Offset -> Main Camera
  3. Kjør. Avatarens EyeMount vil følge VR-kameraets world pos, 
     og HeadPivot roteres slik at den peker i VR-kameraets retning.
*/
using UnityEngine;

public class AvatarVRFollower : MonoBehaviour
{
    [Header("Sett referanser i Inspector")]
    [Tooltip("Drag inn ditt HeadPivot-objekt (eks. mixamorig:HeadTop_End -> HeadPivot)")]
    public Transform headPivot;

    [Tooltip("Drag inn EyeMount (barn av HeadPivot), eller hvasomhelst du vil skal tilsvare øyehøyde")]
    public Transform eyeMount;

    [Tooltip("Drag inn XR-hovedkamera (Main Camera under XR Origin -> Camera Offset)")]
    public Transform mainCamera;

    // Evt. skru av/på om du vil rotere hele pivot
    public bool rotateHeadPivot = true;

    void LateUpdate()
    {
        if (headPivot == null || eyeMount == null || mainCamera == null) return;

        // 1) Flytt hele avatarens root (altså "this.transform") 
        //    med en offset så EyeMount = mainCamera i world space.
        Vector3 offset = mainCamera.position - eyeMount.position;
        this.transform.position += offset;

        // 2) Roter HeadPivot, slik at EyeMount titter samme vei som mainCamera
        if (rotateHeadPivot)
        {
            // Finn nåværende lokal vinkel EyeMount => vi vil matche VR-kameraets retning
            // Enkelt grep: Juster pivotens world-rot slik at EyeMount får
            //  same world-rot som mainCamera.
            // Men vi roterer PIVOT, ikke root, for å unngå at hele avataren "snurrer" unødvendig.
            Quaternion pivotRot = headPivot.rotation; 
            Quaternion camRot   = mainCamera.rotation;

            // For å rotere pivot i world space, men beholde EyeMount == camRot:
            // 1) Temporært: Husk eyeMount's local rotation (skal være (0,0,0) i utgangspkt.)
            Quaternion oldLocal = eyeMount.localRotation;

            // 2) Sett pivot til camRot i world space
            headPivot.rotation = camRot;

            // 3) Gjenopprett EyeMount sin localRotation
            //    (Hvis EyeMount = (0,0,0) local rot, blir det helt likt.)
            eyeMount.localRotation = oldLocal;
        }
    }
}
