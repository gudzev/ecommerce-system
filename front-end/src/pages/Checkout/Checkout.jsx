import "./Checkout.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";
import { CheckoutForm } from "./CheckoutForm";

import { CartContext } from "../../contexts/CartContext/CartContext";

import { useNavigate } from "react-router-dom";
import { useEffect, useState, useContext } from "react";

export default function Checkout({cartProducts, allDeliveryOptions, allCategories})
{
    const [orderID, setOrderID] = useState(null);
    const {cart} = useContext(CartContext);
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
        
        <Header allCategories={allCategories} />
        <section className="checkout">
            <div className="checkout-content">
                <CheckoutForm cartProducts={cartProducts}
                              allDeliveryOptions={allDeliveryOptions}
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