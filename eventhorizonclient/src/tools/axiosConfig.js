import axios from "axios";

var axiosClient = axios.create({
    baseURL: "https://localhost:8081/api/",
    timeout: 2000,
    withCredentials: true,
});

export default axiosClient;