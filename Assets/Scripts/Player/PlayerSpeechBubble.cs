using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Text;
using System.IO;

public class PlayerSpeechBubble : MonoBehaviour {

	public float changeTextInterval;	 	// Countdown interval amount for next bubble.	
	public float displayInterval;			// Time bubble is on screen.

	private bool isVisible = false;			// Is canvas currently visible on scren.
	private Canvas canvas;
	private Text text;						// Text inside speech bubble.
	private float timer = 0f;				// Timer for displaying the bubble.
	private PlayerController.PlayerType playerType;

	void Awake (){

		// Set up references
		text = this.GetComponentInChildren<Text>();
		canvas = this.GetComponentInChildren<Canvas>();
		PlayerController[] playerController = this.GetComponentsInParent<PlayerController>();
		playerType = playerController[0].playerType;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
		timer += Time.deltaTime;

		if (timer >= changeTextInterval && !isVisible) {
			timer = 0f;
			LoadTextFile ("Assets\\Scripts\\Player\\speechBubbleTexts.txt");	 //Load text from text file
			canvas.enabled = true;
			isVisible = true;
		}
		else if (timer >= displayInterval && isVisible) {
			timer = 0f;
			canvas.enabled = false;
			isVisible = false;
		}

	}

	// Orientation is screwed up by PlayerController, this fixes it. There is prob a better solution.
	public void ReRotate(){
			text.transform.Rotate (0,180,0);
	}

	private bool LoadTextFile(string fileName)
	{
		// Handle any problems that might arise when reading the text
		try
		{
			string line;
			// Create a new StreamReader, tell it which file to read and what encoding the file
			// was saved as
			StreamReader theReader = new StreamReader(fileName, Encoding.Default);
			// Immediately clean up the reader after this block of code is done.
			// You generally use the "using" statement for potentially memory-intensive objects
			// instead of relying on garbage collection.
			// (Do not confuse this with the using directive for namespace at the 
			// beginning of a class!)
			using (theReader)
			{
				// While there's lines left in the text file, do this:
				do
				{
					line = theReader.ReadLine();

					if (line != null)
					{
						// Do whatever you need to do with the text line, it's a string now
						// In this example, I split it into arguments based on comma
						// deliniators, then send that array to DoStuff()
						string[] entries = line.Split(',');
						if (entries.Length > 0)
							ProcessTextLine(entries);
					}
				}
				while (line != null);
				// Done reading, close the reader and return true to broadcast success    
				theReader.Close();
				return true;
			}
		}
		// If anything broke in the try block, we throw an exception with information
		// on what didn't work
		catch (Exception e)
		{
			Debug.Log("{0}\n" + e.Message);
			return false;
		}
	}

	// Once the line is read, recognize if it belongs to this player type, and print a random one on screen.
	// The first two members of the array should be int a string identifiers to player type, respectively.
	void ProcessTextLine(string[] entries){

		if (Int32.Parse (entries [0]) != (int)this.playerType)
			return;
		else {
			//First two members are player identifiers
			text.text = (entries[UnityEngine.Random.Range (2, entries.Length)]);
		}
	}
}

