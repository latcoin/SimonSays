using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct SimonLightPlate
{
	public enum eType
	{
		INVALID_TYPE = -1,
		BLUE,
		GREEN,
		RED,
		YELLOW,
		NUM_TYPES
	}
	
	public SimonLightPlate( string plateName )
	{
		plate = GameObject.Find(plateName);
	}
	
	public GameObject plate; // The plate associated with the colors.
}

public class SimonSays : MonoBehaviour 
{
	public float displaySequenceRepeatInterval = 1.0f;
	SimonLightPlate[] lightPlates = new SimonLightPlate[(int)SimonLightPlate.eType.NUM_TYPES];
	
	enum eState
	{
		INVALID_STATE = -1,
		THINKING,
		DISPLAY_SEQUENCE,
		WAITING_FOR_USER,
		NUM_STATES
	}
	
	eState currentState = eState.INVALID_STATE;
	
	List<int> sequence = new List<int>(100); // holds the sequence
	int sequenceCount = 0; // the counter for which index of the sequence we're currently showing.
	
	List<int> clickedSequence = new List<int>(100);
	
	// Use this for initialization
	void Start () 
	{
		lightPlates[(int)SimonLightPlate.eType.BLUE] = new SimonLightPlate("BluePlane");
		lightPlates[(int)SimonLightPlate.eType.GREEN] = new SimonLightPlate("GreenPlane");
		lightPlates[(int)SimonLightPlate.eType.RED] = new SimonLightPlate("RedPlane");
		lightPlates[(int)SimonLightPlate.eType.YELLOW] = new SimonLightPlate("YellowPlane");
		
		ResetGame();
		InvokeRepeating("DisplaySequence", 1, displaySequenceRepeatInterval);
	}
	
	void OnLeftClickDown(SimonLightPlate.eType color)
	{
		if (currentState == eState.WAITING_FOR_USER)
		{
			lightPlates[(int)color].plate.renderer.enabled = true;
			clickedSequence.Add((int)color);
		}
	}
	
	void OnLeftClickUp(SimonLightPlate.eType color)
	{
		if (currentState == eState.WAITING_FOR_USER)
		{
			lightPlates[(int)color].plate.renderer.enabled = false;
			if (!VerifySequence())
			{
				ResetGame();
			}
			else
			{
				if (clickedSequence.Count == sequence.Count)
				{
					currentState = eState.THINKING;
					clickedSequence.Clear();
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (currentState == eState.THINKING)
		{
			// Add new sequence.
			AddNewSequence();
		}
	}
	
	bool VerifySequence()
	{
		// Let's verify the sequence up until that point where the user left off.
		for (int i = 0; i < clickedSequence.Count; ++i)
		{
			if (clickedSequence[i] != sequence[i])
			{
				return false;
			}
		}
		return true;
	}
	
	void DisplaySequence()
	{
		if (currentState == eState.DISPLAY_SEQUENCE)
		{
			// We're at the next sequence so turn off the previous one.
			if (sequenceCount > 0)
			{
				lightPlates[sequence[sequenceCount-1]].plate.renderer.enabled = false;
			}
			// We're at the end, collect user input
			if (sequenceCount == sequence.Count)
			{
				currentState = eState.WAITING_FOR_USER;
			}
			else
			{
				lightPlates[sequence[sequenceCount]].plate.renderer.enabled = true;
				// Add Audio here.
				//AudioSource
				++sequenceCount;
			}
			
		}
	}
	
	void AddNewSequence()
	{
		sequence.Add(Random.Range(0,3));
		currentState = eState.DISPLAY_SEQUENCE;
		sequenceCount = 0;
	}
	
	void ResetGame()
	{
		currentState = eState.THINKING;
		sequence.Clear();
		clickedSequence.Clear();
		sequenceCount = 0;
		
		for (int i = 0; i < (int)SimonLightPlate.eType.NUM_TYPES; ++i)
		{
			lightPlates[i].plate.renderer.enabled = false;
		}
	}
}
