import React, { useEffect, useState } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage';

import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import EventCatalogPage from './pages/EventCatalogPage';
import Navbar from './components/Navbar';
import AuthPage from './pages/AuthPage';
import axiosClient from './tools/axiosConfig';
import ProfilePage from './pages/ProfilePage';
import EventPage from './pages/EventPage';
import AdminPage from './pages/AdminPage';


export const UserContext = React.createContext(null);

function App() {
  const [user, setUser] = useState(null);
  const [userLoading, setUserLoading] = useState(true)

  useEffect(() => {
    axiosClient({
      method: "GET",
      url: "user/me"
    })
      .then(response => response.data)
      .then(data => {
        setUser(data.user)
        setUserLoading(false);
      })
      .catch(err => {
        axiosClient({
          method: "POST",
          url: "user/refresh"
        })
          .then(response => response.data)
          .then(data => {
            setUser(data.user)
            setUserLoading(false);
          })
          .catch(err => {
            setUser(null)
            setUserLoading(false);
          })
      });
  }, [])

  return (
    <BrowserRouter>
      <UserContext.Provider value={{ user: user, setUser: setUser, userLoading: userLoading }}>
        <Navbar />
        <Routes>
          <Route path="/">
            <Route index element={<HomePage />} />
            <Route path='catalog' element={<EventCatalogPage />} />
            <Route path='auth' element={<AuthPage />} />
            <Route path='profile' element={<ProfilePage />} />
            <Route path='event/:id' element={<EventPage />} />
            <Route path='admin' element={<AdminPage />} />
          </Route>
        </Routes>
      </UserContext.Provider>
    </BrowserRouter>
  );
}

export default App;
