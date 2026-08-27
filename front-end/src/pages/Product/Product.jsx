import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";

import { useEffect, useState, useRef, useContext } from "react";

import { formatPrice } from "../../utils/formatPrice";
import { translateToSerbian } from "../../utils/translateToSerbian";
import { API_URL } from "../../App";

import { CartContext } from "../../contexts/CartContext/CartContext";

import { faShoppingCart } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import { useLocation } from "react-router-dom";

import axios from 'axios';

import "./ProductDetails.css";

export default function Product()
{
    const location = useLocation();

    const [thisProduct, setThisProduct] = useState(location.state);
    const [addedText, setAddedText] = useState(false);

    const { addToCart } = useContext(CartContext);

    const timeoutID = useRef(null);

    useEffect(() =>
    {
        if(!thisProduct.id)
        {
            return;
        }

        const getThisProduct = async () =>
        {
            const request = await axios.get(API_URL + "/products/" + thisProduct.id);
            const dbProduct = request.data;
            setThisProduct(dbProduct);
        }
        getThisProduct();

    }, [thisProduct.id]);

    useEffect(() =>
    {
        if(!addedText)
        {
            return;
        }

        timeoutID.current = setTimeout(() =>
        {
            setAddedText(false);
        }, 1500);
    }, [addedText]);

    const handleAddToCart = () =>
    {
        addToCart(thisProduct.id, 1);
        setAddedText(true);
    }

    return (
        <>
            <title>Prodavnica - {thisProduct?.name}</title>

            <Header/>

                <main className="product-container">
                    <div className="product-container-details">
                        <img src={thisProduct?.image_url} alt={thisProduct?.name + " slika"} />
                        <div className="product-container-data">
                            <p className="product-container-article-id">Šifra artikla: {thisProduct?.id}</p>
                            <h1>{thisProduct?.name}</h1>
                            <h2>
                                Cena: {(!thisProduct?.price_on_sale) 
                                ? 
                                <span className="price-regular">{formatPrice(thisProduct?.price_rsd) + " RSD"}</span>
                                : 
                                <><span className="price-old">{formatPrice(thisProduct?.price_rsd) + " RSD"}</span><span className="price-new">{formatPrice(thisProduct?.price_on_sale) + " RSD"}</span></>}
                            </h2>

                            <button className="product-container-add-to-cart-btn" disabled={addedText} onClick={() => handleAddToCart()}><span className="center-items"><FontAwesomeIcon icon={faShoppingCart} className="fa-icon-1x"/>Dodaj u korpu</span></button>
                            <p className="added-to-cart">{addedText ? ("Artikal je uspešno dodat u korpu.") : ""}</p>
                            <hr></hr>
                            <p className="product-container-article-description">{thisProduct?.description || "Nema opisa za ovaj proizvod."}</p>

                        </div>
                        <div className="product-detailed-specifications">
                            {
                                (thisProduct.details)
                                ?
                                <table>
                                    <thead>
                                        <tr>
                                            <th className="product-specification-header">Specifikacija</th>
                                            <th className="product-specification-header">Vrednost</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {
                                            Object.entries(thisProduct.details).map((productDetail, index) =>
                                            {
                                                const [key, value] = productDetail;
                                                return (
                                                    <tr key={index}>
                                                        <td className="product-specification-name">{translateToSerbian(key)}</td>
                                                        <td className="product-specification-value">{value}</td>
                                                    </tr>
                                                )
                                            })
                                        }
                                    </tbody>
                                </table>
                                :
                                <h3 className="product-specification-no-results">Nema detaljnih specifikacija za ovaj proizvod.</h3>
                            }    
                        </div>
                    </div>
                </main>

            <Footer />
        </>
    )
}