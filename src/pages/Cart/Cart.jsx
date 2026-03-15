import "./Cart.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";
import { CartCheckout } from "./CartCheckout";
import { CartPreview } from "./CartPreview";

export function Cart({setSearchText, cart, setCart, cartProducts, shipmentPrice, orderPrice})
{
    return <>

        <title>Prodavnica - korpa</title>

        <Header setSearchText={setSearchText} cart={cart}/>

        <section className="cart">
            <div className="cart-content">

                <h1 className="cart-header">Korpa</h1>

                <div className="cart-flex-container">
                    <CartPreview cartProducts={cartProducts} cart={cart} setCart={setCart}/>
                    <CartCheckout cart={cart} shipmentPrice={shipmentPrice} orderPrice={orderPrice}/>
                </div>

            </div>
        </section>

        <Footer />
    </>
}