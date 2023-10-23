import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import {
  Button,
  Input,
  Avatar,
  Spinner,
  Textarea,
} from "@material-tailwind/react";

const StoreProfilePage = () => {
  const { id } = useParams();

  const [storeData, setStoreData] = useState(null);
  const [isUpdate, setIsUpdate] = useState(false);

  const GetStoreInformation = async () => {
    try {
      axios
        .get(`/api/Store/GetStoreInformation/${id}`)
        .then((response) => {
          setStoreData(response);
        })
        .catch((error) => {
          toast.error(error.response.data);
        });
    } catch (error) {
      console.error("An error occurred", error);
    }
  };

  const handleUpdateBtn = () => {
    setIsUpdate(true);
  };

  const handleSaveBtn = () => {
    try {
      axios
        .put(`/api/Store/UpdateStore/${id}`, storeData)
        .then((response) => {
          console.log(response);
          setIsUpdate(false);
          toast.success("Cập nhật thành công!");
        })
        .catch((error) => {
          toast.error(error.response.data);
        });
    } catch (error) {
      console.error("An error occurred", error);
    }
  };

  const handleInputData = (e) => {
    const { name, value } = e.target;
    setStoreData({ ...storeData, [name]: value });
  };

  useEffect(() => {

    GetStoreInformation();
  }, [id]);
  return (
    <>
      {storeData ? (
        isUpdate ? (
          <div className="bg-white p-4 mt-4 shadow-md rounded-md">
            <div className="grid grid-rows-1 grid-cols-2 ">
              <div className="flex justify-center items-center">
                <Avatar
                  src={storeData.avatarURL}
                  className="w-40 h-40 rounded"
                />
              </div>
              <div className="grid grid-cols-2 grid-rows-4 gap-5">
                <div className="flex items-center">
                  <Input
                    variant="static"
                    label="Tên Cửa Hàng"
                    defaultValue={storeData.storeName}
                    onChange={handleInputData}
                    name="storeName"
                  />
                </div>
                <div className="flex items-center">
                  <Input
                    variant="static"
                    label="Email"
                    defaultValue={storeData.address}
                    onChange={handleInputData}
                    name="address"
                  />
                </div>
                <div className="flex items-center">
                  <Input
                    variant="static"
                    label="Số Điện Thoại"
                    defaultValue={storeData.phoneNumber}
                    onChange={handleInputData}
                    name="phoneNumber"
                  />
                </div>
                <div className="flex items-center">
                  <Input
                    variant="static"
                    label="Địa Chỉ"
                    defaultValue={storeData.address}
                    onChange={handleInputData}
                    name="address"
                  />
                </div>

                <div className="col-span-2">
                  <Textarea
                    label="Mô Tả"
                    defaultValue={storeData.description}
                    onChange={handleInputData}
                    name="description"
                  />
                </div>
                <div className="col-span-2 grid grid-cols-2 gap-5 h-10">
                  <div
                    className="relative mb-3 flex justify-center items-end"
                    data-te-input-wrapper-init
                    id="datetimepicker-dateOptions"
                  >
                    <p className="w-2/3 font-semibold">Mở Cửa</p>
                    <Input
                      variant="static"
                      placeholder="Static"
                      defaultValue={storeData.timeStart}
                      onChange={handleInputData}
                      name="timeStart"
                      type="time"
                    />
                  </div>
                  <div
                    className="relative mb-3 flex justify-center items-end"
                    data-te-input-wrapper-init
                    id="datetimepicker-dateOptions"
                  >
                    <p className="w-2/3 font-semibold">Đóng Cửa</p>
                    <Input
                      variant="static"
                      placeholder="Static"
                      defaultValue={storeData.timeEnd}
                      onChange={handleInputData}
                      name="timeEnd"
                      type="time"
                    />
                  </div>
                </div>
              </div>
            </div>
            <div className="mt-5 flex justify-center items-center">
              <Button className="bg-primary" onClick={handleSaveBtn}>
                Lưu
              </Button>
            </div>
          </div>
        ) : (
          <div className="bg-white p-4 mt-4 shadow-md rounded-md">
            <div className="grid grid-rows-1 grid-cols-2 ">
              <div className="flex justify-center items-center">
                <Avatar
                  src={storeData.avatarURL}
                  className="w-40 h-40 rounded"
                />
              </div>
              <div className="grid grid-cols-2 grid-rows-4 gap-5">
                <div className="flex items-center">
                  <Input
                    variant="static"
                    label="Tên Cửa Hàng"
                    defaultValue={storeData.storeName}
                    readOnly
                  />
                </div>
                <div className="flex items-center">
                  <Input
                    variant="static"
                    label="Email"
                    defaultValue={storeData.address}
                    readOnly
                  />
                </div>
                <div className="flex items-center">
                  <Input
                    variant="static"
                    label="Số Điện Thoại"
                    defaultValue={storeData.phoneNumber}
                    readOnly
                  />
                </div>
                <div className="flex items-center">
                  <Input
                    variant="static"
                    label="Địa Chỉ"
                    defaultValue={storeData.address}
                    readOnly
                  />
                </div>

                <div className="col-span-2">
                  <Textarea
                    label="Mô Tả"
                    defaultValue={storeData.description}
                    readOnly
                  />
                </div>
                <div className="col-span-2">
                  <p>
                    Mở Cửa: {storeData.timeStart} - Đóng Cửa:{" "}
                    {storeData.timeEnd}
                  </p>
                </div>
              </div>
            </div>
            <div className="mt-5 flex justify-center items-center">
              <Button className="bg-primary" onClick={handleUpdateBtn}>
                Cập Nhật
              </Button>
            </div>
          </div>
        )
      ) : (
        <div className="h-full">
          <Spinner></Spinner>
        </div>
      )}
    </>
  );
};

export default StoreProfilePage;
