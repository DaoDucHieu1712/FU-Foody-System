import { useParams } from "react-router-dom";
import axios from "../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import { Button, Input, Spinner, Typography } from "@material-tailwind/react";
import ReportStore from "../(public)/components/ReportStore";
import Cookies from "universal-cookie";

const cookies = new Cookies();
const uId = cookies.get("fu_foody_id");
console.log("uid = ", uId);


const StoreDetailPage = () => {
  const { id } = useParams();
  const backgroundColors = ["bg-gray-50", "bg-gray-200"];
  const [storeData, setStoreData] = useState(null);
  const [foodList, setFoodList] = useState([]);
  const [searchFood, setSearchFood] = useState("");

  const GetStoreInformation = async () => {
    try {
      axios
        .get(`/api/Store/DetailStore/${id}`)
        .then((response) => {
          setStoreData(response);
          setFoodList(response.foods);
        })
        .catch((error) => {
          console.log(error);
        });
    } catch (error) {
      console.error("An error occurred", error);
    }
  };
  useEffect(() => {
    GetStoreInformation();
  }, [id]);

  const handleChangeCategory = (idCategory) => {
    console.log(idCategory);
    try {
      axios
        .get(`/api/Store/GetFoodByCategory/${id}/${idCategory}`)
        .then((response) => {
          setFoodList(response);
        })
        .catch((error) => {
          console.log(error);
        });
    } catch (error) {
      console.error("An error occurred", error);
    }
  };

  const handleSearchFood = (e) => {
    const serachTxt = e.target.value;
    setSearchFood(serachTxt);
    console.log(serachTxt.length);
    try {
      axios
        .get(`/api/Store/GetFoodByName?name=${serachTxt}`)
        .then((response) => {
          setFoodList(response);
        })
        .catch((error) => {
          console.log(error);
        });
    } catch (error) {
      console.error("An error occurred", error);
    }
  };
  const handleViewComment = () => {
    window.location = `/store/comment/${id}`;
  };
  return (
    <>
      {storeData ? (
        <div>
          <div className="grid grid-cols-5 gap-10">
            <div className="col-span-2">
              <img
                className="w-full object-cover object-center"
                src={storeData.avatarURL}
                alt="nature image"
              />
            </div>
            <div className="col-span-3 flex flex-col gap-1">
              <div className="flex items-center space-x-10">
                <span className="text-base">Quán ăn</span>
                <ReportStore uId={uId} sId={storeData.userId} />
              </div>
              <Typography variant="h2" className="">
                {storeData.storeName}
              </Typography>
              <span className="text-base">
                <i className="fas fa-map mr-1"></i>
                {storeData.address}
              </span>
              <span className="text-base">
                <i className="fal fa-phone mr-2"></i>
                Liên hệ : {storeData.phoneNumber}
              </span>
              <span
                className="text-base text-primary cursor-pointer"
                onClick={handleViewComment}
              >
                Xem thêm lượt đánh giá
              </span>

              <div className="flex gap-1 items-center text-base">
                <span>
                  {" "}
                  <i className="fal fa-clock mr-2"></i>Hoạt động từ:
                </span>
                <span>
                  {storeData.timeStart} : {storeData.timeEnd}
                </span>
              </div>
            </div>
          </div>
          <hr className="h-px my-4 bg-gray-400 border-0 dark:bg-gray-700"></hr>
          <div className="grid grid-cols-6">
            <div className="col-span-1">
              <Typography variant="h6" color="red" className="text-center">
                THỰC ĐƠN ({storeData.categories.length})
              </Typography>
              <ul className="m-2 bg-gray-100 rounded">
                {storeData.categories.map((item) => (
                  <li key={item.id}>
                    <Typography
                      className="p-1 text-center font-semibold cursor-pointer hover:bg-primary hover:text-white"
                      onClick={() => handleChangeCategory(item.id)}
                    >
                      {item.categoryName}
                    </Typography>
                  </li>
                ))}
              </ul>
            </div>
            <div className="col-span-5">
              <div>
                <br></br>
              </div>
              <div className="border-solid border-l-[1px] border-gray-400">
                <div className="p-3">
                  <Input
                    label="Tìm món"
                    icon={
                      <svg
                        width="20"
                        height="20"
                        viewBox="0 0 20 20"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                      >
                        <path
                          d="M9.0625 15.625C12.6869 15.625 15.625 12.6869 15.625 9.0625C15.625 5.43813 12.6869 2.5 9.0625 2.5C5.43813 2.5 2.5 5.43813 2.5 9.0625C2.5 12.6869 5.43813 15.625 9.0625 15.625Z"
                          stroke="#191C1F"
                          strokeWidth="1.5"
                          strokeLinecap="round"
                          strokeLinejoin="round"
                        />
                        <path
                          d="M13.7031 13.7031L17.5 17.5"
                          stroke="#191C1F"
                          strokeWidth="1.5"
                          strokeLinecap="round"
                          strokeLinejoin="round"
                        />
                      </svg>
                    }
                    defaultValue={searchFood}
                    onChange={handleSearchFood}
                  />
                </div>
                <div className="border-solid border-t-[1px] border-gray-400">
                  <ul>
                    {foodList.map((item, index) => (
                      <li
                        className={`p-2 ${backgroundColors[index % backgroundColors.length]
                          }`}
                        key={item.id}
                      >
                        <div className="border-collapse grid grid-cols-6 gap-5">
                          <div className="col-span-2">
                            <img
                              className="w-full h-52 rounded-lg object-cover"
                              src={item.imageURL}
                            />
                          </div>
                          <div className="col-span-3">
                            <Typography variant="h6">
                              {item.foodName}
                            </Typography>
                            <Typography variant="paragraph" className="py-2">
                              {item.description}
                            </Typography>
                          </div>
                          <div className="col-span-1">
                            <Typography
                              variant="paragraph"
                              className="relative w-fit"
                            >
                              {item.price},000
                              <span className="absolute font-normal top-0 -right-2 text-xs">
                                đ
                              </span>
                            </Typography>
                            <Button size="sm" className="bg-primary">
                              <svg
                                width="30"
                                height="2.5em"
                                viewBox="0 0 30 40"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                              >
                                <path
                                  d="M10 35C11.1046 35 12 34.1046 12 33C12 31.8954 11.1046 31 10 31C8.89543 31 8 31.8954 8 33C8 34.1046 8.89543 35 10 35Z"
                                  fill="white"
                                />
                                <path
                                  d="M23 35C24.1046 35 25 34.1046 25 33C25 31.8954 24.1046 31 23 31C21.8954 31 21 31.8954 21 33C21 34.1046 21.8954 35 23 35Z"
                                  fill="white"
                                />
                                <path
                                  d="M5.2875 15H27.7125L24.4125 26.55C24.2948 26.9692 24.0426 27.3381 23.6948 27.6001C23.3471 27.862 22.9229 28.0025 22.4875 28H10.5125C10.0771 28.0025 9.65293 27.862 9.30515 27.6001C8.95738 27.3381 8.70524 26.9692 8.5875 26.55L4.0625 10.725C4.0027 10.5159 3.8764 10.3321 3.70271 10.2012C3.52903 10.0704 3.31744 9.99977 3.1 10H1"
                                  stroke="white"
                                  strokeWidth="2"
                                  strokeLinecap="round"
                                  strokeLinejoin="round"
                                />
                              </svg>
                            </Button>
                          </div>
                        </div>
                      </li>
                    ))}
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
      ) : (
        <Spinner></Spinner>
      )}
    </>
  );
};

export default StoreDetailPage;
