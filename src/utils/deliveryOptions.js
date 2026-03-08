export const calculateDelivery = (orderPrice, quantity) =>
{
    if(orderPrice > 50000)
    {
        return 0;
    }
    else
    {
        return quantity * 200;
    }
}