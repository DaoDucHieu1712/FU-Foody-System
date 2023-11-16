import { useEffect, useState } from "react";
import UserBlog from "../../../../../shared/components/icon/UserBlog";
import Calender from "../../../../../shared/components/icon/Calender";
import CommentIcon from "../../../../../shared/components/icon/CommentIcon";
import Elips from "../../../../../shared/components/icon/Elips";
import Cate from "../../../../../shared/components/icon/Cate";
import axiosConfig from "../../../../../shared/api/axiosConfig";
import { useParams } from "react-router-dom";
import FormatDateString from "../../../../../shared/components/format/FormatDate";
import { useSelector } from "react-redux";
import dayjs from "dayjs";
import localizedFormat from "dayjs/plugin/localizedFormat";
dayjs.extend(localizedFormat);
import moment from "moment";
import "moment/dist/locale/vi";
moment.locale("vi");
import {
  Menu,
  MenuHandler,
  MenuList,
  MenuItem,
  Button,
  ButtonGroup,
  Typography,
} from "@material-tailwind/react";
import SocialIcon from "../../../../../shared/components/icon/SocailIcon";
import LastestPost from "../../../../(public)/components/LastestPost";
import PopularFood from "../../../../(public)/components/PopularFood";
import UpdatePost from "./UpdatePost";
import DeletePost from "./DeletePost";

const DetailPost = () => {
  const accesstoken = useSelector((state) => state.auth.accessToken);
  const userInfo = useSelector((state) => state.auth.userProfile);
  const { postId } = useParams();
  const [post, setPost] = useState("");
  const [openComment, setOpenComment] = useState(false);

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
  const handleopenComent = () => {
    setOpenComment((cur) => !cur);
  };
  return (
    <>
      <div className="container mt-8 p-11">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-x-14">
          {/* Column 1 */}
          <div className="md:col-span-2">
            <div className="border bg-white h-max">
              {/* POST AUTHOR */}
              <div className="information_post px-8 pt-6">
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
                      {dayjs(post.createdAt).format("D [Tháng] M, YYYY")}
                    </span>
                  </div>
                  <div className="flex items-center space-x-1">
                    <CommentIcon></CommentIcon>
                    <span className="text-sm"> {post.commentNumber}</span>
                  </div>
                </div>
                <div className="detail-post mt-3">
                  {/* {Tilte-blog} */}
                  <h1 className="text-3xl font-medium">{post.title}</h1>
                </div>
              </div>
              <div className="flex justify-between items-center px-8 py-3">
                <div className="flex items-center space-x-2">
                  <img
                    className="w-10 h-10 rounded-full"
                    src={post.avatar}
                    alt="Avatar"
                  />
                  <span className="font-medium">{post.username}</span>
                </div>
                <div className="flex items-center space-x-2">
                  <Menu>
                    <MenuHandler>
                      <button
                        type="button"
                        data-te-ripple-init
                        data-te-ripple-color="light"
                      >
                        <Elips></Elips>
                      </button>
                    </MenuHandler>
                    <MenuList>
                      <UpdatePost
                        post={post}
                        reloadPost={reloadPost}
                      ></UpdatePost>
                      <DeletePost
                        post={post}
                        reloadPost={reloadPost}
                      ></DeletePost>
                    </MenuList>
                  </Menu>
                </div>
              </div>
              {/* END POST AUTHOR */}

              {/* POST CONTENT */}
              <div className="text-justify px-8 py-2">
                <div dangerouslySetInnerHTML={{ __html: post.content }}></div>
                <img src={post.image} className="w-full h-[450px] mt-3" />
                <style>
                  {`
                  .Content-blog img {
                    width: 100%;
                  }
                `}
                </style>
              </div>
              {/* END POST CONTENT */}
              {/* POST EVENTS */}
              <div className="px-8 py-2">
                <div className="flex items-center justify-between">
                  <div className="flex flex-row-reverse items-center">
                    <span className="ml-2 text-gray-500">
                      {post.likeNumber} lượt thích
                    </span>
                  </div>
                  <div className="text-gray-500">
                    <span onClick="" style={{ cursor: "pointer" }}>
                      {post.commentNumber} bình luận
                    </span>
                  </div>
                </div>
              </div>
              {/* END POST EVENTS */}

              {/* POST ACTION */}
              <div className="py-2 px-8">
                <div className="border border-gray-200 border-l-0 border-r-0 py-1">
                  <div className="flex space-x-2">
                    <div className="w-1/2 flex space-x-2 justify-center items-center hover:bg-gray-100 text-xl py-2 rounded-lg cursor-pointer text-gray-500">
                      <i className="bx bx-like"></i>
                      <span className="text-sm font-semibold">Thích</span>
                    </div>
                    <div className="w-1/2 flex space-x-2 justify-center items-center hover:bg-gray-100 text-xl py-2 rounded-lg cursor-pointer text-gray-500">
                      <i className="bx bx-comment"></i>
                      <span className="text-sm font-semibold">Bình luận</span>
                    </div>
                  </div>
                </div>
              </div>
              {/* END POST ACTION */}
              <Typography
                variant="small"
                className="cursor-pointer hover:text-orange-900 px-8 py-2"
                onClick={handleopenComent}
              >
                <i className="fal fa-angle-double-down p-1"></i>Xem bình luận
              </Typography>
              {/* SUB COMMENT */}
              {openComment ? (
                <div className="ml-10 mt-1">
                  {post.comments.map((comment, index) => (
                    <div className="flex justify-start gap-2 mb-4">
                      <img
                        src={comment.avartar}
                        alt="image 1"
                        className="h-14 w-14 rounded-full object-cover"
                      ></img>
                      <div>
                        <div className="flex gap-1">
                          <Typography variant="small" className="font-bold">
                          {comment.userName}
                          </Typography>
                          <Typography variant="small">
                            -{" "}{moment(comment.commentDate).fromNow()}
                          </Typography>
                        </div>
                        <Typography variant="paragraph">
                          {comment.content}
                        </Typography>
                        <div className="flex gap-2">
                          <Typography
                            variant="small"
                            className="cursor-pointer hover:text-orange-900"
                          >
                            <i className="fal fa-heart pr-1"></i>Thích
                          </Typography>
                          <Typography
                            variant="small"
                            className="cursor-pointer hover:text-orange-900"
                          >
                            <i className="fal fa-angle-double-right fa-rotate-90 p-1"></i>
                            Xem bình luận
                          </Typography>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              ) : null}
              {/* END SUB COMMENT */}
              <div className="px-8 mb-4">
                {accesstoken ? (
                  <div className="flex space-x-2">
                    {userInfo && (
                      <img
                        src={userInfo.avatar}
                        alt="Profile picture"
                        className="w-9 h-9 rounded-full"
                      />
                    )}
                    <div className="flex-1 flex bg-gray-100 dark:bg-dark-third rounded-full items-center justify-between px-3">
                      <input
                        type="text"
                        placeholder="Viết bình luận..."
                        className="outline-none bg-transparent flex-1"
                        name="content"
                      />
                    </div>
                  </div>
                ) : null}
              </div>
            </div>

            {/* // END POST */}
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
