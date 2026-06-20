import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";

import { useEffect, useState, useRef } from "react";

import { formatPrice } from "../../utils/formatPrice";

import { faShoppingCart } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import "./ProductDetails.css";

export function Product({allCategories, allProducts, cart, setCart, setSearchText})
{
    const [thisProduct, setThisProduct] = useState(null);
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
                setThisProduct(product);
            }
        });
    }, [allProducts, allCategories]);

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
        const foundItem = newCart.find((cartItem) => cartItem.productId == thisProduct.id);
        console.log(foundItem);

        if(foundItem)
        {
            console.log(newCart)
            foundItem.quantity++;
            console.log(newCart);
        }
        else
        {
            newCart.push(
                {
                    productId: thisProduct.id,
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
                        <img src={thisProduct?.image_url} alt={thisProduct?.name + " slika"} className="product-img" />
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
                    </div>
                </main>

            <Footer />
        </>
    )
}