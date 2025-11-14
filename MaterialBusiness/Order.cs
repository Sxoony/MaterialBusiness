using MaterialBusiness;

public class Order
{
    public string Id { get; private set; }
    public DateTime Created { get; private set; }
    public List<OrderLine> Lines { get; private set; }

    public Order()
    {
        Id = Guid.NewGuid().ToString();
        Created = DateTime.Now;
        Lines = new List<OrderLine>();
    }

    public void AddItem(Fabric item, decimal quantity)
    {
        Lines.Add(new OrderLine(item, quantity));
    }
}

public class OrderLine
{
    public Fabric Item { get; private set; }
    public decimal Quantity { get; private set; }

    public OrderLine(Fabric item, decimal quantity)
    {
        Item = item;
        Quantity = quantity;
    }
}
