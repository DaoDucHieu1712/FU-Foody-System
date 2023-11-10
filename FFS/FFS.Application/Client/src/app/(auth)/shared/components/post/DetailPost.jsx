import { useEffect, useState } from "react";
import UserBlog from "../../../../../shared/components/icon/UserBlog";
import Calender from "../../../../../shared/components/icon/Calender";
import CommentIcon from "../../../../../shared/components/icon/CommentIcon";
import Elips from "../../../../../shared/components/icon/Elips";
import Cate from "../../../../../shared/components/icon/Cate";
import axiosConfig from "../../../../../shared/api/axiosConfig";
import { useParams } from "react-router-dom";
import FormatDateString from "../../../../../shared/components/format/FormatDate";

import {
  Menu,
  MenuHandler,
  MenuList,
  MenuItem,
  Button,
} from "@material-tailwind/react";
import SocialIcon from "../../../../../shared/components/icon/SocailIcon";
import LastestPost from "../../../../(public)/components/LastestPost";
import PopularFood from "../../../../(public)/components/PopularFood";
import UpdatePost from "./UpdatePost";

const DetailPost = () => {
  const { postId } = useParams();
  const [post, setPost] = useState("");

  const fetchPostDetails = () => {
    axiosConfig
      .get(`/api/Post/GetPostByPostId/${postId}`)
      .then((response) => {
        setPost(response);
        console.log("aare", response);
      })
      .catch((error) => {
        console.error("Error fetching post details: ", error);
      });
  };

  useEffect(() => {
    fetchPostDetails();
  }, [postId]);

  const reloadPost = () => {
    fetchPostDetails();
  };

  return (
    <>
      <div className="container mt-8 p-11">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-x-14">
          {/* Column 1 */}
          <div className="md:col-span-2 px-10 ">
            <div className="information_post">
              <div className="flex items-center space-x-5">
                <div className="flex items-center space-x-1">
                  <Cate></Cate>

                  <span className="text-sm">Món ăn</span>
                </div>
                <div className="flex items-center space-x-1">
                  <UserBlog></UserBlog>
                  <span className="text-sm">{post.username}</span>
                </div>
                <div className="flex items-center space-x-1">
                  <Calender></Calender>
                  <span className="text-sm">
                    {" "}
                    {FormatDateString(post.createdAt)}
                  </span>
                </div>
                <div className="flex items-center space-x-1">
                  <CommentIcon></CommentIcon>
                  <span className="text-sm">738</span>
                </div>
              </div>
            </div>
            <div className="detail-post mt-3">
              {/* {Tilte-blog} */}
              <h1 className="text-3xl font-medium">{post.title}</h1>
              {/* {Avatar} */}
              <div className="flex justify-between items-center mt-4">
                <div className="flex items-center space-x-2">
                  <img
                    className="w-10 h-10 rounded-full"
                    src={post.avatar}
                    alt="Avatar"
                  />
                  <span className="font-medium">{post.username}</span>
                </div>
                <div className="flex items-center space-x-2">

                  <SocialIcon></SocialIcon>
                  <Menu>
                    <MenuHandler>
                      <button
                        type="button"
                        data-te-ripple-init
                        data-te-ripple-color="light"
                        className="bg-gray-800 inline-block rounded-full p-3 text-xs font-medium uppercase leading-normal text-white shadow-md transition duration-150 ease-in-out hover:shadow-lg focus:shadow-lg focus:outline-none focus:ring-0 active:shadow-lg"
                      >
                        <Elips></Elips>
                      </button>
                    </MenuHandler>
                    <MenuList>
                      <UpdatePost
                        post={post}
                        reloadPost={reloadPost}
                      ></UpdatePost>
                      <MenuItem>Xóa bài viết</MenuItem>
                    </MenuList>
                  </Menu>
                </div>
              </div>
              {/* {Content-Blog} */}
              <div className="Content-blog mt-7">
                <div dangerouslySetInnerHTML={{ __html: post.content }}></div>
                {/* <img
                  src={post.image}
                  className="w-full h-auto mt-3"
                /> */}
                <style>
                  {`
                  .Content-blog img {
                    width: 100%;
                  }
                `}
                </style>
              </div>
            </div>
          </div>

          {/* Column 2 */}
          <div className="md:col-span-1 sticky top-0 h-screen">
            <LastestPost></LastestPost>
            <PopularFood></PopularFood>
          </div>
        </div>
      </div>
    </>
  );
};

export default DetailPost;
