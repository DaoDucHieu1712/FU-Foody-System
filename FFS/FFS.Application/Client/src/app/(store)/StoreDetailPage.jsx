import { useParams } from "react-router-dom";
import axios from "../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import { Button, Input, Spinner } from "@material-tailwind/react";
const StoreDetailPage = () => {
  const { id } = useParams();
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
  return (
    <>
      {storeData ? (
        <div>
          <div className="grid grid-cols-5 gap-5 mb-10 ">
            <div className="col-span-2">
              <img
                className="h-50 w-full rounded-lg object-cover object-center shadow-xl shadow-blue-gray-900/50"
                src={storeData.avatarURL}
                alt="nature image"
              />
            </div>
            <div className="col-span-3 flex flex-col">
              <span className="text-base m-4">Quán ăn</span>
              <span className="text-6xl mb-8">{storeData.storeName}</span>
              <span className="text-base mb-4">{storeData.address}</span>
              <span className="text-base mb-4">
                Liên hệ : {storeData.phoneNumber}
              </span>

              <div className="flex items-center text-base mb-4">
                <span>Hoạt động từ</span>
                <span className="p-3">
                  {storeData.timeStart} : {storeData.timeEnd}
                </span>
              </div>
            </div>
          </div>
          <div className="grid grid-cols-6 border-solid border-t-[1px] border-black pt-3">
            <div className="col-span-1">
              <p className="text-center">
                Phân Loại({storeData.categories.length})
              </p>
              <ul>
                {storeData.categories.map((item) => (
                  <li className="p-3" key={item.id}>
                    <Button
                      className="w-full h-10 bg-primary"
                      onClick={() => handleChangeCategory(item.id)}
                    >
                      {item.categoryName}
                    </Button>
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
                    defaultValue={searchFood}
                    onChange={handleSearchFood}
                  />
                </div>
                <div className="border-solid border-t-[1px] border-gray-400">
                  <ul>
                    {foodList.map((item) => (
                      <li className="p-3" key={item.id}>
                        <div className="border-collapse grid grid-cols-6">
                          <div className="col-span-2">
                            <img
                              className="w-full h-52 rounded-lg object-cover"
                              src={item.imageURL}
                            />
                          </div>
                          <div className="col-span-3 ml-3">
                            <p className="font-bold">{item.foodName}</p>
                            <p>{item.description}</p>
                          </div>
                          <div className="col-span-1">
                            <p>{item.price}</p>
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