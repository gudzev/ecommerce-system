import "./Products.css";

import { Product } from "./Product";
import { PageSelector } from "./PageSelector";

import { useState, useEffect, useContext } from "react";
import { useSearchParams } from "react-router-dom";

import { HeaderContext } from "../../contexts/HeaderContext/HeaderContext";
import { PageContext } from "../../contexts/PageContext/PageContext";

import { API_URL, PRODUCTS_PER_PAGE } from "../../App";

import axios from "axios";


export function Products({allCategories})
{
    const {searchText} = useContext(HeaderContext);

    const [products, setProducts] = useState([]);
    const [maxPages, setMaxPages] = useState(0);
    const [allProducts, setAllProducts] = useState([]);

    const {currentPage, setCurrentPage} = useContext(PageContext);
    
    const [searchParams] = useSearchParams();

    // INPUT = category_id
    // OUTPUT = category_name encoded for URL
    const getProductCategoryName = (category_id) =>
    {
        const foundProductCategory = allCategories?.find((category) => category.id == category_id);
        return foundProductCategory?.name.replaceAll(' ', '-').toLowerCase();
    }

    // INPUT = category_name encoded for URL
    // OUTPUT = category-id
    const getProductCategoryID = (category_name) =>
    {
        const foundProductCategory = allCategories?.find((category) => category?.name.toLowerCase() == category_name?.toLowerCase().replace('-', ' '));
        return foundProductCategory?.id;
    }

    useEffect(() =>
    {
        setCurrentPage(0);
    }, [searchParams]);

    useEffect(() =>
    {
        const categoryID = getProductCategoryID(searchParams.get("kategorija"));

        let customSearch = "";

        if(categoryID)
        {
            customSearch += `&category_id=${categoryID}`;
        }
        if(searchText)
        {
            customSearch += `&search_text=${searchText}`;
        }

        const getPagesCount = async () =>
        {
            const response = await axios.get(API_URL + `/product-pages?products_per_page=${PRODUCTS_PER_PAGE}` + customSearch);
            const data = response.data;
            setMaxPages(data);
        }

        const getAllProducts = async () =>
        {
            const response = await axios.get(API_URL + `/products?is_active=true&page=${currentPage}&products_per_page=${PRODUCTS_PER_PAGE}` + customSearch);
            const products = response.data;
            setAllProducts(products);
        }
        getPagesCount();
        getAllProducts();

    }, [searchParams, currentPage, searchText]);

    useEffect(() =>
    {
        const getProducts = async () =>
        {
            if(searchParams.get("kategorija"))
            {
                const eligibleProducts = [];
                allProducts.forEach((product) =>
                {
                    if(searchParams.get("kategorija") == getProductCategoryName(product.category_id))
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
    }, [searchParams, allProducts]);

    return (
        <section className="products">
            <div className="products-grid">
            {
                (products?.length > 0)
                ?
                products?.map((product) =>
                {
                    if(product.name.toLowerCase().includes(searchText))
                    {
                        return <Product image_url={product.image_url} name={product.name} price_rsd={product.price_rsd} price_on_sale={product.price_on_sale} id={product.id} key={product.id}/>
                    }
                })
                :
                <div className="empty-grid-message">
                    <h1>Nije pronađen ni jedan proizvod.</h1>
                </div>
            }
            </div>


        <PageSelector products={products} maxPages={maxPages}/>

        </section>
    )
}