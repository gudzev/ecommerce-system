import "./Home.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";
import { Products } from "../../components/Products/Products";

export function Home({cart, setCart, setSearchText, searchText, allProducts})
{
    return <>
    
        <title>Prodavnica - početna</title>

        <Header setSearchText={setSearchText} cart={cart}/>
        <Products searchText={searchText} cart={cart} setCart={setCart} allProducts={allProducts}/>
        <Footer />
    </>
}