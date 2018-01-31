using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visibility_acc : MonoBehaviour {
	private Renderer rend;
    public GameObject obj;
    private GameObject player;
    private bool invoked;
    private bool check;
    private int[,] mapping = new int[,] { { 7,1 }, { 3,6 }, { 5,2 }, { 8,4 } };
    private int startroom;

    public void Start(){
		rend = GetComponent<Renderer> ();
		rend.enabled = true;
        obj.SetActive(false);
        invoked = false;
        check = false;
        player = GameObject.FindGameObjectWithTag("Player");
        startroom = player.GetComponent<move_acc>().startindex;
    }

	// Update is called once per frame
	void OnTriggerStay (Collider col) {

        if ((Input.GetButton("Submit") && Vector3.Distance(transform.position, col.transform.position) < 2.5f))
        {
            if ((obj.GetComponent<objid>().objind == mapping[startroom, 0]) || (obj.GetComponent<objid>().objind == mapping[startroom, 1]))
            {
                obj.SetActive(true);
                rend.enabled = false;
                obj.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
            }
            else
            {
                if (check == false)
                {
                    check = true;
                    col.GetComponent<move_acc>().boxchecked += 1;
                }

                if (invoked == false)
                {
                    if (obj.GetComponent<objid>().objind == col.GetComponent<move_acc>().objindex)
                    {
                        col.GetComponent<move_acc>().money += Reward(col.GetComponent<move_acc>().boxchecked);
                    }
                    col.GetComponent<move_acc>().found = true;
                    col.GetComponent<move_acc>().objresp = obj.GetComponent<objid>().objind;
                    col.GetComponent<move_acc>().Invoke("StartTrial", 3);
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