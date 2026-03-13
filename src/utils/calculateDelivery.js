// deliveryMethod = 0 => personal pick up
// deliveryMethod = 1 => delivery on home address

// import axios from "axios";

export const calculateDelivery = (orderPrice, quantity, deliveryMethod) =>
{
    // const response = await axios.get("/deliveryOptions.json");
    // const deliveryOptions = response.data.deliveryOptions;

    if(deliveryMethod == 0)
    {
        return 0;
    }
    else if(deliveryMethod == 1)
    {
        if(orderPrice > 75000)
        {
            return 0;
        }
        else
        {
            return quantity * 400;
        }
    }
    else
    {
        return null;
    }
}