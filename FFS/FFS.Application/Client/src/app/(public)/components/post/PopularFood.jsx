import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "../../../../shared/api/axiosConfig";


const PopularFood = () => {
  const navigate = useNavigate();
  const [bestSeller, setBestSeller] = useState([]);

  const GetListFilterFood = async () => {
      try {
          axios
              .get(`/api/Food/ListAllFood?FilterFood=2&PageNumber=1&PageSize=3`)
              .then((response) => {
                  setBestSeller(response.foodDTOs);
              })
              .catch((error) => {
                  console.log(error);
                  toast.error("Lấy sản phẩm bán chạy nhất thất bại!");
              });
      } catch (error) {
          console.error("Category: " + error);
      }
  };

  useEffect(() => {
      GetListFilterFood();
  }, []);
    return (
      <>
        <div className="food_newest border p-4 mt-6">
              <h1 className="text-lg font-bold uppercase">Món ăn yêu thích</h1>
              {bestSeller.map((bestSellerItem) => (
              <div className="flex mb-4 mt-4">
                {/* Image */}
                <div className="w-1/3">
                  <img
                    src={bestSellerItem.imageURL}
                    className="w-full h-auto"
                  />
                </div>
                {/* Title and Timestamp */}
                <div className="w-2/3 px-4">
                  <h2 className="text-sm font-bold">
                    {bestSellerItem.foodName}
                  </h2>
                  <p className="text-sm text-gray-500">12 Nov, 2023</p>
                </div>
              </div>
              ))}
            </div>
      </>
    );
  };
  
  export default PopularFood;
  