[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public InventorySlot(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public bool IsFull()
    {
        return quantity >= item.maxStack;
    }

    public bool CanStack(Item newItem)
    {
        return item == newItem && !IsFull();
    }
}
