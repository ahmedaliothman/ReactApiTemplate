import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Constants } from "../constants";
import { useHistory } from "react-router-dom";
import { StoreState } from '../Store/types/index';


export const MasterPage=({ children }:any) =>{
    return (
        <div className="flex flex-col items-strech">
            <h1>this is header</h1>
            <Navigation/>
            <main className="w-full bg-blue-300 text-center">
                {children}
            </main>
            <footer className="w-full bg-blue-600 text-center">
               <h1> This is my footer.</h1>
            </footer>
        </div>
    );}


export const Navigation = () => {
  
    let userContext:StoreState.state1;
    userContext =  useSelector<StoreState.state1>(state => { return state;}) as StoreState.state1;
    let dispatch = useDispatch();
    let history = useHistory();
  
    const logOut = () => {
      dispatch({ type: Constants.LOGOUT_REQUEST });
    }
  
    let login = () => {
      history.push('/login');
    }
  
    let register = () => {
      history.push('/register');
    }
  
    return (
      <>
        <h3>hello {userContext.user && <><span>{userContext.user.username}</span></>}</h3>
        {userContext.isAuthenticated && <button onClick={logOut}>Log Out</button>}
        {!userContext.isAuthenticated &&
          <>
            <button onClick={login}>Login</button> <br />
            <button onClick={register}>Register</button>
          </>
        }
      </>
    );
  }
  