import React from 'react';

export default props => {
    const selectedGroups = props.routeData
        ? props.routeData.cookieMustContain
            .filter(i => i.indexOf('_s') === 0)
            .map(i => {
                return props.groups.filter(s => s.code === i.replace(/\D/g, ''))[0];
            })
            .filter(i => i)
        : [];
    return !props.routeData || !(props.routeData.name || '').match(/_[a-zA-Z0-9]*conditional/) === -1
        ? <div />
        : (
            <div className="dialog">

                <h4>Selected Groups</h4>
                <ul className="list-group">
                    {(selectedGroups || []).map(s =>
                        <li className="list-group-item" key={s.code}>
                            {s.name}
                            <button type="button" className="btn btn-default btn-sm" onClick={() => props.removeGroup(s.code)}>
                                <span className="glyphicon glyphicon-remove"></span> Remove
                            </button>
                        </li>)}
                </ul>
                <select value={props.groupToAdd} onChange={e => props.setGroupToAdd(e)} className="form-control form-control-sm">
                    <option></option>
                    {props.groups.map(s => <option key={s.code} value={s.code}>{s.name}</option>)}
                </select>
                {props.groupToAdd &&
                    <button type="button" className="btn btn-primary" onClick={props.addSelectedGroup}>Add</button>
                }
                <br />
                <button type="button" className="btn btn-primary" onClick={props.saveRoute}>Save</button>
                <div style={{display:'none'}}>
                    <br /><br />
                    <textarea onChange={props.editJson} cols="30" row="70">
                        {JSON.stringify(props.editedJson || props.routeData)}
                    </textarea>
                    <button type="button" className="btn btn-primary" onClick={props.saveJson}>Save Json</button>
                </div>
            </div>
        );
};