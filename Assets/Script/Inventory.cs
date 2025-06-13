using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int capacity = 20;

    public bool Add(Item item)
    {
        if (items.Count >= capacity)
        {
            Debug.Log("Inventory full");
            return false;
        }

        items.Add(item);
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
}
