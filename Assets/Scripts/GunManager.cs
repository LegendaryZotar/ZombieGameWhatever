using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    #region Singleton

    public static GunManager instance;
    public GameObject t;
    private void Awake()
    {
        instance = this;
    }

	#endregion

	public int test;
    
    public Image Cross_Hair;
    public Sprite Default_CrossHair;
    public Sprite CrossHair;

    public Transform ActiveAim;

	private void OnDrawGizmos()
    {
	}


    void Start()
    {
        //Cross_Hair.sprite = CrossHair;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
