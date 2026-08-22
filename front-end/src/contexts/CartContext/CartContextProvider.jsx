import { useState } from "react";

import { CartContext } from "./CartContext";

export function CartContextProvider({children})
{
    const [cart, setCart] = useState(() =>
    {
      return JSON.parse(localStorage.getItem("cart")) || [];
    });

  return <CartContext value={{cart, setCart}}>
    {children}
  </CartContext>
}