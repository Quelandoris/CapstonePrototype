using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIEventManager : MonoBehaviour {

	public delegate void Reached();
	public static event Reached NavReached;
		
	
}
