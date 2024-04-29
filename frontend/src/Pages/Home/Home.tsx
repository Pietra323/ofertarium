import MultiActionAreaCard from "../../Components/Card/Card";
import ResponsiveAppBar from "../../Components/Navbar/ResponsiveAppBar";
import ProductContainer from "../../Components/Productscontainer/ProductsContainer";

export function Home() {
  return (
    <>
        <ResponsiveAppBar></ResponsiveAppBar>
        <ProductContainer>
            <MultiActionAreaCard></MultiActionAreaCard>
            <MultiActionAreaCard></MultiActionAreaCard>
            <MultiActionAreaCard></MultiActionAreaCard>
        </ProductContainer>
        <ProductContainer>
            <MultiActionAreaCard></MultiActionAreaCard>
            <MultiActionAreaCard></MultiActionAreaCard>
            <MultiActionAreaCard></MultiActionAreaCard>
        </ProductContainer>
    </>
  );
}