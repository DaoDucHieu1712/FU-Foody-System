import { useEffect, useState } from "react";
import axios from "axios";

const Location = () => {
    const wardAPI = axios.get('https://provinces.open-api.vn/api/d/276?depth=2');
    const locationAPI = axios.get('https://localhost:7025/api/Location/ListLocation');
    const [locationList, setLocationList] = useState([]);
    const [wardList, setWardList] = useState([]);
    const [selectWard, setSelectWard] = useState('');

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

    const handleWardChange = (e) => {
        setSelectWard(e.target.value);
    };

    const handleDefaultChange = () => {

    };

    const handleDeleteLocation = () => {

    };

    const handleUpdateLocation = () => {

    };

    return (
        <div className="w-full h-auto">
            <div>
                <h1 className="text-center text-2xl font-bold">List Location</h1>
                <p className="px-5 mx-5 mt-2 font-bold text-lg">Địa chỉ</p>
                {locationList.map((location) => (
                    <div key={location.id} className="flex items-center justify-between w-full h-auto px-5 mx-5 py-2 my-2">
                        <div>
                            <p>Nguyễn Văn A | (+84) 123456789</p>
                            <hr className="h-px my-1 bg-gray-200 border-0 dark:bg-gray-700"></hr>
                            <p>{location.address}</p>
                            <p>Ghi chú: {location.address}</p>
                            {location.isDefault == false ? null : <p className="text-orange-400 text-center font-semibold border-solid border-2 border-orange-400 h-auto w-20">Mặc định</p>}
                        </div>
                        <div >
                            <p className="text-blue-500 font-semibold cursor-pointer hover:underline hover:text-blue-600" onClick={handleUpdateLocation}>Cập nhật</p>
                            {location.isDefault == true ? null : <p className="text-blue-500 font-semibold cursor-pointer hover:underline hover:text-blue-600" onClick={handleDeleteLocation}>Xóa</p>}
                            <p className="text-gray-400 text-center font-semibold border-solid border-2 border-gray-400 h-auto w-36 cursor-pointer hover:text-gray-600 hover:border-gray-600" onClick={handleDefaultChange}>Thiết lập mặc định</p>
                        </div>
                    </div>
                ))}
            </div>
            <form className="bg-white rounded px-40 py-20 mb-4">
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
                        onChange={handleWardChange}
                        value={selectWard}
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
                    <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
                        <svg className="fill-current h-4 w-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"><path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" /></svg>
                    </div>
                </div>
                <div className="mb-4">
                    <textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" placeholder="Địa chỉ cụ thể...(Trọ, Thôn,...)" type="text"></textarea>
                </div>
                <div className="mb-4">
                    <textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" placeholder="Thông tin ghi chú thêm" type="text"></textarea>
                </div>
                <button type="submit" className="text-white bg-orange-500 hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center">Submit</button>
            </form>

            <form className="bg-white rounded px-40 py-20 mb-4">
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
                        onChange={handleWardChange}
                        value={selectWard}
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
                    <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
                        <svg className="fill-current h-4 w-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"><path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" /></svg>
                    </div>
                </div>
                <div className="mb-4">
                    <textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" placeholder="Địa chỉ cụ thể...(Trọ, Thôn,...)" type="text"></textarea>
                </div>
                <div className="mb-4">
                    <textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" placeholder="Thông tin ghi chú thêm" type="text"></textarea>
                </div>
                <button type="submit" className="text-white bg-orange-500 hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center">Submit</button>
            </form>
        </div>
    );
};

export default Location;