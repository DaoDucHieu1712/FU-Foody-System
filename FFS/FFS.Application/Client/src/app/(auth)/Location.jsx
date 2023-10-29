import { useEffect, useState } from "react";
import axios from "../../shared/api/axiosConfig";
import axioss from "axios";
import UpdateLocation from "./shares/components/UpdateLocation";
import AddLocation from "./shares/components/AddLocation";
import DeleteLocation from "./shares/components/DeleteLocation";
import DefaultLocation from "./shares/components/DefaultLocation";

const Location = () => {


    const wardAPI = axioss.get('https://provinces.open-api.vn/api/d/276?depth=2');
    const [locationList, setLocationList] = useState([]);
    const [wardList, setWardList] = useState([]);

    const reloadList = async () => {
        try {
            axios
                .get('/api/Location/ListLocation')
                .then((response) => {
                    setLocationList(response)
                })
                .catch((error) => {
                    console.log(error);
                });
        } catch (error) {
            console.error("Location: " + error);
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

        reloadList();
    }, []);


    return (
        <div className="w-full h-auto">
            <div className="flex items-center justify-between">
                <p className="px-5 mx-5 mt-2 font-bold text-lg pointer-events-none">Địa chỉ của tôi</p>
                <AddLocation reload={reloadList} wardList={wardList}></AddLocation>
            </div>
            <div>
                <hr className="h-px my-4 bg-gray-200 border-0 dark:bg-gray-700" />
                <p className="px-5 mx-5 mt-2 font-bold text-lg pointer-events-none">Địa chỉ</p>
                {locationList.map((location) => (
                    location.isDelete == true ? null :
                        <div key={location.id} className="flex items-center justify-between w-full h-auto px-5 mx-5 py-2 my-2">
                            <div className="pointer-events-none">
                                <p>{location.receiver} | (+84) {location.phoneNumber}</p>
                                <hr className="h-px my-1 bg-gray-200 border-0 dark:bg-gray-700"></hr>
                                <p>{location.address}</p>
                                <p>Ghi chú: {location.description}</p>
                                {/* {location.isDefault == false ? null : <p className="text-orange-400 text-center font-semibold border-solid border-2 border-orange-400 h-auto w-20 pointer-events-none">Mặc định</p>} */}
                            </div>
                            <div >
                                <UpdateLocation item={location} reload={reloadList} wardList={wardList}></UpdateLocation>
                                {location.isDefault == true ? null : <DeleteLocation id={location.id} reload={reloadList}></DeleteLocation>}
                                {location.isDefault == true ? <p className="text-orange-400 text-center font-semibold border-solid border-2 border-orange-400 h-auto w-20 pointer-events-none">Mặc định</p> : <DefaultLocation item={location} reload={reloadList}></DefaultLocation>}
                            </div>
                        </div>
                ))}
            </div>



        </div>
    );
};

export default Location;