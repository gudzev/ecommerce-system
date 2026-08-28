import "./Products.css";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faShoppingCart, faCheck } from "@fortawesome/free-solid-svg-icons";

import { useState, useRef, useContext, useEffect } from "react";
import { useNavigate } from 'react-router-dom';

import { viewProductDetails } from "../../utils/viewProductDetails";

import { formatPrice } from "../../utils/formatPrice";

import { CartContext } from "../../contexts/CartContext/CartContext";

let timeoutList = [];

export function Product({image_url, name, price_rsd, id, price_on_sale, stock_quantity})
{
    const navigate = useNavigate()

    const {addToCart, cart} = useContext(CartContext);

    const [isAddedToCart, setIsAddedToCart] = useState(false);
    const [onStock, setOnStock] = useState(false);

    const quantitySelect = useRef(1);

    useEffect(() =>
    {
        const fn = async () =>
        {
            setOnStock(stock_quantity > 0);
        }
        fn();
    }, [])

    const handleAddToCart = (productId) =>
    {
        const selectedQuantity = Number(quantitySelect.current.value);
        const cartQuantity = (cart?.find((cartItem) => cartItem.productId == id))?.quantity || 0;

        if((cartQuantity + selectedQuantity) <= stock_quantity)
        {
            setOnStock((cartQuantity + selectedQuantity) < stock_quantity);
            addToCart(id, selectedQuantity);
            displayAddedToCartText(productId);
        }
        else
        {
            setOnStock(false);
        }
    }

    const displayAddedToCartText = (productId) =>
    {
        timeoutList.forEach((productTimeOut) =>
        {
            if(productId == productTimeOut.productId)
            {
                clearTimeout(productTimeOut.timeout);
            }
        })

        setIsAddedToCart(true);
        const timeout = setTimeout(() =>
        {
            setIsAddedToCart(false);
        }, 2500);
        timeoutList.push({
            timeout: timeout,
            productId: productId
        });
    }

    return <div className="product">
                <img src={image_url} loading="lazy" alt={name + " image"} className="product-img" onClick={() => viewProductDetails(navigate, name, id)}/>

                <h2 className="product-name" onClick={() => viewProductDetails(navigate, name, id)}>{name}</h2>

                <div className="product-details">
                    <div className="product-quantity">
                        <span className="product-qty-text">Količina: </span>
                        <select className="product-qty-select" ref={quantitySelect}>
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9">9</option>
                            <option value="10">10</option>
                        </select>
                    </div>
                    <span className="product-price">Cena: <span className={price_on_sale ? "product-price-value inactive" : "product-price-value"}>{formatPrice(price_rsd)} RSD</span></span>
                    <span className="product-price-sale"> {price_on_sale ? formatPrice(price_on_sale) + ' ' + "RSD" : ""}</span>
                </div>

                <button className="add-to-cart-btn" disabled={!onStock} onClick={() => handleAddToCart()}>
                    <FontAwesomeIcon icon={faShoppingCart} />{onStock ? "Dodaj u korpu" : "Nema na stanju"}
                </button>
                <span className={isAddedToCart ? `cart-added active` : `cart-added`}><FontAwesomeIcon icon={faCheck} />Dodato</span>
        </div>
}