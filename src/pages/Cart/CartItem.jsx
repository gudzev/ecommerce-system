import "./Cart.css";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash } from "@fortawesome/free-solid-svg-icons";

import { formatPrice } from "../../utils/formatPrice";
import { useState } from "react";

export function CartItem({cartItem, cart, setCart})
{
    const [quantity, setQuantity] = useState(cartItem.quantity);

    const removeItemFromCart = (itemID) =>
    {
        const newCart = cart.filter((cartProduct) => itemID !== cartProduct.productId);
        setCart(newCart);
    }

    const handleQtyInput = async (event) =>
    {
        const quantityInputValue = Number(event.target.value);

        if(quantityInputValue <= 0 || quantityInputValue > 10)
        {
            return;
        }

        const newCart = [...cart];
        const existingCartItem = newCart.find((cartProduct) => cartItem.id == cartProduct.productId);
        existingCartItem.quantity = quantityInputValue;

        setQuantity(Number(event.target.value));
        setCart(newCart);
    }

    return <div className="cart-item">
        <img src={cartItem.image_url} alt={cartItem.name + ' ' + "Image"} className="cart-item-img"/>
        <h2 className="cart-item-name">{cartItem.name}</h2>
        <div className="cart-item-qty">
            <input type="number" min="1" max="10" step="1" onChange={handleQtyInput} defaultValue={quantity} className="cart-item-qty-input"/>
        </div>
        <p className="cart-item-price">
            {formatPrice(cartItem.price_rsd * quantity) + ' ' + "RSD"}
        </p>
        <FontAwesomeIcon icon={faTrash} className="cart-item-delete-btn" onClick={() => removeItemFromCart(cartItem.id)}/>
    </div>
}