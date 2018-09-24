using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour {
    public GameObject basketPrefab;
    public int numBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;
    public List<GameObject> basketList;
	// Use this for initialization
	void Start () {
        basketList = new List<GameObject>();
        for (int i = 0; i < numBaskets; i++)
        {
            GameObject tBassketGo = Instantiate(basketPrefab) as GameObject;
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBassketGo.transform.position = pos;
            basketList.Add(tBassketGo);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AppleDestroyed ()
    // Destroy all apples
    {
        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach (var tGo in tAppleArray)
        {
            Destroy(tGo);
        }
        // Destroy a basket
        // Get the last basket list index
        int basketIndex = basketList.Count - 1;
        GameObject tBasketGo = basketList[basketIndex];
        basketList.RemoveAt(basketIndex);
        Destroy(tBasketGo);
        if (basketList.Count==0)
        {
            SceneManager.LoadSceneAsync("_Scene_0");
        }
    }
}
