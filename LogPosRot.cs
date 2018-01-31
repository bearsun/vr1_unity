using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
// LogPosRot gets position, rotation, events, and timestamp and saves it to a file
/// </summary>
public class LogPosRot : MonoBehaviour
{
    //private Text posText;    // needs some kind of text game object to display position information
    //private PTDataFileWriter dataWriter = new PTDataFileWriter();
    //private string filePath;
    private string fileName;     // file pathname
    private Vector3 tempPos;     // position
    private Vector3 tempRot;     // rotation
    private float tempTime;      // time + deltaTime
    private string timeStamp;    // timestamp for out putfile
    private string sbj;
    private int boxcounter;
    private int trialcounter;
    private float balance;

    private StreamWriter writer;
    public int nframesperwrite;
    private int writingcount;

    void Start()
    {
        // Create filename with experiment conditions
        sbj    			 = "tt";
        string timeStamp = string.Format("_{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
//        filePath 		 = "";  // don't use a path here
        fileName 		 = sbj + '-' + timeStamp + ".txt";
      
        // define variable values at start
        tempPos 	= transform.position;             // save position at start
        tempRot 	= transform.rotation.eulerAngles; // save rotation at start
        tempTime   	= 0;
        
        // create output file and write header
        //dataWriter.CreateNewAppendableFile(string.Format("{0}{1}", filePath, fileName), false); // set timestamp = false!
        //dataWriter.AppendStringToOpenedFile(string.Format("pos.x; pos.y; pos.z; rot.x; rot.y; rot.z; event; time\r\n"));
        writer = File.CreateText(fileName);
        writer.WriteLine("pos.x; pos.y; pos.z; rot.x; rot.y; rot.z; trial; nbox; balance; time\r\n");
        writingcount = 0;
        
    }

    void FixedUpdate()
    {
        //tempTime = tempTime + Time.fixedDeltaTime;
        if (writingcount < nframesperwrite)
        {
            writingcount++;
        }
        else
        {
            // get position, rotation, and time at each frame
            tempPos = transform.position;
            tempRot = transform.rotation.eulerAngles;

            // log events created by the experimentManager
            boxcounter = transform.parent.gameObject.GetComponent<move>().boxchecked;
            trialcounter = transform.parent.gameObject.GetComponent<move>().trial;
            balance = transform.parent.gameObject.GetComponent<move>().money;
            tempTime = Time.time;

            // THIS WON'T WORK; YOU'll need to clean up the string command to your needs	
            // make sure to format strings correctly (events as integers and not floats!)
            string line = String.Format("{0,3:f3};{1,3:f3};{2,3:f3};{3,3:f3};{4,3:f3};{5,3:f3};{6};{7};{8,3:f3};{9,5:f3}\r\n",
                                                tempPos.x, tempPos.y, tempPos.z,
                                                tempRot.x, tempRot.y, tempRot.z,
                                                trialcounter, boxcounter, balance, tempTime);

            // append position, rotation, and time at each frame to file;
            writer.WriteLine(line);

            // Display the current information for debugging
            //ShowPosRotText();
            writingcount = 0;
        }
    }

    void OnApplicationQuit()
    {
        // Close file when application quits
        writer.Close();
    }

}
