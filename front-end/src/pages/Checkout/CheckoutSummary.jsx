import "./Checkout.css";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash } from "@fortawesome/free-solid-svg-icons";

import { formatPrice } from "../../utils/formatPrice";

import { useNavigate } from "react-router-dom";
import { useContext } from "react";

import { viewProductDetails } from "../../utils/viewProductDetails";

import { CartContext } from "../../contexts/CartContext/CartContext";
import { CheckoutContext } from "../../contexts/CheckoutContext/CheckoutContext";

export function CheckoutSummary({cartProducts})
{
    const navigate = useNavigate();
    const {removeFromCart} = useContext(CartContext);
    const {shipmentPrice, orderPrice} = useContext(CheckoutContext);


    return <div className="checkout-summary">
        <h2 className="checkout-heading-h2 overview-heading">Pregled kupovine</h2>

        <div className="checkout-items-overview">
            {
                cartProducts.map((cartProduct) => 
                {
                    return <div className="checkout-items-item" key={cartProduct.id}>
                            <img src={cartProduct.image_url} alt={cartProduct.name + ' ' + "Image"} className="checkout-item-img" onClick={() => viewProductDetails(navigate, cartProduct.name, cartProduct.id)}/>
                            
                            <div className="checkout-items-details">
                                <h2 className="checkout-item-name" onClick={() => viewProductDetails(navigate, cartProduct.name, cartProduct.id)}>{cartProduct.name}</h2>
                                <h3 className="checkout-item-quantity">Količina: {cartProduct.quantity}</h3>
                            </div>

                            <span className="checkout-item-price">{formatPrice((cartProduct.price_on_sale || cartProduct.price_rsd) * cartProduct.quantity) + ' ' + "RSD"}</span>

                            <FontAwesomeIcon icon={faTrash} className="checkout-delete-item-btn fa-icon-2x" onClick={() => removeFromCart(cartProduct.id)}/>
                    </div>
                })
            }
        </div>

        <div className="payment-summary">
            <div className="payment-summary-row">
                <span>Korpa: </span>
                <span>{formatPrice(orderPrice)  + ' ' + "RSD"}</span>
            </div>
            <div className="payment-summary-row">
                <span>Isporuka: </span>
                <span>{formatPrice(shipmentPrice)  + ' ' + "RSD"}</span>
            </div>
            <div className="payment-summary-row">
                <span>Ukupno: </span>
                <span>{formatPrice(orderPrice + shipmentPrice) + ' ' + "RSD"}</span>
            </div>
        </div>
    </div>
}