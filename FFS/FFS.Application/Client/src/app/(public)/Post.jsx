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
import { useEffect, useState } from "react";
import React from "react";
import AddPost from "../(auth)/shares/components/post/AddPost";
import axiosConfig from "../../shared/api/axiosConfig";
import { useNavigate  } from "react-router-dom";

const Post = () => {
  const [active, setActive] = React.useState(1);
  const [posts, setPosts] = React.useState([]);
  let navigate = useNavigate();

  const handleReadMoreClick = (postId) => {
    navigate(`/post-details/${postId}`);
  }

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

  useEffect(() => {
    axiosConfig
      .get("/api/Post/GetListPosts")
      .then((response) => {
        setPosts(response);
      })
      .catch((error) => {
        console.error("Error fetching posts: ", error);
      });
  }, []);

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
                  className="rounded-none focus:rounded-none"
                />
              </div>
              <div className="sort_blog">
                {/* <Select label="Bộ lọc" className="rounded-none">
                  <Option>Phổ biến nhất</Option>
                  <Option>Cũ nhất</Option>
                </Select> */}

                <AddPost></AddPost>
              </div>
            </div>
            <div className="list_post mt-8">
              <div className="grid grid-cols-2 gap-4">
                {posts.map((post) => (
                  <Card
                    key={post.id}
                    className="w-96 border rounded-none shadow-none"
                  >
                    <CardHeader
                      floated={false}
                      className="rounded-none mt-5 mx-5"
                    >
                      <img
                        src={post.image}
                        alt="card-image"
                        className="h-50 w-full object-cover object-center"
                        style={{ height: "230px" }}
                      />
                    </CardHeader>
                    <CardBody>
                      <Typography
                        variant="h5"
                        color="blue-gray"
                        className="mb-2 two-line-text"
                        style={{
                          display: "-webkit-box",
                          WebkitLineClamp: 2,
                          WebkitBoxOrient: "vertical",
                          overflow: "hidden",
                        }}
                      >
                        {post.title}
                      </Typography>
                      <Typography
                        className="three-line-text"
                        style={{
                          display: "-webkit-box",
                          WebkitLineClamp: 3,
                          WebkitBoxOrient: "vertical",
                          overflow: "hidden",
                        }}
                      >
                        {post.content}
                      </Typography>
                    </CardBody>
                    <CardFooter className="pt-0">
                      <Button
                        variant="outlined"
                        className="uppercase rounded-none flex items-center gap-1"
                        color="deep-orange"
                        onClick={() => handleReadMoreClick(post.id)}
                      >
                        Đọc Thêm
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          fill="none"
                          viewBox="0 0 24 24"
                          strokeWidth="1.5"
                          stroke="currentColor"
                          className="w-4 h-4"
                        >
                          <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            d="M13.5 4.5L21 12m0 0l-7.5 7.5M21 12H3"
                          />
                        </svg>
                      </Button>
                    </CardFooter>
                  </Card>
                ))}
              </div>
              {/* <div className="grid grid-cols-2 gap-4">
                {posts.map((post) => (
                  <Card
                    key={post.id}
                    className="w-96 border rounded-none shadow-none"
                  >
                    <CardHeader
                      floated={false}
                      className="rounded-none mt-5 mx-5"
                    >
                      <img
                        src={post.image}
                        alt="card-image"
                        className="h-230 w-full object-cover object-center"
                      
                      />
                    </CardHeader>
                    <CardBody>
                      <Typography
                        variant="h5"
                        color="blue-gray"
                        className="mb-2"
                      >
                        {post.title}
                      </Typography>
                      <Typography>{post.createdAt}</Typography>
                    </CardBody>
                    <CardFooter className="pt-0">
                      <Button
                        variant="outlined"
                        className="uppercase rounded-none flex items-center gap-1"
                        color="deep-orange"
                      >
                        Đọc Thêm
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          fill="none"
                          viewBox="0 0 24 24"
                          strokeWidth="1.5"
                          stroke="currentColor"
                          className="w-4 h-4"
                        >
                          <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            d="M13.5 4.5L21 12m0 0l-7.5 7.5M21 12H3"
                          />
                        </svg>
                      </Button>
                    </CardFooter>
                  </Card>
                ))}
              </div> */}
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
