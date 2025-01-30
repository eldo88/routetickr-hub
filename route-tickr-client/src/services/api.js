import axios from "axios";

const apiBaseUrl = "https://localhost:7071";

export async function getClimbingRouteById(id) {
    try {
        const response = await axios.get(`${apiBaseUrl}/api/Tick/GetById/${id}`);
        return response.data;
    } catch (error) {
        console.error("Error fetching data:", error);
        throw error;
    }
}

export async function getAllTickedRoutes() {
    try {
        const response = await axios.get(`${apiBaseUrl}/api/Tick/GetAll`);
        return response.data;
    } catch (error) {
        console.error("Error fetching data:", error);
        throw error;
    }
}

export async function getTickedRouteIdsByState(state) {
    try {
        const response = await axios.get(`${apiBaseUrl}/api/ClimbingStats/GetTickIdsPerState?state=${state}`)
        return response.data;
    } catch (error) {
        console.error(`Error fetching routes ids for ${state}`, error);
        throw error;
    }
}
