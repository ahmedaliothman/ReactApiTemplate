import React from 'react';
import { BrowserRouter as Router, Switch, Route, Link, Redirect, useRouteMatch, useParams, useHistory } from "react-router-dom";
import { useSelector, useDispatch } from 'react-redux';
import { MasterPage } from './MasterPage';
import PublicMasterPage from './PublicMasterPage';

export const PrivateRoute = ({ component: Component, ...rest }) => {

    const userContext = useSelector(state => {
      return state.userContext;
    });
  
    return (
      <Route {...rest} render={props => {
        return (userContext.isAuthenticated)
          ? <MasterPage><Component {...props} /></MasterPage>
          : <Redirect to={{ pathname: '/login', state: { from: props.location } }} />;
      }} />
    )
  }
  
  
  export const  PublicRoute=({ component: Component, ...rest })=> {
    return (
      <Route {...rest} render={props => {
        return ( <PublicMasterPage><Component {...props} /></PublicMasterPage>)
        }}/>
    )
  }
  