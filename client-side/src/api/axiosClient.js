import axios from "axios";

const axiosInstance = axios.create({
  baseURL: "http://localhost:7217/api/",
  withCredentials: true,
});

export { axiosInstance };
    