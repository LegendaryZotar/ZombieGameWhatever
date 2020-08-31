using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    #region Singleton

    public static GunManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    [System.Serializable]
    public class CrossHair
	{
        public Sprite crossHair;
        public Color tint = Color.white;
	}

    public CrossHair defaultCrosshair;
    public CrossHair customCrosshair;

    public Transform ActiveAim;

    public float recoilSpeed = 10f;
    public float recoilLimit = 20f;

    public void AddRecoil(float recoilAmount)
    {
        //4 is multiplier
        CameraFollow.instance.AddRecoil(recoilAmount / 4, 1 / recoilSpeed, recoilLimit / 4);
	}
}
