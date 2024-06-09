import * as React from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Typography from '@mui/material/Typography';
import { Button, CardActionArea, CardActions } from '@mui/material';

interface Product {
  idProduct: number;
  productName: string;
  subtitle: string;
  amountOf: number;
  price: number;
  photos: string[];
}

interface MultiActionAreaCardProps {
  product: Product;
}

export default function MultiActionAreaCard({ product }: MultiActionAreaCardProps) {
  const imageUrl = product.photos && product.photos.length > 0 ? product.photos[0] : '/static/images/default-image.jpg'; 
  console.log(imageUrl);
  return (
    <Card sx={{ margin: 5, width: 350 }}>
      <CardActionArea>
        <CardMedia
          component="img"
          height="130"
          image={imageUrl}
          alt={product.productName}
        />
        <CardContent>
          <Typography gutterBottom variant="h5" component="div">
            {product.productName}
          </Typography>
          <Typography variant="body2" color="text.secondary">
            {product.subtitle}
          </Typography>
        </CardContent>
      </CardActionArea>
      <CardActions>
        <Button size="small" color="primary">
          Dodaj do koszyka
        </Button>
      </CardActions>
    </Card>
  );
}
