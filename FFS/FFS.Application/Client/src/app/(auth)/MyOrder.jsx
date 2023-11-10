import { useEffect } from "react";
import axios from "../../../src/shared/api/axiosConfig"

const MyOrder = () => {



    const GetMyOrderList = async () => {
        try {
            await axios
                .get(`/api/Food/ListFood`)
                .then((response) => {

                })
                .catch((error) => {
                    console.log(error);
                })
        } catch (error) {
            console.log(`error when get my order: ${error}`);
        }
    }

    useEffect(() => {
        GetMyOrderList();
    }, []);

    return (
        <div>

        </div>
    );
};

export default MyOrder;