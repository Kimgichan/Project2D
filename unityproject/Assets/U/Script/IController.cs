
using System.Collections.Generic;

public interface IController
{
    void OrderAction(params Order[] orders);
    void OrderAction(List<Order> orders);
}

public class Order
{
    public string orderTitle;
    public List<object> parameters;
}
