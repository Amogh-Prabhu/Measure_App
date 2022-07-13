using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARCursor : MonoBehaviour
{
    public GameObject cursor;
    public GameObject point;
    public ARRaycastManager raycastManager;
    public LineRenderer lineRenderer;

    public TextMeshPro mtext;

    private TextMeshPro currText;
    private bool flag = false;
    public bool clicked = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f,0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenPosition,hits,UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if(hits.Count>0) {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }

        if(lineRenderer.positionCount>0) {
            if(flag) {
                lineRenderer.positionCount++;
                flag = false;
            }
            lineRenderer.SetPosition(lineRenderer.positionCount-1,transform.position);
            Vector3 pointA = lineRenderer.GetPosition(lineRenderer.positionCount-1);
            Vector3 pointB = lineRenderer.GetPosition(lineRenderer.positionCount-2);
            float dist = Vector3.Distance(pointA,pointB);
            dist = Mathf.Round(dist * 10000)/100;
            if(currText == null) {
                currText = Instantiate(mtext);
            }
            currText.text = "" + dist;
            currText.transform.position = (pointA+pointB)/2;
            currText.transform.rotation = Camera.main.transform.rotation;

            float size = (Camera.main.transform.position-transform.position).magnitude/100;
            currText.transform.localScale = new Vector3(size,size,size);
        }

        if(clicked) {
            clicked = false;
            GameObject.Instantiate(point,transform.position,transform.rotation);
            lineRenderer.positionCount++;
            if(lineRenderer.positionCount==1) {
                flag = true;
            }
            lineRenderer.SetPosition(lineRenderer.positionCount-1,transform.position);
            if(lineRenderer.positionCount>1) {
                Vector3 pointA = lineRenderer.GetPosition(lineRenderer.positionCount-1);
                Vector3 pointB = lineRenderer.GetPosition(lineRenderer.positionCount-2);
                currText = null;
            }
        }
    }

    public void onClick() {
        clicked = true;
    }
}
