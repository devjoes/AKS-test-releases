import React from 'react';
import PropTypes from 'prop-types';
import Graph from './graph';

const graph = class extends React.Component {
    constructor() {
        super();
    }

    nodeClicked(n){
        const data = n[0]._private.data;
        this.props.routeClicked(data.routeData);
    }
    render() {
        return <Graph elements={this.props.elements} onNodeClicked={n => this.nodeClicked(n)} />
    }
}

graph.propTypes = {
    routeClicked: PropTypes.func,
    loadData: PropTypes.func,
    elements: PropTypes.array
}
export default graph;