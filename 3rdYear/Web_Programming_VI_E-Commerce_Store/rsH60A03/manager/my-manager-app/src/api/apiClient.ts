import axios, { AxiosInstance } from "axios";

const apiClient: AxiosInstance = axios.create({
    baseURL: "http://localhost:5219",
    timeout: 1000,
    headers: {
        "Content-Type": "application/json",
    },
});

export default apiClient;
