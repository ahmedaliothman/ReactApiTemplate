import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Switch, Route, Link, Redirect, useRouteMatch, useParams, useHistory } from "react-router-dom";
import { useForm } from 'react-hook-form';
import { useSelector, useDispatch } from 'react-redux';
import { Constants } from "../constants";
import { StoreState } from '../Store/types';
import { type } from 'os';

export const Login = () => {
    let history = useHistory();
    let dispatch = useDispatch();
    const { register, handleSubmit, watch, errors } = useForm();

    let submitData = (data:any) => {
        dispatch({
            type: Constants.LOGIN_REQUEST, payload: data
        })
        history.push('/');
    }

    const onSubmit = (data:any) => {
        submitData(data);
    };
     let userContext:StoreState.state1;
     userContext =  useSelector<StoreState.state1>(state => { return state;}) as StoreState.state1;

    return (userContext.isAuthenticated) ? <Redirect to={{ pathname: '/' }} /> :
        <>
            <div className="col-md-6 col-md-offset-3">
                <h2>Login</h2>
                <form name="form" onSubmit={handleSubmit(onSubmit)}>
                    <div className="form-group">
                        <label htmlFor="username">Username</label>
                        <input type="text" className="form-control" name="username" ref={register({ required: true })} />
                        <span>{errors.username && 'Username is required'}</span>
                    </div>
                    <div className='form-group'>
                        <label htmlFor="password">Password</label>
                        <input type="password" className="form-control" name="password" ref={register({ required: true })} />
                        <span>{errors.password && 'Password is required'}</span>
                    </div>
                    <div className="form-group">
                        <input type="submit" className="btn btn-primary" />
                    </div>
                </form>
            </div>
        </>;
};