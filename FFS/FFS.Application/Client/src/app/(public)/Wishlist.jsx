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

const Wishlist = () => {
  const [wishlistItems, setWishlistItems] = useState([]);
  const userId = CookieService.getToken("fu_foody_id");
  const dispatch = useDispatch();

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
      <div className="container">
        <h1 className="text-xl font-bold">Món ăn yêu thích</h1>
        <div className="grid grid-cols-4 gap-5 my-5">
          {wishlistItems.map((item) => (
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
                />
              </CardHeader>
              <CardBody className="px-0 py-3">
                <div className="mb-2 flex items-center justify-between">
                  <Typography color="blue-gray" className="font-medium">
                    {item.foodName}
                  </Typography>
                  <Typography color="blue-gray" className="font-medium">
                    ${item.price}.000 VND
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
          ))}
        </div>
      </div>
    </>
  );
};
export default Wishlist;
