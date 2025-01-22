import { getClimbingRouteById } from "@/services/api";
import {getAllTickedRoutes} from "@/services/api";

const state = {
    routes: [],
    route: null,
};

const mutations = {
    SET_ROUTE_DATA(state, data) {
        state.route = data;
    },
    SET_ALL_ROUTES(state, data) {
        state.routes = data;
    },
};

const actions = {
    async fetchRouteData({ commit }, routeId) {
        try {
            const data = await getClimbingRouteById(routeId);
            commit("SET_ROUTE_DATA", data);
        } catch (error) {
            console.error("Error fetching route data:", error);
        }
    },
    async fetchAllRoutes({commit}){
        try {
            const data = await getAllTickedRoutes();
            commit("SET_ALL_ROUTES", data);
        } catch (error) {
            console.error("Error fetching route data:", error);
        }
    },
};

const getters = {
    routeData: (state) => state.route,
    allRoutes: (state) => state.routes,
};

export default {
    state,
    mutations,
    actions,
    getters,
};
