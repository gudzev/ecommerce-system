import { useState } from "react";

import { CheckoutContext } from "./CheckoutContext";

export function CheckoutContextProvider({children})
{
  const [orderPrice, setOrderPrice] = useState(0);
  const [shipmentPrice, setShipmentPrice] = useState(0);
  const [deliveryMethod, setDeliveryMethod] = useState(1);

  return <CheckoutContext value={{orderPrice, setOrderPrice, shipmentPrice, setShipmentPrice, deliveryMethod, setDeliveryMethod}}>
    {children}
  </CheckoutContext>
}