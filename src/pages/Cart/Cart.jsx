import "./Cart.css";

import { Header } from "../../components/Header/Header";
import { Footer } from "../../components/Footer/Footer";
import { CartCheckout } from "./CartCheckout";
import { CartPreview } from "./CartPreview";

export function Cart({setSearchText, cart, setCart, cartProducts})
{
    /*
    const [cartProducts, setCartProducts] = useState([]);

    useEffect(() =>
    {
        const getCartProducts = async () =>
        {
            const response = await axios.get("/products.json");
            const products = response.data.products;
            const newProducts = [];

            cart.forEach((cartProduct) =>
            {
                products.forEach((existingProduct) =>
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
        getCartProducts();
    }, [cart]);
    */

    return <>
        <Header setSearchText={setSearchText} cart={cart}/>

        <section className="cart">
            <div className="cart-content">

                <h1 className="cart-header">Korpa</h1>

                <div className="cart-flex-container">
                    <CartPreview cartProducts={cartProducts} cart={cart} setCart={setCart}/>
                    <CartCheckout cartProducts={cartProducts} cart={cart}/>
                </div>

            </div>
        </section>

        <Footer />
    </>
}