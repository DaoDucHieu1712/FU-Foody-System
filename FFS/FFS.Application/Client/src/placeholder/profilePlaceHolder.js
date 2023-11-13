import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { setAccessToken, setUserProfile } from "../redux/auth";
import CookieService from "../shared/helper/cookieConfig";
import { useSelector } from "react-redux";
import axios from "../shared/api/axiosConfig";

const ProfilePlaceHolder = () => {
  // const accessToken = useSelector((state) => state.auth.accessToken);
  const accessToken = CookieService.getToken("fu_foody_token");

  const dispatch = useDispatch();

  useEffect(() => {
    if (!!accessToken) {
      dispatch(setAccessToken(accessToken));
      const fetchUserInfo = async () => {
        try {
          const response = await axios.get("/api/Authenticate/GetCurrentUser");

          dispatch(setUserProfile(response));
        } catch (error) {
          console.error("Error fetching user data:", error);
        }
      };
      fetchUserInfo();
    }
  }, [accessToken]);

  return null;
};
export default ProfilePlaceHolder;
