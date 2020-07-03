import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { ItemWiki } from "./components/ItemWiki";
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';

import './custom.css'
import PainterCanvas from "./components/PainterCanvas";
import GameCanvas from "./components/GameCanvas";

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/painter' component={PainterCanvas} />
        <Route path='/game' component={GameCanvas} />
        <Route path='/counter' component={Counter} />
        <AuthorizeRoute path='/fetch-data' component={FetchData} />
        <Route path='/item-wiki' component={ItemWiki} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Layout>
    );
  }
}
