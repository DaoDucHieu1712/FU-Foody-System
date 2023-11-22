import React, { useEffect, useState } from "react";
import axios from "../../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import CookieService from "../../../../shared/helper/cookieConfig";

const WishlistDetails = ({ foodId }) => {
  const userId = CookieService.getToken("fu_foody_id");
  const [isInWishlist, setIsInWishlist] = useState(false);

  useEffect(() => {
    const checkWishlistStatus = async () => {
      try {
        const response = await axios.get(
          `/api/Wishlist/IsInWishlist?userId=${userId}&foodId=${foodId}`
        );
        setIsInWishlist(response);
      } catch (error) {
        console.error("Error checking wishlist status:", error);
      }
    };

    checkWishlistStatus();
  }, [foodId]);

  const addToWishlist = async () => {
    try {
      await axios.post(
        `/api/Wishlist/AddToWishlist?userId=${userId}&foodId=${foodId}`
      );
      setIsInWishlist(true);
      toast.success("Thêm vào wishlist thành công !");
    } catch (error) {
      console.error("Error adding to wishlist:", error);
      toast.error("Món ăn này đã có trong wishlist");
    }
  };

  const removeFromWishlist = async () => {
    try {
      await axios.delete(
        `/api/Wishlist/RemoveFromWishlistv2/${userId}/${foodId}`
      );
      toast.success("Xóa khỏi wishlist thành công !");
      setIsInWishlist(false);
    } catch (error) {
      console.error("Error removing from wishlist:", error);
    }
  };

  return (
    <button
      type="button"
      className="flex items-center space-x-2 text-dark font-medium text-sm w-full px-5 py-2.5 text-center"
      onClick={() => (isInWishlist ? removeFromWishlist() : addToWishlist())}
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24"
        strokeWidth={1.5}
        className={`w-5 h-5 mr-1 ${isInWishlist ? 'fill-[#fe5303] stroke-[#fe5303]' : 'fill-none stroke-current'}`}
      >
        <path
          strokeLinecap="round"
          strokeLinejoin="round"
          d="M21 8.25c0-2.485-2.099-4.5-4.688-4.5-1.935 0-3.597 1.126-4.312 2.733-.715-1.607-2.377-2.733-4.313-2.733C5.1 3.75 3 5.765 3 8.25c0 7.22 9 12 9 12s9-4.78 9-12z"
        />
      </svg>
      {isInWishlist ? "Xóa khỏi Wishlist" : "Thêm vào Wishlist"}
    </button>
  );
};

export default WishlistDetails;
