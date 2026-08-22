import "./Cart.css";

import { CartItem } from "./CartItem";

export function CartPreview({cartProducts})
{
    return <table className="cart-preview">
                <thead>
                    <tr className="cart-preview-header">
                        <th className="preview-product-heading">Proizvod</th>
                        <th className="preview-quantity-heading">Količina</th>
                        <th className="preview-price-heading">Ukupno</th>
                        <th className="preview-delete-heading"></th>
                    </tr>
                </thead>
                <tbody>
                    {
                    cartProducts?.map((cartItem) =>
                    {
                        return <CartItem key={cartItem.id} cartItem={cartItem}/>
                    })}
                </tbody>
            </table>
}