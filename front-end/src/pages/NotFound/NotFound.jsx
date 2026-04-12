import "./NotFound.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";

import { Link } from "react-router-dom";

export function NotFound({allCategories})
{
    return <>

        <title>Prodavnica - Nije Pronađeno</title>

        <Header allCategories={allCategories}/>

        <section className="not-found">
            <div className="not-found-content">
                <h1>Error 404</h1>
                <h2>Requested page not found.</h2>
                <Link to="/" className="home-link">Go Back</Link>
            </div>
        </section>

        <Footer />
    </>
}