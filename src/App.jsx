import './App.css'

import { Home } from "./pages/Home/Home";
import { Cart } from './pages/Cart/Cart';
import { NotFound } from "./pages/NotFound/NotFound";

import { Route, Routes } from 'react-router-dom';

import { useState, useEffect } from 'react';

function App() 
{
  const [cart, setCart] = useState(() =>
  {
    return JSON.parse(localStorage.getItem("cart")) || [];
  });
  const [searchText, setSearchText] = useState("");

  useEffect(() =>
  {
    localStorage.setItem("cart", JSON.stringify(cart));
    console.log(cart);
  }, [cart]);

  return (
      <Routes>
        <Route path="/" element={<Home cart={cart} setCart={setCart} searchText={searchText} setSearchText={setSearchText}/>} />
        <Route path="/cart" element={<Cart cart={cart} setCart={setCart} setSearchText={setSearchText}/>} />
        <Route path="*" element={<NotFound />} />
      </Routes>
  )
}

export default App
