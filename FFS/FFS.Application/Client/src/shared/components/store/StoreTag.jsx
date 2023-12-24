import propTypes from "prop-types";

const StoreTag = ({ name, url }) => {
	return (
		<a href={url} className="p-1 border-[1px] border-gray-500 text-white">
			{name}
		</a>
	);
};

StoreTag.propTypes = {
	name: propTypes.string,
	url: propTypes.string,
};

export default StoreTag;
