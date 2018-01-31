using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visibility : MonoBehaviour {
	private Renderer rend;
    public GameObject obj;
    private bool invoked;
    private bool check;

	public void Start(){
		rend = GetComponent<Renderer> ();
		rend.enabled = true;
        obj.SetActive(false);
        invoked = false;
        check = false;
    }

	// Update is called once per frame
	void OnTriggerStay (Collider col) {
        
        if ((Input.GetButton ("Submit")&&Vector3.Distance(transform.position, col.transform.position)<2.5f)) {
            obj.SetActive(true);
            rend.enabled = false;
            obj.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

            if (check == false)
            {
                check = true;
                col.GetComponent<move>().boxchecked += 1;
            }

            if (invoked == false)
            {
                if (obj.GetComponent<objid>().objind == col.GetComponent<move>().objindex)
                {
                    col.GetComponent<move>().money += Reward(col.GetComponent<move>().boxchecked);
                    col.GetComponent<move>().found = true;
                    col.GetComponent<move>().Invoke("StartTrial", 3);
                    invoked = true;
                }
            }

        } else {
			rend.enabled = true;
            obj.SetActive(false);
		}
	}

    private float Reward(int n)
    {
        float r;
        if (n == 1 || n == 2)
            r = 0.20f;
        else if (n == 3 || n == 4)
            r = 0.10f;
        else
            r = 0f;

        return r;        
    }
}