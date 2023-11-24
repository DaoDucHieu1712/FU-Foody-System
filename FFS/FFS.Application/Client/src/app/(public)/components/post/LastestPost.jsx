import React, { useEffect, useState } from "react";
import axios from "../../../../shared/api/axiosConfig";
import dayjs from "dayjs";
import localizedFormat from "dayjs/plugin/localizedFormat";
import { useNavigate } from "react-router-dom";
dayjs.extend(localizedFormat);
const LastestPost = () => {
  const [lastestPost, setLastestPost] = useState([]);
  let navigate = useNavigate();

  const GetLastestPost = async () => {
    try {
      axios
        .get("/api/Post/GetTop3NewestPosts")
        .then((response) => {
          setLastestPost(response);
        })
        .catch((error) => {
          console.log(error);
          toast.error("Lấy bài viết mới nhất thất bại!");
        });
    } catch (error) {
      console.error("Lastest post: " + error);
    }
  };

  useEffect(() => {
    GetLastestPost();
  }, []);
  return (
    <>
      <div className="Blog_newest border p-4">
        <h1 className="text-lg font-bold uppercase">Bài viết phổ biến</h1>
        {lastestPost.map((post) => (
          <div key={post.id} className="flex mb-4 mt-4 cursor-poiter "  onClick={() => navigate(`/post-details/${post.id}`)}>
            {/* Image */}
            <div className="w-1/3">
              <img
                src={post.image}
                className="w-full h-[75px] object-cover"
              />
            </div>
            {/* Title and Timestamp */}
            <div className="w-2/3 px-4">
              <h2 className="text-sm font-bold">
                {post.title}
              </h2>
              <p className="text-sm text-gray-500">{dayjs(post.createdAt).format("D [Tháng] M, YYYY")}</p>
            </div>
          </div>
        ))}
      </div>
    </>
  );
};

export default LastestPost;
