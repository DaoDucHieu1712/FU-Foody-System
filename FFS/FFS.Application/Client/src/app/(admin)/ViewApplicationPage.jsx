import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "../../shared/api/axiosConfig";
import ShipperApplication from "./shared/components/ShipperApplication";
import StoreOwnderApplication from "./shared/components/StoreOwnderApplication";

const ViewApplicationPage = () => {
	const { id } = useParams();
	const [role, setRole] = useState();

	useEffect(() => {
		GetRoleAsync();
	}, [id]);

	const GetRoleAsync = async () => {
		axios.get(`/api/Authenticate/GetRoleByUser/${id}`).then((res) => {
			setRole(res);
		});
	};

	if (role == "Shipper") {
		return <ShipperApplication />;
	}

	if (role == "StoreOwner") {
		return <StoreOwnderApplication />;
	}
};

export default ViewApplicationPage;
