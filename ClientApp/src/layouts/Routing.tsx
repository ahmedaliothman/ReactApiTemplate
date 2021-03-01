import React from 'react';
import { BrowserRouter as Router, Switch, Route, Link, Redirect, useRouteMatch, useParams, useHistory } from "react-router-dom";
import { useSelector, useDispatch } from 'react-redux';
import { MasterPage } from './MasterPage';
import PublicMasterPage from './PublicMasterPage';
import { StoreState } from '../Store/types/index';

export const PrivateRoute = ({ component: Component, ...rest }:any) => {
  // const userContext=useSelector<StoreState.state1>(state=>state)
    // const userContext = useSelector(state => {
    //   return state.userContext;
    // });
    let userContext:StoreState.state1;
    userContext =  useSelector<StoreState.state1>(state => { return state;}) as StoreState.state1;
    return (
      <Route {...rest} render={props => {
        return (userContext.isAuthenticated)
          ? <MasterPage><Component {...props} /></MasterPage>
          : <Redirect to={{ pathname: '/login', state: { from: props.location } }} />;
      }} />
    )
  }
  
  
  export const  PublicRoute=({ component: Component, ...rest }:any)=> {
    return (
      <Route {...rest} render={props => {
        return ( <PublicMasterPage><Component {...props} /></PublicMasterPage>)
        }}/>
    )
  }
  