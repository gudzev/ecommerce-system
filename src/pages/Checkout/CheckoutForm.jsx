import "./Checkout.css";

import { CheckoutSummary } from "./CheckoutSummary";

import { useState } from "react";

import axios from "axios";

export function CheckoutForm({cartProducts, shipmentPrice, orderPrice, cart, setCart, deliveryMethod, setDeliveryMethod})
{
    const [email, setEmail] = useState("");
    const [name, setName] = useState("");
    const [surname, setSurname] = useState("");
    const [street, setStreet] = useState("");
    const [apartmentNumber, setApartmentNumber] = useState("");
    const [city, setCity] = useState("");
    const [additional, setAdditional] = useState("");
    const [phoneNumber, setPhoneNumber] = useState("");

    const [displayError, setDisplayError] = useState(false);

    const validateEmail = (email) =>
    {
        const regex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return regex.test(String(email).toLowerCase());
    }

    const validatePhoneNumber = (phoneNumber) => 
    {
        const regex = /^(1\s?)?(\(\d{3}\)|\d{3})[\s\-]?\d{3}[\s\-]?\d{4}$/;
        return regex.test(phoneNumber);
    }

    const validateText = (text) =>
    {
        return (text != "") ? true : false;
    }

    const makeAnOrder = async (cart) =>
    {
        if(!(
           validateEmail(email) &&
           validatePhoneNumber(phoneNumber) &&
           validateText(name) &&
           validateText(surname) &&
           validateText(city) &&
           validateText(apartmentNumber) &&
           validateText(street)
        ))
        {
            setDisplayError(true);
            return;
        }

        setDisplayError(false);

        try
        {
            const request = await axios.post("https://jsonplaceholder.typicode.com/posts",
                {
                    orderId: crypto.randomUUID(),
                    email: email,
                    name: name,
                    surname: surname,
                    street: street,
                    apartmentNumber: apartmentNumber,
                    city: city,
                    additional: additional,
                    phoneNumber: phoneNumber,
                    deliveryMethod: deliveryMethod,
                    cart: cart
                }
            )
            console.log(request);
        }
        catch(error)
        {
            console.log(error);
        }
    }

    const handleFormSubmit = (e) =>
    {
        e.preventDefault();
        makeAnOrder(cart);
    }

    return <form method="post" action="" className="checkout-form" onSubmit={handleFormSubmit}>
                <div className="checkout-column">
                    <div className="checkout-row">
                        <h1 className="checkout-heading">Adresa za dostavu</h1>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="" className="checkout-label">E-mail adresa</label>
                        <input type="email" className="checkout-input" required onChange={(e) => setEmail(e.target.value)}/>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="" className="checkout-label">Ime</label>
                        <input type="text" className="checkout-input" required onChange={(e) => setName(e.target.value)}/>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="" className="checkout-label">Prezime</label>
                        <input type="text" className="checkout-input" required onChange={(e) => setSurname(e.target.value)}/>
                    </div>

                    <div className={deliveryMethod == 0 ? "checkout-row hidden" : "checkout-row"}>
                        <label htmlFor="" className="checkout-label">Naziv ulice</label>
                        <input type="text" className="checkout-input" required onChange={(e) => setStreet(e.target.value)}/>
                    </div>

                    <div className={deliveryMethod == 0 ? "checkout-row hidden" : "checkout-row"}>
                        <label htmlFor="" className="checkout-label">Broj kuce, zgrade, stana</label>
                        <input type="text" className="checkout-input" required onChange={(e) => setApartmentNumber(e.target.value)}/>
                    </div>

                    <div className={deliveryMethod == 0 ? "checkout-row hidden" : "checkout-row"}>
                        <label htmlFor="" className="checkout-label">Grad</label>
                        <input list="cities" className="checkout-input" required onChange={(e) => setCity(e.target.value)}/>

                        <datalist id="cities">
                            <option value="Beograd"></option>
                            <option value="Novi Sad"></option>
                            <option value="Niš"></option>
                            <option value="Kragujevac"></option>
                            <option value="Subotica"></option>
                            <option value="Zrenjanin"></option>
                            <option value="Pančevo"></option>
                            <option value="Čačak"></option>
                            <option value="Kraljevo"></option>
                            <option value="Smederevo"></option>
                            <option value="Leskovac"></option>
                            <option value="Valjevo"></option>
                            <option value="Kruševac"></option>
                            <option value="Šabac"></option>
                            <option value="Užice"></option>
                            <option value="Vranje"></option>
                            <option value="Sombor"></option>
                            <option value="Sremska Mitrovica"></option>
                            <option value="Požarevac"></option>
                            <option value="Jagodina"></option>
                            <option value="Loznica"></option>
                            <option value="Bor"></option>
                            <option value="Zaječar"></option>
                            <option value="Pirot"></option>
                            <option value="Prokuplje"></option>
                            <option value="Novi Pazar"></option>
                            <option value="Prijepolje"></option>
                            <option value="Priboj"></option>
                            <option value="Aranđelovac"></option>
                            <option value="Gornji Milanovac"></option>
                            <option value="Paraćin"></option>
                            <option value="Ćuprija"></option>
                            <option value="Smederevska Palanka"></option>
                            <option value="Vrbas"></option>
                            <option value="Bečej"></option>
                            <option value="Kikinda"></option>
                            <option value="Bačka Palanka"></option>
                            <option value="Ruma"></option>
                            <option value="Inđija"></option>
                            <option value="Stara Pazova"></option>
                            <option value="Šid"></option>
                            <option value="Temerin"></option>
                            <option value="Apatin"></option>
                            <option value="Kula"></option>
                            <option value="Kanjiža"></option>
                            <option value="Ada"></option>
                            <option value="Senta"></option>
                            <option value="Titel"></option>
                        </datalist>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="" className="checkout-label">Napomena</label>
                        <textarea className="checkout-textarea" onChange={(e) => setAdditional(e.target.value)}/>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="" className="checkout-label">Broj telefona</label>
                        <input type="text" className="checkout-input" required onChange={(e) => setPhoneNumber(e.target.value)}/>
                    </div>
                </div>
                <div className="checkout-column">
                    <div className="checkout-row">
                        <h2 className="checkout-heading-h2">Metod plaćanja</h2>
                        <div className="delivery-options-row">
                            <input type="radio" name="delivery-options" checked={deliveryMethod == 1 ? true : false} onChange={(e) => { if(e.target.value == "on") setDeliveryMethod(1)}}></input>
                            <label htmlFor="" className="checkout-label">Plaćanje pouzećem</label>
                        </div>

                        <div className="delivery-options-row">
                            <input type="radio" name="delivery-options" checked={deliveryMethod == 0 ? true : false} onChange={(e) => { if(e.target.value == "on") setDeliveryMethod(0)}}></input>
                            <label htmlFor="" className="checkout-label">Lično preuzimanje u radnji</label>
                        </div>
                    </div>

                    <div className="checkout-row">
                        <CheckoutSummary cartProducts={cartProducts} orderPrice={orderPrice} shipmentPrice={shipmentPrice} cart={cart} setCart={setCart} />
                    </div>

                    <div className="checkout-row">
                        <button type="submit" className="checkout-btn">Naruči</button>
                    </div>

                    <div className="checkout-row">
                        <p className={displayError ? "checkout-error" : "checkout-error hidden"}>Greška prilikom kreiranja narudžbine! Molimo Vas pregledajte unete podatke.</p>
                    </div>
                </div>
            </form>
    
}