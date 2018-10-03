using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoundsTest
{
    center, //游戏对象的中心是否位于屏幕中？
    onScreen, //游戏对象是否完全位于屏幕之中？
    offScreen //游戏对象是否完全位于屏幕之外？
}

public class Utils : MonoBehaviour {
    //接受两个bounds类型变量，并返回包含这两个Bounds的新Bounds
    public static Bounds BoundsUnion(Bounds b0,Bounds b1)
    {
        //如果其中一个Bounds的size为0，则忽略它
        if (b0.size==Vector3.zero && b1.size !=Vector3.zero)
        {
            return (b1);
        }
        else if (b0.size!=Vector3.zero&&b1.size==Vector3.zero)
        {
            return (b0);
        }
        else if (b0.size==Vector3.zero && b1.size==Vector3.zero)
        {
            return (b0);
        }
        //扩展b0，使其可以包含b1.min和b1.max
        b0.Encapsulate(b1.min);
        b0.Encapsulate(b1.max);
        return (b0);
    }

    public static Bounds CombineBoundsOfChildren(GameObject go)
    {
        //创建一个空白Bounds变量b
        Bounds b = new Bounds(Vector3.zero, Vector3.zero);
        //如果游戏对象具有渲染器组件……
        if (go.GetComponent<Renderer>()!=null)
        {
            b = BoundsUnion(b, go.GetComponent<Renderer>().bounds);
        }

        //如果游戏对象具有碰撞器组件……
        if (go.GetComponent<Collider>() != null)
        {
            //扩展b使其包含碰撞器的边界框
            b = BoundsUnion(b, go.GetComponent<Collider>().bounds);
        }

        //递归遍历游戏对象Transform组件的每个子对象
        foreach (Transform t in go.GetComponent<Transform>())
        {
            //扩展b将这些子对象的边界框也包含在内
            b = BoundsUnion(b, CombineBoundsOfChildren(t.gameObject));
        }
        return (b);
    }

    static public Bounds camBounds
    {
        get
        {
            if (_camBounds.size==Vector3.zero)
            {
                SetCameraBounds();
            }
            return (_camBounds);
        }
    }

    static private Bounds _camBounds;

    public static void SetCameraBounds(Camera cam = null)
    {
        //如果未传入任何摄像机作为参数，则使用主摄像机
        if (cam == null) cam = Camera.main;
        //这里对摄像机做一些重要假设：
        // 1.摄像机为正投影摄像机
        // 2.摄像机的旋转为R：[0,0,0]
        //根据屏幕左上角和右下角坐标创建两个三维向量
        Vector3 topLeft = new Vector3(0, 0, 0);
        Vector3 bottomRight = new Vector3(Screen.width, Screen.height, 0);
        //将两个坐标转化为世界坐标
        Vector3 boundTLN = cam.ScreenToWorldPoint(topLeft);
        Vector3 boundBRF = cam.ScreenToWorldPoint(bottomRight);
        //将两个三维向量的z坐标值分别设置为摄像机远剪切平面和近剪切平面的z坐标
        boundTLN.z += cam.nearClipPlane;
        boundBRF.z += cam.farClipPlane;
        //查找边界框的中心
        Vector3 center = (boundTLN + boundBRF) / 2f;
        _camBounds = new Bounds(center, Vector3.zero);
        //扩展_camBounds,使其具有尺寸
        _camBounds.Encapsulate(boundTLN);
        _camBounds.Encapsulate(boundBRF);
    }

    // 检查边界框bnd是否位于镜头边界框camBounds之内
    public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest test = BoundsTest.center)
    {
        return (BoundsInBoundsCheck(camBounds, bnd, test));
    }

    // 检查边界款lilB是否位于边界框bigB之内
    public static Vector3 BoundsInBoundsCheck(Bounds bigB,Bounds lilB, BoundsTest test = BoundsTest.onScreen)
    {
        //根据所选的BoundsTest，本函数的行为也会有所不同

        //获取边界框lilB的中心
        Vector3 pos = lilB.center;

        //INitialize the offset at [0,0,0]
        Vector3 off = Vector3.zero;
        switch (test)
        {
            //当test参数值为center时，函数将确定要将lilB的中心平移到bigB之内，
            //需要平移的方向和距离，用三维向量off表示
            case BoundsTest.center:
                if (bigB.Contains(pos))
                {
                    return (Vector3.zero);
                }
                if (pos.x>bigB.max.x)
                {
                    off.x = pos.x - bigB.max.x;
                }
                else if (pos.x<bigB.min.x)
                {
                    off.x = pos.x - bigB.min.x;
                }
                if (pos.y > bigB.max.y)
                {
                    off.y = pos.y - bigB.max.y;
                }
                else if (pos.y<bigB.max.y)
                {
                    off.y = pos.y - bigB.min.y;
                }
                if (pos.z > bigB.max.z)
                {
                    off.z = pos.z - bigB.max.z;
                }
                else if (pos.z<bigB.min.z)
                {
                    off.z = pos.z - bigB.min.z;
                }
                return (off);
            //当test参数值为onScreen时，函数将确定要将lilB整体平移到bigB之内
            //需要平移的方向和距离，用三维向量off表示
            case BoundsTest.onScreen:
                if (bigB.Contains(lilB.max)&&bigB.Contains(lilB.min))
                {
                    return (Vector3.zero);
                }
                if (lilB.max.x>bigB.max.x)
                {
                    off.x = lilB.max.x - bigB.max.x;
                }
                else if (lilB.min.x<bigB.min.x)
                {
                    off.x = lilB.min.x - bigB.min.x;
                }
                if (lilB.max.y>bigB.max.y)
                {
                    off.y = lilB.max.y - bigB.max.y;
                }
                else if (lilB.min.y<bigB.min.y)
                {
                    off.y = lilB.min.y - bigB.min.y;
                }
                if (lilB.max.z<bigB.max.z)
                {
                    off.z = lilB.max.z - bigB.max.z;
                }
                else if (lilB.min.z<bigB.min.z)
                {
                    off.z = lilB.min.z - bigB.min.z;
                }
                return (off);
            //当test参数值为offScreen时，函数将确定要将lilB的任意一部分
            //平移到bigB之内需要平移的方向和距离，用三维向量off表示
            case BoundsTest.offScreen:
                bool cMin = bigB.Contains(lilB.min);
                bool cMax = bigB.Contains(lilB.max);
                if (cMin||cMax)
                {
                    return (Vector3.zero);
                }
                if (lilB.min.x>bigB.max.x)
                {
                    off.x = lilB.min.x - bigB.max.x;
                }
                else if (lilB.max.x<bigB.min.x)
                {
                    off.x = lilB.max.x - bigB.min.x;
                }
                if (lilB.min.y>bigB.max.y)
                {
                    off.y = lilB.min.y - bigB.max.y;
                }
                else if (lilB.max.y<bigB.min.y)
                {
                    off.y = lilB.max.y - bigB.min.y;
                }
                if (lilB.min.z>bigB.max.z)
                {
                    off.z = lilB.min.z - bigB.max.z;
                }
                else if (lilB.max.z<bigB.min.z)
                {
                    off.z = lilB.max.z - bigB.min.z;
                }
                return (off);
        }
        return (Vector3.zero);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
