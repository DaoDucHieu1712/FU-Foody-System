import {
  Card,
  CardHeader,
  CardBody,
  CardFooter,
  Typography,
  Button,
  ButtonGroup,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import axios from "../../shared/api/axiosConfig";
import CookieService from "../../shared/helper/cookieConfig";
import RemoveWishlist from "./components/wishlist/RemoveWishlist";
import { useDispatch } from "react-redux";
import { cartActions } from "../(auth)/shared/cartSlice";
import FormatPriceHelper from "../../shared/components/format/FormatPriceHelper";
import { useNavigate } from "react-router-dom";

const Wishlist = () => {
  const [wishlistItems, setWishlistItems] = useState([]);
  
  const userId = CookieService.getToken("fu_foody_id");
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleImageClick = (foodId) => {
    // Navigate to the food details route
    navigate(`/food-details/${foodId}`);
  };

  const fetchWishlist = async () => {
    try {
      await axios
        .get(`/api/Wishlist/GetWishlistByUserId?userId=${userId}`)
        .then((response) => {
          setWishlistItems(response);
        });
    } catch (error) {
      console.error("An error occurred while adding to the wishlist: ", error);
    }
  };

  useEffect(() => {
    fetchWishlist();
  }, [userId]);
  const reloadWishlist = async () => {
    await fetchWishlist();
  };

  

  const handleAddToCart =  async (cartItem) => {
    console.log(cartItem);
    console.log("ok");
    const item = {
      foodId: cartItem.id,
      foodName: cartItem.foodName,
      storeId: cartItem.storeId,
      img: cartItem.imageURL,
      price: cartItem.price,
      quantity: 1,
    };
    try {
      // Dispatch action to add to cart
      dispatch(cartActions.addToCart(item)); 
      await axios.delete(
        `/api/Wishlist/RemoveFromWishlistv2/${userId}/${cartItem.foodId}`
      );
      await fetchWishlist();
    } catch (error) {
      console.error("An error occurred while adding to the cart: ", error);
    }
  };


  

  return (
    <>
      <div className="container" style={{minHeight:"400px"}}>
      
        <Typography variant="h4">Món ăn yêu thích</Typography>
        <div className="grid grid-cols-4 gap-5 my-5">

         {wishlistItems && wishlistItems.length != 0 ? (
                   wishlistItems.map((item) => (
                    <Card
                      key={item.id}
                      className="max-w-[18rem] overflow-hidden rounded-none shadow-none"
                    >
                      <CardHeader
                        floated={false}
                        shadow={false}
                        color="transparent"
                        className="m-0 rounded-none"
                      >
                        <img
                          src={item.imageURL}
                          alt="ui/ux review check "
                          style={{ height: "216px", width:"100%" }}
                          onClick={() => handleImageClick(item.foodId)}
                        />
                      </CardHeader>
                      <CardBody className="px-0 py-3">
                        <div className="mb-2 flex items-center justify-between">
                          <Typography color="blue-gray" className="font-medium">
                            {item.foodName}
                          </Typography>
                          <Typography color="blue-gray" className="font-medium">
                            $ {FormatPriceHelper(item.price)} đ
                            
                          </Typography>
                        </div>
                        <Typography variant="small" className="font-normal">
                          Tình trạng:{" "}
                          {item.isOutStock ? (
                            <span className="font-bold" style={{ color: "red" }}>
                              Hết hàng
                            </span>
                          ) : (
                            <span className="font-bold" style={{ color: "green" }}>
                              Còn hàng
                            </span>
                          )}
                        </Typography>
                      </CardBody>
                      <CardFooter className="pt-0 px-0">
                        <ButtonGroup variant="outlined" fullWidth>
                          <Button  onClick={() => handleAddToCart(item)}
                            className="text-xs rounded-none px-0 bg-primary text-white px-0 border-orange-700"
                            disabled={item.isOutStock}
                          >
                            Thêm vào giỏ hàng
                          </Button>
        
                          <RemoveWishlist wishlistId={item.id} reloadWishlist={reloadWishlist}/>
                        </ButtonGroup>
                      </CardFooter>
                    </Card>
                  ))

                  ) : (
                   
                      <span className="">
                        Không có món ăn yêu thích nào!
                      </span>
                  
                  )}
          
        </div>
      </div>
    </>
  );
};
export default Wishlist;
