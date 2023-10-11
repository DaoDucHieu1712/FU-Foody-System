import axios from "axios";
import Cookies from "js-cookie";

const accessTokenKey = "fu_foody_token";

const instance = axios.create({
  baseURL: import.meta.env.VITE_FU_FOODY_PUBLIC_API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

//set Authorization when logged in
instance.interceptors.request.use(function (config) {
  const token = Cookies.get(accessTokenKey);
  if (token === undefined) {
    config.headers.Authorization = "";
  } else {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Add a response interceptor
instance.interceptors.response.use(
  function (response) {
    // Any status code that lie within the range of 2xx cause this function to trigger
    // Do something with response data
    return response.data;
  },
  function (error) {
    // Any status codes that falls outside the range of 2xx cause this function to trigger
    // Do something with response error
    return Promise.reject(error);
  }
);

export default instance;
