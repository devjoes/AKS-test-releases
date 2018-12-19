import React from 'react';
import PropTypes from 'prop-types';
import RouteDetails from './routeDetails';
import { put } from '../../utils/httpRequest';
import { ns } from '../../utils/settings';

const routeDetails = class extends React.Component {
    constructor() {
        super();
        this.state = {
            groups,
            saving: false
        };
    }
    componentDidMount() {
        this.setState({ routeData: this.props.routeData });
    }
    componentWillReceiveProps(newProps) {
        if (!this.state.routeData || this.state.routeData.name !== newProps.routeData.name) {
            this.setState({ routeData: newProps.routeData });
        }
    }
    addSelectedGroup() {
        const routeData = this.state.routeData;
        routeData.cookieMustContain = [...new Set([`_s${this.state.groupToAdd}_`, ...routeData.cookieMustContain])];
        this.setState({ routeData, groupToAdd: '' });
    }
    setGroupToAdd(e) {
        this.setState({ groupToAdd: e.target.value })
    }
    removeGroup(groupCode) {
        const routeData = this.state.routeData;
        routeData.cookieMustContain = routeData.cookieMustContain.filter(r => r !== `_s${groupCode}_`);
        this.setState({ routeData });
    }
    saveRoute() {
        const routeData = this.state.routeData;
        this.setState({ routeData: undefined });
        put('haproxy/route', {
            hostname: routeData.host,
            routeName: routeData.name,
            cookieSubstrings: routeData.cookieMustContain
        }).then(() => {
            this.props.loadData(ns);
        });
    }
    editJson(json) {
        this.setState({ editedJson: json });
    }
    saveJson() {
        const routeData = JSON.parse(this.state.editJson);
        put('haproxy/route', {
            hostname: routeData.host,
            routeName: routeData.name,
            cookieSubstrings: routeData.cookieMustContain
        }).then(() => {
            this.props.loadData(ns);
        });
    }
    render() {
        return (<RouteDetails {...this.state}
            setGroupToAdd={e => this.setGroupToAdd(e)}
            addSelectedGroup={() => this.addSelectedGroup()}
            removeGroup={s => this.removeGroup(s)}
            saveRoute={() => this.saveRoute()}
            editJson={e => this.editJson(e)}
            saving={this.saving} />);
    }
}
routeDetails.propTypes = { routeData: PropTypes.object };

export default routeDetails;

const groups = [
    { code: "1", name: "Test group A" },
    { code: "2", name: "Test group B" }
];