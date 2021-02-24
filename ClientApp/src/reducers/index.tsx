import { combineReducers, Store } from "redux";
import { StoreState } from "../Store/types/index";
import {posts} from "./postsReducer";
import {User} from "./userReducer";


const rootReducer = combineReducers<any,any>({
    User,
    posts
});

export default rootReducer;
//export type rootReducer = ReturnType<typeof rootReducer>;