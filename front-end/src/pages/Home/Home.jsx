import "./Home.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";
import { Products } from "../../components/Products/Products";

export default function Home({allCategories})
{
    return <>
    
        <title>Prodavnica - početna</title>

        <Header allCategories={allCategories}/>
        <Products allCategories={allCategories}/>
        <Footer />
    </>
}