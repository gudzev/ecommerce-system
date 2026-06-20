import "./Products.css";

import { Product } from "./Product";

import { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";


export function Products({searchText, cart, setCart, allProducts, allCategories})
{
    const [products, setProducts] = useState([]);
    const location = useLocation();

    const getProductCategory = (category_id) =>
    {
        const foundProductCategory = allCategories?.find((category) => category.id == category_id);
        return foundProductCategory?.name.replaceAll(' ', '-').toLowerCase();
    }

    useEffect(() =>
    {
        const getProducts = async () =>
        {
            const searchParams = new URLSearchParams(document.location.search);


            if(searchParams.get("kategorija"))
            {
                const eligibleProducts = [];
                allProducts.forEach((product) =>
                {
                    if(searchParams.get("kategorija") == getProductCategory(product.category_id))
                    {
                        eligibleProducts.push(product);
                    }
                });
                setProducts(eligibleProducts);
            }
            else
            {
                setProducts(allProducts);
            }

        }
        getProducts();
    }, [location, allProducts]);

    return (
        <section className="products">
            <div className="products-grid">
            {
                (products.length > 0)
                ?
                products?.map((product) =>
                {
                    if(product.name.toLowerCase().includes(searchText))
                    {
                        console.log(product);
                        return <Product image_url={product.image_url} name={product.name} price_rsd={product.price_rsd} price_on_sale={product.price_on_sale} id={product.id} key={product.id} cart={cart} setCart={setCart}/>
                    }
                })
                :
                <div className="empty-grid-message">
                    <h1>Nije pronađen ni jedan proizvod.</h1>
                </div>
            }
            </div>
        </section>
    )
}