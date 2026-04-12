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
  const [orderPrice, setOrderPrice] = useState(0);
  const [shipmentPrice, setShipmentPrice] = useState(0);
  const [deliveryOptions, setDeliveryOptions] = useState([]);
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
      const response = await axios.get("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/products?is_active=1");
      const products = response.data;
      setAllProducts(products);
    }

    const getAllDeliveryOptions = async () =>
    {
        const response = await axios.get("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/delivery-options");
        const deliveryOptions = response.data;
        setDeliveryOptions(deliveryOptions);
    }

    getAllProducts();
    getAllDeliveryOptions();
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
  }, [cartProducts, deliveryMethod]);

  return (
      <Routes>
        <Route path="/" element={<Home cart={cart}
                                       setCart={setCart}
                                       searchText={searchText}
                                       setSearchText={setSearchText}
                                       allProducts={allProducts}/>}
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
                                                   setDeliveryMethod={setDeliveryMethod}
                                                   deliveryOptions={deliveryOptions}/>}
                                    />
        <Route path="*" element={<NotFound />} />
      </Routes>
  )
}

export default App
