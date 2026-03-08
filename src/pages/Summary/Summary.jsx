import "./Summary.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";

export function Summary({setSearchText, cart})
{
    return <>
        <Header setSearchText={setSearchText} cart={cart} />
        <section className="summary">
            <div className="summary-content">

            </div>
        </section>
        <Footer />
    </>
}