import './App.css'

import { Home } from "./pages/Home/Home";
import { Cart } from './pages/Cart/Cart';
import { NotFound } from "./pages/NotFound/NotFound";
import { Checkout } from './pages/Checkout/Checkout';

import { Route, Routes } from 'react-router-dom';

import { useState, useEffect } from 'react';

import { calculateDelivery } from "./utils/calculateDelivery";

import axios from 'axios';

function App() 
{
  const [orderPrice, setOrderPrice] = useState(0);
  const [shipmentPrice, setShipmentPrice] = useState(0);
  const [cartProducts, setCartProducts] = useState([]);
  const [allProducts, setAllProducts] = useState([]);
  const [searchText, setSearchText] = useState("");
  const [deliveryMethod, setDeliveryMethod] = useState(1);
  const [cart, setCart] = useState(() =>
  {
    return JSON.parse(localStorage.getItem("cart")) || [];
  });

  useEffect(() =>
  {
    const getAllProducts = async () =>
    {
      const response = await axios.get("/products.json");
      const products = response.data.products;
      setAllProducts(products);
    }
    getAllProducts();
  }, []);

  useEffect(() =>
  {
    const getCartProducts = () =>
    {
        const newProducts = [];
        cart.forEach((cartProduct) =>
        {
            allProducts.forEach((existingProduct) =>
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
  }, [cart, allProducts]);

  useEffect(() =>
  {
      let price = 0;
      const calculateProductsTotal = () =>
      {
          cartProducts.forEach((cartProduct) =>
          {
              price += cartProduct.price_rsd * cartProduct.quantity;
          })
          setOrderPrice(price);
      }

      const calculateDeliveryTotal = () =>
      {
          let itemQuantity = 0;
          cartProducts.forEach((product) => itemQuantity += product.quantity)
          setShipmentPrice(calculateDelivery(price, itemQuantity, deliveryMethod));
      }

      calculateProductsTotal();
      calculateDeliveryTotal();
  }, [cartProducts, deliveryMethod]);

  return (
      <Routes>
        <Route path="/" element={<Home cart={cart}
                                       setCart={setCart}
                                       searchText={searchText}
                                       setSearchText={setSearchText}/>} 
                                  />
        <Route path="/cart" element={<Cart cart={cart}
                                           setCart={setCart}
                                           setSearchText={setSearchText}
                                           cartProducts={cartProducts}
                                           orderPrice={orderPrice}
                                           shipmentPrice={shipmentPrice}/>} 
                                  />

        <Route path="/checkout" element={<Checkout setSearchText={setSearchText}
                                                   cart={cart}
                                                   setCart={setCart}
                                                   cartProducts={cartProducts}
                                                   orderPrice={orderPrice}
                                                   shipmentPrice={shipmentPrice}
                                                   deliveryMethod={deliveryMethod}
                                                   setDeliveryMethod={setDeliveryMethod}/>}
                                    />
        <Route path="*" element={<NotFound />} />
      </Routes>
  )
}

export default App
