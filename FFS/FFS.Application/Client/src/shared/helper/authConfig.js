import Cookies from "js-cookie";
const token = "fu_foody_token";
const objCookies = {
  expires: 30,
  domain: import.meta.env.VITE_FU_FOODY_COOKIE_DOMAIN,
};

export const saveToken = (token) => {
  if (token) {
    Cookies.set(token, {
      ...objCookies,
    });
    Cookies.set(token, {
      ...objCookies,
    });
  } else {
    Cookies.remove(token, {
      ...objCookies,
      path: "/",
      domain: import.meta.env.VITE_FU_FOODY_COOKIE_DOMAIN,
    });
  }
};

export const getToken = () => {
  const _token = Cookies.get(token);
  return _token;
};
export const logOut = () => {
  const _token = Cookies.get(token);
  if (_token) {
    Cookies.remove(_token, {
      ...objCookies,
      path: "/",
      domain: import.meta.env.VITE_FU_FOODY_COOKIE_DOMAIN,
    });
  }
};
