import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';

import { HeaderContextProvider } from './contexts/HeaderContext/HeaderContextProvider.jsx';
import { CheckoutContextProvider} from "./contexts/CheckoutContext/CheckoutContextProvider.jsx";
import { CartContextProvider } from './contexts/CartContext/CartContextProvider.jsx';

import './index.css'

import App from './App.jsx'

// HeaderContext - searchText, setSearchText
// CartContext - cart, setCart
// CheckoutContext - orderPrice, setOrderPrice, shipmentPrice, deliveryMethod, setDeliveryMethod

createRoot(document.getElementById('root')).render(
  <BrowserRouter>
    <CartContextProvider>
      <CheckoutContextProvider>
        <HeaderContextProvider> 
          <App />
        </HeaderContextProvider>
      </CheckoutContextProvider>
    </CartContextProvider>
  </BrowserRouter>,
)
