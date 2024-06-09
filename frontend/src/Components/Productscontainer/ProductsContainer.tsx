import React, { useEffect, useState } from 'react';
import MultiActionAreaCard from '../Card/Card';

interface Product {
  idProduct: number;
  productName: string;
  subtitle: string;
  amountOf: number;
  price: number;
  photos: string[];
  categoryIds: { $values: number[] };
}

interface ProductContainerProps {
  categoryId: number;
  categoryName: string;
}

export default function ProductContainer({ categoryId, categoryName }: ProductContainerProps) {
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    fetch('http://localhost:5004/api/products')
      .then(response => response.json())
      .then(data => {
        console.log(data);  // Log the data to verify its structure
        const filteredProducts = data.$values.filter((product: Product) =>
          product.categoryIds.$values.includes(categoryId)
        );
        setProducts(filteredProducts);
      })
      .catch(error => console.error('Error fetching products:', error));
  }, [categoryId]);

  return (
    <>
      <div className="label" style={{
        marginTop: "1.5rem",
        marginLeft: "150px",
        fontSize: "1.5rem",
        overflow: "hidden"
      }}>Kategoria: {categoryName}</div>
      <div className="ProductContainer" style={{
        display: "flex",
        width: "100vw",
        overflow: "hidden",
        flexWrap: "wrap",
        justifyContent: "center",
        marginLeft: "-30px"
      }}>
        {products.length > 0 ? (
          products.map(product => (
            <MultiActionAreaCard key={product.idProduct} product={product} />
          ))
        ) : (
          <div style={{ margin: "1rem", marginBottom:"80px" }}>Brak produktów z danej kategorii</div>
        )}
      </div>
    </>
  );
}
