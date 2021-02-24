export namespace StoreState{

    export type state1 = {
        isAuthenticated: Boolean;
        user: any;
        token: any;
        isRegistered: Boolean;
        error: any;
    }

    export type state2 = {
        postList:[any];
        selectedPost: any;
        selectedComments: [any];
        notificationText: any;
    }

    export type All = {
        state1:state1;
        state2:state2;
    }
}
