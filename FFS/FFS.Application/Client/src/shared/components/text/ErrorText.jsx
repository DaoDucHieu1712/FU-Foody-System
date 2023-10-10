import propTypes from "prop-types";

const ErrorText = ({ text }) => {
  return <p className="text-sm text-red-500 my-1">{text}</p>;
};

ErrorText.propTypes = {
  text: propTypes.string.isRequired,
};

export default ErrorText;
