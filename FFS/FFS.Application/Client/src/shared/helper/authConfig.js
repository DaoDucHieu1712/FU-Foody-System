import Cookies from "js-cookie";
const accessTokenKey = "crow_access_token";
const refreshTokenKey = "crow_refresh_token";
const objCookies = {
  expires: 30,
  domain: import.meta.env.VITE_FU_FOODY_COOKIE_DOMAIN,
};



export const saveToken = (access_token, refresh_token) => {
  if (access_token && refresh_token) {
    Cookies.set(accessTokenKey, access_token, {
      ...objCookies,
    });
    Cookies.set(refreshTokenKey, refresh_token, {
      ...objCookies,
    });
  } else {
    Cookies.remove(accessTokenKey, {
      ...objCookies,
      path: "/",
      domain: import.meta.env.VITE_FU_FOODY_COOKIE_DOMAIN,
    });
    Cookies.remove(refreshTokenKey, {
      ...objCookies,
      path: "/",
      domain: import.meta.env.VITE_FU_FOODY_COOKIE_DOMAIN,
    });
  }
};

export const saveTokenv2 = (access_token) => {
  console.log(access_token);
    Cookies.set(accessTokenKey, access_token, {
      ...objCookies,
   
    });
   
    
};


export const getToken = () => {
  const access_token = Cookies.get(accessTokenKey);
  const refresh_token = Cookies.get(refreshTokenKey);
  return {
    access_token,
    refresh_token,
  };
};

export const getTokenv2 = () => {
  const access_token = Cookies.get(accessTokenKey);
  
  return {
    access_token,
  };
};
export const logOut = () => {
  const access_token = Cookies.get(accessTokenKey);
  if (access_token) {
    Cookies.remove(accessTokenKey, {
      ...objCookies,
      path: "/",
      domain: import.meta.env.VITE_FU_FOODY_COOKIE_DOMAIN,
    });
    Cookies.remove(refreshTokenKey, {
      ...objCookies,
      path: "/",
      domain: import.meta.env.VITE_FU_FOODY_COOKIE_DOMAIN,
    });
  }
};
