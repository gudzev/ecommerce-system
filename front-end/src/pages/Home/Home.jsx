import "./Home.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";
import { Products } from "../../components/Products/Products";

export default function Home({allProducts, allCategories, cart, setCart, searchText, setSearchText})
{
    return <>
    
        <title>Prodavnica - početna</title>

        <Header setSearchText={setSearchText} allCategories={allCategories} cart={cart}/>
        <Products searchText={searchText} cart={cart} setCart={setCart} allProducts={allProducts} allCategories={allCategories}/>
        <Footer />
    </>
}