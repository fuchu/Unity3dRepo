using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class boundary
{
    public float min_x, max_x, min_z, max_z;
}

public class PlayerController : MonoBehaviour {
    public float moveSpeed;
    private Rigidbody rd;
    private Vector3 vt;
    public float tilt;
    public boundary playBoundary;
    public GameObject blazes;
    public float fireRate;
    private float nextFire;
    public Transform shotSpawn;

    // Use this for initialization
    void Start () {
		
	}

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 vt = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //up,down,right,left move
        GetComponent<Rigidbody>().velocity = vt * moveSpeed;
        //限制飞行范围
        //x:-6.58-6.52,z:-1-12
        GetComponent<Rigidbody>().position = new Vector3(
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, playBoundary.min_x, playBoundary.max_x),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z,playBoundary.min_z,playBoundary.max_z)
            );
        //左右位移时鸡翅的倾斜效果
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetButton("Fire1")&&Time.time>= nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(blazes, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }
    }
}
