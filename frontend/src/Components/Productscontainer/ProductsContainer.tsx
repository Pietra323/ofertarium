import { ReactNode} from 'react';

interface UserPageLayoutProps {
    children: ReactNode;
  }

  export default function ProductContainer({ children }: UserPageLayoutProps) {

    return (
    <>
    <div className="label" style={{
        marginTop: "1.5rem",
        marginLeft: "150px",
        fontSize: "1.5rem",
        overflow: "hidden"
      }}>Kategoria: test</div>
      <div className="ProductContainer" style={{
        display: "flex",
        width: "100vw",
        overflow: "hidden",
        flexWrap: "wrap",
        justifyContent: "center",
        marginLeft: "-30px"
      }}>
          {children}     
      </div>
    </>
    );
  }