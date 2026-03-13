import "./Checkout.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";
import { CheckoutForm } from "./CheckoutForm";


import { useNavigate } from "react-router-dom";
import { useEffect } from "react";

export function Checkout({setSearchText, cart, cartProducts, shipmentPrice, orderPrice, setCart, deliveryMethod, setDeliveryMethod})
{
    const navigate = useNavigate();

    useEffect(() =>
    {
        if(cart.length < 1)
        {
            navigate("/");
        }
    }, [cart])

    return <>
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
                />
            </div>
        </section>
        <Footer />
    </>
}