using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    static public Hero S;
    //以下字段用来控制飞船运行
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    //飞船状态信息
    public float shieldLevel = 1;
    public bool _____________________________;
    public Bounds bounds;

    private void Awake()
    {
        S = this; //设置单例对象
        bounds = Utils.CombineBoundsOfChildren(this.gameObject);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // 从Input（用户输入）类中获取信息
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        // 基于获取的水平轴和竖直轴信息修改transform.position
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        bounds.center = transform.position;

        //使飞船保持在屏幕边界内
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
        if (off!=Vector3.zero)
        {
            pos -= off;
            transform.position = pos;
        }

        //让飞船旋转一个角度，使它更具动感
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
	}
}
