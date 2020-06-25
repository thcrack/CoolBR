import React, { Component } from 'react';

export class ItemWiki extends Component {
    static displayName = ItemWiki.name;

    constructor(props) {
        super(props);
        this.state = { items: [], loading: true };
    }

    componentDidMount() {
        this.populateItemData();
    }

    static renderItemTable(items) {
        return (
            <table className='table table-striped' aria-labelledby="tableLabel">
                <thead>
                <tr>
                    <th>Name</th>
                    <th>Class</th>
                    <th>Durability</th>
                    <th>Cost</th>
                </tr>
                </thead>
                <tbody>
                {items.map(item =>
                    <tr key={item.name}>
                        <td>{item.name}</td>
                        <td>{item.class}</td>
                        <td>{item.durability}</td>
                        <td>{item.cost}</td>
                    </tr>
                )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : ItemWiki.renderItemTable(this.state.items);

        return (
            <div>
                <h1 id="tableLabel" >Item Wiki</h1>
                <p>This component demonstrates fetching data from JSON.</p>
                {contents}
            </div>
        );
    }

    async populateItemData() {
        const response = await fetch('item/all');
        const data = await response.json();
        this.setState({ items: data, loading: false });
    }
}
