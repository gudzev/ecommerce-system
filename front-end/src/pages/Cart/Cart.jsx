import "./Cart.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";
import { CartCheckout } from "./CartCheckout";
import { CartPreview } from "./CartPreview";

export default function Cart({cartProducts, allCategories})
{

    return <>

        <title>Prodavnica - korpa</title>

        <Header allCategories={allCategories}/>

        <section className="cart">
            <div className="cart-content">

                <h1 className="cart-heading">Korpa</h1>

                <div className="cart-flex-container">
                    <CartPreview cartProducts={cartProducts}/>
                    <CartCheckout/>
                </div>

            </div>
        </section>

        <Footer />
    </>
}