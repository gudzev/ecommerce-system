    export const viewProductDetails = (navigate, name, id) =>
    {
        navigate(
            {
                pathname: `/proizvod/${id}`
            },
            {
                state: 
                {
                    id: id
                }
            });
    }