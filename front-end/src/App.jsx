import './App.css'

import { Route, Routes } from 'react-router-dom';

import { useState, useEffect, lazy, Suspense } from 'react';

import { Loader } from "./components/Loader/Loader";

import axios from 'axios';

const Home = lazy(() => import('./pages/Home/Home'));
const Cart = lazy(() => import('./pages/Cart/Cart'));
const NotFound = lazy(() => import('./pages/NotFound/NotFound'));
const Checkout = lazy(() => import('./pages/Checkout/Checkout'));
const Product = lazy(() => import('./pages/Product/Product'));

function App() 
{
  const [orderPrice, setOrderPrice] = useState(0);
  const [shipmentPrice, setShipmentPrice] = useState(0);
  const [cartProducts, setCartProducts] = useState([]);
  const [deliveryOptions, setDeliveryOptions] = useState([]);
  const [allProducts, setAllProducts] = useState([]);
  const [allCategories, setAllCategories] = useState([]);
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
      const response = await axios.get("https://localhost:7097/products?is_active=true");
      const products = response.data;
      setAllProducts(products);
    }

    const getAllDeliveryOptions = async () =>
    {
        const response = await axios.get("https://localhost:7097/delivery-options");
        const deliveryOptions = response.data;
        setDeliveryOptions(deliveryOptions);
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
          const selectedOption = deliveryOptions?.find((option) => option.id == deliveryMethod) || 1;
          if(!selectedOption.price_per_item) return 0;
          return (price >= selectedOption.free_shipping_minimum_value) ? 0 : selectedOption.price_per_item * itemQuantity;
        });
      }

      calculateProductsTotal();
      calculateDeliveryTotal();
  }, [cartProducts, deliveryMethod, deliveryOptions]);

  return (
    <Suspense fallback={<Loader/>}>
      <Routes>
        <Route path="/" element={<Home cart={cart}
                                       setCart={setCart}
                                       searchText={searchText}
                                       setSearchText={setSearchText}
                                       allProducts={allProducts}
                                       allCategories={allCategories}/>}
                                  />
        <Route path="/cart" element={<Cart cart={cart}
                                           setCart={setCart}
                                           setSearchText={setSearchText}
                                           cartProducts={cartProducts}
                                           orderPrice={orderPrice}
                                           shipmentPrice={shipmentPrice}
                                           allCategories={allCategories}/>} 
                                  />

        <Route path="/checkout" element={<Checkout setSearchText={setSearchText}
                                                   cart={cart}
                                                   setCart={setCart}
                                                   cartProducts={cartProducts}
                                                   orderPrice={orderPrice}
                                                   shipmentPrice={shipmentPrice}
                                                   deliveryMethod={deliveryMethod}
                                                   setDeliveryMethod={setDeliveryMethod}
                                                   deliveryOptions={deliveryOptions}
                                                   allCategories={allCategories}/>}
                                    />
                                    
        <Route path="/proizvod/*" element={<Product allCategories={allCategories}
                                                    allProducts={allProducts}
                                                    cart={cart}
                                                    setCart={setCart}
                                                    setSearchText={setSearchText}/>}
                                    />

        <Route path="*" element={<NotFound allCategories={allCategories} />} />
      </Routes>
    </Suspense>
  )
}

export default App
