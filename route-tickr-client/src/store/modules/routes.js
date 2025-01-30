import {getClimbingRouteById, getTickedRouteIdsByState} from "@/services/api";
import {getAllTickedRoutes} from "@/services/api";

const state = {
    routes: [],
    route: null,
    tickedRoutesByState: {},
    filteredRoutes: []
};

const mutations = {
    SET_ROUTE_DATA(state, data) {
        state.route = data;
    },
    SET_ALL_ROUTES(state, data) {
        state.routes = data;
    },
    SET_TICKED_ROUTE_IDS(state, { stateName, routeIds }) {
        state.tickedRoutesByState[stateName] = routeIds;
    },
    SET_FILTERED_TICKED_ROUTES(state, routes) {
        state.filteredRoutes = routes;
    }
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
    async fetchRouteIdsByState({commit}, stateName){
        if (state.tickedRoutesByState[stateName]) {
            console.log(`Data for ${stateName} is already cached`);
            return;
        }

        try {
            const response = await getTickedRouteIdsByState(stateName);
            const routeIds = response.data;
            console.log(`ids ${routeIds}`);
            commit("SET_TICKED_ROUTE_IDS", { stateName, routeIds });
            
            commit("SET_FILTERED_TICKED_ROUTES", routeIds.map(id => state.routes.find(r => r.id === id)));
        } catch (error) {
            console.error(`Error fetching ticked routes for ${stateName}:`, error);
        }
    },
};

const getters = {
    routeData: (state) => state.route,
    allRoutes: (state) => state.routes,
    tickedRoutesByState: (state) => state.tickedRoutesByState,
    filteredRoutes: (state) => state.filteredRoutes
};

export default {
    state,
    mutations,
    actions,
    getters,
};
