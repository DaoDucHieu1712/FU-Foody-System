import propTypes from "prop-types";

const StoreTag = ({ name }) => {
  return (
    <div className="p-1 border-[1px] border-gray-500 text-white">{name}</div>
  );
};

StoreTag.propTypes = {
  name: propTypes.string.isRequired,
};

export default StoreTag;
