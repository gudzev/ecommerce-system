import "./Cart.css";

import { useEffect, useState } from "react";

import { calculateDelivery } from "../../utils/deliveryOptions";
import { formatPrice } from "../../utils/formatPrice";

export function CartCheckout({cartProducts})
{
    const [orderPrice, setOrderPrice] = useState(0);
    const [shipmentPrice, setShipmentPrice] = useState(0);

    useEffect(() =>
    {
        let price = 0;

        const calculateProductsTotal = () =>
        {
            cartProducts.forEach((cartProduct) =>
            {
                price = price + (cartProduct.price_rsd * cartProduct.quantity);
            })
            setOrderPrice(price);
        }

        const calculateDeliveryTotal = () =>
        {
            let itemQuantity = 0;
            cartProducts.forEach((product) => itemQuantity += product.quantity)

            setShipmentPrice(calculateDelivery(price, itemQuantity));
        }

        calculateProductsTotal();
        calculateDeliveryTotal();
    }, [cartProducts]);

    return <div className="cart-checkout">
        <div className="cart-checkout-heading">
            <h1>Pregled porudžbine</h1>
        </div>

        <div className="cart-checkout-summary">
            <div className="cart-checkout-summary-row">
                <h2>Cena proizvoda: </h2>
                <h2>{formatPrice(orderPrice) + ' ' + "RSD"}</h2>
            </div>
            <div className="cart-checkout-summary-row">
                <h2>Isporuka: </h2>
                <h2>{formatPrice(shipmentPrice) + ' ' + "RSD"}</h2>
            </div>
            <div className="cart-checkout-summary-row">
                <h2>Ukupno: </h2>
                <h2>{formatPrice(orderPrice + shipmentPrice) + ' ' + "RSD"}</h2>
            </div>
        </div>

        <div className="cart-checkout-summary-row">
            <button className="checkout-btn">
                Nastavi ka plaćanju
            </button>
        </div>
    </div>
}