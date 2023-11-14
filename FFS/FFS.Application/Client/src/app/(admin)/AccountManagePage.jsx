import Pagination from "../../shared/components/Pagination";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import {
  Input,
  Option,
  Select,
  Typography,
  Button,
} from "@material-tailwind/react";
import axios from "../../shared/api/axiosConfig";

const AccountManagePage = () => {
  const [dataSearch, setDateSearch] = useState({
    pageNumber: 1,
    pageSize: 5,
    username: "",
    email: "",
    role: "",
  });
  const handleExportExcel = () => {
    const fileDownloadUrl = `https://localhost:7025/api/Admin/ExportUser`;

    window.location.href = fileDownloadUrl;
  };
  const [userList, setUserList] = useState([]);
  const [roleList, setRoleList] = useState([]);

  const [totalPage, setTotalPage] = useState(0);
  const [roleSelect, setRoleSelect] = useState("");

  const changePage = async (page) => {
    dataSearch.pageNumber = page;
    setDateSearch(dataSearch);
    await getUser();
  };
  const handleOnChange = async (e) => {
    dataSearch[e.target.name] = e.target.value;
    setDateSearch(dataSearch);

    await getUser();
  };

  const getUser = async () => {
    try {
      await axios
        .post(`/api/Admin/GetAccounts`, dataSearch)
        .then((res) => {
          setUserList(res.data);
          const totalPages = Math.ceil(res.total / dataSearch.pageSize);
          setTotalPage(totalPages);
        })
        .catch((error) => {
          toast.error("Có lỗi xảy ra!");
          console.log(error);
        });
    } catch (error) {
      console.log("Food error: " + error);
    }
  };

  const getRoles = async () => {
    try {
      await axios
        .get(`/api/Admin/GetRoles`)
        .then((res) => {
          console.log(res);
          setRoleList(res);
        })
        .catch((error) => {
          toast.error("Có lỗi xảy ra!");
          console.log(error);
        });
    } catch (error) {
      console.log("Food error: " + error);
    }
  };

  const handleClickOption = (id, name) => {
    console.log(name);
    dataSearch.role = id;
    setDateSearch(dataSearch);
    setRoleSelect(name);
    getUser();
  };

  const handleBanAccountBtn = async (id, username) => {
    const check = confirm(`Bạn có chắc chắn muốn khóa tài khoản ${username}`);
    if (check) {
      const data = {
        id: id,
        username: username,
      };
      try {
        await axios
          .post(`/api/Admin/BanAccount`, data)
          .then((res) => {
            toast.success(res);
            getUser();
          })
          .catch((error) => {
            toast.error("Có lỗi xảy ra!");
            console.log(error);
          });
      } catch (error) {
        console.log("Food error: " + error);
      }
    }
  };

  const handleUnBanAccountBtn = async (id, username) => {
    const check = confirm(
      `Bạn có chắc chắn muốn mở khóa tài khoản ${username}`
    );
    if (check) {
      const data = {
        id: id,
        username: username,
      };
      try {
        await axios
          .post(`/api/Admin/UnBanAccount`, data)
          .then((res) => {
            toast.success(res);
            getUser();
          })
          .catch((error) => {
            toast.error("Có lỗi xảy ra!");
            console.log(error);
          });
      } catch (error) {
        console.log("Food error: " + error);
      }
    }
  };

  useEffect(() => {
    getUser();
    getRoles();
  }, []);

  return (
    <>
      <div className="relative overflow-x-auto">
        <div className="mb-4 flex flex-col gap-8 md:flex-row md:items-center">
          <Typography variant="h4" color="blue-gray">
            Danh sách tài khoản
          </Typography>
          <div className="flex gap-5">
            <Button
              className=" text-white text-center font-bold bg-primary cursor-pointer hover:bg-orange-900"
              onClick={handleExportExcel}
            >
              Xuất Excel
            </Button>
          </div>
        </div>
        <table className="w-full text-sm text-left text-gray-500 dark:text-gray-400">
          <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
            <tr>
              <th scope="col" className="px-6 py-3">
                STT
              </th>
              <th scope="col" className="px-6 py-3">
                Avatar
              </th>
              <th scope="col" className="px-6 py-3">
                Username
              </th>
              <th scope="col" className="px-6 py-3">
                Email
              </th>
              <th scope="col" className="px-6 py-3">
                Vai trò
              </th>
              <th scope="col" className="px-6 py-3">
                Trạng thái
              </th>

              <th scope="col" className="px-6 py-3">
                Thao tác
              </th>
            </tr>
            <tr>
              <th scope="col" className="px-6 py-3"></th>
              <th scope="col" className="px-6 py-3"></th>
              <th scope="col" className="px-6 py-3">
                <Input
                  variant="static"
                  placeholder="Username"
                  onChange={handleOnChange}
                  name="username"
                />
              </th>
              <th scope="col" className="px-6 py-3">
                <div className="w-full">
                  <Input
                    variant="static"
                    placeholder="Email"
                    onChange={handleOnChange}
                    name="email"
                  />
                </div>
              </th>
              <th scope="col" className="px-6 py-3">
                <div className="w-full">
                  <Select variant="static" name="role" value={roleSelect}>
                    <Option
                      value=""
                      onClick={() => handleClickOption("", "All")}
                    >
                      All
                    </Option>
                    {roleList.map((role) => {
                      return (
                        <Option
                          key={role.id}
                          value={role.id}
                          onClick={() => handleClickOption(role.id, role.name)}
                        >
                          {role.name}
                        </Option>
                      );
                    })}
                  </Select>
                </div>
              </th>
              <th scope="col" className="px-6 py-3"></th>
              <th scope="col" className="px-6 py-3"></th>
            </tr>
          </thead>
          <tbody>
            {userList.map((user, index) => (
              <tr
                key={user.Id}
                className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
              >
                <th
                  scope="row"
                  className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                >
                  {index + 1}
                </th>
                <td className="px-6 py-4">
                  <img
                    src={user.Avatar}
                    height={100}
                    width={100}
                    alt="Avatar"
                  />
                </td>
                <td className="px-6 py-4">{user.UserName}</td>
                <td className="px-6 py-4">{user.Email}</td>
                <td className="px-6 py-4">{user.Role}</td>
                <td className="px-6 py-4">
                  {user.Allow ? (
                    <span className="text-green-500 font-bold">
                      Đang hoạt động{" "}
                    </span>
                  ) : (
                    <span className="text-red-600 font-bold">
                      Tài khoản bị khóa
                    </span>
                  )}
                </td>

                <td className="px-6 py-4 text-center">
                  {user.Allow ? (
                    <Button
                      variant="outlined"
                      className="border-primary hover:bg-primary hover:text-white"
                      onClick={() =>
                        handleBanAccountBtn(user.Id, user.UserName)
                      }
                    >
                      Khóa tài khoản
                    </Button>
                  ) : (
                    <Button
                      variant="outlined"
                      className="border-primary hover:bg-primary hover:text-white"
                      onClick={() =>
                        handleUnBanAccountBtn(user.Id, user.UserName)
                      }
                    >
                      Mở khóa
                    </Button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        <div className="mt-4 flex justify-center">
          <Pagination
            className="mt-4"
            totalPage={totalPage}
            currentPage={dataSearch.pageNumber}
            handleClick={changePage}
          ></Pagination>
        </div>
      </div>
    </>
  );
};

export default AccountManagePage;
