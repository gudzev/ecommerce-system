import { useContext } from "react"

import { PageContext } from "../../contexts/PageContext/PageContext"

export function PageSelector({products, maxPages})
{
    const {currentPage, setCurrentPage} = useContext(PageContext);

    return (
                <div className={products?.length > 0 ? "page-selector active" : "page-selector"}>
                {
                    currentPage >= 2
                    ?
                    <span className="page-number" onClick={() => setCurrentPage(0)}>
                    {
                        // Display of the first page number
                        1
                    }
                    </span>
                    :
                    ""
                }
                {
                    currentPage >= 1
                    ?
                    <span className="page-number" onClick={() => setCurrentPage(prev => prev - 1)}>
                    {
                        // Display of the previous page
                        currentPage
                    }
                    </span>
                    :
                    ""
                }
                <span className="page-number active" onClick={() => setCurrentPage(currentPage)}>
                    {
                        // Display of current page, it is always displayed.
                        currentPage + 1
                    }
                </span>
                {
                    (currentPage >= 0 && currentPage < (maxPages - 2))
                    ?
                    <span className="page-number" onClick={() => setCurrentPage(prev => prev + 1)}>
                    {
                        // Display of the next page
                        currentPage + 2
                    }
                    </span>
                    :
                    ""
                }
                {
                    (currentPage < (maxPages - 1))
                    ?
                    <span className="page-number" onClick={() => setCurrentPage(maxPages - 1)}>
                    {
                        // Display of the last page
                        maxPages
                    }
                    </span>
                    :
                    ""
                }
            </div>
    )
}