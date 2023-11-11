import axiosConfig from "../../../shared/api/axiosConfig";

const LocationService = {
  async getAll(email) {
    const url = `/api/Location/ListLocation?email=${email}`;
    return axiosConfig.get(url);
  },
};

export default LocationService;
