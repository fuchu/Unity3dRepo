using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour {
    public GameObject shot;
    public float shotRate;
    public float delay;
	// Use this for initialization
	void Start () {
        InvokeRepeating("Fire", delay, shotRate);
	}
	
	// Update is called once per frame
void Fire()
    {
        Instantiate(shot, transform.position, transform.rotation);
        GetComponent<AudioSource>().Play();
    }
}
