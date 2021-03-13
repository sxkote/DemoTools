import { createStore } from 'vuex';
import SYMBOLS from './symbols';
// define injection key
export const key = SYMBOLS.STORE;
export const store = createStore({
    state: {
        authToken: undefined
    },
    getters: {
        isAuthenticated: state => !!state.authToken && state.authToken.isValid(),
        token: state => state.authToken,
        userName: state => !!state.authToken && state.authToken.isValid() ? state.authToken.Name : undefined
    },
    mutations: {
        setToken(state, payload) {
            state.authToken = payload.token;
        }
    },
    actions: {
        setToken(context, payload) {
            context.commit('setToken', payload);
        }
    }
});
//# sourceMappingURL=store.config.js.map