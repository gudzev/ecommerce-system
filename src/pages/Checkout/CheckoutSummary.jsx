import "./Checkout.css";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash } from "@fortawesome/free-solid-svg-icons";

import { formatPrice } from "../../utils/formatPrice";

export function CheckoutSummary({cartProducts})
{
    return <div className="checkout-summary">
        <h2 className="checkout-heading-h2">Pregled kupovine</h2>

        <div className="checkout-items-overview">
            {
                cartProducts.map((cartProduct) => 
                {
                    return <div className="checkout-items-item" key={cartProduct.id}>
                            <img src={cartProduct.image_url} alt={cartProduct.name + ' ' + "Image"} className="checkout-item-img" />
                            
                            <div className="checkout-items-details">
                                <h2 className="checkout-item-name">{cartProduct.name}</h2>
                                <h3 className="checkout-item-quantity">Količina: {cartProduct.quantity}</h3>
                            </div>

                            <span className="checkout-item-price">{formatPrice(cartProduct.price_rsd) + ' ' + "RSD"}</span>

                            <FontAwesomeIcon icon={faTrash} className="checkout-delete-item-btn" />
                    </div>
                })
            }
        </div>
    </div>
}