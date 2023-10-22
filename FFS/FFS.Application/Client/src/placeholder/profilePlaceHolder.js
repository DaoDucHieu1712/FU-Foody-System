import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { setAccessToken } from "../redux/auth";
import CookieService from "../shared/helper/cookieConfig";
import { useSelector } from "react-redux";
const ProfilePlaceHolder = () => {
    // const accessToken = CookieService.getToken("ACCESS_TOKEN");
    const accessToken = useSelector((state) => state.auth.accessToken);

  console.log("Place: ", accessToken);
  const dispatch = useDispatch();
  useEffect(() => {
    
    
    if (!!accessToken) {
      dispatch(setAccessToken(accessToken));
    }else{
        return;
    }
    console.log("Useeffect ", accessToken);
  }, [accessToken]);

  return null;
};
export default ProfilePlaceHolder;
