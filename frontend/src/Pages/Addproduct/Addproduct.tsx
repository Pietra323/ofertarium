import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Button,
  Checkbox,
  FormControl,
  FormControlLabel,
  FormGroup,
  TextField,
  Typography
} from '@mui/material';

const AddProduct = () => {
  const [productName, setProductName] = useState('');
  const [subtitle, setSubtitle] = useState('');
  const [amountOf, setAmountOf] = useState(0);
  const [price, setPrice] = useState(0);
  const [categoryIds, setCategoryIds] = useState<number[]>([]);
  const [photos, setPhotos] = useState<string[]>(['']);
  const [categories, setCategories] = useState<any[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    // Fetch categories from the backend
    fetch('https://localhost:7235/api/category')
      .then(response => response.json())
      .then(data => {
        setCategories(data.$values);
      })
      .catch(error => {
        console.error('There was an error fetching the categories!', error);
      });
  }, []);

  const handleCategoryChange = (event: React.ChangeEvent<HTMLInputElement>, categoryId: number) => {
    console.log("Checkbox ID: ", categoryId); 
    setCategoryIds(prevCategoryIds => {
      if (prevCategoryIds.includes(categoryId)) {
        return prevCategoryIds.filter(id => id !== categoryId);
      } else {
        return [...prevCategoryIds, categoryId];
      }
    });
  };

  const handlePhotoChange = (index: number, event: React.ChangeEvent<HTMLInputElement>) => {
    const newPhotos = [...photos];
    newPhotos[index] = event.target.value;
    setPhotos(newPhotos);
  };

  const handleAddPhotoField = () => {
    setPhotos([...photos, '']);
  };

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const newProduct = {
      idProduct: 0,
      productName,
      subtitle,
      amountOf,
      price,
      categoryIds,
      photos
    };
    console.log(newProduct)
    fetch('https://localhost:7235/api/products/add_product', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(newProduct)
    })
      .then(response => response.json())
      .then(data => {
        console.log('Product added successfully', data);
        navigate('/'); // Navigate to home or another appropriate page
      })
      .catch(error => {
        console.error('There was an error adding the product!', error);
      });
  };

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ mt: 3 }}>
      <Typography variant="h6">Add a New Product</Typography>
      <TextField
        margin="normal"
        required
        fullWidth
        id="productName"
        label="Product Name"
        name="productName"
        value={productName}
        onChange={(e) => setProductName(e.target.value)}
      />
      <TextField
        margin="normal"
        fullWidth
        id="subtitle"
        label="Subtitle"
        name="subtitle"
        value={subtitle}
        onChange={(e) => setSubtitle(e.target.value)}
      />
      <TextField
        margin="normal"
        required
        fullWidth
        id="amountOf"
        label="Amount Of"
        name="amountOf"
        type="number"
        value={amountOf}
        onChange={(e) => setAmountOf(parseInt(e.target.value))}
      />
      <TextField
        margin="normal"
        required
        fullWidth
        id="price"
        label="Price"
        name="price"
        type="number"
        value={price}
        onChange={(e) => setPrice(parseFloat(e.target.value))}
      />
      <FormControl component="fieldset" margin="normal">
        <Typography variant="subtitle1">Categories</Typography>
        <FormGroup>
          {categories.map((category) => (
            <FormControlLabel
            key={category.$id}
            control={
              <Checkbox
                checked={categoryIds.includes(category.$id)}
                onChange={(event) => handleCategoryChange(event, category.$id)}
                value={category.$id}
              />
            }
            label={category.nazwa}
          />
          ))}
        </FormGroup>
      </FormControl>
      <Typography variant="subtitle1">Photos</Typography>
      {photos.map((photo, index) => (
        <TextField
          key={index}
          margin="normal"
          fullWidth
          id={`photo-${index}`}
          label={`Photo URL ${index + 1}`}
          name={`photo-${index}`}
          value={photo}
          onChange={(event) => handlePhotoChange(index, event)}
        />
      ))}
      <Button
        type="button"
        fullWidth
        variant="contained"
        sx={{ mt: 1, mb: 1 }}
        onClick={handleAddPhotoField}
      >
        Add Another Photo
      </Button>
      <Button
        type="submit"
        fullWidth
        variant="contained"
        sx={{ mt: 3, mb: 2 }}
      >
        Add Product
      </Button>
    </Box>
  );
};

export default AddProduct;
