import { useState } from "react";

import { CartContext } from "./CartContext";

export function CartContextProvider({children})
{
    const [cart, setCart] = useState(() =>
    {
      return JSON.parse(localStorage.getItem("cart")) || [];
    });

    const addToCart = (productId, quantity) =>
    {
        const newCart = [...cart];
        const existingItem = newCart.find((cartItem) => cartItem.productId == productId);
        
        if(!existingItem)
        {
            newCart.push(
                {
                    productId: productId,
                    quantity: quantity
                }
            )
        }
        else
        {
            existingItem.quantity += quantity;
        }

        setCart(newCart);
    }

    const removeFromCart = (id) =>
    {
        const newCart = cart.filter((cartProduct) => id !== cartProduct.productId);
        setCart(newCart);
    }

  return <CartContext value={{cart, setCart, addToCart, removeFromCart}}>
    {children}
  </CartContext>
}