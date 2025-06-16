using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public GameObject slotPrefab;
    public Inventory inventory; 


    // Start is called before the first frame update
    void Start()
    {
        RefreshInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void RefreshInventory()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in inventory.items)
        {
            GameObject slot = Instantiate(slotPrefab, transform);
            Image icon = slot.transform.Find("ItemIcon").GetComponent<Image>();
            icon.sprite = item.icon;
        }
    }
}
