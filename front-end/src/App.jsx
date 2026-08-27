import './App.css'

import { Route, Routes } from 'react-router-dom';

import { useState, useEffect, useContext, lazy, Suspense } from 'react';

import { CartContext } from './contexts/CartContext/CartContext';
import { CheckoutContext } from './contexts/CheckoutContext/CheckoutContext';

import { Loader } from "./components/Loader/Loader";

import axios from 'axios';


import Home from "./pages/Home/Home";
import Cart from './pages/Cart/Cart';
// import NotFound from "./pages/NotFound/NotFound";
import Checkout from './pages/Checkout/Checkout';
import Product from './pages/Product/Product';

// const Home = lazy(() => import('./pages/Home/Home'));
// const Cart = lazy(() => import('./pages/Cart/Cart'));
const NotFound = lazy(() => import('./pages/NotFound/NotFound'));
// const Checkout = lazy(() => import('./pages/Checkout/Checkout'));
//const Product = lazy(() => import('./pages/Product/Product'));

export const API_URL = "https://localhost:7097";
export const PRODUCTS_PER_PAGE = 16;

function App() 
{
  const [cartProducts, setCartProducts] = useState([]);
  const [allDeliveryOptions, setAllDeliveryOptions] = useState([]);
  const [allCategories, setAllCategories] = useState([]);

  const {cart} = useContext(CartContext);
  const {deliveryMethod, setOrderPrice, setShipmentPrice} = useContext(CheckoutContext);

  useEffect(() =>
  {
    const getAllDeliveryOptions = async () =>
    {
        const response = await axios.get(API_URL + "/delivery-options");
        const deliveryOptions = response.data;
        setAllDeliveryOptions(deliveryOptions);
    }

    const getAllCategories = async () =>
    {
      const response = await axios.get(API_URL + "/categories");
      const categories = response.data;
      setAllCategories(categories);
    }
    getAllCategories();
    getAllDeliveryOptions();
  }, []);
  
  useEffect(() =>
  {
    const getCartProducts = async () =>
    {
      const cartProductList = cart.map((cartProduct) => cartProduct.productId);
      const response = await axios.get("https://localhost:7097/products", 
        {
          params: 
          {
              is_active: true,
              product_ids: cartProductList
          },
          // indexes: null separates indexes in the following way: product_ids=1&product_ids=2&...
          paramsSerializer: 
          {
            indexes: null
          }
        });

      const allProducts = response.data;
      const cartProducts = [];

      allProducts?.map((product) =>
      {
        const existingProduct = cart.find((cartProduct) => cartProduct.productId == product.id);

        if(existingProduct)
        {
          cartProducts.push(
            {
              id: product.id,
              name: product.name,
              image_url: product.image_url,
              price_rsd: product.price_rsd,
              price_on_sale: product.price_on_sale,
              quantity: Number(existingProduct.quantity)
            })
        }
      });
      setCartProducts(cartProducts);
    }
    getCartProducts();
    localStorage.setItem("cart", JSON.stringify(cart));
  }, [cart]);

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
        <Route path="/" element={<Home allCategories={allCategories}/>}
                                  />
        <Route path="/cart" element={<Cart cartProducts={cartProducts}
                                           allCategories={allCategories}/>} 
                                  />

        <Route path="/checkout" element={<Checkout cartProducts={cartProducts}
                                                   allDeliveryOptions={allDeliveryOptions}
                                                   allCategories={allCategories}/>}
                                    />
                                    
        <Route path="/proizvod/*" element={<Product/>}/>

        <Route path="*" element={<NotFound/>} />
      </Routes>
    </Suspense>
  )
}

export default App
