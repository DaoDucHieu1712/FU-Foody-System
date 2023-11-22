import { useEffect, useState } from "react";
import { NavLink, Outlet } from "react-router-dom";
import Cookies from "universal-cookie";

const navigations = [
  { href: "/store/manager", name: "Cửa hàng của tôi" },
  { href: "/category", name: "Danh mục của tôi" },
  { href: "/food", name: "Thực phẩm" },
  { href: "/store/order", name: "Đơn hàng" },
];

var cookies = new Cookies();
const StoreLayout = () => {
  const [email, setEmail] = useState();
  useEffect(() => {
    setEmail(cookies.get("fu_foody_email"));
  }, []);
  return (
    <div className="flex">
      <div className="p-3 flex flex-col gap-y-12 h-[100vh] shadow-md w-[20vw]">
        <div className="flex items-center justify-center border-b border-gray-300">
          <a href="/store/manager" className="logo">
            <svg
              width="115"
              height="48"
              viewBox="0 0 194 48"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                fillRule="evenodd"
                clipRule="evenodd"
                d="M48 24C48 37.2548 37.2548 48 24 48C10.7452 48 0 37.2548 0 24C0 10.7452 10.7452 0 24 0C37.2548 0 48 10.7452 48 24ZM36 24C36 30.6274 30.6274 36 24 36C17.3726 36 12 30.6274 12 24C12 17.3726 17.3726 12 24 12C30.6274 12 36 17.3726 36 24ZM24 32C28.4183 32 32 28.4183 32 24C32 19.5817 28.4183 16 24 16C19.5817 16 16 19.5817 16 24C16 28.4183 19.5817 32 24 32Z"
                fill="#FE5303"
              />
              <path
                d="M58.592 12.864H74.336L74.32 16.768H63.296V22.576H72.848V26.4H63.296V36H58.592V12.864ZM96.089 27.184C96.089 29.0507 95.785 30.672 95.177 32.048C94.569 33.4133 93.5823 34.4693 92.217 35.216C90.8623 35.952 89.065 36.32 86.825 36.32C84.5743 36.32 82.7717 35.9467 81.417 35.2C80.0623 34.4427 79.081 33.3707 78.473 31.984C77.8757 30.5973 77.577 28.944 77.577 27.024V12.864H82.329V27.376C82.329 29.104 82.745 30.368 83.577 31.168C84.409 31.9573 85.4917 32.352 86.825 32.352C87.7103 32.352 88.4943 32.1813 89.177 31.84C89.8597 31.488 90.393 30.9493 90.777 30.224C91.1717 29.488 91.369 28.5387 91.369 27.376V12.864H96.089V27.184ZM106.235 12.864H121.979L121.963 16.768H110.939V22.576H120.491V26.4H110.939V36H106.235V12.864ZM131.075 36.32C129.454 36.32 128.041 35.9733 126.835 35.28C125.641 34.5867 124.713 33.6053 124.051 32.336C123.401 31.0667 123.075 29.5627 123.075 27.824C123.075 26.0853 123.401 24.5653 124.051 23.264C124.713 21.9627 125.641 20.9493 126.835 20.224C128.041 19.4987 129.459 19.136 131.091 19.136C132.723 19.136 134.137 19.4933 135.331 20.208C136.526 20.912 137.449 21.9147 138.099 23.216C138.75 24.5173 139.075 26.0533 139.075 27.824C139.075 29.488 138.755 30.96 138.115 32.24C137.486 33.5093 136.574 34.5067 135.379 35.232C134.195 35.9573 132.761 36.32 131.075 36.32ZM131.091 32.848C131.913 32.848 132.574 32.608 133.075 32.128C133.577 31.648 133.939 31.0187 134.163 30.24C134.398 29.4507 134.515 28.592 134.515 27.664C134.515 26.7893 134.409 25.9627 134.195 25.184C133.993 24.4053 133.641 23.776 133.139 23.296C132.638 22.816 131.955 22.576 131.091 22.576C130.27 22.576 129.603 22.8053 129.091 23.264C128.59 23.712 128.222 24.3253 127.987 25.104C127.753 25.872 127.635 26.7253 127.635 27.664C127.635 28.528 127.742 29.36 127.955 30.16C128.169 30.9493 128.526 31.5947 129.027 32.096C129.529 32.5973 130.217 32.848 131.091 32.848ZM148.654 36.32C147.033 36.32 145.619 35.9733 144.414 35.28C143.219 34.5867 142.291 33.6053 141.63 32.336C140.979 31.0667 140.654 29.5627 140.654 27.824C140.654 26.0853 140.979 24.5653 141.63 23.264C142.291 21.9627 143.219 20.9493 144.414 20.224C145.619 19.4987 147.038 19.136 148.67 19.136C150.302 19.136 151.715 19.4933 152.91 20.208C154.105 20.912 155.027 21.9147 155.678 23.216C156.329 24.5173 156.654 26.0533 156.654 27.824C156.654 29.488 156.334 30.96 155.694 32.24C155.065 33.5093 154.153 34.5067 152.958 35.232C151.774 35.9573 150.339 36.32 148.654 36.32ZM148.67 32.848C149.491 32.848 150.153 32.608 150.654 32.128C151.155 31.648 151.518 31.0187 151.742 30.24C151.977 29.4507 152.094 28.592 152.094 27.664C152.094 26.7893 151.987 25.9627 151.774 25.184C151.571 24.4053 151.219 23.776 150.718 23.296C150.217 22.816 149.534 22.576 148.67 22.576C147.849 22.576 147.182 22.8053 146.67 23.264C146.169 23.712 145.801 24.3253 145.566 25.104C145.331 25.872 145.214 26.7253 145.214 27.664C145.214 28.528 145.321 29.36 145.534 30.16C145.747 30.9493 146.105 31.5947 146.606 32.096C147.107 32.5973 147.795 32.848 148.67 32.848ZM165.273 36.32C163.054 36.32 161.326 35.5573 160.089 34.032C158.851 32.496 158.233 30.3627 158.233 27.632C158.233 25.904 158.483 24.4053 158.985 23.136C159.486 21.8667 160.227 20.8853 161.209 20.192C162.19 19.488 163.39 19.136 164.809 19.136C165.406 19.136 165.961 19.2053 166.473 19.344C166.995 19.472 167.47 19.6533 167.897 19.888C168.323 20.1227 168.697 20.3947 169.017 20.704C169.337 21.0133 169.598 21.344 169.801 21.696V12.224H174.489V36H171.097L170.569 32.608C170.419 33.0667 170.211 33.5147 169.945 33.952C169.678 34.3893 169.337 34.7893 168.921 35.152C168.505 35.504 167.993 35.7867 167.385 36C166.787 36.2133 166.083 36.32 165.273 36.32ZM166.297 32.96C167.47 32.96 168.345 32.5493 168.921 31.728C169.507 30.9067 169.801 29.52 169.801 27.568C169.79 26.4373 169.657 25.4987 169.401 24.752C169.145 24.0053 168.761 23.4507 168.249 23.088C167.747 22.7147 167.107 22.528 166.329 22.528C165.315 22.528 164.473 22.9173 163.801 23.696C163.129 24.464 162.793 25.7547 162.793 27.568C162.793 29.4133 163.107 30.7733 163.737 31.648C164.366 32.5227 165.219 32.96 166.297 32.96ZM180.44 41.28C179.309 41.28 178.509 41.184 178.04 40.992C177.581 40.8107 177.352 40.72 177.352 40.72V37.744L178.984 37.808C179.698 37.8507 180.274 37.8133 180.712 37.696C181.149 37.5893 181.48 37.4453 181.704 37.264C181.938 37.0827 182.104 36.8907 182.2 36.688C182.296 36.496 182.365 36.3413 182.408 36.224L182.712 35.312L176.28 19.456H180.904L184.872 30.24L188.888 19.456H193.448L186.744 36.096C186.168 37.504 185.56 38.5813 184.92 39.328C184.28 40.0747 183.586 40.5867 182.84 40.864C182.104 41.1413 181.304 41.28 180.44 41.28Z"
                fill="#FE5303"
              />
            </svg>
          </a>
        </div>
        <div className="list flex flex-col justify-center items-center gap-y-10 text-md font-medium text-white">
          {navigations.map((item) => {
            return (
              <NavLink
                key={item.href}
                to={item.href}
                className="lowercase text-gray-700"
              >
                {item.name}
              </NavLink>
            );
          })}
          <button
            className="uppercase"
            onClick={() => {
              console.log("Ok");
            }}
          >
            Đăng xuất
          </button>
        </div>
      </div>
      <section className="w-full">
        <div className="flex items-center justify-end p-3 shadow-sm">
          <p className="text-gray-900 font-medium cursor-pointer text-xl">
            {email}
          </p>
        </div>
        <div className="p-10">
          <Outlet></Outlet>
        </div>
      </section>
    </div>
  );
};

export default StoreLayout;
