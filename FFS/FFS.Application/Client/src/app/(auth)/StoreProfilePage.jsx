import { useParams } from "react-router-dom";
import { useEffect } from "react";
import axios from '../axiosInstance'; 
import { toast } from "react-toastify";

const StoreProfilePage = () => {
    const { id } = useParams();

    // const [storeData, setStoreData] = useState(null);
    const GetStoreInformation = async () => {
        try {
          axios
          .get(`/api/Store/GetStoreInformation/${id}`)
          .then((response) => {
            console.log(response);
          })
          .catch((error) => {
            toast.error(error.response.data);
          });
        } catch (error) {
          console.error('An error occurred', error);
        }
      };

  useEffect(() => {
    GetStoreInformation();
  }, [id]);
    return <>
     Store Profile Page for ID: {id}
     
     </>;
}

export default StoreProfilePage;