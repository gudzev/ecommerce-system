import "./Cart.css";

import { CartItem } from "./CartItem";

export function CartPreview({cartProducts, cart, setCart})
{
    return <div className="cart-preview">
                <div className="cart-preview-header">
                    <h2 className="preview-product-heading">Proizvod</h2>
                    <h2 className="preview-quantity-heading">Količina</h2>
                    <h2 className="preview-price-heading">Ukupno</h2>
                </div>
            {
            cartProducts?.map((cartItem) =>
            {                return <CartItem key={cartItem.id} cartItem={cartItem} cart={cart} setCart={setCart} />
            })
            }
            </div>
}