import { useState } from "react";

import { HeaderContext } from "./HeaderContext";

export function HeaderContextProvider({children})
{
    const [searchText, setSearchText] = useState("");

  return <HeaderContext value={{searchText, setSearchText}}>
    {children}
  </HeaderContext>
}