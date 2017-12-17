using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPoint : MonoBehaviour {

    public TurningPoint[] nextPoints;
    public Vector2[] vectToNextPoint;

	// Use this for initialization
	void Start () {
        vectToNextPoint = new Vector2[nextPoints.Length];
        for (int i = 0; i < nextPoints.Length; i++) {
            TurningPoint nextPoint = nextPoints[i];
            //Debug.Log("nextPoint.transform="+nextPoint.transform.localPosition);
            Vector2 pointVect = nextPoint.transform.localPosition - transform.localPosition;

            vectToNextPoint[i] = pointVect.normalized;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
