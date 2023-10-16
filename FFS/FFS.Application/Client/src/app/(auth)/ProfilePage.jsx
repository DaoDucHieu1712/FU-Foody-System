import { Button, Input, Radio } from "@material-tailwind/react";
import UploadImage from "../../shared/components/form/UploadImage";

const ProfilePage = () => {
  return (
    <>
      <div className="grid grid-cols-5 my-12 gap-4 p-8">
        <div className="flex flex-col col-span-1">
          <div className="flex gap-x-3 items-center p-3 border-b border-gray-300">
            <img
              src="https://images.unsplash.com/photo-1697081544011-e472e6a19cc8?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxlZGl0b3JpYWwtZmVlZHw1fHx8ZW58MHx8fHx8&auto=format&fit=crop&w=500&q=60"
              alt=""
              className="rounded-full w-[50px] h-[50px]"
            />
            <span className="text-center font-medium">dao duc hieu</span>
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
        <div className="flex flex-col col-span-3 shadow-md px-3">
          <div className="border-b border-gray-300 p-3">
            <h1 className="text-xl">Hồ sơ của tôi</h1>
            <p className="text-gray-400">
              Quản lý thông tin hồ sơ để bảo mật thông tin
            </p>
          </div>
          <div className="flex mt-3 p-3 pb-20 items-center">
            <form className="flex flex-col gap-y-6">
              <div className="grid grid-cols-3">
                <span className="font-semibold text-gray-500 ">
                  Tên đăng nhập
                </span>
                <p className="col-span-2">username</p>
              </div>
              <div className="grid grid-cols-3">
                <span className="font-semibold text-gray-500">Tên</span>
                <div className="flex w-full col-span-2 gap-x-3">
                  <Input label="First Name" />
                  <Input label="Last Name" />
                </div>
              </div>
              <div className="grid grid-cols-3">
                <span className="font-semibold text-gray-500 ">Email</span>
                <p className="col-span-2">
                  user@gmail.com{" "}
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
                  0123456789{" "}
                  <a href="#" className="text-blue-500">
                    Thay đổi
                  </a>
                </p>
              </div>
              <div className="grid grid-cols-3">
                <span className="font-semibold text-gray-500">Giới tính</span>
                <div className="flex gap-10 col-span-2">
                  <Radio name="gender" label="Nam" defaultChecked />
                  <Radio name="gender" label="Nữ" />
                </div>
              </div>
              <div className="grid grid-cols-3">
                <span className="font-semibold text-gray-500">Ngày sinh</span>
                <Input
                  type="datetime-local"
                  className="col-span-2 w-full"
                  label="Ngày sinh"
                />
              </div>
              <div className="grid grid-cols-3 mt-4">
                <Button className="bg-primary">Lưu</Button>
              </div>
            </form>
          </div>
        </div>
        <div className="flex flex-col col-span-1">
          <UploadImage></UploadImage>
        </div>
      </div>
    </>
  );
};

export default ProfilePage;
