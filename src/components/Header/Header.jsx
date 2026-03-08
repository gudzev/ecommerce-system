import "./Header.css";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars, faMagnifyingGlass, faCartShopping, faHouse, faSuitcase, faPhoneVolume, faHeart } from "@fortawesome/free-solid-svg-icons";

import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";

export function Header({setSearchText, cart})
{
    const [inputText, setInputText] = useState("");

    const navigate = useNavigate();

    const searchProducts = (inputText) =>
    {
        setSearchText(inputText);

        if(window.location.pathname == "/cart")
        {
            navigate("/");
        }
    }

    const listenForEnterKeyPress = (event) =>
    {
        if(event.key == "Enter")
        {
            searchProducts(event.target.value);
        }
    }

    const calculateCartItemNumber = () =>
    {
        let num = 0;
        cart?.forEach((item) =>
        {
            num += Number(item.quantity);
        });
        return num;
    }

    return <header>
        <div className="header-main">
            <Link className="header-img-wrapper" to="/">
                <img src="/logo.png" className="header-img" alt="Prodavnica logo"/>
            </Link>

            <button className="header-categories">
                <FontAwesomeIcon icon={faBars} className="fa-icon-2x" /> Sve kategorije
            </button>

            <div className="header-search-container">
                <input type="text" className="header-search" placeholder="Pronađite proizvod" onChange={(event) => setInputText(event.target.value)} onKeyDown={listenForEnterKeyPress} />
                <button className="header-search-btn" onClick={() => searchProducts(inputText)}>
                    <FontAwesomeIcon icon={faMagnifyingGlass} className="fa-icon-1x" />
                </button>
            </div>

            <div className="header-additional">
                <button className="header-wishlist">
                    <FontAwesomeIcon icon={faHeart} className="fa-icon-2x" />
                    Lista zelja
                </button>

                <Link className="header-cart" to="/cart">
                    <FontAwesomeIcon icon={faCartShopping} className="fa-icon-2x" />
                    Korpa
                    
                    <span className="cart-counter">{calculateCartItemNumber()}</span>
                </Link>
            </div>
        </div>

        <div className="header-links">
            <div className="header-pages">
                <ul className="ul-links">
                    <li><Link to="/"><FontAwesomeIcon icon={faHouse} className="fa-icon-1x" /></Link></li>
                    <li><a href="#">Grafičke karte</a></li>
                    <li><a href="#">Procesori</a></li>
                    <li><a href="#">Matične ploče</a></li>
                    <li><a href="#">Memorija</a></li>
                    <li><a href="#">SSD</a></li>
                    <li><a href="#">HDD</a></li>
                    <li><a href="#">Napajanja</a></li>
                    <li><a href="#">Kućišta</a></li>
                </ul>

                <ul className="ul-contact">
                    <li><a href="#"><FontAwesomeIcon icon={faSuitcase} className="fa-textright" />Pravna lica</a></li>
                    <li><a href="#"><FontAwesomeIcon icon={faPhoneVolume} className="fa-textright" />Podrška za potrošače</a></li>
                </ul>
            </div>
        </div>

    </header>
}