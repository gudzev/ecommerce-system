// param1 = useNavigate() object, param2 = product name (Ryzen 7 9700X for example)
export const viewProductDetails = (navigate, name) =>
{
    navigate(`/proizvod/${encodeURIComponent(name.toLowerCase().replaceAll(' ', '-'))}`);
}