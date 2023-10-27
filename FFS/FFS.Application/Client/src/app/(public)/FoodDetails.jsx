import { Rating, Typography } from "@material-tailwind/react";
import { useState } from "react";
import propTypes from "prop-types";

const FoodDetails = ({ foodData }) => {
  const [value, setValue] = useState(0);
  console.log(foodData);
  const increaseValue = () => {
    setValue(value + 1);
  };

  const decreaseValue = () => {
    if (value > 0) {
      setValue(value - 1);
    }
  };

  return (
    <>
      <div className="container mt-8 mb-8 px-12 py-4">
        <div className="grid grid-cols-[4fr,6fr] gap-12">
          <div className="Sidebar">
            <img
              className="h-90 w-full object-cover object-center"
              src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
              alt="nature image"
            />
          </div>
          <div className="content-food">
            <div className="flex items-center gap-2 font-bold">
              <Rating value={2} readonly />
              <Typography color="" className="font-medium">
                4.7 Sao
              </Typography>
            </div>
            <div className="food-name mx-1">
              <h1 className="text-lg font-bold">Sushi Cá Hồi</h1>
              <p className="text-base">Phân loại: Đồ ăn</p>
              <p className="text-base">Tình trạng: còn hàng</p>
              <p className="text-base">
                Mô tả: LeCastella chỉ bán bánh tươi trong ngày không chất bảo
                quản Nếu chưa dùng hết quý khách có thể bảo quản tủ lạnh 2-3
                ngày/khi dùng mons sấy trong lò vi sóng 2p cùng 1 BÁT NƯỚC NÓNG
                để bánh mềm trở lại
              </p>
              <p className="my-2 text-base font-bold flex items-center ">
                <span className="rounded-full">
                  <svg
                    className="w-4 h-4 text-blue-500 dark:text-white"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 11 20"
                  >
                    <path
                      stroke="currentColor"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      d="M1.75 15.363a4.954 4.954 0 0 0 2.638 1.574c2.345.572 4.653-.434 5.155-2.247.502-1.813-1.313-3.79-3.657-4.364-2.344-.574-4.16-2.551-3.658-4.364.502-1.813 2.81-2.818 5.155-2.246A4.97 4.97 0 0 1 10 5.264M6 17.097v1.82m0-17.5v2.138"
                    />
                  </svg>
                </span>
                <span className="text-blue-500">25.000 VND</span>
              </p>

              <hr></hr>
              <div className="flex items-center space-x-3 mt-3">
                <div className="h-10 w-28">
                  <div className="flex flex-row h-10 w-full relative bg-transparent border border-gray-300">
                    <button
                      className="h-full w-20 rounded-sm cursor-pointer outline-none"
                      onClick={decreaseValue}
                    >
                      <span className="m-auto text-2xl font-thin">−</span>
                    </button>
                    <input
                      type="number"
                      className="text-center w-full font-semibold text-md hover:text-black focus:text-black cursor-default flex items-center outline-none"
                      value={value}
                      readOnly
                    />
                    <button
                      className="h-full w-20 rounded-sm cursor-pointer outline-none"
                      onClick={increaseValue}
                    >
                      <span className="m-auto text-2xl font-thin">+</span>
                    </button>
                  </div>
                </div>

                <div className="">
                  <button
                    type="submit"
                    className="flex items-center space-x-2  text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-sm text-sm w-full px-5 py-2.5 text-center"
                  >
                    THÊM VÀO GIỎ HÀNG
                    <svg
                      className="w-4 h-4 text-white ml-1"
                      aria-hidden="true"
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 18 20"
                    >
                      <path
                        stroke="currentColor"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        width="2"
                        d="M6 15a2 2 0 1 0 0 4 2 2 0 0 0 0-4Zm0 0h8m-8 0-1-4m9 4a2 2 0 1 0 0 4 2 2 0 0 0 0-4Zm-9-4h10l2-7H3m2 7L3 4m0 0-.792-3H1"
                      />
                    </svg>
                  </button>
                </div>
              </div>
              <div className="flex items-center space-x-2 mt-3">
                <div className="h-10 w-28">
                </div>

                <div className="">
                  <button
                    type="submit"
                    className="flex items-center space-x-2  text-dark focus:ring-4 focus:outline-none font-medium text-sm w-full px-5 py-2.5 text-center"
                  >

                    <svg className="w-4 h-4 mr-2 text-gray-800 dark:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 21 19">
                      <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" width="2" d="M11 4C5.5-1.5-1.5 5.5 4 11l7 7 7-7c5.458-5.458-1.542-12.458-7-7Z" />
                    </svg>
                    Thêm vào Wishlist
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="">
          Recomment
        </div>
        <div className="">
          Comment
        </div>
      </div>
    </>
  );
};
FoodDetails.propTypes = {
  foodData: propTypes.any.isRequired,
};
export default FoodDetails;
