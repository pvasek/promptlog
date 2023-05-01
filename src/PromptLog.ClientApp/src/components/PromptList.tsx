import React, { Component } from 'react';
import { PromptDto } from "../api/PromptDto";
import { Raiting } from "./Raiting";

interface PromptListProps {
    items: PromptDto[];
    selectedItem: PromptDto;
    onSelected: (item: any) => void;
    raitingFilter: number;
    onRatingFilterClick: () => void;
}

export class PromptList extends Component<PromptListProps> {
    static displayName = PromptList.name;

    constructor(props: PromptListProps) {
        super(props);
        this.state = { items: [], loading: true };
    }

    render() {
        return (
            <ul className="prompt-list">
                <li className="filter">
                    <label>Filter: </label><Raiting raiting={this.props.raitingFilter} onClick={() => this.props.onRatingFilterClick()} />
                </li>
                {this.props.items.map(i =>
                    <li key={i.id} onClick={e => this.props.onSelected(i)}
                        className={this.props.selectedItem?.id == i.id ? "selected item" : "item"}
                    >
                        <div>{i.experiment}</div>
                        <div>{i.created}</div>
                    </li>
                )}
            </ul>
        );
    }

}
