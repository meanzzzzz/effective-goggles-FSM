using UnityEngine;
using System.Collections;

public class PatrolerFSM : FSM
{
	public enum State
	{
		NoState,
		Patrol,
		Chase,
	}
	//state the Patrolera  in
				public State currentState;
	//Speed of the Patroler
			public float currentSpeed;
	//Patrolers Rotation Speed .. too high is too fast
				public float currentRotSpeed;
	
	//Initialized the Fsm for the Patroler
	protected override void Initialize ()
	{
		 currentState = State.Patrol;
		currentSpeed = 5.0f;
		currentRotSpeed = 1.5f;				
		
		//PatrolerPoints marked with tag
		patrolPointList = GameObject.FindGameObjectsWithTag ("PatrolPoint");
		
		//Set Random destination point for the patrol state first
		FindAnotherPoint ();
		
		//Get the target enemy(Player)
		GameObject objPlayer = GameObject.FindGameObjectWithTag ("Player");
		playerTransform = objPlayer.transform;
		

		
	}
	
	//Update each frame
	protected override void StateUpdate ()
	{
		switch (currentState) {
		case State.Patrol:
			UpdatePatrolState ();
			break;
		case State.Chase:
			UpdateChaseState ();
			break;
		}
	}
	
	protected void UpdatePatrolState ()
	{

		if (Vector3.Distance (transform.position, destination) <= 2.5f) {

			FindAnotherPoint ();
		}
		
		//Check the distance with player (taged)
		//When near, change to chase
		else if (Vector3.Distance (transform.position, playerTransform.position) <= 15.0f) {
			print ("Switched state to chase");
			currentState = State.Chase;
		}
		
		//Rotate to the target points
		Quaternion targetRotation = Quaternion.LookRotation (destination - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * currentRotSpeed);
		
		//Go Forward
		transform.Translate (Vector3.forward * Time.deltaTime * currentSpeed);
	}
	
	protected void FindAnotherPoint ()
	{
		print ("Finding another Patrolpoint" );
		int randomIndex = Random.Range (0, patrolPointList.Length);
		float randomRadius = 5.0f;
		Vector3 randPosition = Vector3.zero;
		destination = patrolPointList [randomIndex].transform.position + randPosition;
		
		//Check Range to Move and decide the random point
		//as the same as before
		if ( CurrentRange (destination)) {
			randPosition = new Vector3 (Random.Range (-randomRadius, randomRadius), 0.0f, Random.Range (-randomRadius, randomRadius));
			destination = patrolPointList [randomIndex].transform.position + randPosition;
		}
	}
	
	protected bool CurrentRange (Vector3 pos)
	{
		float xPos = Mathf.Abs (pos.x - transform.position.x);
		float zPos = Mathf.Abs (pos.z - transform.position.z);
		if (xPos <= 8 && zPos <= 8)
			return true;
		return false;
	}
	
	protected void UpdateChaseState ()
	{

		destination = playerTransform.position;
		//Checking the distance 
		float distance = Vector3.Distance (transform.position, playerTransform.position);
		
		//revert back to patrol
		if (distance >= 10.0f) {
			currentState = State.Patrol;
			FindAnotherPoint ();
		}
		//Rotate to the target point
		Quaternion targetRotation = Quaternion.LookRotation (destination - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * currentRotSpeed);
		//Go Forward
		transform.Translate (Vector3.forward * Time.deltaTime * currentSpeed);
	}
}