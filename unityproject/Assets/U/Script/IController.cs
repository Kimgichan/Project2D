
using System.Collections.Generic;

public interface IController
{
    void OrderAction(params object[] orders);

    public class Order 
    {
        public string orderTitle;
        public List<object> parameters;
    }
}
