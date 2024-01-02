using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public Transform backgrounds;

    float lastX = float.NaN;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(backgrounds != null) {
            
            if(lastX.Equals(float.NaN)) {
                lastX = transform.position.x;
            }

            float camDiff = transform.position.x - lastX;
            
            if(camDiff == 0.0f) {
                return;
            }

            foreach(Transform child in backgrounds) {

                int index = int.Parse(child.name) + 1;

                if(index == backgrounds.childCount) {
                    break;
                }

                child.position += new Vector3(camDiff - (camDiff * index / (backgrounds.childCount * 4)), 0.0f, 0.0f);
            }

            lastX = transform.position.x;

        }
        
    }
}
