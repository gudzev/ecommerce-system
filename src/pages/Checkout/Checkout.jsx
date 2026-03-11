import "./Checkout.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";
import { CheckoutForm } from "./CheckoutForm";


import { useNavigate } from "react-router-dom";
import { useEffect } from "react";

export function Checkout({setSearchText, cart, cartProducts})
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
                <CheckoutForm cartProducts={cartProducts}/>
            </div>
        </section>
        <Footer />
    </>
}