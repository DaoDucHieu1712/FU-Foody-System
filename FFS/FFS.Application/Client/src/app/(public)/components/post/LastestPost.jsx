import React, { useEffect, useState } from "react";
import axios from "../../../../shared/api/axiosConfig";
const LastestPost = () => {
  const [lastestPost, setLastestPost] = useState([]);

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
        <h1 className="text-lg font-bold uppercase">Blog mới nhất</h1>
        {lastestPost.map((post) => (
          <div className="flex mb-4 mt-4">
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
              <p className="text-sm text-gray-500">12 Nov, 2023</p>
            </div>
          </div>
        ))}
      </div>
    </>
  );
};

export default LastestPost;
