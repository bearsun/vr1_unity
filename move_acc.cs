using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[RequireComponent(typeof(CharacterController))]
public class move_acc : MonoBehaviour {
    public float speed = 100.0F;
    public float rotateSpeed = 20.0F;
    public float picshowtime = 5f;
    public GameObject eyes;
    public GameObject canvastext;
    public GameObject canvaspic;
    public TextAsset seqfile;
    private GameObject[] objs;

    private string datafile;     // file to moniter performance
    private StreamWriter datawriter;
    private float starttime;
    private float stime;

    private Text onscreen;
    private Image im;
    private float imtime;

    public float money = 0.00f;
    public int trial = 0;

    // random sequence
    private int[,] seq = new int[24, 2] {{3,6},
{3,3},
{3,7},
{3,8},
{3,1},
{3,4},
{2,4},
{2,8},
{2,2},
{2,5},
{2,1},
{2,7},
{1,8},
{1,5},
{1,4},
{1,3},
{1,2},
{1,6},
{4,1},
{4,2},
{4,7},
{4,5},
{4,3},
{4,6}};

    public int startindex;
    //private int destindex;
    public int objresp;
    public int objindex;
    public int boxchecked;
    public bool found = false;
    private bool imstart;
    private bool covered;

    private Vector3[] positions = new[] { new Vector3(-20f, 2.8f, 20f), //NW
                                          new Vector3(20f, 2.8f, 20f),  //NE
                                          new Vector3(-20f, 2.8f, -20f), //SW
                                          new Vector3(20f, 2.8f, -20f) }; //SE

    private Vector3[] rotations = new[] { new Vector3(0f, 0f, 0f),
                                          new Vector3(0f, 90f, 0f),
                                          new Vector3(0f, -90f, 0f),
                                          new Vector3(0f, 180f, 0f)};

    void Start()
    {
        string sbj = "tt";
        string dataStamp = string.Format("_{0:yyyy-MM-dd_hh-mm-ss-tt}_data", DateTime.Now);
        datafile = sbj + '-' + dataStamp + ".txt";
        datawriter = File.CreateText(datafile);
        datawriter.WriteLine("trial; start_room; target; response; searchT\r\n");

        objs = GameObject.FindGameObjectsWithTag("objcover");
        onscreen = canvastext.GetComponent<Text>();
        im = canvaspic.GetComponent<Image>();
        StartTrial();
    }

    void Update() {
        onscreen.text = "Trial: " + trial.ToString();
        if (imstart == false)
        {
            CharacterController controller = GetComponent<CharacterController>();
            transform.Rotate(0f, Input.GetAxis("Mouse X") * rotateSpeed, 0f);
            eyes.transform.Rotate(-Input.GetAxis("Mouse Y") * rotateSpeed, 0f, 0f);
            if (found == false)
            {
                Vector3 trans = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
                Vector3 forward = transform.TransformDirection(trans);
                controller.SimpleMove(forward * speed);
            }

            foreach (GameObject obj in objs)
            {
                if (!obj.GetComponent<Renderer>().enabled)
                {
                    covered = false;
                }
            }

            if (Input.GetButton("Cancel") && covered)
            {
                im.enabled = true;
            }
            else if (im.enabled)
            {
                im.enabled = false;
            }
            covered = true;
        }

    }

    public void StartTrial()
    {
        if (trial > 0)
        {
            stime = Time.time - starttime - 3;
            string line = String.Format("{0,2};{1,1};{2,1};{3,1};{4,3:f3}\r\n",
                                               trial, startindex, objindex, objresp, stime);
            datawriter.WriteLine(line);
        }
        boxchecked = 0;
        trial++;
        if (trial == 25)
        {
            Application.Quit();
        }
        found = false;

        objindex = seq[trial-1, 1];
        startindex = seq[trial-1, 0] - 1;
        //destindex = seq[trial, 2];

        foreach (GameObject obj in objs)
        {
            obj.GetComponent<visibility_acc>().Start();
        }

        transform.position = positions[startindex];
        transform.rotation = Quaternion.Euler(rotations[startindex]);

        int randint = Mathf.CeilToInt(UnityEngine.Random.value * 6);
        string file = "700_obj" + objindex.ToString() + "-" + randint.ToString();
        im.sprite = Resources.Load<Sprite>(file);
        im.enabled = true;
        Invoke("disablepic", picshowtime);
        imstart = true;
        starttime = Time.time + picshowtime;
    }

    private void disablepic()
    {
        im.enabled = false;
        imstart = false;
    }

    void OnApplicationQuit()
    {
        // Close file when application quits
        datawriter.Close();
    }
}
