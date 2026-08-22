import "./Cart.css";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash } from "@fortawesome/free-solid-svg-icons";

import { useState, useEffect, useContext } from "react";
import { useNavigate } from "react-router-dom";

import { formatPrice } from "../../utils/formatPrice";
import { viewProductDetails } from "../../utils/viewProductDetails";

import { CartContext } from "../../contexts/CartContext/CartContext";

export function CartItem({cartItem})
{
    const [quantity, setQuantity] = useState(cartItem.quantity);
    const {cart, setCart} = useContext(CartContext);
    const navigate = useNavigate();

    useEffect(() =>
    {
        const newCart = cart.map((_cartItem) =>
        {
            if(cartItem.id == _cartItem.productId)
            {
                return {
                    productId: _cartItem.productId,
                    quantity: quantity
                }
            }
            else
            {
                return _cartItem;
            }
        });
        setCart(newCart);
    }, [quantity])

    const removeItemFromCart = () =>
    {
        const newCart = cart.filter((cartProduct) => cartItem.id !== cartProduct.productId);
        setCart(newCart);
    }

    const handleQtyChange = (changeType) =>
    {
        if(changeType === "increase")
        {
            if(quantity < 10)
                setQuantity(prev => prev + 1);
        }
        else
        {
            if(quantity > 1)
                setQuantity(prev => prev - 1);
        }
    }

    return <tr className="cart-item">
                <td className="cart-item-product-cell">
                    <img src={cartItem.image_url} alt={cartItem.name + ' ' + "Image"} className="cart-item-img" onClick={() => viewProductDetails(navigate, cartItem.name)}/>
                    <h2 onClick={() => viewProductDetails(navigate, cartItem.name)}>{cartItem.name}</h2>
                </td>
                <td>
                    <div className="cart-item-qty-input">
                        <button className="decrease-qty-btn" onClick={() => handleQtyChange("decrease")}>-</button>
                        <span className="cart-item-qty">{quantity}</span>
                        <button className="increase-qty-btn" onClick={() => handleQtyChange("increase")}>+</button>
                    </div>
                </td>
                <td><h2>{formatPrice(cartItem.price_on_sale ? cartItem.price_on_sale * quantity : cartItem.price_rsd * quantity) + ' ' + "RSD"}</h2></td>
                <td><FontAwesomeIcon icon={faTrash} className="cart-item-delete-btn" onClick={() => removeItemFromCart()}/></td>
            </tr>
}