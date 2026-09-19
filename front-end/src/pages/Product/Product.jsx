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

export default function Product({allCategories})
{
    const location = useLocation();

    const [thisProduct, setThisProduct] = useState(location.state);
    const [isTextAdded, setIsTextAdded] = useState(thisProduct?.stock_quantity > 0);
    const [stockQuantity, setStockQuantity] = useState(thisProduct?.stock_quantity);
    const [messageForBuyer, setMessageForBuyer] = useState("");
    const [activeImage, setActiveImage] = useState(null);

    const { addToCart, cart } = useContext(CartContext);

    const timeoutID = useRef(null);

    useEffect(() =>
    {
        const getThisProduct = async () =>
        {
            const request = await axios.get(API_URL + "/products/" + location.pathname.slice(10, location.pathname.length));
            const dbProduct = request.data;
            setThisProduct(dbProduct);
            setActiveImage(dbProduct?.images?.find((image) => image.is_main_image == true));
            setStockQuantity(dbProduct?.stock_quantity);
        }
        getThisProduct();

    }, [location]);

    useEffect(() =>
    {
        if(!isTextAdded)
        {
            return;
        }

        timeoutID.current = setTimeout(() =>
        {
            setIsTextAdded(false);
        }, 1500);
    }, [isTextAdded]);

    const handleAddToCart = () =>
    {
        if(stockQuantity == null || stockQuantity == undefined) return;

        const cartItem = cart.find((cartItem) => cartItem.productId == thisProduct?.id);

        if(cartItem?.quantity >= 10)
        {
            return;
        }

        // If product is already in cart
        if(Number(stockQuantity) - Number(cartItem?.quantity + 1) >= 0)
        {
            addToCart(thisProduct.id, 1);
            setIsTextAdded(true);
            setStockQuantity(prev => prev - 1);
            setMessageForBuyer("✅ Na stanju");
        }
        // If product is not in cart
        else if(!cartItem && stockQuantity > 0)
        {
            addToCart(thisProduct.id, 1);
            setIsTextAdded(true);
            setStockQuantity(prev => prev - 1);
            setMessageForBuyer("✅ Na stanju");
        }
        else
        {
            console.log(cartItem && stockQuantity != 0);
            if(cartItem && stockQuantity != 0)
            {
                setMessageForBuyer("❌ Svi dostupni proizvodi su već u korpi.");
                setStockQuantity(0);
                return;
            }
            else(stockQuantity != 0)
            {
                setStockQuantity(0);
                setMessageForBuyer("❌ Nema dovoljno proizvoda na stanju.");
            }
        }
    }

    return (
        <>
            <title>Prodavnica - {thisProduct?.name}</title>

            <Header allCategories={allCategories}/>
                <main className="product-container">

                    {
                        (thisProduct != null)
                        ?
                    <div className="product-container-details">
                        <div className="alternative-images-container">
                        {
                            (thisProduct.images?.length > 0)
                            ?
                            thisProduct.images.map((image) =>
                            {
                                const active = image.id == activeImage.id;
                                return <img key={image.id} src={image.url} alt={thisProduct?.name + " slika"} className={active ? "alternative-img active" : "alternative-img"} onClick={() => setActiveImage(image)}/>
                            })
                            :
                            ""
                        }
                        </div>

                        <img src={activeImage?.url} alt={thisProduct?.name + " slika"} className="product-container-main-img" />

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

                            <span className="stock-quantity">{messageForBuyer}</span>

                            <button className="product-container-add-to-cart-btn" disabled={!stockQuantity > 0} onClick={() => handleAddToCart()}><span className="center-items"><FontAwesomeIcon icon={faShoppingCart} className="fa-icon-1x"/>Dodaj u korpu</span></button>
                            <p className="added-to-cart">{isTextAdded ? ("Artikal je uspešno dodat u korpu.") : ""}</p>
                            <hr></hr>
                            <p className="product-container-article-description">{thisProduct?.description || "Nema opisa za ovaj proizvod."}</p>

                        </div>
                        <div className="product-detailed-specifications">
                            {
                                (thisProduct.specifications)
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
                                            thisProduct.specifications.map((specification, index) =>
                                            {
                                                return (
                                                    <tr key={index}>
                                                        <td className="product-specification-name">{translateToSerbian(specification.name, {capitalize: true})}</td>
                                                        <td className="product-specification-value">{translateToSerbian(specification.value, {capitalize: true})}</td>
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
                        :
                        ""
                    }


                </main>

            <Footer />
        </>
    )
}