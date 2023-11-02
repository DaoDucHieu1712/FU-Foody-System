import React from "react";
import { useEffect, useState } from "react";
import UserBlog from "../../../../../shared/components/icon/UserBlog";
import Calender from "../../../../../shared/components/icon/Calender";
import CommentIcon from "../../../../../shared/components/icon/CommentIcon";
import Elips from "../../../../../shared/components/icon/Elips";
import Cate from "../../../../../shared/components/icon/Cate";
import axiosConfig from "../../../../../shared/api/axiosConfig";
import { useParams } from "react-router-dom";
import FormatDateString from "../../../../../shared/components/format/FormatDate";

const DetailPost = () => {
  const { postId } = useParams();  
  const [post, setPost] = useState("");

  useEffect(() => {
    axiosConfig
      .get(`/api/Post/GetPostByPostId/${postId}`)
      .then((response) => {
        setPost(response);
        console.log("aare",response);
      })
      .catch((error) => {
        console.error("Error fetching post details: ", error);
      });
  }, [postId]);
  
  return (
    <>
      <div className="container mt-8 p-11">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-x-14">
          {/* Column 1 */}
          <div className="md:col-span-2 ">
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
                  <span className="text-sm"> {FormatDateString(post.createdAt)}</span>
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
              <div className="flex justify-between items-center mt-2">
                <div className="flex items-center space-x-2">
                  <img
                    className="w-9 h-9 rounded-full"
                    src={post.avatar}
                    alt="Avatar"
                  />
                  <span className="font-medium">{post.username}</span>
                </div>
                <div className="flex items-center space-x-2">
                  <button
                    type="button"
                    data-te-ripple-init
                    data-te-ripple-color="light"
                    className="mb-2 bg-green-500 inline-block rounded-full p-3 text-xs font-medium uppercase leading-normal text-white shadow-md transition duration-150 ease-in-out hover:shadow-lg focus:shadow-lg focus:outline-none focus:ring-0 active:shadow-lg"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      class="h-4 w-4"
                      fill="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path d="M.057 24l1.687-6.163c-1.041-1.804-1.588-3.849-1.587-5.946.003-6.556 5.338-11.891 11.893-11.891 3.181.001 6.167 1.24 8.413 3.488 2.245 2.248 3.481 5.236 3.48 8.414-.003 6.557-5.338 11.892-11.893 11.892-1.99-.001-3.951-.5-5.688-1.448l-6.305 1.654zm6.597-3.807c1.676.995 3.276 1.591 5.392 1.592 5.448 0 9.886-4.434 9.889-9.885.002-5.462-4.415-9.89-9.881-9.892-5.452 0-9.887 4.434-9.889 9.884-.001 2.225.651 3.891 1.746 5.634l-.999 3.648 3.742-.981zm11.387-5.464c-.074-.124-.272-.198-.57-.347-.297-.149-1.758-.868-2.031-.967-.272-.099-.47-.149-.669.149-.198.297-.768.967-.941 1.165-.173.198-.347.223-.644.074-.297-.149-1.255-.462-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.297-.347.446-.521.151-.172.2-.296.3-.495.099-.198.05-.372-.025-.521-.075-.148-.669-1.611-.916-2.206-.242-.579-.487-.501-.669-.51l-.57-.01c-.198 0-.52.074-.792.372s-1.04 1.016-1.04 2.479 1.065 2.876 1.213 3.074c.149.198 2.095 3.2 5.076 4.487.709.306 1.263.489 1.694.626.712.226 1.36.194 1.872.118.571-.085 1.758-.719 2.006-1.413.248-.695.248-1.29.173-1.414z" />
                    </svg>
                  </button>
                  <button
                    type="button"
                    data-te-ripple-init
                    data-te-ripple-color="light"
                    className="mb-2 bg-blue-800 inline-block rounded-full p-3 text-xs font-medium uppercase leading-normal text-white shadow-md transition duration-150 ease-in-out hover:shadow-lg focus:shadow-lg focus:outline-none focus:ring-0 active:shadow-lg"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      class="h-4 w-4"
                      fill="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path d="M9 8h-3v4h3v12h5v-12h3.642l.358-4h-4v-1.667c0-.955.192-1.333 1.115-1.333h2.885v-5h-3.808c-3.596 0-5.192 1.583-5.192 4.615v3.385z" />
                    </svg>
                  </button>
                  <button
                    type="button"
                    data-te-ripple-init
                    data-te-ripple-color="light"
                    className="mb-2 bg-pink-700 inline-block rounded-full p-3 text-xs font-medium uppercase leading-normal text-white shadow-md transition duration-150 ease-in-out hover:shadow-lg focus:shadow-lg focus:outline-none focus:ring-0 active:shadow-lg"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      class="h-4 w-4"
                      fill="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path d="M12 2.163c3.204 0 3.584.012 4.85.07 3.252.148 4.771 1.691 4.919 4.919.058 1.265.069 1.645.069 4.849 0 3.205-.012 3.584-.069 4.849-.149 3.225-1.664 4.771-4.919 4.919-1.266.058-1.644.07-4.85.07-3.204 0-3.584-.012-4.849-.07-3.26-.149-4.771-1.699-4.919-4.92-.058-1.265-.07-1.644-.07-4.849 0-3.204.013-3.583.07-4.849.149-3.227 1.664-4.771 4.919-4.919 1.266-.057 1.645-.069 4.849-.069zm0-2.163c-3.259 0-3.667.014-4.947.072-4.358.2-6.78 2.618-6.98 6.98-.059 1.281-.073 1.689-.073 4.948 0 3.259.014 3.668.072 4.948.2 4.358 2.618 6.78 6.98 6.98 1.281.058 1.689.072 4.948.072 3.259 0 3.668-.014 4.948-.072 4.354-.2 6.782-2.618 6.979-6.98.059-1.28.073-1.689.073-4.948 0-3.259-.014-3.667-.072-4.947-.196-4.354-2.617-6.78-6.979-6.98-1.281-.059-1.69-.073-4.949-.073zm0 5.838c-3.403 0-6.162 2.759-6.162 6.162s2.759 6.163 6.162 6.163 6.162-2.759 6.162-6.163c0-3.403-2.759-6.162-6.162-6.162zm0 10.162c-2.209 0-4-1.79-4-4 0-2.209 1.791-4 4-4s4 1.791 4 4c0 2.21-1.791 4-4 4zm6.406-11.845c-.796 0-1.441.645-1.441 1.44s.645 1.44 1.441 1.44c.795 0 1.439-.645 1.439-1.44s-.644-1.44-1.439-1.44z" />
                    </svg>
                  </button>
                  <button
                    type="button"
                    data-te-ripple-init
                    data-te-ripple-color="light"
                    className="mb-2 bg-red-700 inline-block rounded-full p-3 text-xs font-medium uppercase leading-normal text-white shadow-md transition duration-150 ease-in-out hover:shadow-lg focus:shadow-lg focus:outline-none focus:ring-0 active:shadow-lg"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      class="h-4 w-4"
                      fill="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path
                        d="M12 0c-6.627 0-12 5.372-12 12 0 5.084 3.163 9.426 7.627 11.174-.105-.949-.2-2.405.042-3.441.218-.937 1.407-5.965 1.407-5.965s-.359-.719-.359-1.782c0-1.668.967-2.914 2.171-2.914 1.023 0 1.518.769 1.518 1.69 0 1.029-.655 2.568-.994 3.995-.283 1.194.599 2.169 1.777 2.169 2.133 0 3.772-2.249 3.772-5.495 0-2.873-2.064-4.882-5.012-4.882-3.414 0-5.418 2.561-5.418 5.207 0 1.031.397 2.138.893 2.738.098.119.112.224.083.345l-.333 1.36c-.053.22-.174.267-.402.161-1.499-.698-2.436-2.889-2.436-4.649 0-3.785 2.75-7.262 7.929-7.262 4.163 0 7.398 2.967 7.398 6.931 0 4.136-2.607 7.464-6.227 7.464-1.216 0-2.359-.631-2.75-1.378l-.748 2.853c-.271 1.043-1.002 2.35-1.492 3.146 1.124.347 2.317.535 3.554.535 6.627 0 12-5.373 12-12 0-6.628-5.373-12-12-12z"
                        fillRule="evenodd"
                        clipRule="evenodd"
                      />
                    </svg>
                  </button>
                  <button
                    type="button"
                    data-te-ripple-init
                    data-te-ripple-color="light"
                    className="mb-2 bg-gray-600 inline-block rounded-full p-3 text-xs font-medium uppercase leading-normal text-white shadow-md transition duration-150 ease-in-out hover:shadow-lg focus:shadow-lg focus:outline-none focus:ring-0 active:shadow-lg"
                  >
                    <Elips></Elips>
                  </button>
                </div>
              </div>
              {/* {Content-Blog} */}
              <div className="Content-blog mt-4">
                {post.content}
                <img
                  src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                  className="w-full h-auto mt-3"
                />
              </div>
            </div>
          </div>

          {/* Column 2 */}
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
        </div>
      </div>
    </>
  );
};

export default DetailPost;
