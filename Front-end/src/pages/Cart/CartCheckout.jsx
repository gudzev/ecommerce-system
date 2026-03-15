import "./Cart.css";
import { useNavigate } from "react-router-dom";

import { formatPrice } from "../../utils/formatPrice";

export function CartCheckout({cart, orderPrice, shipmentPrice})
{
    const navigate = useNavigate();

    const proceedToCheckout = () =>
    {
        if(cart.length > 0)
        {
            navigate("/checkout");
        }
    }

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
            <button className="checkout-btn" onClick={proceedToCheckout}>
                Nastavi ka plaćanju
            </button>
        </div>
    </div>
}