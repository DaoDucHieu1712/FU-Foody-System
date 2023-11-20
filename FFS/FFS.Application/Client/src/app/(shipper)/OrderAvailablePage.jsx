import { useEffect, useState } from "react";
import axios from "../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import Loading from "../../shared/components/Loading";
import {
  Button,
  Card,
  CardBody,
  CardFooter,
  List,
  ListItem,
  Typography,
} from "@material-tailwind/react";
import { useSelector } from "react-redux";

const OrderAvailablePage = () => {
  const userProfile = useSelector((state) => state.auth.userProfile);
  const [dataSearch, setDateSearch] = useState({
    pageNumber: 1,
    pageSize: 15,
  });
  const [orders, setOrders] = useState([]);
  const [totalPage, setTotalPage] = useState(0);

  const getOrder = async () => {
    await axios
      .post("/api/Order/GetOrderUnbook", dataSearch)
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
    getOrder();
  }, []);

  const handleClickRecieveOrder = async (id) => {
    await axios
      .put(`/api/Order/ReceiveOrderUnbook/${userProfile.id}/${id}`)
      .then((res) => {
        getOrder();
        toast.success(res);
      })
      .catch((err) => {
        toast.error(err.response.data);
      });
  };
  return (
    <>
      {!orders ? (
        <Loading />
      ) : (
        <div>
          {orders.map((order, index) => {
            return (
              <Card className="w-full mb-3" key={order.Id}>
                <CardBody>
                  <Typography
                    variant="h6"
                    color="gray"
                    className="mb-4 uppercase"
                  >
                    Đơn hàng {index + 1}
                  </Typography>
                  <Typography color="red" className="font-bold">
                    Ghi chú của người đặt: {order.Note}
                  </Typography>
                  <Typography color="gray" className="mb-2 font-normal">
                    Địa chỉ giao hàng: {order.Location}
                  </Typography>
                  <Typography color="gray" className="mb-2 font-normal">
                    Ngày đặt hàng: {order.CreatedAt}
                  </Typography>
                  <Typography color="gray" className="mb-2 font-normal">
                    Giá trị đơn hàng: {order.TotalPrice}
                  </Typography>
                  <Typography color="gray" className="mb-2 font-normal">
                    Chi tiết đơn hàng
                  </Typography>
                  <Card className="w-full">
                    <List>
                      {order.Detail ??
                        order.detail.map((detail, index) => {
                          return (
                            <ListItem
                              key={index}
                              className="grid grid-cols-4 justify-between"
                            >
                              <div className="w-full">
                                <img
                                  src={detail.ImageURL}
                                  width={100}
                                  className=" rounded-lg h-auto max-w-full"
                                />
                              </div>
                              <div>
                                <span>{detail.FoodName}</span>
                              </div>
                              <div>
                                <span>{detail.UnitPrice}đ</span>
                              </div>
                              <div>
                                <span>x {detail.Quantity}</span>
                              </div>
                            </ListItem>
                          );
                        })}
                    </List>
                  </Card>
                </CardBody>
                <CardFooter className="pt-0">
                  <Button
                    onClick={() => handleClickRecieveOrder(order.Id)}
                    className="bg-primary"
                  >
                    Nhận đơn
                  </Button>
                </CardFooter>
              </Card>
            );
          })}
        </div>
      )}
    </>
  );
};

export default OrderAvailablePage;
