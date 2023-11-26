import {
  Button,
  Rating,
  Spinner,
  Textarea,
  Typography,
} from "@material-tailwind/react";
import axios from "../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import CookieService from "../../shared/helper/cookieConfig";
import ReportStore from "../(public)/components/ReportStore";
import Cookies from "universal-cookie";

const cookies = new Cookies();
const uId = cookies.get("fu_foody_id");

const StoreCommentPage = () => {
  const { id } = useParams();
  const [storeData, setStoreData] = useState(null);
  const [commentList, setCommentList] = useState([]);
  const [commentReply, setCommentReply] = useState({});
  const [dataComment, setDataComment] = useState({});
  const [commentText, setCommentText] = useState("");

  const [rate, setRate] = useState([]);

  const GetStoreInformation = async () => {
    try {
      axios
        .get(`/api/Store/DetailStore/${id}`)
        .then((response) => {
          console.log(response);
          setStoreData(response);
        })
        .catch((error) => {
          console.log(error);
        });
    } catch (error) {
      console.error("An error occurred", error);
    }
  };

  const GetComment = async (rateSearch) => {
    try {
      axios
        .get(`/api/Store/GetCommentByStore/${rateSearch}/${id}`)
        .then((response) => {
          console.log(response);
          setCommentList(response);
          var rate = calcRateStore(response);
          console.log(rate);
          setRate(rate);
        })
        .catch((error) => {
          console.log(error);
        });
    } catch (error) {
      console.error("An error occurred", error);
    }
  };
  useEffect(() => {
    GetStoreInformation();
    GetComment(0);
  }, [id]);

  const handleSearchByRate = (rate) => {
    try {
      axios
        .get(`/api/Store/GetCommentByStore/${rate}/${id}`)
        .then((response) => {
          setCommentList(response);
        })
        .catch((error) => {
          console.log(error);
        });
    } catch (error) {
      console.error("An error occurred", error);
    }
  };

  const handleViewMore = (id) => {
    try {
      axios
        .get(`/api/Store/GetCommentReply/${id}`)
        .then((response) => {
          console.log(response);
          const updatedCommentReply = { ...commentReply };

          updatedCommentReply[id] = response;

          setCommentReply(updatedCommentReply);
          console.log(commentReply[5]);
        })
        .catch((error) => {
          console.log(error);
        });
    } catch (error) {
      console.error("An error occurred", error);
    }
  };

  const calcRateStore = (comments) => {
    const res = {};
    var rate = 0;
    var totalComment = comments.length;
    comments.forEach((comment) => {
      if (res[comment.Rate]) {
        res[comment.Rate]++;
      } else {
        res[comment.Rate] = 1;
      }
      rate += comment.Rate;
    });
    var rateAvg = rate / totalComment;
    return { ...res, rateAvg: rateAvg.toFixed(1) };
  };
  const handleInputData = (content, commentId) => {
    setCommentText(content);
    setDataComment({
      UserId: CookieService.getToken("fu_foody_id"),
      Content: content,
      ParentCommentId: commentId,
      StoreId: id,
    });
  };

  const postComment = (id) => {
    try {
      axios
        .post(`/api/Store/RatingStore`, dataComment)
        .then((response) => {
          console.log(response);
          const updatedCommentReply = { ...commentReply };

          updatedCommentReply[id] = response;

          setCommentReply(updatedCommentReply);
          setCommentText("");
        })
        .catch((error) => {
          console.log(error);
        });
    } catch (error) {
      console.error("An error occurred", error);
    }
  };
  return (
    <>
      {storeData ? (
        <div>
          <div className="grid grid-cols-5 gap-10">
            <div className="col-span-2">
              <img
                className="w-full object-cover object-center"
                src={storeData.avatarURL}
                alt="nature image"
              />
            </div>
            <div className="col-span-3 flex flex-col gap-1">
              <span className="text-base">Quán ăn</span>
              {(uId !== undefined && uId !== null) ? <ReportStore uId={uId} sId={storeData.userId} /> : null}
              <Typography variant="h2">{storeData.storeName}</Typography>
              <div className="border-b border-t border-gray-200 h-14 grid grid-cols-7">
                <div className="flex justify-center items-center">
                  <div className="rounded-full bg-primary h-12 w-12 flex justify-center items-center">
                    <span className="h-full flex items-center font-semibold text-white text-2xl">
                    {rate.rateAvg ?? 0}
                    </span>
                  </div>
                </div>
                <div className="flex flex-col text-center">
                  <span className="text-primary font-bold text-2xl">
                    {rate["5"] ?? 0}
                  </span>
                  <span className="text-xs text-gray-600">5 sao</span>
                </div>
                <div className="flex flex-col text-center">
                  <span className="text-primary font-bold text-2xl">
                    {rate["4"] ?? 0}
                  </span>
                  <span className="text-xs text-gray-600">4 sao</span>
                </div>
                <div className="flex flex-col text-center">
                  <span className="text-primary font-bold text-2xl">
                    {rate["3"] ?? 0}
                  </span>
                  <span className="text-xs text-gray-600">3 sao</span>
                </div>
                <div className="flex flex-col text-center">
                  <span className="text-primary font-bold text-2xl">
                    {rate["2"] ?? 0}
                  </span>
                  <span className="text-xs text-gray-600">2 sao</span>
                </div>
                <div className="flex flex-col text-center">
                  <span className="text-primary font-bold text-2xl">
                    {rate["1"] ?? 0}
                  </span>
                  <span className="text-xs text-gray-600">1 sao</span>
                </div>
                <div className="flex flex-col text-center">
                  <span className="text-2xl text-gray-700">
                    {commentList.length}
                  </span>
                  <span className="text-xs text-gray-600">Bình luận</span>
                </div>
              </div>
              <span className="text-base">
                <i className="fas fa-map mr-1"></i>
                {storeData.address}
              </span>

              <span className="text-base">
                <i className="fal fa-phone mr-2"></i>
                Liên hệ : {storeData.phoneNumber}
              </span>
              <div className="flex items-center text-base gap-1">
                <span>
                  <i className="fal fa-clock mr-1"></i> Hoạt động từ:
                </span>
                <span>
                  {storeData.timeStart} : {storeData.timeEnd}
                </span>
              </div>
            </div>
          </div>
          <hr className="h-px my-4 bg-gray-400 border-0 dark:bg-gray-700"></hr>
          <div className="grid grid-cols-6">
            <div className="col-span-1">
              <Typography variant="h6" color="red" className="text-center">Bình Luận({commentList.length})</Typography>
              <ul>
                <li className="p-1">
                  <Button
                    className="w-full h-10 bg-white flex justify-center items-center hover:bg-primary"
                    onClick={() => handleSearchByRate(5)}
                  >
                    <Rating value={5} readonly />{" "}
                    <p className="text-black text-base m-1">
                      ({rate["5"] ?? 0})
                    </p>
                  </Button>
                </li>
                <li className="p-1">
                  <Button
                    className="w-full h-10 bg-white flex justify-center items-center hover:bg-primary"
                    onClick={() => handleSearchByRate(4)}
                  >
                    <Rating value={4} readonly />{" "}
                    <p className="text-black text-base m-1">
                      ({rate["4"] ?? 0})
                    </p>
                  </Button>
                </li>
                <li className="p-1">
                  <Button
                    className="w-full h-10 bg-white flex justify-center items-center hover:bg-primary"
                    onClick={() => handleSearchByRate(3)}
                  >
                    <Rating value={3} readonly />{" "}
                    <p className="text-black text-base m-1">
                      ({rate["3"] ?? 0})
                    </p>
                  </Button>
                </li>
                <li className="p-1">
                  <Button
                    className="w-full h-10 bg-white flex justify-center items-center hover:bg-primary"
                    onClick={() => handleSearchByRate(2)}
                  >
                    <Rating value={2} readonly />{" "}
                    <p className="text-black text-base m-1">
                      ({rate["2"] ?? 0})
                    </p>
                  </Button>
                </li>
                <li className="p-1">
                  <Button
                    className="w-full h-10 bg-white flex justify-center items-center hover:bg-primary"
                    onClick={() => handleSearchByRate(1)}
                  >
                    <Rating value={1} readonly />{" "}
                    <p className="text-black text-base m-1">
                      ({rate["1"] ?? 0})
                    </p>
                  </Button>
                </li>
              </ul>
            </div>
            <div className="col-span-5">
              <div>
                <br></br>
              </div>
              <div className="border-solid border-l-[1px] border-gray-400">
                <div className="p-3"></div>
                <div className="border-solid border-t-[1px] border-gray-400">
                  <ul>
                    {commentList.map((comment) => (
                      <li
                        className="p-4 border-[1px] border-gray-400 m-2"
                        key={comment.Id}
                      >
                        <div className="flex justify-between items-center border-b-[1px] border-gray-400 pb-1">
                          <div className="flex justify-center">
                            <div>
                              <img
                                className="rounded-full w-14 h-14  object-cover object-center"
                                src={comment.Avatar}
                                alt="nature image"
                              />
                            </div>
                            <div className="ml-2">
                              <p className="font-bold">{comment.UserName}</p>
                              <p className="text-sm text-gray-600">
                                {comment.CreatedAt}
                              </p>
                            </div>
                          </div>
                          <div>
                            <div className="rounded-full bg-primary text-white w-10 h-10 flex justify-center items-center font-bold">
                              {comment.Rate}
                            </div>
                          </div>
                        </div>
                        <div className="mt-2">{comment.Content}</div>
                        <div className="flex ">
                          <div className="flex justify-center items-center m-2 cursor-pointer">
                            <i className="fal fa-heart p-1"></i>
                            <p>Thích</p>
                          </div>
                          {/* <div className="flex justify-center items-center m-2 cursor-pointer">
                            <i className="fal fa-comment-alt-dots p-1"></i>
                            <p>Thảo luận</p>
                          </div> */}
                          <div
                            className="flex justify-center items-center m-2 cursor-pointer hover:text-orange-900"
                            onClick={() => {
                              handleViewMore(comment.Id);
                            }}
                          >
                            <i className="fal fa-angle-double-down p-1"></i>
                            <p>Xem bình luận</p>
                          </div>
                        </div>
                        <div className="ml-10">
                          {commentReply[comment.Id] ? (
                            <>
                              {commentReply[comment.Id].map((commentreply) => (
                                <div
                                  key={commentreply.Id}
                                  className="m-3 border-[1px] p-3"
                                >
                                  <div className="flex justify-between items-center border-b-[1px] border-gray-400 pb-1">
                                    <div className="flex justify-center">
                                      <div>
                                        <img
                                          className="rounded-full w-14 h-14  object-cover object-center"
                                          src={commentreply.Avatar}
                                          alt="nature image"
                                        />
                                      </div>
                                      <div className="ml-2">
                                        <p className="font-bold">
                                          {commentreply.UserName}
                                        </p>
                                        <p className="text-sm text-gray-600">
                                          {commentreply.CreatedAt}
                                        </p>
                                      </div>
                                    </div>
                                    <div></div>
                                  </div>
                                  <div className="mt-2">
                                    {commentreply.Content}
                                  </div>
                                </div>
                              ))}
                            </>
                          ) : (
                            <></>
                          )}
                        </div>
                        <div className="flex flex-col items-end">
                          <Textarea
                            defaultValue={commentText}
                            label="Nhập nội dung bình luận"
                            onChange={(e) =>
                              handleInputData(e.target.value, comment.Id)
                            }
                          ></Textarea>
                          <button
                            className="text-right bg-primary text-white font-bold p-3 rounded-md"
                            onClick={() => postComment(comment.Id)}
                          >
                            Bình Luận
                          </button>
                        </div>
                      </li>
                    ))}
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
      ) : (
        <Spinner></Spinner>
      )}
    </>
  );
};

export default StoreCommentPage;
