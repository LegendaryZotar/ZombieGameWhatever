using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    #region Singleton

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public Image crossHair;
    GunManager GM;

    // Start is called before the first frame update
    void Start()
    {
        GM = GunManager.instance;
    }

	private void Update()
	{
		if(StateManager.getCameraState() == StateManager.cameraStates.Aiming)
		{
            GunManager.CrossHair ch = GM.customCrosshair.crossHair == null ? GM.defaultCrosshair : GM.customCrosshair;
            crossHair.sprite = ch.crossHair;
            crossHair.color = ch.tint;
            crossHair.enabled = true;
		}
		else
		{
            crossHair.enabled = false;
		}
	}
}
