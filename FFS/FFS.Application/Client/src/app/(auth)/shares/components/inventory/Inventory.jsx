import { useEffect, useState } from "react";
import axios from "axios";
import AddInventory from "../inventory/AddInventory";

const Inventory = () => {
    return(
        <>
         <div className="w-full h-auto">
            <div className="flex items-center justify-between">
                <p className="px-5 mx-5 mt-2 font-bold text-lg pointer-events-none">Tồn kho</p>
                <AddInventory></AddInventory>
            </div>
            {/* <div>
                <hr className="h-px my-4 bg-gray-200 border-0 dark:bg-gray-700" />
                <p className="px-5 mx-5 mt-2 font-bold text-lg pointer-events-none">Địa chỉ</p>
               
                
            </div> */}



        </div></>
    )
};
export default Inventory;