import { PageContext } from "./PageContext";

import { useState } from "react";

export function PageContextProvider({children})
{
    const [currentPage, setCurrentPage] = useState(0);
    return <PageContext value={{currentPage, setCurrentPage}}>
        {children}
    </PageContext>
}