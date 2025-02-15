import React, { useState } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage';

import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import EventCatalogPage from './pages/EventCatalogPage';


export const UserContext = React.createContext(null);

function App() {
  const [user, setUser] = useState(null);

  return (
    <BrowserRouter>
      <UserContext.Provider value={{ user: { name: "dubina" }, setUser: setUser }}>
        <Routes>
          <Route path="/">
            <Route index element={<HomePage />} />
            <Route path='catalog' element={<EventCatalogPage />} />
          </Route>
        </Routes>
      </UserContext.Provider>
    </BrowserRouter>
  );
}

export default App;
