using UnityEngine;
using System.Collections;

public class BallParticles : MonoBehaviour {

	ParticleSystem aParticleSystem;
	ParticleSystem.EmissionModule emit;

	int amountInBurst = 30;

	// Use this for initialization
	void Start () {
		aParticleSystem = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		//CreateParticles ();
	}

	public void CreateParticles(){
		//aParticleSystem.Clear();
		aParticleSystem.Emit(amountInBurst);
	}
}
