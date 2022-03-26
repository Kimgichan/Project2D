
using System.Collections.Generic;

public interface IController
{
    void OrderAction(params Order[] orders);
    void OrderAction(List<Order> orders);

    public struct Order
    {
        public OrderTitle orderTitle;
        public List<object> parameters;
    }

    public enum OrderTitle
    {
        Idle,
        Move,
        Attack,
        Follow,
        Avoiding,
        Dead
    }
}
