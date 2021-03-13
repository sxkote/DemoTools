import { InjectionKey } from 'vue'
import { createStore, Store } from 'vuex'
import { IToken } from '../interfaces/interfaces'
import SYMBOLS from './symbols'

// define your typings for the store state
export interface State {
    authToken?: IToken
}

// define injection key
export const key: InjectionKey<Store<State>> = SYMBOLS.STORE;

export const store = createStore<State>({
    state: {
        authToken: undefined
    },
    getters: {
        isAuthenticated: state => !!state.authToken && state.authToken.isValid(),
        token: state => state.authToken,
        userName: state => !!state.authToken && state.authToken.isValid() ? state.authToken.Name : undefined
    },
    mutations: {
        setToken(state: State, payload) {
            state.authToken = payload.token
        }
    },
    actions: {
        setToken(context, payload) {
            context.commit('setToken', payload);
        }
    }
})
