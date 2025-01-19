import { getClimbingRouteById } from "@/services/api";

const state = {
    routeData: null,
};

const mutations = {
    SET_ROUTE_DATA(state, data) {
        state.routeData = data;
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
};

const getters = {
    routeData: (state) => state.routeData,
};

export default {
    state,
    mutations,
    actions,
    getters,
};
