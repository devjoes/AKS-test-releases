import Cytoscape from 'cytoscape';
import Dagre from 'cytoscape-dagre';
import React from 'react';
import CytoscapeComponent from 'react-cytoscapejs';

Cytoscape.use(Dagre);

const graph = props => {
    return props.elements
        ? <CytoscapeComponent
            elements={props.elements}
            style={{ width: '100%', height: '800px' }}
            stylesheet={[
                {
                    selector: 'edge',
                    style: {
                        'width': 3,
                        'line-color': '#ccc',
                        'target-arrow-color': '#ccc',
                        'target-arrow-shape': 'triangle',
                        'label': 'data(label)',
                        'font-size': '8'
                    }
                },
                {
                    selector: 'node',
                    style: {
                        'background-color': '#ccc',
                        'label': 'data(label)',
                        'width': 'mapData(weight,0,1,20,50)',
                        'height': 'mapData(weight,0,1,20,50)',
                        'font-size':'8px'

                    }
                },
            ]}
            layout={{ name: 'dagre' }}
            cy={cy => cy.nodes().on('click', function () { props.onNodeClicked(this); })} />
        : <div>loading...</div>;
};

export default graph;