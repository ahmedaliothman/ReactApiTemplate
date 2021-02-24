import React, { useState, useEffect } from 'react';
import './App.css';
import { BrowserRouter as Router, Switch } from "react-router-dom";
import { useSelector, useDispatch, TypedUseSelectorHook  } from 'react-redux';
import { PostDetail, PostCreate, PostEdit, PostDelete, Posts, Home } from "./components/Posts";
import { Login } from "./components/Login";
// import { Register } from "./components/Register";
import { PrivateRoute,PublicRoute } from "./layouts/Routing";
import { StoreState } from './Store/types/index';



const App = () => {
const userContext=useSelector<StoreState.All>(state=>state.state1)


  return (
    <Router>
     
              <Switch>
                <PrivateRoute path="/post-detail/:id" component={PostDetail}></PrivateRoute>
                <PrivateRoute path="/post-create" component={PostCreate}></PrivateRoute>
                <PrivateRoute path="/post-edit/:id" component={PostEdit}></PrivateRoute>
                <PrivateRoute path="/post-delete/:id" component={PostDelete}></PrivateRoute>
                <PrivateRoute path="/posts" component={Posts}></PrivateRoute>
                <PublicRoute path="/login" component={Login}></PublicRoute>
                {/* <PublicRoute path="/register" component={Register}></PublicRoute> */}
                <PublicRoute path="/" component={Home}></PublicRoute>


              </Switch>
        
    </Router>
  );
}

export default App;