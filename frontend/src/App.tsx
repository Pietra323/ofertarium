import React from 'react';
import { createBrowserRouter, Route, createRoutesFromElements, RouterProvider} from "react-router-dom";
import { Home, SignUp } from './Pages';
import { SignIn } from './Pages';
import AddProduct from './Pages/Addproduct/Addproduct';

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route>
      <Route index element={<Home />} />
      <Route path="/Login" element={<SignIn />} />
      <Route path="/Register" element={<SignUp />} />
      <Route path="/Addproduct" element={<AddProduct />} />
    </Route>,
  ),
);

const App: React.FC = () => {
  return (
      <RouterProvider router={router} />
  );
};

export default App;
