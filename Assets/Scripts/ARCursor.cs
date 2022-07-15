using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.UI;

public class ARCursor : MonoBehaviour
{
    public GameObject cursor;
    public GameObject point;
    public ARRaycastManager raycastManager;
    public LineRenderer lineRenderer;
    public TextMeshPro mtext;
    public TextMeshProUGUI distance_measureP;
    public Button undoButtonP;
    public Button finishButtonP;
    public Button addButtonP;
    public TextMeshProUGUI distance_measureL;
    public Button undoButtonL;
    public Button finishButtonL;
    public Button addButtonL;
    

    private TextMeshPro currText;
    private bool flag = false;
    private bool clicked = false;
    private float TotalDistance;
    private float currDist;
    private List<GameObject> points;
    private List<TextMeshPro> texts;
    private List<float> distances;
    private bool finished;
    // Start is called before the first frame update
    void Start()
    {
        points = new List<GameObject>();
        texts = new List<TextMeshPro>();
        distances = new List<float>();
        distances.Add(0);
        undoButtonP.interactable = false;
        undoButtonL.interactable = false;
        TotalDistance = 0;
        currDist = 0;
        finished = false;
        finishButtonL.interactable = false;
        finishButtonP.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!finished) {
            Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f,0.5f));
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            raycastManager.Raycast(screenPosition,hits,UnityEngine.XR.ARSubsystems.TrackableType.Planes);

            if(hits.Count>0) {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
            }
        }

        if(lineRenderer.positionCount>0 && !finished) {
            if(flag) {
                lineRenderer.positionCount++;
                flag = false;
            }
            lineRenderer.SetPosition(lineRenderer.positionCount-1,transform.position);
            Vector3 pointA = lineRenderer.GetPosition(lineRenderer.positionCount-1);
            Vector3 pointB = lineRenderer.GetPosition(lineRenderer.positionCount-2);
            float dist = Vector3.Distance(pointA,pointB);
            dist = Mathf.Round(dist * 10000)/100;
            currDist = TotalDistance + dist;
            if(currText == null) {
                currText = Instantiate(mtext);
            }
            currText.text = "" + dist;
            currText.transform.position = (pointA+pointB)/2;
            currText.transform.rotation = Camera.main.transform.rotation;

            float size = (Camera.main.transform.position-transform.position).magnitude/100;
            currText.transform.localScale = new Vector3(size,size,size);
        }

        if(clicked && !finished) {
            clicked = false;
            GameObject p = GameObject.Instantiate(point,transform.position,transform.rotation);
            points.Add(p);
            lineRenderer.positionCount++;
            if(lineRenderer.positionCount==1) {
                flag = true;
            }
            lineRenderer.SetPosition(lineRenderer.positionCount-1,transform.position);
            if(currText!=null)
                texts.Add(currText);
            if(lineRenderer.positionCount>1) {
                TotalDistance = currDist;
                distances.Add(TotalDistance);
                distance_measureP.text = "Total Distance: " + TotalDistance + " cm";
                distance_measureL.text = "Total Distance: " + TotalDistance + " cm";
                currText = null;
            }
        }
    }

    public void AddPoint() {
        clicked = true;
        if(lineRenderer.positionCount>1){
            finishButtonL.interactable = true;
            finishButtonP.interactable = true;
        }
        if(lineRenderer.positionCount>1) {
            undoButtonP.interactable = true;
            undoButtonL.interactable = true;
        }
    }

    public void Finish() {
        clicked = true;
        finished = true;
        lineRenderer.positionCount--;
        Destroy(currText.gameObject);
        addButtonL.interactable = false;
        addButtonP.interactable = false;
        undoButtonL.interactable = false;
        undoButtonP.interactable = false;
        finishButtonL.interactable = false;
        finishButtonP.interactable = false;
        Destroy(this.gameObject);
    }

    public void undo() {
        Destroy(points[points.Count-1]);
        Destroy(texts[texts.Count-1].gameObject);

        distances.RemoveAt(distances.Count-1);
        TotalDistance = distances[distances.Count-1];
        distance_measureP.text = "Total Distance: " + TotalDistance + " cm";
        distance_measureL.text = "Total Distance: " + TotalDistance + " cm";

        points.RemoveAt(points.Count-1);
        texts.RemoveAt(texts.Count-1);
        lineRenderer.positionCount--;
        if(texts.Count==0) {
            undoButtonP.interactable = false;
            undoButtonL.interactable = false;
        }
        if(lineRenderer.positionCount<=2) {
            finishButtonP.interactable = false;
            finishButtonL.interactable = false;
        }        
    }
}

