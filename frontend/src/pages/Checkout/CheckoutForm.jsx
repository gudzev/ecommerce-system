import "./Checkout.css";

import { CheckoutSummary } from "./CheckoutSummary";

import { useState } from "react";

import axios from "axios";

export function CheckoutForm({cartProducts, shipmentPrice, orderPrice, cart, setCart, deliveryMethod, setDeliveryMethod, deliveryOptions})
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

    const selectedOption = deliveryOptions?.find((option) => option.id == deliveryMethod);

    const validateEmail = (email) =>
    {
        const regex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return regex.test(String(email).toLowerCase());
    }

    const validatePhoneNumber = (phoneNumber) => 
    {
        // eslint-disable-next-line no-useless-escape
        const regex = /^(1\s?)?(\(\d{3}\)|\d{3})[\s\-]?\d{3}[\s\-]?\d{4}$/;
        return regex.test(phoneNumber);
    }

    const validateText = (text) =>
    {
        return (text != "") ? true : false;
    }

    const makeAnOrder = async (cart) =>
    {
        if(selectedOption?.name == "Lično preuzimanje u radnji")
        {
            if(!(
            validateEmail(email) &&
            validatePhoneNumber(phoneNumber) &&
            validateText(name) &&
            validateText(surname)
            ))
            {
                setDisplayError(true);
                return;
            }
        }
        else
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
        }

        setDisplayError(false);

        try
        {
            const order =
            {
                email: email,
                name: name,
                surname: surname,
                street: street,
                apartment_number: apartmentNumber,
                city: city,
                additional: additional,
                phone_number: phoneNumber,
                delivery_method_id: deliveryMethod,
                orderItems: cart
            }
            const request = await axios.post("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/orders", order);
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
                        <label htmlFor="email" className="checkout-label">E-mail adresa <span className="mandatory-field">*</span></label>
                        <input type="email" id="email" className="checkout-input" required onChange={(e) => setEmail(e.target.value)}/>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="name" className="checkout-label">Ime <span className="mandatory-field">*</span></label>
                        <input type="text" id="name" className="checkout-input" required onChange={(e) => setName(e.target.value)}/>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="surname" className="checkout-label">Prezime <span className="mandatory-field">*</span></label>
                        <input type="text" id="surname" className="checkout-input" required onChange={(e) => setSurname(e.target.value)}/>
                    </div>

                    <div className={selectedOption?.name == "Plaćanje pouzećem" ? "checkout-row" : "checkout-row hidden"}>
                        <label htmlFor="street" className="checkout-label">Naziv ulice <span className="mandatory-field">*</span></label>
                        <input type="text" id="street" className="checkout-input" onChange={(e) => setStreet(e.target.value)}/>
                    </div>

                    <div className={selectedOption?.name == "Plaćanje pouzećem" ? "checkout-row" : "checkout-row hidden"}>
                        <label htmlFor="apartmentNumber" className="checkout-label">Broj kuce, zgrade, stana <span className="mandatory-field">*</span></label>
                        <input type="text" id="apartmentNumber" className="checkout-input" onChange={(e) => setApartmentNumber(e.target.value)}/>
                    </div>

                    <div className={selectedOption?.name == "Plaćanje pouzećem" ? "checkout-row" : "checkout-row  hidden"}>
                        <label htmlFor="city" className="checkout-label">Grad <span className="mandatory-field">*</span></label>
                        <input list="cities" id="city" className="checkout-input" onChange={(e) => setCity(e.target.value)}/>

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
                        <label htmlFor="additional" className="checkout-label">Napomena</label>
                        <textarea className="checkout-textarea" id="additional" onChange={(e) => setAdditional(e.target.value)}/>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="phoneNumber" className="checkout-label">Broj telefona <span className="mandatory-field">*</span></label>
                        <input type="text" id="phoneNumber" className="checkout-input" required onChange={(e) => setPhoneNumber(e.target.value)}/>
                    </div>
                </div>
                <div className="checkout-column">
                    <div className="checkout-row">
                        <h2 className="checkout-heading-h2">Metod plaćanja</h2>
                        {
                            deliveryOptions?.map((option) =>
                            {
                                return <div className="delivery-options-row" key={option.id}>
                                            <input type="radio" id={"deliveryMethod" + option.id} name="delivery-options" checked={deliveryMethod == option.id ? true : false} onChange={() => setDeliveryMethod(option.id)}></input>
                                            <label htmlFor={"deliveryMethod" + option.id} className="checkout-label">{option.name}</label>
                                        </div>
                            })
                        }
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