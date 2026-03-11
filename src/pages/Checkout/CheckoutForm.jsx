import "./Checkout.css";

import { CheckoutSummary } from "./CheckoutSummary";

export function CheckoutForm({cartProducts})
{
    return <form method="post" action="" className="checkout-form">
                <div className="checkout-column">
                    <div className="checkout-row">
                        <h1 className="checkout-heading">Adresa za dostavu</h1>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="" className="checkout-label">E-mail adresa</label>
                        <input type="email" className="checkout-input" required/>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="" className="checkout-label">Ime</label>
                        <input type="text" className="checkout-input" required/>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="" className="checkout-label">Prezime</label>
                        <input type="text" className="checkout-input" required/>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="" className="checkout-label">Naziv ulice</label>
                        <input type="text" className="checkout-input" required/>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="" className="checkout-label">Broj kuce, zgrade, stana</label>
                        <input type="text" className="checkout-input" required/>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="" className="checkout-label">Grad</label>
                        <input list="cities" className="checkout-input" required/>

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
                        <textarea className="checkout-textarea"/>
                    </div>

                    <div className="checkout-row">
                        <label htmlFor="" className="checkout-label">Broj telefona</label>
                        <input type="text" className="checkout-input" required/>
                    </div>
                </div>
                <div className="checkout-column">
                    <div className="checkout-row">
                        <h2 className="checkout-heading-h2">Metod plaćanja</h2>
                        <div className="delivery-options-row">
                            <input type="radio" name="delivery-options"></input>
                            <label htmlFor="" className="checkout-label" defaultChecked>Plaćanje pouzećem</label>
                        </div>

                        <div className="delivery-options-row">
                            <input type="radio" name="delivery-options"></input>
                            <label htmlFor="" className="checkout-label">Lično preuzimanje</label>
                        </div>
                    </div>

                    <div className="checkout-row">
                        <CheckoutSummary cartProducts={cartProducts} />
                    </div>

                    <div className="checkout-row">
                        <button type="submit" className="checkout-btn">Naruči</button>
                    </div>
                </div>
            </form>
    
}