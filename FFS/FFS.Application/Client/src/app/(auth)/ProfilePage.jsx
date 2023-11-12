import { Button, Input, Radio } from "@material-tailwind/react";
import UploadImage from "../../shared/components/form/UploadImage";
import { useLocation } from "react-router-dom";
import FormatDateString from "../../shared/components/format/FormatDate";
import { useEffect, useState } from "react";
import axios from "../../shared/api/axiosConfig";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import ErrorText from "../../shared/components/text/ErrorText";
import UpdateImage from "../../shared/components/form/UpdateImage";
import { useSelector, useDispatch } from "react-redux";
import { setUserProfile } from "../../redux/auth";
import dayjs from "dayjs";

const schema = yup.object({
  firstName: yup.string().required("Hãy điền tên!"),
  lastName: yup.string().required("Hãy điền họ!"),
  // avatar: yup.string().required("Hãy thêm ảnh!"),
});
const ProfilePage = () => {
  const dispatch = useDispatch();
  const user = useSelector((state) => state.auth.userProfile);
  

  const {
    register,
    setValue,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
    mode: "onSubmit",
  });

  const onSubmit = async (data) => {
  
    try {
      const updatedUserData = {
        ...user,
        avatar: data.avatar ? data.avatar : user.avatar,
        firstName: data.firstName,
        lastName: data.lastName,
        gender: data.gender === "true",
        allow: user.allow,
        birthDay: data.birthDay,
      };


      const response = await axios.put(
        `/api/Authenticate/Profile?email=${user.email}`,
        updatedUserData
      );
      console.log("edit",response);

      

      if (response) {
        dispatch(setUserProfile(updatedUserData));
        toast.success("Sửa profile thành công!");
      }
    } catch (error) {
      console.error("Error updating profile: ", error);
      toast.error("Sửa profile thất bại!");
    }
  };

  return (
    <>
      {user && (
        <div className="grid grid-cols-5 my-12 gap-4 p-8">
          <div className="flex flex-col col-span-1">
            <div className="flex gap-x-2 items-center p-3 border-b border-gray-300">
              <img
                src={user.avatar}
                alt=""
                className="rounded-full w-[45px] h-[45px]"
              />
              <span className="text-center font-medium">
                {user.firstName} {user.lastName}
              </span>
            </div>

            <div className="mt-3">
              <ul className="flex flex-col items-center gap-y-3 text-gray-700">
                <li>
                  <a href="#" className="text-primary">
                    Hồ Sơ
                  </a>
                </li>
                <li>
                  <a href="#">Ngân hàng</a>
                </li>
                <li>
                  <a href="#">Địa chỉ</a>
                </li>
                <li>
                  <a href="#">Đổi mật khẩu</a>
                </li>
                <li>
                  <a href="#">Cài đặt</a>
                </li>
                <li>
                  <a href="#">Đơn hàng</a>
                </li>
                <li>
                  <a href="#">Thông báo</a>
                </li>
              </ul>
            </div>
          </div>

          <div className="flex flex-col col-span-4 shadow-md px-3">
            <div className="border-b border-gray-300 p-3">
              <h1 className="text-xl">Hồ sơ của tôi</h1>
              <p className="text-gray-400">
                Quản lý thông tin hồ sơ để bảo mật thông tin
              </p>
            </div>
            <form onSubmit={handleSubmit(onSubmit)}>
              <div className="flex mt-3 p-3 pb-20 items-center">
                <div className="flex flex-col gap-y-6 w-3/4 pr-6">
                  <div className="grid grid-cols-3">
                    <span className="font-semibold text-gray-500 ">
                      Tên đăng nhập
                    </span>
                    <p className="col-span-2">{user.userName}</p>
                  </div>
                  <div className="grid grid-cols-3">
                    <span className="font-semibold text-gray-500">Tên</span>
                    <div className="flex w-full col-span-2 gap-x-3">
                      <div>
                        <Input
                          label="First Name"
                          {...register("firstName")}
                          defaultValue={user.firstName}
                        />
                        {errors.firstName && (
                          <ErrorText
                            text={errors.firstName.message}
                          ></ErrorText>
                        )}
                      </div>

                      <div>
                        <Input
                          label="Last Name"
                          {...register("lastName")}
                          defaultValue={user.lastName}
                        />
                        {errors.lastName && (
                          <ErrorText text={errors.lastName.message}></ErrorText>
                        )}
                      </div>
                    </div>
                  </div>

                  <div className="grid grid-cols-3">
                    <span className="font-semibold text-gray-500 ">Email</span>
                    <p className="col-span-2">
                      {user.email}{" "}
                      <a href="#" className="text-blue-500">
                        Thay đổi
                      </a>
                    </p>
                  </div>
                  <div className="grid grid-cols-3">
                    <span className="font-semibold text-gray-500 ">
                      Số điện thoại
                    </span>
                    <p className="col-span-2">
                      {user.phoneNumber}{" "}
                      <a href="#" className="text-blue-500">
                        Thay đổi
                      </a>
                    </p>
                  </div>
                  <div className="grid grid-cols-3">
                    <span className="font-semibold text-gray-500">
                      Giới tính
                    </span>
                    <div className="flex gap-10 col-span-2">
                      <Radio
                        {...register("gender", { required: true })}
                        name="gender"
                        label="Nam"
                        defaultChecked={user.gender === true}
                      />
                      <Radio
                        {...register("gender", { required: true })}
                        name="gender"
                        label="Nữ"
                        defaultChecked={user.gender === false}
                      />
                    </div>
                  </div>
                  <div className="grid grid-cols-3">
                    <span className="font-semibold text-gray-500">
                      Ngày sinh
                    </span>
                    <Input
                      {...register("birthDay")}
                      type="datetime-local"
                      className="col-span-2 w-full"
                      label="Ngày sinh"                    
                      defaultValue={user.birthDay}
                 

                    />
                  </div>
                  <div className="grid grid-cols-3 mt-4">
                    <Button type="submit" className="bg-primary">
                      Lưu
                    </Button>
                  </div>
                </div>
                <div className="border-l-2 border-gray-400 pl-6 w-1/4">
                  <UpdateImage
                    name="avatar"
                    onChange={setValue}
                    url={user.avatar}
                  ></UpdateImage>
                </div>
              </div>
            </form>
          </div>
        </div>
      )}
    </>
  );
};

export default ProfilePage;
