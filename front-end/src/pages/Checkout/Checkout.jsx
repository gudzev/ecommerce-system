import "./Checkout.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";
import { CheckoutForm } from "./CheckoutForm";


import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

export function Checkout({setSearchText, cart, cartProducts, shipmentPrice, orderPrice, setCart, deliveryMethod, setDeliveryMethod, deliveryOptions})
{
    const [orderID, setOrderID] = useState(null);

    const navigate = useNavigate();

    useEffect(() =>
    {
        if(cart.length < 1 && orderID == null)
        {
            navigate("/");
        }
    }, [cart])

    return <>
        
        <title>Prodavnica - plaćanje</title>
        
        <Header setSearchText={setSearchText} cart={cart} />
        <section className="checkout">
            <div className="checkout-content">
                <CheckoutForm cartProducts={cartProducts}
                              orderPrice={orderPrice}
                              shipmentPrice={shipmentPrice}
                              cart={cart}
                              setCart={setCart}
                              deliveryMethod={deliveryMethod}
                              setDeliveryMethod={setDeliveryMethod}
                              deliveryOptions={deliveryOptions}
                              setOrderID={setOrderID}
                              orderID={orderID}
                />
                <div className={orderID == null ? "order-confirmation hidden" : "order-confirmation"}>
                    <h1>Porudžbina uspešno naručena. ID vaše porudžbine je {orderID}</h1>
                </div>
            </div>

        </section>
        <Footer />
    </>
}