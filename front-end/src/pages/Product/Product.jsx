import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";

import { useEffect, useState, useRef } from "react";

import { formatPrice } from "../../utils/formatPrice";
import {translateToSerbian} from "../../utils/translateToSerbian";

import { faShoppingCart } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import axios from 'axios';

import "./ProductDetails.css";

export default function Product({allCategories, allProducts, cart, setCart, setSearchText})
{
    const [thisProduct, setThisProduct] = useState(null);
    const [thisProductDetails, setThisProductDetails] = useState(null);
    const [thisProductID, setThisProductID] = useState(null);
    const [addedText, setAddedText] = useState(false);

    const timeoutID = useRef(null);

    useEffect(() =>
    {
        const encodedProductName = window.location.pathname;
        const productName = encodedProductName.substring(10, window.location.pathname.length).replaceAll('-', ' ');

        allProducts?.forEach((product) =>
        {
            if(product.name.toLowerCase().replaceAll('-', ' ') == productName)
            {
                setThisProductID(product.id);
            }
        });
    }, [allProducts, allCategories]);

    useEffect(() =>
    {
        if(!thisProductID)
        {
            return;
        }

        const getThisProduct = async () =>
        {
            const request = await axios.get("https://localhost:7097/products/" + thisProductID);
            const dbProduct = request.data;
            setThisProduct(dbProduct);
            setThisProductDetails(dbProduct.details);
        }
        getThisProduct();
    }, [thisProductID]);

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

    const addToCart = () =>
    {
        let newCart = [...cart];
        const foundItem = newCart.find((cartItem) => cartItem.productId == thisProductID);

        if(foundItem)
        {
            foundItem.quantity++;
        }
        else
        {
            newCart.push(
                {
                    productId: thisProductID,
                    quantity: 1
                })
        }

        setCart(newCart);
        setAddedText(true);
    }

    return (
        <>
            <title>Prodavnica - {thisProduct?.name}</title>

            <Header allCategories={allCategories} cart={cart} setSearchText={setSearchText}/>

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

                            <button className="product-container-add-to-cart-btn" disabled={addedText} onClick={() => addToCart()}><span className="center-items"><FontAwesomeIcon icon={faShoppingCart} className="fa-icon-1x"/>Dodaj u korpu</span></button>
                            <p className="added-to-cart">{addedText ? ("Artikal je uspešno dodat u korpu.") : ""}</p>
                            <hr></hr>
                            <p className="product-container-article-description">{thisProduct?.description || "Nema opisa za ovaj proizvod."}</p>

                        </div>
                        <div className="product-detailed-specifications">
                            {
                                (thisProductDetails)
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
                                            Object.entries(thisProductDetails).map((productDetail, index) =>
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