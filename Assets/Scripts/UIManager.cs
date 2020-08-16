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

    public Image Crosshair;
    public Sprite DefaultCrossHair;
    public Color DefaultTint = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        setDefaultCrosshair();
        disableCrosshair();
    }

    public void enableCrosshair()
	{
        Crosshair.enabled = true;
	}

    public void disableCrosshair()
	{
        Crosshair.enabled = false;
	}

    public void setCrosshair(Sprite crosshair, Color? tint = null)
	{
        Crosshair.sprite = crosshair;
        Crosshair.color = tint == null ? new Color(255f, 255f, 255f, 204f) : tint.Value;
	}

    public void setDefaultCrosshair()
	{
        Crosshair.sprite = DefaultCrossHair;
        Crosshair.color = DefaultTint;
	}
}
