import {Dispatch} from 'react';
import {applyMiddleware} from 'redux';
import {createStore} from 'react-hooks-global-state';
import {AccountData} from "./Model/LoginResponse.ts";



// Your other imports remain the same



type State = {
    authToken: string | null;
    accountData: AccountData | null;
};

type Action =
    | { type: 'setAuthToken'; authToken: string | null }
    | { type: 'setAccountData'; accountData: AccountData }


const defaultState: State = {
   authToken: null, 
   accountData: null


};

const LOCAL_STORAGE_KEY = 'OVS_TurboWebUi';
const parseState = (str: string | null): State | null => {
    try {
        const state = JSON.parse(str || '');
        if (typeof state.authToken !== 'string' && state.authToken !== null ) throw new Error();
        return state as State;
    } catch (e) {
        return null;
    }
};
const stateFromStorage = parseState(localStorage.getItem(LOCAL_STORAGE_KEY));
const initialState: State = stateFromStorage || defaultState;


const reducer = (state = initialState, action: Action) => {
    switch (action.type) {
        case 'setAuthToken':
            return {
                ...state,
                authToken: action.authToken
            };
        case 'setAccountData':
            return {
                ...state,
                accountData: action.accountData
            };
        
        default:
            return state;
    }
};

const saveStateToStorage = (
    {getState}: { getState: () => State },
) => (next: Dispatch<Action>) => (action: Action) => {
    const returnValue = next(action);
    localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(getState()));
    return returnValue;
};

export const {dispatch, useStoreState, } = createStore(
    reducer,
    initialState,
    applyMiddleware(saveStateToStorage),
);


