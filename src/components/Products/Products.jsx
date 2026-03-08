import "./Products.css";

import { Product } from "./Product";
import { useState, useEffect } from "react";

import axios from "axios";

export function Products({searchText, cart, setCart})
{
    const [products, setProducts] = useState([]);

    useEffect(() =>
    {
        const getProducts = async () =>
        {
            const searchParams = new URLSearchParams(document.location.search);
            const response = await axios.get("/products.json");

            if(searchParams.size > 0)
            {
                const eligibleProducts = [];
                response.data.products.forEach((product) =>
                {
                    if(searchParams.get("category") == product.category_id)
                    {
                        eligibleProducts.push(product);
                    }
                });
                setProducts(eligibleProducts);
            }
            else
            {
                setProducts(response.data.products);
            }

        }
        getProducts();
    }, []);

    return (
        <section className="products">
            <div className="products-grid">
            {
                products?.map((product) =>
                {
                    if(product.name.toLowerCase().includes(searchText))
                    {
                        return <Product image_url={product.image_url} name={product.name} price_rsd={product.price_rsd} id={product.id} key={product.id} cart={cart} setCart={setCart}/>
                    }
                })
            }
            </div>
        </section>
    )
}