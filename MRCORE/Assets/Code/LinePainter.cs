using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class LinePainter : MonoBehaviour
{
    public Transform handTrans;
    public XRNode handRole = XRNode.RightHand;
    readonly float minDistance = 0.01f;

    List<Vector3> points = new();
    LineRenderer line;
    MakeMesh makeMesh;
    bool lastTrigger = true;
    bool active = true;


    void Start()
    {
        makeMesh = GetComponent<MakeMesh>();
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
    }

    void Update()
    {
        if (!active) return;

        InputDevice controller = InputDevices.GetDeviceAtXRNode(handRole);
        controller.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger);

        if (trigger)
        {
            MakeLine();
        }
        else if (!trigger && lastTrigger)
        {
            makeMesh.MakeFill(points.ToArray());
            line.loop = true;
            active = false;
        }
        lastTrigger = trigger;
    }

    void MakeLine()
    {
        Vector3 currentPos = transform.InverseTransformPoint(handTrans.position);
        if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], currentPos) > minDistance)
        {
            points.Add(currentPos);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, currentPos);
        }
    }
}