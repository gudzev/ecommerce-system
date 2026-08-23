import './App.css'

import { Route, Routes } from 'react-router-dom';

import { useState, useEffect, useContext, lazy, Suspense } from 'react';

import { CartContext } from './contexts/CartContext/CartContext';
import { CheckoutContext } from './contexts/CheckoutContext/CheckoutContext';

import { Loader } from "./components/Loader/Loader";

import axios from 'axios';


import Home from "./pages/Home/Home";
import Cart from './pages/Cart/Cart';
import NotFound from "./pages/NotFound/NotFound";
import Checkout from './pages/Checkout/Checkout';
import Product from './pages/Product/Product';


/*
const Home = lazy(() => import('./pages/Home/Home'));
const Cart = lazy(() => import('./pages/Cart/Cart'));
const NotFound = lazy(() => import('./pages/NotFound/NotFound'));
const Checkout = lazy(() => import('./pages/Checkout/Checkout'));
const Product = lazy(() => import('./pages/Product/Product'));
*/

function App() 
{
  const [cartProducts, setCartProducts] = useState([]);
  const [allDeliveryOptions, setAllDeliveryOptions] = useState([]);
  const [allProducts, setAllProducts] = useState([]);
  const [allCategories, setAllCategories] = useState([]);

  const {cart} = useContext(CartContext);
  const {deliveryMethod, setOrderPrice, setShipmentPrice} = useContext(CheckoutContext);

  useEffect(() =>
  {
    const getAllProducts = async () =>
    {
      const response = await axios.get("https://localhost:7097/products?is_active=true");
      const products = response.data;
      setAllProducts(products);
    }

    const getAllDeliveryOptions = async () =>
    {
        const response = await axios.get("https://localhost:7097/delivery-options");
        const deliveryOptions = response.data;
        setAllDeliveryOptions(deliveryOptions);
    }

    const getAllCategories = async () =>
    {
      const response = await axios.get("https://localhost:7097/categories");
      const categories = response.data;
      setAllCategories(categories);
    }

    getAllProducts();
    getAllDeliveryOptions();
    getAllCategories();
  }, []);

  useEffect(() =>
  {
    const getCartProducts = () =>
    {
        const newProducts = [];
        cart.forEach((cartProduct) =>
        {
            allProducts?.forEach((existingProduct) =>
            {
                if(cartProduct.productId == existingProduct.id)
                {
                    newProducts.push(
                        {
                            id: existingProduct.id,
                            name: existingProduct.name,
                            image_url: existingProduct.image_url,
                            price_rsd: existingProduct.price_rsd,
                            price_on_sale: existingProduct.price_on_sale,
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
          if(!cartProduct.price_on_sale)
          {
            price += cartProduct.price_rsd * cartProduct.quantity;
          }
          else
          {
            price += cartProduct.price_on_sale * cartProduct.quantity;
          }
        })
        setOrderPrice(price);
      }

      const calculateDeliveryTotal = () =>
      {
        let itemQuantity = 0;
        cartProducts.forEach((product) => itemQuantity += product.quantity)
        setShipmentPrice(() =>
        {
          const selectedOption = allDeliveryOptions?.find((option) => option.id == deliveryMethod) || 1;
          if(!selectedOption.price_per_item) return 0;
          return (price >= selectedOption.free_shipping_minimum_value) ? 0 : selectedOption.price_per_item * itemQuantity;
        });
      }

      calculateProductsTotal();
      calculateDeliveryTotal();
  }, [cartProducts, deliveryMethod, allDeliveryOptions]);

  return (
    <Suspense fallback={<Loader />}>
      <Routes>
        <Route path="/" element={<Home allProducts={allProducts}
                                       allCategories={allCategories}/>}
                                  />
        <Route path="/cart" element={<Cart cartProducts={cartProducts}
                                           allCategories={allCategories}/>} 
                                  />

        <Route path="/checkout" element={<Checkout cartProducts={cartProducts}
                                                   allDeliveryOptions={allDeliveryOptions}
                                                   allCategories={allCategories}/>}
                                    />
                                    
        <Route path="/proizvod/*" element={<Product allCategories={allCategories}
                                                    allProducts={allProducts}/>}
                                    />

        <Route path="*" element={<NotFound allCategories={allCategories} />} />
      </Routes>
    </Suspense>
  )
}

export default App
