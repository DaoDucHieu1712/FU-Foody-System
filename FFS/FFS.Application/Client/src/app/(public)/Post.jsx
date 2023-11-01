import {
  Input,
  Select,
  Option,
  Card,
  CardHeader,
  CardBody,
  CardFooter,
  Typography,
  Button,
  IconButton,
} from "@material-tailwind/react";
import React from "react";

const Post = () => {
  const [active, setActive] = React.useState(1);
  const getItemProps = (index) => ({
    variant: active === index ? "filled" : "text",
    color: "gray",
    onClick: () => setActive(index),
    className: "rounded-full",
  });

  const next = () => {
    if (active === 5) return;

    setActive(active + 1);
  };

  const prev = () => {
    if (active === 1) return;

    setActive(active - 1);
  };

  return (
    <>
      <div className="container mt-8 p-11">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-x-14">
          {/* Column 1 */}
          <div className="md:col-span-1">
            <div className="Blog_newest border p-4">
              <h1 className="text-lg font-bold uppercase">Blog mới nhất</h1>
              <div className="flex mb-4 mt-4">
                {/* Image */}
                <div className="w-1/3">
                  <img
                    src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                    className="w-full h-auto"
                  />
                </div>
                {/* Title and Timestamp */}
                <div className="w-2/3 px-4">
                  <h2 className="text-sm font-bold">
                    Top 5 món ăn truyền thống đốn tim gen Z
                  </h2>
                  <p className="text-sm text-gray-500">12 Nov, 2023</p>
                </div>
              </div>
              <div className="flex mb-4">
                {/* Image */}
                <div className="w-1/3">
                  <img
                    src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                    className="w-full h-auto"
                  />
                </div>
                {/* Title and Timestamp */}
                <div className="w-2/3 px-4">
                  <h2 className="text-sm font-bold">
                    Top 5 món ăn truyền thống đốn tim gen Z
                  </h2>
                  <p className="text-sm text-gray-500">12 Nov, 2023</p>
                </div>
              </div>
              <div className="flex mb-4">
                {/* Image */}
                <div className="w-1/3">
                  <img
                    src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                    className="w-full h-auto"
                  />
                </div>
                {/* Title and Timestamp */}
                <div className="w-2/3 px-4">
                  <h2 className="text-sm font-bold">
                    Top 5 món ăn truyền thống đốn tim gen Z
                  </h2>
                  <p className="text-sm text-gray-500">12 Nov, 2023</p>
                </div>
              </div>
            </div>
            <div className="food_newest border p-4 mt-6">
              <h1 className="text-lg font-bold uppercase">Món ăn yêu thích</h1>
              <div className="flex mb-4 mt-4">
                {/* Image */}
                <div className="w-1/3">
                  <img
                    src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                    className="w-full h-auto"
                  />
                </div>
                {/* Title and Timestamp */}
                <div className="w-2/3 px-4">
                  <h2 className="text-sm font-bold">
                    Top 5 món ăn truyền thống đốn tim gen Z
                  </h2>
                  <p className="text-sm text-gray-500">12 Nov, 2023</p>
                </div>
              </div>
              <div className="flex mb-4">
                {/* Image */}
                <div className="w-1/3">
                  <img
                    src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                    className="w-full h-auto"
                  />
                </div>
                {/* Title and Timestamp */}
                <div className="w-2/3 px-4">
                  <h2 className="text-sm font-bold">
                    Top 5 món ăn truyền thống đốn tim gen Z
                  </h2>
                  <p className="text-sm text-gray-500">12 Nov, 2023</p>
                </div>
              </div>
              <div className="flex mb-4">
                {/* Image */}
                <div className="w-1/3">
                  <img
                    src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                    className="w-full h-auto"
                  />
                </div>
                {/* Title and Timestamp */}
                <div className="w-2/3 px-4">
                  <h2 className="text-sm font-bold">
                    Top 5 món ăn truyền thống đốn tim gen Z
                  </h2>
                  <p className="text-sm text-gray-500">12 Nov, 2023</p>
                </div>
              </div>
            </div>
          </div>
          {/* Column 2 */}
          <div className="md:col-span-2 ">
            <div className="flex items-center justify-between">
              <div className="w-72">
                <Input
                  label="Tìm kiếm"
                  icon={<i className="fas fa-search" />}
                  className="rounded-none"
                />
              </div>
              <div className="sort_blog">
                <Select label="Bộ lọc" className="rounded-none">
                  <Option>Phổ biến nhất</Option>
                  <Option>Cũ nhất</Option>
                </Select>
              </div>
            </div>
            <div className="list_post mt-8">
              <div className="grid grid-cols-2 gap-4">
                <Card className="w-96 border rounded-none shadow-none">
                  <CardHeader
                    floated={false}
                    className="rounded-none mt-5 mx-5"
                  >
                    <img
                      src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                      alt="card-image"
                      className="h-50 w-full object-cover object-center"
                    />
                  </CardHeader>
                  <CardBody>
                    <Typography variant="h5" color="blue-gray" className="mb-2">
                      Top 5 món ăn truyền thống đốn tim gen Z
                    </Typography>
                    <Typography>
                      Món ăn độc lạ nằm số 1 trong danh sách các món ăn được
                      GenZ săn lùng nhiều nhất khi đến Huế chính là chè bột lọc
                      heo quay.
                    </Typography>
                  </CardBody>
                  <CardFooter className="pt-0">
                    <Button className="uppercase">Đọc Thêm</Button>
                  </CardFooter>
                </Card>

                <Card className="w-96 border rounded-none shadow-none">
                  <CardHeader
                    floated={false}
                    className="rounded-none mt-5 mx-5"
                  >
                    <img
                      src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                      alt="card-image"
                      className="h-50 w-full object-cover object-center"
                    />
                  </CardHeader>
                  <CardBody>
                    <Typography variant="h5" color="blue-gray" className="mb-2">
                      Top 5 món ăn truyền thống đốn tim gen Z
                    </Typography>
                    <Typography>
                      Món ăn độc lạ nằm số 1 trong danh sách các món ăn được
                      GenZ săn lùng nhiều nhất khi đến Huế chính là chè bột lọc
                      heo quay.
                    </Typography>
                  </CardBody>
                  <CardFooter className="pt-0">
                    <Button className="uppercase">Đọc Thêm</Button>
                  </CardFooter>
                </Card>
                <Card className="w-96 border rounded-none shadow-none">
                  <CardHeader
                    floated={false}
                    className="rounded-none mt-5 mx-5"
                  >
                    <img
                      src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                      alt="card-image"
                      className="h-50 w-full object-cover object-center"
                    />
                  </CardHeader>
                  <CardBody>
                    <Typography variant="h5" color="blue-gray" className="mb-2">
                      Top 5 món ăn truyền thống đốn tim gen Z
                    </Typography>
                    <Typography>
                      The place is close to Barceloneta Beach and bus stop just
                      2 min by walk and near to &quot;Naviglio&quot; where you
                      can enjoy the main night life in Barcelona.
                    </Typography>
                  </CardBody>
                  <CardFooter className="pt-0">
                    <Button className="uppercase">Đọc Thêm</Button>
                  </CardFooter>
                </Card>
                <Card className="w-96 border rounded-none shadow-none">
                  <CardHeader
                    floated={false}
                    className="rounded-none mt-5 mx-5"
                  >
                    <img
                      src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                      alt="card-image"
                      className="h-50 w-full object-cover object-center"
                    />
                  </CardHeader>
                  <CardBody>
                    <Typography variant="h5" color="blue-gray" className="mb-2">
                      Top 5 món ăn truyền thống đốn tim gen Z
                    </Typography>
                    <Typography>
                      The place is close to Barceloneta Beach and bus stop just
                      2 min by walk and near to &quot;Naviglio&quot; where you
                      can enjoy the main night life in Barcelona.
                    </Typography>
                  </CardBody>
                  <CardFooter className="pt-0">
                    <Button className="uppercase">Đọc Thêm</Button>
                  </CardFooter>
                </Card>
              </div>
              <div className="flex items-center justify-center gap-4 mt-7">
                <Button
                  variant="text"
                  className="flex items-center gap-2 rounded-full"
                  onClick={prev}
                  disabled={active === 1}
                >
                  {/* <ArrowLeftIcon strokeWidth={2} className="h-4 w-4" />  */}
                  Previous
                </Button>
                <div className="flex items-center gap-2">
                  <IconButton {...getItemProps(1)}>1</IconButton>
                  <IconButton {...getItemProps(2)}>2</IconButton>
                  <IconButton {...getItemProps(3)}>3</IconButton>
                  <IconButton {...getItemProps(4)}>4</IconButton>
                  <IconButton {...getItemProps(5)}>5</IconButton>
                </div>
                <Button
                  variant="text"
                  className="flex items-center gap-2 rounded-full"
                  onClick={next}
                  disabled={active === 5}
                >
                  Next
                  {/* <ArrowRightIcon strokeWidth={2} className="h-4 w-4" /> */}
                </Button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Post;
