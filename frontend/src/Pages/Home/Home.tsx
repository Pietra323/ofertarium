import React, { useEffect, useState } from 'react';
import ResponsiveAppBar from "../../Components/Navbar/ResponsiveAppBar";
import ProductContainer from "../../Components/Productscontainer/ProductsContainer";

interface Category {
  id: number;
  nazwa: string;
  description: string;
}

export function Home() {
  const [categories, setCategories] = useState<Category[]>([]);

  useEffect(() => {
    fetch('https://localhost:7235/api/category')
      .then(response => response.json())
      .then(data => {
        console.log("Kategorie: ",data);  // Log the data to verify its structure
        setCategories(data.$values || []);
      })
      .catch(error => console.error('Error fetching categories:', error));
  }, []);

  return (
    <>
      <ResponsiveAppBar />
      {categories.map(category => (
        <ProductContainer key={category.id} categoryId={category.id} categoryName={category.nazwa} />
      ))}
    </>
  );
}
