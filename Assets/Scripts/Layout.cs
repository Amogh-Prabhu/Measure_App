using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layout : MonoBehaviour
{
    public GameObject portrait;
    public GameObject landscape;
    // Start is called before the first frame update
    void Start()
    {
        portrait.SetActive(false);
        landscape.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight) {
            landscape.SetActive(true);
            portrait.SetActive(false);
        } else {
            portrait.SetActive(true);
            landscape.SetActive(false);
        }
    }
}
