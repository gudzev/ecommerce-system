import './App.css'

import { Home } from "./pages/Home/Home";
import { Cart } from './pages/Cart/Cart';
import { NotFound } from "./pages/NotFound/NotFound";
import { Checkout } from './pages/Checkout/Checkout';

import { Route, Routes } from 'react-router-dom';

import { useState, useEffect } from 'react';

import axios from 'axios';

function App() 
{
  const [cartProducts, setCartProducts] = useState([]);
  const [searchText, setSearchText] = useState("");
  const [cart, setCart] = useState(() =>
  {
    return JSON.parse(localStorage.getItem("cart")) || [];
  });


  useEffect(() =>
  {
    const getCartProducts = async () =>
    {
        const response = await axios.get("/products.json");
        const products = response.data.products;
        const newProducts = [];

        cart.forEach((cartProduct) =>
        {
            products.forEach((existingProduct) =>
            {
                if(cartProduct.productId == existingProduct.id)
                {
                    newProducts.push(
                        {
                            id: existingProduct.id,
                            name: existingProduct.name,
                            image_url: existingProduct.image_url,
                            price_rsd: existingProduct.price_rsd,
                            quantity: Number(cartProduct.quantity)
                        });
                }
            })
        })
        setCartProducts(newProducts);
    }

    localStorage.setItem("cart", JSON.stringify(cart));
    getCartProducts();
  }, [cart]);

  return (
      <Routes>
        <Route path="/" element={<Home cart={cart} setCart={setCart} searchText={searchText} setSearchText={setSearchText}/>} />
        <Route path="/cart" element={<Cart cart={cart} setCart={setCart} setSearchText={setSearchText} cartProducts={cartProducts}/>} />
        <Route path="/checkout" element={<Checkout setSearchText={setSearchText} cart={cart} cartProducts={cartProducts} />}/>
        <Route path="*" element={<NotFound />} />
      </Routes>
  )
}

export default App
