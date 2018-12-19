import 'bootstrap/dist/css/bootstrap.min.css';
import React, { Component } from 'react';
import './App.css';
import Graph from './components/graph';
import RouteDetails from './components/routeDetails';
import loadGraphData from './utils/loadGraphData';
import { ns } from './utils/settings';


class App extends Component {
  constructor(){
    super();
    this.state = {};
  }
  componentDidMount(){
    this.loadData();
  }
  routeClicked(selectedRouteData) {
    this.setState({...this.state, selectedRouteData});
  }
  async loadData() {
    this.setState({graphElements: undefined, selectedRouteData: undefined});
    const graphElements = await loadGraphData(ns);
    this.setState({graphElements})
  }
  render() {
    return (
      <div>
        <div style={{width: '100%'}}>
          <Graph routeClicked={routeData => this.routeClicked(routeData)} elements={this.state.graphElements} />
        </div>
        <div>
          {
            this.state.selectedRouteData
            ? <RouteDetails routeData={this.state.selectedRouteData} loadData={() => this.loadData()} />
            : <div/>
          }
        </div>
      </div>
    );
  }
}

export default App;
