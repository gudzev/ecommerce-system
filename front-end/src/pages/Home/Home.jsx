import "./Home.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";
import { Products } from "../../components/Products/Products";

export default function Home({allProducts, allCategories})
{
    return <>
    
        <title>Prodavnica - početna</title>

        <Header allCategories={allCategories}/>
        <Products allProducts={allProducts} allCategories={allCategories}/>
        <Footer />
    </>
}