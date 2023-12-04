import { useEffect, useState } from "react";
import axios from "../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import Loading from "../../shared/components/Loading";
import Pagination from "../../shared/components/Pagination";
import {
  Button,
  Card,
  CardFooter,
  Dialog,
  DialogBody,
  DialogFooter,
  Typography,
} from "@material-tailwind/react";
import { useSelector } from "react-redux";

const OrderFinishedPage = () => {
  const userProfile = useSelector((state) => state.auth.userProfile);
  const [dataSearch, setDateSearch] = useState({
    pageNumber: 1,
    pageSize: 15,
    ShipperId: userProfile?.id,
  });
  const [orders, setOrders] = useState([]);
  const [totalPage, setTotalPage] = useState(0);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedOrderDetails, setSelectedOrderDetails] = useState([]);

  const handleViewDetails = (detail) => {
    setSelectedOrderDetails(detail);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setSelectedOrderDetails(null);
    setIsModalOpen(false);
  };

  const getOrder = async () => {
    await axios
      .post("/api/Order/GetOrderFinish", dataSearch)
      .then((res) => {
        console.log(res);
        setOrders(res.data);
        const totalPages = Math.ceil(res.total / dataSearch.pageSize);
        setTotalPage(totalPages);
      })
      .catch((err) => {
        toast.error("Có lỗi xảy ra!");
      });
  };

  useEffect(() => {
    if (userProfile) {
      setDateSearch((prevDataSearch) => ({
        ...prevDataSearch,
        ShipperId: userProfile.id,
      }));
      getOrder();
    }
  }, [userProfile]);

  const changePage = async (page) => {
    dataSearch.pageNumber = page;
    setDateSearch(dataSearch);
    await getOrder();
  };

  const TABLE_HEAD = [
    "STT",
    "Khách hàng",
    "Địa chỉ",
    "Ngày tạo đơn",
    "Giá trị đơn hàng",
    "Trạng thái",
    "",
  ];
  return (
    <>
      {!orders ? (
        <Loading />
      ) : (
        <div>
          <Card className="h-full w-full">
            <table className="w-full min-w-max table-auto text-left">
              <thead>
                <tr>
                  {TABLE_HEAD.map((head) => (
                    <th
                      key={head}
                      className="border-b border-blue-gray-100 bg-blue-gray-50 p-4"
                    >
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="font-normal leading-none opacity-70"
                      >
                        {head}
                      </Typography>
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {orders.map((order, index) => (
                  <tr
                    key={order.Id}
                    className="even:bg-blue-gray-50/50 cursor-pointer"
                    onClick={() => handleViewDetails(order.detail)}
                  >
                    <td className="p-4">
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="font-normal"
                      >
                        {index + 1}
                      </Typography>
                    </td>
                    <td className="p-4">
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="font-normal"
                      >
                        {order.customer}
                      </Typography>
                    </td>
                    <td className="p-4">
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="font-normal"
                      >
                        {order.Location}
                      </Typography>
                    </td>
                    <td className="p-4">
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="font-normal"
                      >
                        {order.CreatedAt}
                      </Typography>
                    </td>
                    <td className="p-4">
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="font-normal"
                      >
                        {order.TotalPrice}
                      </Typography>
                    </td>
                    <td>
                      <Typography
                        variant="small"
                        className={`font-bold ${
                          order.orderCode === 3
                            ? "text-primary"
                            : order.orderStatus === 4
                            ? "text-red-900"
                            : ""
                        }`}
                      >
                        {order.orderStatus}
                      </Typography>
                    </td>
                    <td className="p-4">
                      <Typography
                        as="a"
                        href="#"
                        variant="small"
                        color="blue-gray"
                        className="font-medium"
                      >
                        Chi tiết
                      </Typography>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            <CardFooter>
              <div className="mt-4 flex justify-center">
                <Pagination
                  className="mt-4"
                  totalPage={totalPage}
                  currentPage={dataSearch.pageNumber}
                  handleClick={changePage}
                ></Pagination>
              </div>
            </CardFooter>
          </Card>
        </div>
      )}

      {selectedOrderDetails && (
        <Dialog size="lg" handler={handleCloseModal} open={isModalOpen}>
          <DialogBody>
            <h1 className="font-bold text-primary">Chi tiết đơn hàng</h1>
            <table className="table-auto w-full">
              <thead>
                <tr>
                  <th>Ảnh</th>
                  <th>Tên sản phẩm</th>
                  <th>Đơn giá</th>
                  <th>Số lượng</th>
                </tr>
              </thead>
              <tbody>
                {selectedOrderDetails.map((detail, index) => (
                  <tr key={index} className="text-center">
                    <td className="flex justify-center">
                      <img
                        src={detail.ImageURL}
                        width={50}
                        className="rounded-lg h-auto max-w-full"
                        alt={detail.FoodName}
                      />
                    </td>
                    <td>{detail.FoodName}</td>
                    <td>{detail.UnitPrice}đ</td>
                    <td>{detail.Quantity}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </DialogBody>
          <DialogFooter>
            <Button onClick={handleCloseModal}>Close</Button>
          </DialogFooter>
        </Dialog>
      )}
    </>
  );
};

export default OrderFinishedPage;
