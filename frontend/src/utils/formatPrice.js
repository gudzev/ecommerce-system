export const formatPrice = (price) =>
{
    return new Intl.NumberFormat().format(price);
}