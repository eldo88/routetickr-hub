import { createStore } from 'vuex';
import routes from './modules/routes'; // Import your routes module

const store = createStore({
    modules: {
        routes, // Register your module
    },
});

export default store;
