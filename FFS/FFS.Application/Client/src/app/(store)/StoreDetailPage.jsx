import { useParams } from "react-router-dom";
import axios from "../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import { Avatar, Button, Spinner } from "@material-tailwind/react";
const StoreDetailPage = () => {
  const { id } = useParams();
  const [storeData, setStoreData] = useState(null);

  const GetStoreInformation = async () => {
    try {
      axios
        .get(`/api/Store/DetailStore/${id}`)
        .then((response) => {
          setStoreData(response);
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
              <ul>
                <li>Phân Loại</li>
                {storeData.categories.map((item) => {
                  <li className="p-3">
                    <Button variant="outlined">
                      {item.categoryName} - {item.id}
                    </Button>
                  </li>;
                })}
              </ul>
            </div>
            <div className="col-span-5">Food</div>
          </div>
        </div>
      ) : (
        <Spinner></Spinner>
      )}
    </>
  );
};

export default StoreDetailPage;
