import { useEffect, useState } from "react";
import axios from "axios";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import ErrorText from "../../shared/components/text/ErrorText";

const schema = yup
    .object({
        ward: yup
            .string()
            .required("Hãy chọn xã!"),
        address: yup
            .string()
            .required("Hãy ghi thêm thông tin!"),
        description: yup
            .string()
            .nullable(),
    });

const Location = () => {
    const {
        register,
        setValue,
        handleSubmit,
        formState: { errors },
    } = useForm({
        resolver: yupResolver(schema),
        mode: "onChange",
    });


    const wardAPI = axios.get('https://provinces.open-api.vn/api/d/276?depth=2');
    const locationAPI = axios.get('https://localhost:7025/api/Location/ListLocation');
    const [locationList, setLocationList] = useState([]);
    const [wardList, setWardList] = useState([]);
    const [updateData, setUpdateData] = useState({});
    const [selectLocationId, setSelectLocationId] = useState(null);

    const onSubmit = async (data) => {
        try {
            alert(JSON.stringify(data));
            const newLocation = {
                address: data.address + "-" + data.ward + "-Thạch Thất-Hà Nội",
                // description: data.description || null,
            };
            const response = await axios.post("https://localhost:7025/api/Location/AddLocation", newLocation);
            alert(JSON.stringify(response.data));
        } catch (error) {
            console.error("Error add location: ", error);
        }
    }

    const handleUpdate = async (data) => {
        try {
            const newLocation = {
                id: selectLocationId,
                address: data.address + "-" + data.ward + "-Thạch Thất-Hà Nội",
                // description: data.description || null,
            };
            await axios.put(`https://localhost:7025/api/Location/UpdateLocation/${selectLocationId}`, newLocation);
            alert(JSON.stringify(newLocation));
            alert(JSON.stringify(selectLocationId));
        } catch (error) {
            console.error("Error update location: ", error);
        }
    }

    useEffect(() => {
        wardAPI.then(response => {
            const wards = response.data.wards || [];
            setWardList(wards);
        })
            .catch(error => {
                console.error("Ward: " + error);
            });

        locationAPI.then(response => {
            const locations = response.data || [];
            setLocationList(locations);
        })
            .catch(error => {
                console.error("Location: " + error);
            });
    }, []);

    const handleDefaultChange = () => {

    };

    const handleDeleteLocation = () => {

    };

    const handleUpdateLocation = (locationId, locationData) => {
        console.log(locationId + locationData);
        setSelectLocationId(locationId);
        setUpdateData(locationData);
    };

    return (
        <div className="w-full h-auto">
            <div className="flex items-center justify-between">
                <p className="px-5 mx-5 mt-2 font-bold text-lg">Địa chỉ của tôi</p>
                <p className="h-auto w-40 text-white text-center font-semibold bg-orange-700 cursor-pointer hover:bg-orange-900" onClick={handleDefaultChange}> + Thêm địa chỉ mới</p>
            </div>
            <div>
                <hr className="h-px my-4 bg-gray-200 border-0 dark:bg-gray-700" />
                <p className="px-5 mx-5 mt-2 font-bold text-lg">Địa chỉ</p>
                {locationList.map((location) => (
                    <div key={location.id} className="flex items-center justify-between w-full h-auto px-5 mx-5 py-2 my-2">
                        <div>
                            <p>{location.user.firstName + " " + location.user.lastName} | (+84) 123456789</p>
                            <hr className="h-px my-1 bg-gray-200 border-0 dark:bg-gray-700"></hr>
                            <p>{location.address}</p>
                            <p>Ghi chú: {location.address}</p>
                            {/* {location.isDefault == false ? null : <p className="text-orange-400 text-center font-semibold border-solid border-2 border-orange-400 h-auto w-20 pointer-events-none">Mặc định</p>} */}
                        </div>
                        <div >
                            <p className="text-blue-500 font-semibold cursor-pointer hover:underline hover:text-blue-600" onClick={() => handleUpdateLocation(location.id, location)}>Cập nhật</p>
                            {location.isDefault == true ? null : <p className="text-blue-500 font-semibold cursor-pointer hover:underline hover:text-blue-600" onClick={handleDeleteLocation}>Xóa</p>}
                            {location.isDefault == true ? <p className="text-orange-400 text-center font-semibold border-solid border-2 border-orange-400 h-auto w-20 pointer-events-none">Mặc định</p> : <p className="text-gray-400 text-center font-semibold border-solid border-2 border-gray-400 h-auto w-36 cursor-pointer hover:text-gray-600 hover:border-gray-600" onClick={handleDefaultChange}>Thiết lập mặc định</p>}
                        </div>
                    </div>
                ))}
            </div>
            <form className="form bg-white rounded px-40 py-20 mb-4" onSubmit={handleSubmit(onSubmit)}>
                <p className="font-bold text-2xl text-center mb-4">Thêm địa chỉ mới</p>
                <div className="mb-4">
                    <input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none" type="text" placeholder="Thành phố Hà Nội" readOnly></input>
                </div>
                <div className="mb-4">
                    <input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none" type="text" placeholder="Huyện Thạch Thất" readOnly></input>
                </div>
                <div className="inline-block relative mb-4">
                    <select
                        className="block appearance-none w-full bg-white border border-gray-400 hover:border-gray-500 px-4 py-2 pr-8 rounded shadow leading-tight focus:outline-none focus:shadow-outline"
                        {...register("ward")}
                        onChange={(e) => setValue('ward', e.target.value)}
                    >
                        <option value="">Chọn xã</option>
                        {wardList.map((ward) => (
                            <option
                                key={ward.code}
                                value={ward.name}
                            >
                                {ward.name}
                            </option>
                        ))}
                    </select>
                    {errors.ward && (
                        <ErrorText text={errors.ward.message}></ErrorText>
                    )}
                    <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
                        <svg className="fill-current h-4 w-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"><path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" /></svg>
                    </div>
                </div>
                <div className="mb-4">
                    <textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        placeholder="Địa chỉ cụ thể...(Trọ, Thôn,...)"
                        type="text"
                        {...register("address")}></textarea>
                    {errors.address && (
                        <ErrorText text={errors.address.message}></ErrorText>
                    )}
                </div>
                <div className="mb-4">
                    <textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        placeholder="Thông tin ghi chú thêm"
                        type="text"
                        {...register("description")}></textarea>
                </div>
                <button type="submit" className="text-white bg-orange-500 hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center">Submit</button>
            </form>

            <form className="bg-white rounded px-40 py-20 mb-4" onSubmit={() => handleUpdate()}>
                <p className="font-bold text-2xl text-center mb-4">Chỉnh sửa địa chỉ</p>
                <div className="mb-4">
                    <input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none" type="text" placeholder="Thành phố Hà Nội" readOnly></input>
                </div>
                <div className="mb-4">
                    <input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none" type="text" placeholder="Huyện Thạch Thất" readOnly></input>
                </div>
                <div className="inline-block relative mb-4">
                    <select
                        className="block appearance-none w-full bg-white border border-gray-400 hover:border-gray-500 px-4 py-2 pr-8 rounded shadow leading-tight focus:outline-none focus:shadow-outline"
                        {...register("ward")}
                        onChange={(e) => setValue('ward', e.target.value)}
                    >
                        <option value="">Chọn xã</option>
                        {wardList.map((ward) => (
                            <option
                                key={ward.code}
                                value={ward.name}
                            >
                                {ward.name}
                            </option>
                        ))}
                    </select>
                    {errors.select && <ErrorText text={errors.ward.message}></ErrorText>}
                    <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
                        <svg className="fill-current h-4 w-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"><path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" /></svg>
                    </div>
                </div>
                <div className="mb-4">
                    <textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        placeholder="Địa chỉ cụ thể...(Trọ, Thôn,...)"
                        type="text"
                        onChange={(e) => setValue('address', e.target.value)}
                    // value={updateData.address}
                    // {...register("address")}
                    >{updateData.address}</textarea>
                    {/* {errors.address && (
                        <ErrorText text={errors.address.message}></ErrorText>
                    )} */}
                </div>
                <div className="mb-4">
                    <textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        placeholder="Thông tin ghi chú thêm"
                        type="text"
                        onChange={(e) => setValue('description', e.target.value)}
                    // value={updateData.description}
                    // {...register("description")}
                    >{updateData.description}</textarea>
                    {/* {errors.description && (
                        <ErrorText text={errors.description.message}></ErrorText>
                    )} */}
                </div>
                <button type="submit" className="text-white bg-orange-500 hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center">Submit</button>
            </form>
        </div>
    );
};

export default Location;