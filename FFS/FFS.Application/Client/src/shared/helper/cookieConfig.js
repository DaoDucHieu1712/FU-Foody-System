import Cookies from "universal-cookie";

const cookies = new Cookies();
export default class CookieService {
  static saveToken(name, value, expires) {
    cookies.set(name, value, {
      sameSite: true,
      path: "/",
      expires,
      // domain: import.meta.env.VITE_ENV === "dev" ? "" : import.meta.env.VITE_SUB_DOMAIN,
    });
  }
  static getToken(name) {
    return cookies.get(name);
  }

  static removeToken(name) {
    console.log("Name: ", name);
    cookies.remove(name, { path: "/" });
  }
}
