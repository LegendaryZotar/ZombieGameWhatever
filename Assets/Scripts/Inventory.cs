using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> storedItems;
    public int ItemCap;
    public int SelectedInvSlot;
    public GameObject ItemHolder;

    public Dictionary<int, Item> ItemSlots = new Dictionary<int, Item>();

    private void Start()
    {
        
    }

    private void Update()
    {
        for (int i = 1; i <= ItemCap; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
                SelectedInvSlot = i;
        }

        for (int i = 0; i < storedItems.Count; i++)
        {
            if (storedItems[i].GetComponent<Item>().InventorySlot == SelectedInvSlot)
            {
                storedItems[i].active = true;
            }
        }

        if (Input.GetKeyDown("g") && SelectedInvSlot <= storedItems.Count)
        {
            DropItem(storedItems[SelectedInvSlot-1].gameObject);
            
        }
    }

    public void PickUpItem(GameObject item)
    {

        if (ItemCap > storedItems.Count)
        {
            storedItems.Add(item);
            item.transform.SetParent(ItemHolder.transform);
            item.GetComponent<Item>().InInventory = true;

            if (item.GetComponent<Rigidbody>()) item.GetComponent<Rigidbody>().isKinematic = true;
            if (item.GetComponent<Collider>()) item.GetComponent<Collider>().enabled = false;
            item.transform.localPosition = Vector3.zero;

            item.GetComponent<Item>().InventorySlot = storedItems.Count;
            SelectedInvSlot = item.GetComponent<Item>().InventorySlot;
        }
        else
        {
            DropItem(storedItems[SelectedInvSlot - 1].gameObject);
            PickUpItem(item);
        }
    }

    public void DropItem(GameObject item)
    {
        storedItems.Remove(item);

        item.transform.localPosition = new Vector3(0, 0, 1);

        item.transform.SetParent(null);
        item.GetComponent<Item>().InInventory = false;

        if (item.GetComponent<Rigidbody>()) item.GetComponent<Rigidbody>().isKinematic = false;
        if (item.GetComponent<Collider>()) item.GetComponent<Collider>().enabled = true;

        item.GetComponent<Item>().InventorySlot = 0;
    }

    

}


