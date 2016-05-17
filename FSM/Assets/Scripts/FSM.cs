using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour
{
	//Player Transform
	protected Transform playerTransform;
	//Next destination position of the Box
	protected Vector3 destination;
	//List of points for patrolling
	protected GameObject[] patrolPointList;
	protected virtual void Initialize (){
	}
	protected virtual void StateUpdate (){
	}
	protected virtual void StateFixedUpdate (){
	}
	void Start ()
	{
		Initialize ();
	}
	void Update ()
	{
		StateUpdate ();
	}
	void FixedUpdate ()
	{
		StateFixedUpdate ();
	}
}
