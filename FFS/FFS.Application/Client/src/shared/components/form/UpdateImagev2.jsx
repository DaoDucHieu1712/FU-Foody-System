import axios from "axios";
import { useState } from "react";
import { toast } from "react-toastify";
import propTypes from "prop-types";


const UpdateImagev2 = ({ onChange, name, url }) => {
  const [imageURL, setImageURL] = useState(url);
  const handleUploadImage = async (e) => {
    const file = e.target.files;
    if (!file) return;
    const bodyFormData = new FormData();
    bodyFormData.append("image", file[0]);
    const response = await axios({
      method: "post",
      url: import.meta.env.VITE_FU_FOODY_PUBLIC_API_BASE_URL_UPLOAD_IMAGE,
      data: bodyFormData,
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    const imageData = response.data.data;
    if (!imageData) {
      toast.error("Không thể upload ảnh !");
      return;
    }
    const imageObj = {
      thumb: imageData.thumb.url,
      url: imageData.url,
    };
    onChange(name, imageObj.thumb);
    setImageURL(imageObj.thumb);
  };
  return (
    <>
      <label className="flex flex-col items-center justify-center w-full cursor-pointer border boder-gray-200 border-dashed px-0 py-6">
      <img
      className="h-[300px] w-full  object-cover object-center"
      src={imageURL}
      alt="anh upload"
    />
        {/* <img src={imageURL} alt="anh upload" className="object-cover"/> */}
        <input type="file" onChange={handleUploadImage} hidden />
        <span className="p-2 border border-gray-700 mt-5">Thay ảnh</span>
      </label>
    </>
  );
};

UpdateImagev2.propTypes = {
  name: propTypes.any,
  onChange: propTypes.any,
  url: propTypes.any.isRequired,
};

export default UpdateImagev2;
