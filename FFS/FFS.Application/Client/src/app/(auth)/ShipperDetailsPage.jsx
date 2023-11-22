import React from "react";
import { useParams } from "react-router-dom";
import axios from "../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import moment from "moment";
import "moment/dist/locale/vi";
import {
  Tabs,
  TabsHeader,
  TabsBody,
  Tab,
  TabPanel,
  Avatar,
  Button,
  Typography,
} from "@material-tailwind/react";

const ShipperDetailsPage = () => {
  moment.locale("vi");
  const { id } = useParams();
  const [activeTab, setActiveTab] = useState("reviews");
  const [reviews, setReviews] = useState([]);
  const [shipperData, setShipperData] = useState([]);

  const GetShipperInformation = async () => {
    try {
      axios
        .get(`/api/Authenticate/GetShipperById/${id}`)
        .then((response) => {
          console.log(response);
          setShipperData(response);
        })
        .catch((error) => {
          console.log(error);
        });
    } catch (error) {
      console.error("An error occurred", error);
    }
  };

  useEffect(() => {
    GetShipperInformation();
  }, [id]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(
          `/api/Comment/GetAllCommentByShipperId/${id}`
        );

        // Assuming your API response contains an array of reviews
        setReviews(response);
      } catch (error) {
        console.error("Error fetching reviews:", error);
      }
    };

    fetchData();
  }, [id]);

  return (
    <>
      <div className="container px-20 py-10">
        <div className="flex justify-between items-center mb-4">
          {/* Avatar and User Info */}
          <div className="flex items-center gap-4">
            <Avatar
              src={shipperData.avatar}
              alt="avatar"
              withBorder={true}
              className="p-0.5 border-gray-500 "
              size="xxl"
            />
            <div>
              <Typography className="font-medium text-3xl">
                {shipperData.firstName} {shipperData.lastName}
              </Typography>
              {/* <Typography variant="small" color="gray" className="font-normal">
                Web Developer
              </Typography> */}
            </div>
          </div>

          {/* Action Buttons */}
          <div className="contact self-end flex items-center gap-4 ">
            <Button className="flex items-center gap-1 bg-primary">
              <svg
                className="h-4 w-4"
                fill="currentColor"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
                fillRule="evenodd"
                clipRule="evenodd"
              >
                <path d="M12 0c-6.627 0-12 4.975-12 11.111 0 3.497 1.745 6.616 4.472 8.652v4.237l4.086-2.242c1.09.301 2.246.464 3.442.464 6.627 0 12-4.974 12-11.111 0-6.136-5.373-11.111-12-11.111zm1.193 14.963l-3.056-3.259-5.963 3.259 6.559-6.963 3.13 3.259 5.889-3.259-6.559 6.963z" />
              </svg>
              Nhắn tin
            </Button>
            <Button variant="outlined" className="flex items-center gap-1">
              Báo cáo
              <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth={1.5}
                stroke="currentColor"
                className="w-4 h-4"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126zM12 15.75h.007v.008H12v-.008z"
                />
              </svg>
            </Button>
          </div>
        </div>

        {/* Tabs */}
        <Tabs value={activeTab} className="flex-col">
          <TabsHeader
            className="rounded-none border-b border-blue-gray-50 bg-transparent p-0"
            indicatorProps={{
              className:
                "bg-transparent border-b-2 border-orange-900 shadow-none rounded-none",
            }}
          >
            <Tab
              value="reviews"
              onClick={() => setActiveTab("reviews")}
              className={activeTab === "reviews" ? "text-orange-900" : ""}
            >
              Đánh giá ({reviews.length})
            </Tab>
            <Tab
              value="overview"
              onClick={() => setActiveTab("overview")}
              className={activeTab === "overview" ? "text-orange-900" : ""}
            >
              Tổng quan
            </Tab>
          </TabsHeader>
          <TabsBody>
            <TabPanel value="reviews">
              {reviews.map((review, index) => (
                <div key={index} className="mb-4">
                  <div className="w-full mx-auto rounded-lg bg-white border border-gray-200 p-5 text-black font-light mb-6">
                    <div className="w-full flex items-center">
                      <div className="flex items-center mb-4">
                        <img
                          className="w-10 h-10 me-2 rounded-full"
                          src={review.avatar}
                          alt=""
                        />
                        <div className="font-medium dark:text-white">
                          <p>
                            {review.username}{" "}
                            <span className="block text-xs text-gray-500 dark:text-gray-400">
                              {moment(review.createdAt).fromNow()}
                            </span>
                          </p>
                        </div>
                      </div>
                    </div>
                    <div className="w-full">
                      <p className="text-base">
                        Lorem ipsum dolor sit amet consectetur adipisicing elit.
                        Quos sunt ratione dolor exercitationem minima quas
                        itaque saepe quasi architecto vel! Accusantium, vero
                        sint recusandae cum tempora nemo commodi soluta
                        deleniti.
                        {review.noteForShipper}
                      </p>
                    </div>
                  </div>
                </div>
              ))}
            </TabPanel>
          </TabsBody>
        </Tabs>
      </div>
    </>
  );
};

export default ShipperDetailsPage;
