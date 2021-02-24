import { Constants } from "../constants";
import {StoreState} from ".././Store/types/index";

const initialState = {
    isAuthenticated: false,
    user: null,
    token: null,
    isRegistered: false,
    error: null
}
// type Props = {
//     state:StoreState.state1,
//     action:{type:string,payload:{
//         data:{
//             Status   : number,
//             Message  : string, 
//             HasError : Boolean,
//             Response :any   ,
//             userInfo:any,
//             jwtToken:any
//             }
//         }
//    }
// }
  


export const User = (state=initialState,action:any):StoreState.state1 => {
    switch (action.type) {
        case Constants.LOGIN_SUCCESS:
            const data = action.payload.data;
            localStorage.setItem('data', JSON.stringify(data));
            return {
                ...state,
                isAuthenticated: true,
                user: { username: data.userInfo.email },
                token: data.jwtToken,
            };
        case Constants.LOGOUT_REQUEST:
            localStorage.removeItem('data');
            return {
                ...state,
                isAuthenticated: initialState.isAuthenticated,
                user: initialState.user,
                token: initialState.token,
            };
        case Constants.REGISTER_SUCCESS:
            console.log('REGISTER_SUCCESS', action.payload);
            return {
                ...state,
                isRegistered: true,
            };
        case Constants.REGISTER_FAILURE:
            const error = action.payload;
            let message = JSON.stringify(error.data);
            return {
                ...state,
                isRegistered: false,
                error: { status: error.data.Status, message: message }
            };
        default:
            let localStorageData:any = localStorage.getItem('data');
            if (localStorageData) {
                localStorageData = JSON.parse(localStorageData);
                return {
                    ...state,
                    isAuthenticated: true,
                    user: { username: localStorageData.userInfo.email },
                    token: localStorageData.jwtToken,
                }
            }

            return state;
    }
}