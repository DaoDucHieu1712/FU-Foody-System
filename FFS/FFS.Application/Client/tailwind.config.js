import withMT from "@material-tailwind/react/utils/withMT";
/** @type {import('tailwindcss').Config} */
// eslint-disable-next-line no-undef
export default withMT({
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        primary: "#FE5303",
        borderpri: "#EBEDEE",
      },
    },
  },
  plugins: [],
});
