    export const viewProductDetails = (navigate, name, id) =>
    {
        navigate(
            {
                pathname: `/proizvod/${encodeURIComponent(name.toLowerCase().replaceAll(' ', '-'))}`
            },
            {
                state: 
                {
                    id: id
                }
            });
    }