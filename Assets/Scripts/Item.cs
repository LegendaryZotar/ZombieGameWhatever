using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    public int InventorySlot;
    public bool InInventory;

    public Sprite Image;
    public MonoBehaviour item;

    GameObject Player;

    public void Interact(GameObject player)
    {
        Player = player;
        Player.GetComponent<Inventory>().PickUpItem(gameObject);

    }

    void Update()
    {
        if (InInventory == true)
        {

            if (InventorySlot != Player.GetComponent<Inventory>().SelectedInvSlot)
            {
                gameObject.active = false;
            }


        }
    }


    
    public string Save()
    {
         return JsonUtility.ToJson(gameObject);
    }

    public void Load(string json)
    {
        JsonUtility.FromJsonOverwrite(json, gameObject);
    }
}

