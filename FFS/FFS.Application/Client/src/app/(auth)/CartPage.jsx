import { Button, Card, Input, Typography } from "@material-tailwind/react";
import DeleteIcon from "../../shared/components/icon/DeleteIcon";
const TABLE_HEAD = ["SẢN PHẨM", "ĐƠN GIÁ", "SỐ LƯỢNG", "THÀNH TIỀN"];

const CartPage = () => {
  return (
    <>
      <div className="grid grid-cols-3 my-10 gap-x-3">
        <div className="col-span-2 border border-borderpri rounded-lg">
          <div className="heading">
            <h1 className="font-medium uppercase text-lg p-3">Giỏ Hàng</h1>
          </div>
          <div className="w-full">
            <Card className="h-full w-full rounded-none">
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
                  <tr>
                    <td className="p-4 border-b border-blue-gray-50 flex items-center gap-x-2">
                      <DeleteIcon></DeleteIcon>
                      <img
                        src="https://images.unsplash.com/photo-1697186216555-572c0c5374ac?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxlZGl0b3JpYWwtZmVlZHw0fHx8ZW58MHx8fHx8&auto=format&fit=crop&w=500&q=60"
                        alt=""
                        className="w-[70px]"
                      />
                      <span>Cơm tấm</span>
                    </td>
                    <td className="p-4 border-b border-blue-gray-50">
                      500,000 đ
                    </td>
                    <td className="p-4 border-b border-blue-gray-50">
                      <div className="flex items-center justify-between border p-2">
                        <span className="text-gray-500 text-3xl font-medium cursor-pointer">
                          -
                        </span>
                        <p>3</p>
                        <span className="text-3xl font-medium cursor-pointer">
                          +
                        </span>
                      </div>
                    </td>
                    <td className="p-4 border-b border-blue-gray-50">
                      1,500,000 đ
                    </td>
                  </tr>
                  <tr>
                    <td className="p-4 border-b border-blue-gray-50 flex items-center gap-x-2">
                      <DeleteIcon></DeleteIcon>
                      <img
                        src="https://images.unsplash.com/photo-1697186216555-572c0c5374ac?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxlZGl0b3JpYWwtZmVlZHw0fHx8ZW58MHx8fHx8&auto=format&fit=crop&w=500&q=60"
                        alt=""
                        className="w-[70px]"
                      />
                      <span>Cơm tấm</span>
                    </td>
                    <td className="p-4 border-b border-blue-gray-50">
                      500,000 đ
                    </td>
                    <td className="p-4 border-b border-blue-gray-50">
                      <div className="flex items-center justify-between border p-2">
                        <span className="text-gray-500 text-3xl font-medium cursor-pointer">
                          -
                        </span>
                        <p>3</p>
                        <span className="text-3xl font-medium cursor-pointer">
                          +
                        </span>
                      </div>
                    </td>
                    <td className="p-4 border-b border-blue-gray-50">
                      1,500,000 đ
                    </td>
                  </tr>
                  <tr>
                    <td className="p-4 border-b border-blue-gray-50 flex items-center gap-x-2">
                      <DeleteIcon></DeleteIcon>
                      <img
                        src="https://images.unsplash.com/photo-1697186216555-572c0c5374ac?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxlZGl0b3JpYWwtZmVlZHw0fHx8ZW58MHx8fHx8&auto=format&fit=crop&w=500&q=60"
                        alt=""
                        className="w-[70px]"
                      />
                      <span>Cơm tấm</span>
                    </td>
                    <td className="p-4 border-b border-blue-gray-50">
                      500,000 đ
                    </td>
                    <td className="p-4 border-b border-blue-gray-50">
                      <div className="flex items-center justify-between border p-2">
                        <span className="text-gray-500 text-3xl font-medium cursor-pointer">
                          -
                        </span>
                        <p>3</p>
                        <span className="text-3xl font-medium cursor-pointer">
                          +
                        </span>
                      </div>
                    </td>
                    <td className="p-4 border-b border-blue-gray-50">
                      1,500,000 đ
                    </td>
                  </tr>
                </tbody>
              </table>
            </Card>
          </div>
          <div className="p-3 flex justify-between">
            <Button
              variant="outlined"
              className="border-blue-500 font-medium text-blue-500 rounded-none border-[2px]"
            >
              {" "}
              Mua hàng típ{" "}
            </Button>
            <Button
              variant="outlined"
              className="border-blue-500 font-medium text-blue-500 rounded-none border-[2px]"
            >
              {" "}
              CẬP NHẬT{" "}
            </Button>
          </div>
        </div>
        <div className="flex flex-col gap-y-5">
          <div className="border-borderpri border pb-5 rounded-lg">
            <div className="heading">
              <h1 className="text-2xl p-3">Thanh toán giỏ hàng</h1>
            </div>
            <div className="mt-3 p-3 pb-10 border-b border-borderpri">
              <div className="flex justify-between">
                <p className="font-medium text-lg text-gray-500">
                  Tổng đơn hàng
                </p>
                <span>$105.0</span>
              </div>
              <div className="flex justify-between">
                <p className="font-medium text-lg text-gray-500">Phí ship</p>
                <span>Free</span>
              </div>
              <div className="flex justify-between">
                <p className="font-medium text-lg text-gray-500">Giảm giá</p>
                <span>$5.0</span>
              </div>
            </div>
            <div className="p-3 flex justify-between">
              <p className="font-medium text-lg ">Tổng</p>
              <span>$15.0</span>
            </div>
            <div className="p-3 w-full">
              <Button className="bg-primary w-full">Đến Thanh toán</Button>
            </div>
          </div>
          <div className="border-borderpri border pb-5 rounded-lg">
            <div className="p-3 border-b border-borderpri">
              <h1 className="font-medium">Mã Giảm giá</h1>
            </div>
            <div className="p-3 flex flex-col gap-y-3">
              <Input label="Mã giảm giá"></Input>
              <Button className="bg-blue-500 w-full">
                Sử dụng mã giảm giá
              </Button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default CartPage;
