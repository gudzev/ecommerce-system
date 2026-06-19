import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";

import { useEffect, useState } from "react";

import "./ProductDetails.css";

export function Product({allCategories, allProducts})
{
    const [thisProduct, setThisProduct] = useState(null); 

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

    return (
        <>
            <title>Prodavnica - {thisProduct?.name}</title>
            <Header allCategories={allCategories}/>
                <main className="product-container">
                    <div className="product-details">
                        <img src={thisProduct?.image_url} alt={thisProduct?.name + " slika"} className="product-img" />
                    </div>
                </main>
            <Footer />
        </>
    )
}